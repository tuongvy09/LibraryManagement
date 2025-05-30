using LibraryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BUS
{
    public class UserBLL
    {
        private readonly UserRepository _userRepo;

        public UserBLL()
        {
            DBConnection dbConnection = new DBConnection();
            _userRepo = new UserRepository(dbConnection);
        }

        public bool ThemNguoiDung(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Tên người dùng không được để trống.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Mật khẩu không được để trống.");

            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Vai trò không được để trống.");

            return _userRepo.AddUser(username, password, role);
        }
    }
}
