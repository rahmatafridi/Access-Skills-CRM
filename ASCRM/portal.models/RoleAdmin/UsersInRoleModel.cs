using System;
using System.Collections.Generic;
using System.Text;

namespace portal.models.RoleAdmin
{
    public class UsersInRoleModel
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
    }

}
