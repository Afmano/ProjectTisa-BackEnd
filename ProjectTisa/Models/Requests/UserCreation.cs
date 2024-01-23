namespace ProjectTisa.Models.Requests
{
    public record class UserCreation
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}
