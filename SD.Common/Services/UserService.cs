using SD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SD.Common.Services
{
    public  class UserService : IUserService
    {
        /* Mockup User anlegen */
        private readonly List<User> users =
        [ new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                UserName = "Test",
                Password = new NetworkCredential("Test", "12345").SecurePassword
            }
        ];

        public async Task<User> Authenticate(string username, string password, CancellationToken cancellationToken)
        {
            var user = users.SingleOrDefault(w => string.Compare(w.UserName, username, true) == 0 &&
                                                  new NetworkCredential(w.UserName, w.Password).Password == password);

            if(user == null)
            {
                return user;
            }

            return await Task.FromResult(user.UserWithoutPassword);

        }   
    }
}
