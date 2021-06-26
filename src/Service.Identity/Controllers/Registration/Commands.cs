using System;

namespace App.Identity.Registration
{
    public static class Commands
    {
        public static class V1
        {
            public class RegisterUser
            {
                public string UserId { get; set; }
                public string Email { get; set; }
                public string Password { get; set; }
                public string FullName { get; set; }
            }
        }
    }
}