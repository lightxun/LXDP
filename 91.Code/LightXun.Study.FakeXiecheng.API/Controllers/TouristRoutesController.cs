using LightXun.Study.FakeXiecheng.API.Dtos;
using LightXun.Study.FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Text.RegularExpressions;
using LightXun.Study.FakeXiecheng.API.ResourceParameters;
using LightXun.Study.FakeXiecheng.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using LightXun.Study.FakeXiecheng.API.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;

namespace LightXun.Study.FakeXiecheng.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        // 私有仓库变量
        private readonly ITouristRouteRepository _touristRouteRepository;
        // 私有 automapper 变量
        private readonly IMapper _mapper;
        // 私有 urlhelper 变量
        private readonly IUrlHelper _urlHelper;
        // 私有 PropertyMappingService 变量
        private readonly IPropertyMappingService _propertyMappingService;

        /// <summary>
        /// 通过构造函数, 为数据仓库进行注入
        /// </summary>
        /// <param name="touristRouteRepository"></param>
        public TouristRoutesController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IPropertyMappingService propertyMappingService)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _propertyMappingService = propertyMappingService;
        }

        private string GenerateTouristRouteResourceURL(
            TouristRouteResourceParameters parameters,
            PaginationResourceParameters parameters2,
            ResourceUrlType type)
        {
            return type switch
            {
                ResourceUrlType.PreviousPage => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        keyword = parameters.Keyword,
                        rating = parameters.Rating,
                        pageNumber = parameters2.PageNumber - 1,
                        pageSize = parameters2.PageSize
                    }),
                ResourceUrlType.NextPage => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        keyword = parameters.Keyword,
                        rating = parameters.Rating,
                        pageNumber = parameters2.PageNumber + 1,
                        pageSize = parameters2.PageSize
                    }),
                _ => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        keyword = parameters.Keyword,
                        rating = parameters.Rating,
                        pageNumber = parameters2.PageNumber,
                        pageSize = parameters2.PageSize
                    })
            };
        }

        private IEnumerable<LinkDto> CreateLinkForTouristRoute(
            Guid touristRouteId,
            string fields)
        {
            var links = new List<LinkDto>();
            // 获取
            links.Add(
                new LinkDto(
                    Url.Link("GetTouristRouteById", new { touristRouteId, fields }),
                    "self",
                    "GET"
                    )
                );
            // 更新
            links.Add(
                new LinkDto(
                    Url.Link("UpdateTouristRoute", new { touristRouteId }),
                    "update",
                    "PUT"
                    )
                );
            // 局部更新
            links.Add(
                new LinkDto(
                    Url.Link("PartiallyUpdateTouristRoute", new { touristRouteId }),
                    "partially_update",
                    "PATCH"
                    )
                );
            // 删除
            links.Add(
                new LinkDto(
                    Url.Link("DeleteTouristRoute", new { touristRouteId }),
                    "delete",
                    "DELETE"
                    )
                );
            // 获取旅游路线图片
            links.Add(
                new LinkDto(
                    Url.Link("GetPictureListForTourstRoute", new { touristRouteId }),
                    "get_pictures",
                    "GET"
                    )
                );
            // 添加旅游路线图片
            links.Add(
                new LinkDto(
                    Url.Link("CreateTouristRoutePicture", new { touristRouteId }),
                    "add_picture",
                    "POST"
                    )
                );

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForTouristRouteList(
            TouristRouteResourceParameters parameters,
            PaginationResourceParameters parameters2)
        {
            var links = new List<LinkDto>();

            // 添加 self, 自我连接
            links.Add(new LinkDto(
                    GenerateTouristRouteResourceURL(parameters, parameters2, ResourceUrlType.CurrentPage),
                    "self",
                    "GET"
                ));

            // api/touristRoutes
            // 添加创建旅游路线
            links.Add(new LinkDto(
                    Url.Link("CreateTouristRoute", null),
                    "create_tourist_route",
                    "POST"
                ));

            return links;
        }

        /// <summary>
        /// 获取旅游路线 API
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetTouristRoutes")]
        [HttpHead]
        // api/touristRoutes?keyword=参数
        // 1. application/json -> 旅游路线资源
        // 2. application/vnd.aleks.hateoas+json
        // 3. application/vnd.aleks.touristRoute.simplify+json
        // 4. application/vnd.aleks.touristRoute.simplify.hateoas+json
        [Produces(
            "application/json",
            "application/vnd.aleks.hateoas+json",
            "application/vnd.aleks.touristRoute.simplify+json",
            "application/vnd.aleks.touristRoute.simplify.hateoas+json")]
        public async Task<IActionResult> GetTouristRoutes(
            [FromQuery] TouristRouteResourceParameters parameters,
            [FromQuery] PaginationResourceParameters parameters2,
            [FromHeader(Name = "Accept")] string mediaType
            //[FromQuery] string keyword, 
            //string rating   // 小于 lessThan, 大于 largerThan, 等于 equalTo, 例如 lessThan3, largerThan5, equalTo
        )
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediatype))
            {
                return BadRequest("请输入正确的媒体类型");
            }
            if(!_propertyMappingService.IsMappingExists<TouristRouteDto, TouristRoute>(parameters.OrderBy))
            {
                return BadRequest("请输入正确的排序参数");
            }

            if (!_propertyMappingService.IsPropertiesExists<TouristRouteDto>(parameters.Fields))
            {
                return BadRequest("请输入正确的塑形参数");
            }

            var touristRoutesFromRepo = await _touristRouteRepository
                .GetRouristRoutesAsync(
                parameters.Keyword, 
                parameters.RatingOperator, 
                parameters.RatingValue, 
                parameters2.PageSize, 
                parameters2.PageNumber,
                parameters.OrderBy);

            if(touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }

            // 使用 AutoMapper 完成数据映射
            var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);

            // 配备分页信息
            var previousPageLink = touristRoutesFromRepo.HasPrevious ?
                GenerateTouristRouteResourceURL(parameters, parameters2, ResourceUrlType.PreviousPage) :
                null; 
            var nextPageLink = touristRoutesFromRepo.HasNext ?
                GenerateTouristRouteResourceURL(parameters, parameters2, ResourceUrlType.NextPage) :
                null;
            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = touristRoutesFromRepo.TotalCount,
                pageSize = touristRoutesFromRepo.PageSize,
                currentPage = touristRoutesFromRepo.CurrentPage,
                totalPages = touristRoutesFromRepo.TotalPages
            };
            Response.Headers.Add("x-pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            // 此处的 Ok , 就是 HTTP 码 200 的状态
            var shapedDtoList = touristRoutesDto.ShapeData(parameters.Fields);

            if(parsedMediatype.MediaType == "application/vnd.aleks.hateoas+json")
            {
                var linkDto = CreateLinksForTouristRouteList(parameters, parameters2);
                var shapedDtoWithLinkList = shapedDtoList.Select(t =>
                {
                    var touristRouteDictionary = t as IDictionary<string, object>;
                    var links = CreateLinkForTouristRoute((Guid)touristRouteDictionary["Id"], null);
                    touristRouteDictionary.Add("links", links);
                    return touristRouteDictionary;
                });
                var result = new
                {
                    value = shapedDtoWithLinkList,
                    links = linkDto
                };
                return Ok(result);
            }

            return Ok(shapedDtoList);

        }

        /// <summary>
        /// 获取旅游路线 API
        /// </summary>
        /// <param name="touristRouteId"></param>
        /// <returns></returns>
        [HttpGet("{touristRouteId:Guid}", Name = "GetTouristRouteById")]
        [HttpHead("{touristRouteId:Guid}", Name = "GetTouristRouteById")]
        // api/touristRoutes/{touristRouteId}
        public async Task<IActionResult> GetTouristRoutesById(
            Guid touristRouteId,
            string fields)
        {
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);

            if(touristRouteFromRepo == null)
            {
                return NotFound($"旅游路线{touristRouteId}找不到");
            }

            // 注释, 使用 AutoMapper 完成数据映射
            //var touristRouteDto = new TouristRouteDto()
            //{
            //    Id = touristRouteFromRepo.Id,
            //    Title = touristRouteFromRepo.Title,
            //    Description = touristRouteFromRepo.Description,
            //    Price = touristRouteFromRepo.OriginalPrice * (decimal)(touristRouteFromRepo.DiscountPresent ?? 1),
            //    CreateTime = touristRouteFromRepo.CreateTime,
            //    UpdateTime = touristRouteFromRepo.UpdateTime,
            //    Features = touristRouteFromRepo.Features,
            //    Fees = touristRouteFromRepo.Fees,
            //    Notes = touristRouteFromRepo.Notes,
            //    Rating = touristRouteFromRepo.Rating,
            //    TravelDays = touristRouteFromRepo.TravelDays.ToString(),
            //    TripType = touristRouteFromRepo.TripType.ToString(),
            //    DepartureCity = touristRouteFromRepo.DepartureCity.ToString()
            //};
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);

            //return Ok(touristRouteDto.ShapData(fields));
            var linkDtos = CreateLinkForTouristRoute(touristRouteId, fields);
            var result = touristRouteDto.ShapData(fields) as IDictionary<string, object>;
            result.Add("links", linkDtos);
            return Ok(result);
        }

        /// <summary>
        /// 创建旅游路线
        /// </summary>
        /// <param name="touristRouteForCreationDto"></param>
        /// <returns></returns>
        [HttpPost(Name = "CreateTouristRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();

            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);

            var linkDtos = CreateLinkForTouristRoute(touristRouteModel.Id, null);

            var result = touristRouteToReturn.ShapData(null) as IDictionary<string, object>;
            result.Add("links", linkDtos);

            return CreatedAtRoute("GetTouristRoutesById", new { touristRouteId = result["Id"] }, result);
        }

        /// <summary>
        /// 更新旅游路线所有信息
        /// </summary>
        /// <param name="touristRouteId"></param>
        /// <param name="touristRouteForUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("{touristRouteId}", Name = "UpdateTouristRoute")]
        public async Task<IActionResult> UpdateTouristRoute([FromRoute]Guid touristRouteId, [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        {
            if (! await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("旅游路线找不到");
            }

            var touristRouteFormRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);

            //1. 映射为 dto
            //2. 更新 dto
            //3 映射 model
            _mapper.Map(touristRouteForUpdateDto, touristRouteFormRepo);

            // 在数据仓库中, 我们调用的是底层框架 EF, 而在 EF 中, 数据模型 touristRouteFormRepo 是根据上下文关系对象 context 来追踪的
            // 当我们在执行 _mapper.Map 时, 数据模型中的数据就已经被修改, 而此时, 数据中的追踪状态也相应的发生了变化
            // 模型的追踪状态是有 EF 的 context 自我管理的, 所以在最后执行 Save 时, 会将模型中的状态更新入数据库
            await _touristRouteRepository.SaveAsync();

            // 仅返回 204 状态码, 不包含任何数据, 至于是否返回数据是根据实际需要 
            return NoContent();
        }

        /// <summary>
        /// 更新旅游路线局部信息
        /// </summary>
        /// <param name="touristRouteId"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{touristRouteId}", Name = "PartiallyUpdateTouristRoute")]
        public async Task<IActionResult> PartiallyUpdateTouristRoute(
            [FromRoute] Guid touristRouteId,
            [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument)
        {
            if (! await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("旅游路线找不到");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);
            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// 删除旅游路线
        /// </summary>
        /// <param name="touristRouteId"></param>
        /// <returns></returns>
        [HttpDelete("{touristRouteId}", Name = "DeleteTouristRoute")]
        public async Task<IActionResult> DeleteTouristRoute([FromRoute]Guid touristRouteId)
        {
            if (! await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("旅游路线找不到");
            }

            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(touristRoute);

            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// 删除旅游路线 - 批量
        /// </summary>
        /// <param name="touristRouteIds"></param>
        /// <returns></returns>
        [HttpDelete("({touristRouteIds})")]
        public async Task<IActionResult> DeleteTouristRouteByIds(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute]IEnumerable<Guid> touristRouteIds)
        {
            if (touristRouteIds == null)
            {
                return BadRequest();
            }

            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesByIdsAsync(touristRouteIds);
            _touristRouteRepository.DeleteTouristRoutes(touristRoutesFromRepo);

            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }
    }
}
