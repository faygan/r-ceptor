using System.Web.Http;
using System.Web.Http.Description;
using Rceptor.Core.ServiceClient;
using Rceptor.Core.ServiceProxy;
using Service.PersonelService.Models;

namespace Service.PersonelService.ServiceContract
{

    [ServiceContract("api/account", "Authentication service.")]
    public interface IAuthenticationService
    {

        [HttpGet]
        [Route("verifyAuth")]
        [ResponseType(typeof(VerifyUserTokenModel))]
        IServiceResponse VerifyUserAuth();

    }

}
