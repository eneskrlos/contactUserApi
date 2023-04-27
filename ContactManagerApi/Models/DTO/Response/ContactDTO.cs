namespace ContactManagerApi.Models.DTO.Response
{
    public class ContactDTO
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string? Lastname { get; set; }
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = null!;
        
        public Guid? Owner { get; set; } 

        //public string? OwnerName { get; set; }
    }
}
