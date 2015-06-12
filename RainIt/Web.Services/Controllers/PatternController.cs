using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RainIt.Interfaces.Business;

namespace Web.Services.Controllers
{
    public class PatternController : ApiController
    {
        public IPatternManager PatternManager { get; set; }

        public PatternController(IPatternManager patternManager)
        {
            PatternManager = patternManager;
        }
        public IHttpActionResult Get(int id)
        {
            return Ok(PatternManager.GetAllPatterns());
        }

        [AllowAnonymous]
        public IHttpActionResult GetAll()
        {
            return Ok(PatternManager.GetAllPatterns());
        }
    }
}
