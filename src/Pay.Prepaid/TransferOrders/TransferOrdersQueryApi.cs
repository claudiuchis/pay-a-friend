using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using static Pay.Prepaid.Projections.ReadModels;

namespace Pay.Prepaid.TransferOrders
{
    [ApiController]
    [Route("api/prepaid/transfer")]
    public class PrepaidAccountsQueryApi : ControllerBase 
    {
        TransferOrdersQueryService _transferOrdersQueryService;
        public PrepaidAccountsQueryApi(
            TransferOrdersQueryService transferOrdersQueryService
        )
        {
            _transferOrdersQueryService = transferOrdersQueryService;
        }

        [HttpGet]
        public Task<TransferOrder> GetOrderStatus([FromBody] string transferOrderId)
            => _transferOrdersQueryService.GetTransferOrder(transferOrderId);
    }
}