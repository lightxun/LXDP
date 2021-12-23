using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Stateless;

namespace LightXun.Study.FakeXiecheng.API.Models
{
    public enum OrderStateEnum
    {
        Pending,            // 订单已生成
        Processing,         // 支付处理中
        Completed,          // 交易成功
        Declined,           // 交易失败
        Cancelled,          // 订单取消
        Refund              // 已退款
    }

    public enum OrderStateTrigeerEnum
    {
        PlaceOrder,         // 支付
        Approve,            // 支付成功
        Reject,             // 支付失败
        Cancel,             // 取消
        Return              // 退货
    }

    public class Order
    {
        public Order()
        {
            StateMachineInit();
        }

        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 用户的外键
        /// 购物车必须依存一个用户, 也就是说, 一个购物车有且只有一个用户
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 既然有了用户的外键, 那么从购物车应该也可以访问用户, 得到用户的信息
        /// 该字段只负责外键关系对象的引用, 由 EF 自我管理, 所以该字段不会参与数据库结构的创建
        /// </summary>
        public ApplicationUser User { get; set; }
        public ICollection<LineItem> OrderItems { get; set; }
        public OrderStateEnum State { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public string TransactionMetadata { get; set; }
        StateMachine<OrderStateEnum, OrderStateTrigeerEnum> _machine;
        private void StateMachineInit()
        {
            _machine = new StateMachine<OrderStateEnum, OrderStateTrigeerEnum>(OrderStateEnum.Pending);

            _machine.Configure(OrderStateEnum.Pending).Permit(OrderStateTrigeerEnum.PlaceOrder, OrderStateEnum.Processing)
                                                      .Permit(OrderStateTrigeerEnum.Cancel, OrderStateEnum.Cancelled);
            _machine.Configure(OrderStateEnum.Processing).Permit(OrderStateTrigeerEnum.Approve, OrderStateEnum.Completed)
                                                         .Permit(OrderStateTrigeerEnum.Reject, OrderStateEnum.Declined);
            _machine.Configure(OrderStateEnum.Declined).Permit(OrderStateTrigeerEnum.PlaceOrder, OrderStateEnum.Processing);
            _machine.Configure(OrderStateEnum.Completed).Permit(OrderStateTrigeerEnum.Return, OrderStateEnum.Refund);
        }
    }
}
