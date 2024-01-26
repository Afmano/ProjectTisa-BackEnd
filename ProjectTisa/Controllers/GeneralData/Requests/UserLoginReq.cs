namespace ProjectTisa.Controllers.GeneralData.Requests
{
    /// <summary>
    /// Request for reveice token using existing <b>User</b>'s credentials. Contains <c>Password</c> and optional <c>Username</c>/<c>Email</c>. 
    /// </summary>
    public record class UserLoginReq
    {
        public string? Username { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
    }
}
