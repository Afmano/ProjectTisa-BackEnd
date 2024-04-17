namespace ProjectTisa.Controllers.GeneralData.Requests.UserReq
{
    /// <summary>
    /// Request for reveice token using existing <see cref="Models.User"/>'s credentials. Contains <c>Password</c> and optional <c>Username</c>/<c>Email</c>. 
    /// </summary>
    public record UserLoginReq
    {
        public string? Username { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
    }
}
