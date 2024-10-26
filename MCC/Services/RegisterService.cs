using AutoMapper;
using MCC.Data.Dtos;
using MCC.Models;
using Microsoft.AspNetCore.Identity;

namespace MCC.Services
{
    public class RegisterService
    {
        private IMapper _mapper;
        private UserManager<User> _userManager;


        public RegisterService(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;

        }

        public async Task RegisterAsync(CreateUserDto dto)
        {
            User user = _mapper.Map<User>(dto);
            user.UserName = dto.Email;

            IdentityResult result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Falha ao Cadastrar: {errors}");

            }


        }
    }
}
