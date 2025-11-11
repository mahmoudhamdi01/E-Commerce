using AutoMapper;
using CoreLayer.Entities.IdentityModule;
using CoreLayer.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstractionLayer;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager,
        IConfiguration _configuration, IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string Email)
        {
            var User = await _userManager.FindByEmailAsync(Email);
            return User is not null;
        }

        public async Task<AddressDTO> GetCurrentUserAddress(string Email)
        {
            var User = await _userManager.Users.Include(U => U.address)
                                               .FirstOrDefaultAsync(U => U.Email == Email) ?? throw new UserNotFoundException(Email);

            if (User.address is not null)
                return _mapper.Map<Address, AddressDTO>(User.address);
            else
                throw new AddressNotFoundException(User.UserName!);
        }

        public async Task<UserDTO> GetCurrentUserAsync(string Email)
        {
            var User = await _userManager.FindByEmailAsync(Email) ?? throw new UserNotFoundException(Email);
            return new UserDTO()
            {
                Email = Email,
                DisplayName = User.DisplayName,
                Token = await CreateTokenAsync(User)
            };
        }

        public async Task<AddressDTO> UpdateCurrentUserAddress(AddressDTO addressDTO, string Email)
        {
            var User = await _userManager.Users.Include(U => U.address)
                                               .FirstOrDefaultAsync(U => U.Email == Email) ?? throw new UserNotFoundException(Email);

            if(User.address is not null)
            {
                User.address.FirstName = addressDTO.FirstName;
                User.address.LastName = addressDTO.LastName;
                User.address.Street = addressDTO.Street;
                User.address.City = addressDTO.City;
                User.address.Country = addressDTO.Country;
            }
            else
            {
                User.address = _mapper.Map<AddressDTO, Address>(addressDTO);
            }
            await _userManager.UpdateAsync(User);
            return _mapper.Map<AddressDTO>(User.address);
        }

        public async Task<UserDTO> LoginAsync(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email) ?? throw new UserNotFoundException(loginDTO.Email);

            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDTO.Password);
            if (IsPasswordValid)
                return new UserDTO()
                {
                    DisplayName = User.DisplayName,
                    Email = User.Email,
                    Token = await CreateTokenAsync(User)
                };
            else
                throw new UnAuthorizedException();
        }


        public async Task<UserDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber
            };

            var Result = await _userManager.CreateAsync(User, registerDTO.Password);
            if (Result.Succeeded)
                return new UserDTO()
                {
                    DisplayName = User.DisplayName,
                    Email = User.Email,
                    Token = await CreateTokenAsync(User)
                };
            else
            {
                var Errors = Result.Errors.Select(E => E.Description).ToList();
                throw new BadRequestException(Errors);
            }
        }

      
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
            };
            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var SecretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken
            (
                issuer: _configuration.GetSection("JWTOptions")["ISSuer"],
                audience: _configuration.GetSection("JWTOptions")["Audience"],
                claims: Claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: Creds
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
