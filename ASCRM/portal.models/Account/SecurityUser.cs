using System;
using System.Collections.Generic;
using System.Text;

namespace portal.models.Account
{
    public class SecurityUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime? LastLockoutDate { get; set; }

    }
}
