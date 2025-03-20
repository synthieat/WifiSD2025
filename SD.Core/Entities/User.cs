using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public SecureString Password { get; set; }

        public User UserWithoutPassword
        {
            get => new User
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                UserName = this.UserName,
                Password = null
            };            
        }
    }
}
