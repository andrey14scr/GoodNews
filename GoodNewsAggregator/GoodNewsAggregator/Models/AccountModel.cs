using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.Models
{
    public class AccountModel
    {
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
