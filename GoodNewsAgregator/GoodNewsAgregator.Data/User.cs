using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAgregator.Data
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public byte RoleId { get; set; }
    }
}
