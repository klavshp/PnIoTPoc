using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PnIotPoc.WebApi.Controllers
{
    [RoutePrefix("api/v1/autofac")]
    public class AutofacController : ApiController
    {
        private readonly IAutofacRepository _repository;

        public AutofacController(IAutofacRepository repository)
        {
            _repository = repository;
        }

        [Route("list")]
        [HttpGet]
        public string GetAutofacData()
        {
            var repository = new AutofacRepository();
            var data = repository.GetData();
            return data;
        }
    }

    public interface IAutofacRepository
    {
        string GetData();
    }

    public class AutofacRepository : IAutofacRepository
    {
        public string GetData()
        {
            return "Hello world.";
        }
    }

}