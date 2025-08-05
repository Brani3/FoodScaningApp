using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FoodRecognitionAppC_.Models
{
    public class User : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; }

        public User()
        {
            RefreshTokens = new List<RefreshToken>();
        }
    }
}
