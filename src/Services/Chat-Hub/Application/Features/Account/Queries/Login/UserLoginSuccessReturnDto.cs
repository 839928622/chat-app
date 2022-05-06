namespace Application.Features.Account.Queries.Login
{
    public class UserLoginSuccessReturnDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string? MainPhotoUrl { get; set; }
    }
}
