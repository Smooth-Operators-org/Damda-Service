using Damda_Service.Data;
using Damda_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Services
{
    public class AuthService
    {
        private DataContext _context;

        public AuthService(
            DataContext context
            )
        {
            _context = context;
        }

        public async Task<object> Login(AuthLogin authLogin)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.UserEmail == authLogin.Email.ToLower());

            if (user != null)
            {
                if (!VerifyPassword(authLogin.Password, user.UserPassword))
                {
                    return new StatusResponse
                    {
                        message = "Bad Credentials"
                    };
                }
                else
                {
                    var userInfo = new UserInfo
                    {
                        Name = user.UserName,
                        Lastname = user.UserLastname,
                        Email = user.UserEmail,
                        Serial = user.UserSerial,
                        Plan = user.PlanId,
                        Level = user.LevelId,
                        IsEnable = user.UserEnable
                    };
                    return userInfo;
                }
            }

                return new StatusResponse
                {
                    message = "Bad Credentials"
                };
        }

        private bool VerifyPassword(string password, string storedPassword)
        {
            if (password != storedPassword) return false;

            return true;
        }
    }
}
