using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooreApi.Business;
using Newtonsoft.Json.Linq;
using KooreApi.Model;
using KooreApi.Authorization;
using Microsoft.Extensions.Options;
using System.Net;

namespace KooreApi.Controllers
{
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuth _auth;
        private readonly AppSettings _appSettings;

        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        public AuthController(IOptions<AppSettings> appSettings, IAuth auth)
        {
            this._auth = auth;

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("api/v1/[controller]")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string email)
        {
            try
            {
                bool ret = await _auth.CheckEmailExists(email);

                return StatusCode(200, ret);
            }
            catch (Exception ex)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/v1/[controller]")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(User user)
        {
            try
            {
                var retSignUp = await _auth.SignUp(user);

                return StatusCode(200, retSignUp);
                //return Ok(_audience_reports);
            }
            catch (Exception ex)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/v1/[controller]/session")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostSession(UserSession user)
        {
            try
            {

                var retSignUp = await _auth.SignIn(user);

                return StatusCode(200, retSignUp);
            }
            catch (Exception ex)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [AllowAnonymous]
        [HttpPatch]
        [Route("api/v1/[controller]/session")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> PatchSession()
        {
            try
            {
                string refreshToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                var retSignUp = await _auth.RefreshToken(refreshToken);

                return StatusCode(200, retSignUp);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "UnauthorizedAccessException")
                    return (StatusCode(StatusCodes.Status401Unauthorized, ex.Message));
                return (StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [AllowAnonymous]
        [HttpDelete]
        [Route("api/v1/[controller]/session")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteSession()
        {
            try
            {
                string refreshToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                var retSignUp = await _auth.DeleteToken(refreshToken);
                if (retSignUp)
                    return StatusCode(200);
                else
                    return StatusCode(500);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "UnauthorizedAccessException")
                    return (StatusCode(StatusCodes.Status401Unauthorized, ex.Message));
                return (StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/v1/[controller]/reset-password/{email}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostSendPasswordEmail(string email)
        {
            try
            {
                var retSignUp = await _auth.SendPasswordEmail(email);
                if (retSignUp)
                    return StatusCode(200);
                else
                    return StatusCode(500);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "UnauthorizedAccessException")
                    return (StatusCode(StatusCodes.Status401Unauthorized, ex.Message));
                else if (ex.GetType().Name == "KeyNotFoundException")
                    return (StatusCode(StatusCodes.Status404NotFound));

                return (StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/v1/[controller]/password/reset/{userId}/{token}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostResetPassword(string userId, string token, [FromBody] User user)
        {
            try
            {
                //string password = Request.HttpContext..ReadAsStringAsync();
                var retSignUp = await _auth.ResetPassword(userId, token, user.Password);
                if (retSignUp)
                    return StatusCode(200);
                else
                    return StatusCode(500);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "UnauthorizedAccessException")
                    return (StatusCode(StatusCodes.Status401Unauthorized, ex.Message));
                else if (ex.GetType().Name == "KeyNotFoundException")
                    return (StatusCode(StatusCodes.Status404NotFound));

                return (StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("api/v1/healthcheck")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> HealthCheck()
        {

            return StatusCode(200, "Server Up and Running!");

        }
    }
}
