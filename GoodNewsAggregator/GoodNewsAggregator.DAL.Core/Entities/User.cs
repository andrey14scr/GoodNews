using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
