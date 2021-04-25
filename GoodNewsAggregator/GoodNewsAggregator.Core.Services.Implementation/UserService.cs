using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using GoodNewsAggregator.Core.Services.Interfaces;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        public string GetPasswordHash(string password)
        {
            var sha256 = new SHA256CryptoServiceProvider();
            var sha256data = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = Encoding.UTF8.GetString(sha256data);
            return hashedPassword;
        }
    }
}
