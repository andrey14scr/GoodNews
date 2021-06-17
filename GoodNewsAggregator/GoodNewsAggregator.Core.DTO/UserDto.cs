using System;

namespace GoodNewsAggregator.Core.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public Guid RoleId { get; set; }
    }
}
