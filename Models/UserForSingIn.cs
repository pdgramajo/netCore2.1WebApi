using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class UserForSingIn
    {
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
