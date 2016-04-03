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
    [ServiceContract("api/person", "Personel Service")]
    public interface IPersonService
    {


        #region GET

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("{id:int}")]
        [OperationContract("{id:int}", "GET", ReplyType = typeof(PersonDto))]
        IServiceResponse GetPerson(int id);

        [HttpGet]
        [Route("{id:int}/{deptId:int}")]
        [OperationContract(typeof(PersonDto))]
        IServiceResponse GetPersonWithDeptId(int id, int deptId);

        //[Route("departments/{deptId:int}")]
        //[HttpGet]
        //[ResponseType(typeof(IEnumerable<Person>))]
        [OperationContract("departments/{deptId:int}", "get", ReplyType = typeof(IEnumerable<PersonDto>))]
        IServiceResponse GetPersonsByDepartment(int deptId);

        [HttpPost]
        [OperationContract(typeof(IEnumerable<PersonDto>))]
        [Route("GetPersonsByContext")]
        IServiceResponse GetPersonsByContext(PersonQueryContext query);

        [HttpPost]
        [OperationContract(typeof(IEnumerable<PersonDto>))]
        [Route("getactivepersons/{onlyActive:bool}")]
        [ActionName("activePersons")]
        IServiceResponse GetActivePersonsByContext(bool onlyActive, PersonQueryContext query);

        [HttpGet]
        [OperationContract(typeof(IEnumerable<PersonPayInfoDto>))]
        [Route("payments")]
        IServiceResponse GetPersonPaymentInfos([FromUri] PersonQueryContext filter);

        [HttpPost]
        [OperationContract(typeof(void))]
        [Route("udpatepayments")]
        IServiceResponse SetPersonPayments(PersonQueryContext personFilter, PaymentQueryContext payFilter);

        [OperationContract("getpersonsbyname", "POST", typeof(IEnumerable<PersonDto>))]
        IServiceResponse GetPersonsByName([FromBody] string name);


        [HttpGet]
        [Route("departments/{deptName:notnullorempty}", Name = "departmentName", Order = 1)]
        [ResponseType(typeof(IEnumerable<PersonDto>))]
        IServiceResponse GetPersonsDepartmansInfo(string deptName);

        [Route("image/{personId:int}")]
        [HttpGet]
        IServiceResponse GetPersonImage(int personId);

        #endregion

        #region PUT

        [Route("{personId:int}/debit/{debit:decimal}")]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [OperationContract("{personId:int}/debit/{debit:decimal}", "PUT")]
        IServiceResponse SetPersonDebit(int personId, decimal debit, string status = "default debit status");

        [Route("{personId:int}/credit/{credit:decimal}")]
        [AcceptVerbs("PUT")]
        [HttpPut]
        IServiceResponse SetPersonCredit(int personId, decimal credit, string status = "default credit status");

        #endregion

        #region DELETE

        [Route("{personId:int}")]
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        IServiceResponse DeletePerson(int personId);

        #endregion

    }
}