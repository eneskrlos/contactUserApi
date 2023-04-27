namespace ContactManagerApi.Models.DTO.Response
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Username { get; set; } = null!;
        

    }
}
