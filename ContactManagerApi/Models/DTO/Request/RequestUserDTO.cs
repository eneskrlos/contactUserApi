namespace ContactManagerApi.Models.DTO.Request
{
    public class RequestUserDTO
    {
        public Guid Id { get; set; }

        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime DateOfBirth { get; set; } 
    }
}
