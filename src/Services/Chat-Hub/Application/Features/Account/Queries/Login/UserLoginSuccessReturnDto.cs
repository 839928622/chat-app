namespace Application.Features.Account.Queries.Login
{
    public class UserLoginSuccessReturnDto
    {
        public string UserName { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string? MainPhotoUrl { get; set; } = null!;
    }
}
