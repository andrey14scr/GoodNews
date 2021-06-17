using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GoodNewsAggregator.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Это поле должно быть заполнено")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Введите email адрес")]
        [Remote("CheckEmail", "Account", ErrorMessage = "Такой email уже существует")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле должно быть заполнено")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Это поле должно быть заполнено")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Неправильный пароль")]
        public string PasswordConfirmation { get; set; }
    }
}
