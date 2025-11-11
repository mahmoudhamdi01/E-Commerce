using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstractionLayer
{
    public interface IAuthenticationService
    {
        public Task<UserDTO> LoginAsync(LoginDTO loginDTO);
        public Task<UserDTO> RegisterAsync(RegisterDTO registerDTO);
        public Task<bool> CheckEmailAsync(string Email);
        public Task<AddressDTO> GetCurrentUserAddress(string Email);
        public Task<AddressDTO> UpdateCurrentUserAddress(AddressDTO addressDTO, string Email);
        public Task<UserDTO> GetCurrentUserAsync(string Email);
    }
}
