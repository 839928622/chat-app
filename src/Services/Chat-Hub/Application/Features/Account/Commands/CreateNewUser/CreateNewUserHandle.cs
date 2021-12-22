using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Account.Commands.CreateNewUser
{
    public class CreateNewUserHandle : IRequestHandler<CreateNewUserCommand, OperationResult<RegisteredUserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public CreateNewUserHandle(IUserRepository userRepository, UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<OperationResult<RegisteredUserDto>> Handle(CreateNewUserCommand request, CancellationToken cancellationToken)
        {
            var existUser = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (existUser != null) return OperationResult<RegisteredUserDto>.Error("Username is taken");
            //var user = _mapper.Map<AppUser>(input);
            //using var hmac = new HMACSHA512();
            var user = new AppUser()
            {
                UserName = request.Username,
                KnownAs = request.KnowAs,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                City = request.City,
                Country = request.Country

            };
           
            //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input.Password));
            //user.PasswordSalt = hmac.Key;

            //await _context.Users.AddAsync(user);
            //await _context.SaveChangesAsync();
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return OperationResult<RegisteredUserDto>.Error("An error occurs while creating an new user."); 
            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return OperationResult<RegisteredUserDto>.Error("An error occurs while adding a new user to Member role");
            var res = new RegisteredUserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
            return OperationResult<RegisteredUserDto>.Success(res);
        }
    }
}
