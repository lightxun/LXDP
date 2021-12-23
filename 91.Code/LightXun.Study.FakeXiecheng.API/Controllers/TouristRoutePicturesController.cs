using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LightXun.Study.FakeXiecheng.API.Services;
using LightXun.Study.FakeXiecheng.API.Dtos;
using LightXun.Study.FakeXiecheng.API.Models;

namespace LightXun.Study.FakeXiecheng.API.Controllers
{
    [Route("api/touristRoutes/{touristRouteId}/pictures")]
    [ApiController]
    public class TouristRoutePicturesController: ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutePicturesController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetPictureListForTouristRoute")]
        public async Task<IActionResult> GetPictureListForTouristRoute(Guid touristRouteId)
        {
            if (!( await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线不存在");
            }

            var picturesFromRepo = await _touristRouteRepository.GetPicturesByTouristRouteIdAsync(touristRouteId);
            if(picturesFromRepo == null || picturesFromRepo.Count() <= 0)
            {
                return NotFound("图片不存在");
            }

            return Ok(_mapper.Map<IEnumerable<TouristRoutePictureDto>>(picturesFromRepo));
        }

        [HttpGet("{pictureId}", Name = "GetPicture")]
        public async Task<IActionResult> GetPicture(Guid touristRouteId, int pictureId)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线不存在");
            }

            var pictureFromRepo = await _touristRouteRepository.GetPictureAsync(pictureId);
            if(pictureFromRepo == null)
            {
                return NotFound("图片不存在");
            }

            return Ok(_mapper.Map<TouristRoutePictureDto>(pictureFromRepo));
        }

        [HttpPost(Name = "CreateTouristRoutePicture")]
        public async Task<IActionResult> CreateTouristRoutePicture(
            [FromRoute] Guid touristRouteId,
            [FromBody] TouristRoutePictureForCreationDto touristRoutePictureForCreationDto)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线不存在");
            }

            var pictureModel = _mapper.Map<TouristRoutePicture>(touristRoutePictureForCreationDto);

            _touristRouteRepository.AddTouristRoutePicture(touristRouteId, pictureModel);
            await _touristRouteRepository.SaveAsync();

            var pictureToReturn = _mapper.Map<TouristRoutePictureDto>(pictureModel);
            return CreatedAtRoute(
                "GetPicture",
                new { touristRouteId = pictureModel.TouristRouteId, pictureId = pictureModel.Id },
                pictureToReturn
                );
        }

        [HttpDelete("{pictureId}")]
        public async Task<IActionResult> DeletePicture(
            [FromRoute]Guid touristRouteId,
            [FromRoute]int pictureId)
        {
            if(!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线不存在");
            }

            var picture = await _touristRouteRepository.GetPictureAsync(pictureId);
            _touristRouteRepository.DeleteTouristRoutePicture(picture);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }
    }
}
