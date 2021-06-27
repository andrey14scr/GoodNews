using System.ComponentModel.DataAnnotations;

namespace GoodNewsAggregator.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Это поле должно быть заполнено")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Это поле должно быть заполнено")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
