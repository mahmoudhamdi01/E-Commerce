using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstractionLayer;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    public class AccountController(IServiceManager _serviceManager) : APIBaseController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var User = await _serviceManager.AuthenticationService.LoginAsync(loginDTO);
            return Ok(User);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var User = await _serviceManager.AuthenticationService.RegisterAsync(registerDTO);
            return Ok(User);
        }

        [HttpGet("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var Result = await _serviceManager.AuthenticationService.CheckEmailAsync(email);
            return Ok(Result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var AppUser = await _serviceManager.AuthenticationService.GetCurrentUserAsync(GetEmailForToken());  
            return Ok(AppUser);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTO>> GetCurrentUserAddress()
        {
            var AppUser = await _serviceManager.AuthenticationService.GetCurrentUserAddress(GetEmailForToken());
            return Ok(AppUser);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDTO>> UpdateCurrentUserAddress(AddressDTO addressDTO)
        {
            var AppUser = await _serviceManager.AuthenticationService.UpdateCurrentUserAddress(addressDTO, GetEmailForToken());
            return Ok(AppUser);
        }
    }
}
