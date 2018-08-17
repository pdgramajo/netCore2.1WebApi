namespace Api.Models
{
    public class LoginUserResponse
    {
        public int UserId { get; set; }
        public int PersonId { get; set; }
        public string UserName { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
    }
}
