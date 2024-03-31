using System.ComponentModel.DataAnnotations;
using HistoricalMonumentsWebApplication.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HistoricalMonumentsWebApplication.Models.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name can't be blank")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email Already Registered")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Phone can't be blank")]
        [Phone(ErrorMessage = "Phone number should contain numbers only")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }


        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password doesn't match with Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
    }
}
