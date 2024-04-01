using System.ComponentModel.DataAnnotations;
using HistoricalMonumentsWebApplication.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HistoricalMonumentsWebApplication.Models.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Поле імені повинне бути заповненим")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Поле пошти повинне бути заповненим")]
        [EmailAddress(ErrorMessage = "Поле пошти не є дійсною адресою електронної пошти.")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Пошта вже існує")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Поле телефону повинне бути заповненим")]
        [Phone(ErrorMessage = "Дозволені тільки числа")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }


        [Required(ErrorMessage = "Поле повтору паролю повинне бути заповненим")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Поле повтору паролю повинно бути заповненим")]
        [Compare(nameof(Password), ErrorMessage = "Паролі повинні бути однаковими")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
    }
}
