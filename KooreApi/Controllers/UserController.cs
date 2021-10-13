using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooreApi.Business;
using KooreApi.Model;
using KooreApi.Authorization;
using Microsoft.Extensions.Options;

namespace KooreApi.Controllers
{
    [Authorize]
    public class UserController : ControllerBase
    {
        private IAuth _auth;

        public UserController(IOptions<AppSettings> appSettings, IAuth auth)
        {

            this._auth = auth;
        }

        [HttpGet]
        [Route("/api/v1/[controller]")]
        [ProducesResponseType(typeof(audience_report), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string email)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
    }
}
