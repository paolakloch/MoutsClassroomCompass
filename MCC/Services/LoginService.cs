using AutoMapper;
using MCC.Data.Dtos;
using MCC.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace MCC.Services
{
    public class LoginService
    {
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private TokenService _tokenService;

        public LoginService(
            IMapper mapper,
            UserManager<User> userManager,
            TokenService tokenService,
            SignInManager<User> signInManager
            )
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public async Task<string> LoginAsync(LoginUserDto dto)
        {
            var result =
            await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, false, false);

            if (!result.Succeeded)
            {
                throw new Exception("Usuário não autenticado");
            }

            var user = _signInManager
                .UserManager
                .Users
                .FirstOrDefault(user =>
                user.NormalizedUserName == dto.UserName.ToUpper());

            var token = _tokenService.GenerateToken(user);

            return token;

        }
    }
}
