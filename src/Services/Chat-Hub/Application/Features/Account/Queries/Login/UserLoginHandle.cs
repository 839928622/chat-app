using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Account.Queries.Login
{
    public class UserLoginHandle : IRequestHandler<LoginRequest, OperationResult<UserLoginSuccessReturnDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IUserPhotoRepository _userPhotoRepository;

        public UserLoginHandle(IUserRepository userRepository, SignInManager<AppUser> signInManager,
                               ITokenService tokenService,IUserPhotoRepository userPhotoRepository)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _userPhotoRepository = userPhotoRepository;
        }
        public  async Task<OperationResult<UserLoginSuccessReturnDto>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
           var user=await _userRepository.GetUserByUsernameAsync(request.Username);
           if (user == null)
            return OperationResult<UserLoginSuccessReturnDto>.Error("Invalid username or password");

           var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
           if (!result.Succeeded)
               return OperationResult<UserLoginSuccessReturnDto>.Error("Invalid username or password");
           
           var photoIdCollection = await _userPhotoRepository.GetPhotoIdCollectionAsync(user.Id, cancellationToken);
           var tasks = photoIdCollection.Select(_userPhotoRepository.GetPhotoByIdFromCache);
           var photos  =  await Task.WhenAll(tasks);

           return OperationResult<UserLoginSuccessReturnDto>.Success(new UserLoginSuccessReturnDto()
           {
               UserName = user.UserName,
               Token = await _tokenService.CreateToken(user),
               MainPhotoUrl = photos.FirstOrDefault(x => x is { IsMain: true })?.Url
           });
        }
    }
}
