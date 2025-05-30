using LibraryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Services
{
    public class AuthService
    {
        private UserRepository userRepository;

        public AuthService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool Login(string username, string password, out string role)
        {
            role = null;

            var user = userRepository.GetUserByUsername(username);
            if (user == null)
                return false;

            // Ở đây giả sử password là plain text, nếu hash thì cần so sánh hash
            if (user.Password == password)
            {
                role = user.Role;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
