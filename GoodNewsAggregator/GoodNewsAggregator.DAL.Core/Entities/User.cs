using System;
using System.Collections.Generic;

namespace GoodNewsAggregator.DAL.Core
{
    public class User : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public byte RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
