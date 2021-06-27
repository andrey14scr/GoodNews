using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace GoodNewsAggregator.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Это поле должно быть заполнено")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Введите email")]
        [Remote("CheckEmail", "Account", ErrorMessage = "Такой email уже занят")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле должно быть заполнено")]
        [Remote("CheckUserName", "Account", ErrorMessage = "Такое имя уже занято")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Это поле должно быть заполнено")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Неправильный пароль")]
        public string PasswordConfirmation { get; set; }
    }
}
