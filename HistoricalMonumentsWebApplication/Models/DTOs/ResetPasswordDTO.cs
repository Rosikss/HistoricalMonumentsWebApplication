using System.ComponentModel.DataAnnotations;

namespace HistoricalMonumentsWebApplication.Models.DTOs
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Поле пошти повинне бути заповненим")]
        [EmailAddress(ErrorMessage = "Поле пошти не є дійсною адресою електронної пошти.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Поле паролю повинне бути заповненим")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Поле повтору паролю повинне бути заповненим")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі повинні бути однаковими")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
