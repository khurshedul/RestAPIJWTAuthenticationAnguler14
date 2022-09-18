using Core.EMS.Entities;
using Core.Utils.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Core.EMS.Services;
using Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IUserService _userService;


        public HomeController(IJwtAuthenticationManager jwtAuthenticationManager, IUserService userService)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User userCred)
        {
            var loginResult = await _userService.Save(userCred);
            return Ok(loginResult);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] User userCred)
        {

            var loginResult = await _jwtAuthenticationManager.AuthenticateAsync(userCred.Username, userCred.Password);
            if (!loginResult.IsSuccessful)
            {
                return Unauthorized();
            }
            else
            {
                var loginInfo = (LoginInformation)loginResult.Data;
                return Ok(new BaseResponse { IsSuccessful = true, Message = "Successful", Data = loginInfo.AccessToken });
            }
        }
    }
}
