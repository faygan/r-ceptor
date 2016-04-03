using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Rceptor.Core.ServiceClient;
using Rceptor.Core.ServiceProxy;
using Service.PersonelService.Models;

namespace Service.PersonelService.ServiceContract
{
    /// <summary>
    ///     Sample service contract
    /// </summary>
    [ServiceContract("api/person-rl", "Personel Service (No Route)")]
    public interface IPersonServiceRouteLess
    {


        #region GET

        [HttpGet]
        [OperationContract(typeof(PersonDto))]
        IServiceResponse GetPersonWithDeptId(int id, int deptId);


        [HttpPost]
        [ResponseType(typeof(IEnumerable<PersonDto>))]
        [Route("getactivepersons")]
        [ActionName("activePersons")]
        IServiceResponse GetActivePersonsByContext(bool onlyActive, PersonQueryContext query);

        #endregion



    }
}