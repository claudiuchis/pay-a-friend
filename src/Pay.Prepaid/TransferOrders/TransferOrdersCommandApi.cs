using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using static Pay.Prepaid.TransferOrders.Commands.V1;

namespace Pay.Prepaid.TransferOrders
{
    [ApiController]
    [Route("api/prepaid/transfer")]
    public class PrepaidAccountsCommandApi : ControllerBase 
    {
        TransferOrdersCommandService _transferOrdersCommandService;
        public PrepaidAccountsCommandApi(
            TransferOrdersCommandService transferOrdersCommandService
        )
        {
            _transferOrdersCommandService = transferOrdersCommandService;
        }

        [HttpPost]
        public async Task CreateTransferOrder([FromBody] CreateTransferOrder command)
        {
            await _transferOrdersCommandService.Handle(command, default);
        }
    }
}