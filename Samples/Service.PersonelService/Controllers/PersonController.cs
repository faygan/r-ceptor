using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Rceptor.Core.ServiceProxy;
using Service.PersonelService.App_Data;
using Service.PersonelService.Models;

namespace Service.PersonelService.Controllers
{

    [RoutePrefix("api/person")]
    public class PersonController : ApiController
    {

        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(PersonDto))]
        public IHttpActionResult GetPerson(int id)
        {
            var negatiator = Configuration.Services.GetContentNegotiator();
            var formatter = negatiator.Negotiate(typeof(PersonDto), Request, Configuration.Formatters);

            var person = PersonDataProvider.PersonData.FirstOrDefault(p => p.PersonId == id);
            if (person == null)
                return NotFound();

            return new ResponseMessageResult(new HttpResponseMessage
            {
                Content = new ObjectContent<PersonDto>(person, formatter.Formatter),
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpGet]
        [Route("{id:int}/{deptId:int}")]
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

        [HttpGet]
        [Route("departments/{deptId:int}")]
        [ResponseType(typeof(IEnumerable<PersonDto>))]
        public IHttpActionResult GetPersonsByDepartment(int deptId)
        {
            var persons = PersonDataProvider.PersonData.
                Where(p => p.DeptId == deptId)
                .ToArray();

            var responseCode = !persons.Any() ? HttpStatusCode.NoContent : HttpStatusCode.OK;

            return new ResponseMessageResult(Request.CreateResponse(responseCode, persons));
        }

        [HttpPost]
        [ResponseType(typeof(IEnumerable<PersonDto>))]
        [Route("GetPersonsByContext")]
        public IHttpActionResult GetPersonsByContext(PersonQueryContext query)
        {
            if (query == null)
                return BadRequest("Query context is missing..");

            var persons = PersonDataProvider.PersonData.
                Where(p =>
                    (!query.DeptId.HasValue || p.DeptId == query.DeptId) &&
                    (!query.PersonId.HasValue || p.DeptId == query.PersonId) &&
                    (query.StartWithName == "" || p.Name.StartsWith(query.StartWithName))
                )
                .ToArray();

            var responseCode = !persons.Any() ? HttpStatusCode.NoContent : HttpStatusCode.OK;

            return new ResponseMessageResult(Request.CreateResponse(responseCode, persons));
        }

        [HttpPost]
        [ResponseType(typeof(IEnumerable<PersonDto>))]
        [Route("getactivepersons/{onlyActive:bool}")]
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

        [HttpGet]
        [OperationContract(typeof(IEnumerable<PersonPayInfoDto>))]
        [ActionName("payments")]
        [Route("payments")]
        public IHttpActionResult GetPersonPaymentInfos([FromUri]PersonQueryContext filter)
        {
            if (filter == null)
                return BadRequest("Search context is missing..");

            var infos = PersonDataProvider.PersonPaymentData
                .Where(p =>
                    (!filter.DeptId.HasValue || p.Person.DeptId == filter.DeptId)
                    && (!filter.PersonId.HasValue || p.Person.DeptId == filter.PersonId)
                    && (filter.StartWithName == null || p.Person.Name.StartsWith(filter.StartWithName)))
                .Select(p => p).
                ToArray();

            if (!infos.Any())
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Not found any person records!"));

            return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.OK, infos));
        }

        [HttpPost]
        [OperationContract(typeof(void))]
        [Route("udpatepayments")]
        [MultipartContent(typeof(PersonQueryContext), "personFilter")]
        [MultipartContent(typeof(PaymentQueryContext), "payFilter")]
        public async Task<IHttpActionResult> SetPersonPayments()
        {
            var mediaType = new UnsupportedMediaTypeException("Method allows only mutipart content.",
                Request.Headers.Accept.FirstOrDefault());

            if (!Request.Content.IsMimeMultipartContent())
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, mediaType));

            var contentProvider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(contentProvider);

            PersonQueryContext personFilter = null;
            PaymentQueryContext payFilter = null;

            foreach (var content in contentProvider.Contents)
            {
                if (content.Headers.ContentDisposition.Name == "personFilter")
                {
                    personFilter = await content.ReadAsAsync<PersonQueryContext>();
                }
                else if (content.Headers.ContentDisposition.Name == "payFilter")
                {
                    payFilter = await content.ReadAsAsync<PaymentQueryContext>();
                }
            }

            if (personFilter == null || payFilter == null)
                return new BadRequestResult(Request);

            return new OkResult(Request);
        }


        [HttpPost]
        [Route("getpersonsbyname")]
        public IHttpActionResult GetPersonsByName([FromBody]string name)
        {
            var result = PersonDataProvider.PersonData
                .Where(p => string.IsNullOrEmpty(name) || p.Name.StartsWith(name))
                .ToArray();

            if (!result.Any())
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.NoContent,
                        "Not found any person records!"));

            return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.OK, result));
        }

        [HttpGet]
        [Route("departments/{deptName:notnullorempty}", Name = "departmentName", Order = 1)]
        [ResponseType(typeof(IEnumerable<PersonDto>))]
        public IHttpActionResult GetPersonsDepartmansInfo(string deptName)
        {
            var persons = PersonDataProvider.PersonData.
                Where(p => p.DeptName == deptName)
                .ToArray();

            var responseCode = !persons.Any() ? HttpStatusCode.NoContent : HttpStatusCode.OK;

            return new ResponseMessageResult(Request.CreateResponse(responseCode, persons));
        }

    }
}