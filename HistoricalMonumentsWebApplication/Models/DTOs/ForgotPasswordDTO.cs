using System.ComponentModel.DataAnnotations;

namespace HistoricalMonumentsWebApplication.Models.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Поле пошти повинне бути заповненим")]
        [EmailAddress(ErrorMessage = "Поле пошти не є дійсною адресою електронної пошти.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
