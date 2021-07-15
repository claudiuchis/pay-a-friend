using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pay.Prepaid.TransferOrders
{
    [ApiController]
    [Route("api/prepaid")]

    public class PrepaidAccountsCommandApi : ControllerBase 
    {
        public PrepaidAccountsCommandApi()
        {

        }

        public async Task<IActionResult> TransferFunds()
        {
            
        }
    }
}