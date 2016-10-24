using System;

using System.Web.Http;
using Service.PersonelService.Models;

namespace Service.PersonelService.Controllers
{

    [RoutePrefix("api/account")]
    public class AuthenticationController : ApiController
    {

        [HttpGet]
        [Route("verifyAuth")]
        public IHttpActionResult VerifyUserAuth()
        {
            return Ok(new VerifyUserTokenModel
            {
                UserName = "@@devadmin",
                Token = "FnUPFvabn8p6gqZ21aCC0I1qXFn-63t4woc7umm2SF7XEbZrHrQH4ijwIlDaNSrX19GEIAF9Vz4aOUsaSO-xEvT_lEZiMw6o8Jy1i9sn5r8BtFTLx0pAzB9qDSZDUWEteI_3Zbye5C8L69FFV2gXPDlslmY8O-fG5IXwEBSF5Zap6kD1b8JpHquu0dG9VtAv-NM2xrQvHEoEf7JAeP7Y_xHDMpYVCwcf2i4aVLJG_Q4gUeXtF9rDlIQL07hDcxqOF768EoTMHH42p0rOIl_SsPnFULB0CQLi01qoJ_4ColzN2rtdFtMrqg5SsCCkIISthR1EKnsscMWGsswcUS0RP8QB2qV38rYykpLCCGhGBwMwThcDRGlY6zrdgoBNuNYxM9FccE3JsyRrHDY59Du4Y-QfmHC8jo44BXT9II191dhWDUZLf2SbUV-e7F8PAG6aaizhvv9QST2i66ULJm4Tgl3mp4uYaT978FaoKIG0oe73uxpEYnrkTyRFkCzDk6de"
            });
        }
    }

}