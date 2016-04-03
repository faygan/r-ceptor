using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Rceptor.Core.ServiceProxy;
using Service.PersonelService.App_Data;
using Service.PersonelService.Models;

namespace Service.PersonelService.Controllers
{

    [RoutePrefix("api/person-rl")]
    public class PersonRouteLessController : ApiController
    {

        [HttpGet]
        [OperationContract(typeof(PersonDto))]
        public IHttpActionResult GetPersonWithDeptId(int id, int deptId)
        {
            var negatiator = Configuration.Services.GetContentNegotiator();
            var formatter = negatiator.Negotiate(typeof(PersonDto), Request, Configuration.Formatters);

            var person = PersonDataProvider.PersonData.FirstOrDefault(p => p.PersonId == id &&
                                                                           p.DeptId == deptId);
            if (person == null)
                return NotFound();

            return new ResponseMessageResult(new HttpResponseMessage
            {
                Content = new ObjectContent<PersonDto>(person, formatter.Formatter),
                StatusCode = HttpStatusCode.OK
            });
        }


        [HttpPost]
        [ResponseType(typeof(IEnumerable<PersonDto>))]
        [Route("getactivepersons")]
        [ActionName("activePersons")]
        public IHttpActionResult GetActivePersonsByContext(bool onlyActive, PersonQueryContext query)
        {
            if (query == null)
                return BadRequest("Query context is missing..");

            var persons = PersonDataProvider.PersonData.
                Where(p =>
                    (!onlyActive || p.IsActive) &&
                    (!query.DeptId.HasValue || p.DeptId == query.DeptId) &&
                    (!query.PersonId.HasValue || p.DeptId == query.PersonId) &&
                    (query.StartWithName == "" || p.Name.StartsWith(query.StartWithName))
                )
                .ToArray();

            if (!persons.Any())
                return new NotFoundResult(Request);

            return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.OK, persons));
        }
    }

}
