﻿using System;
using System.Collections.Generic;
using System.Text;

namespace portal.models.Account
{
    public sealed class Login
    {
        public string Email { get; set; }        
        public string Password { get; set; }        
        public bool RememberMe { get; set; }
    }
}
