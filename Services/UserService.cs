using Damda_Service.Data;
using Damda_Service.Models;
using Damda_Service.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Services
{
    public class UserService
    {
        private Utilities _utilities;
        private readonly DataContext _context;
        public UserService
            (
            DataContext context,
            Utilities utilities
            )
        {
            _context = context;
            _utilities = utilities;

        }

        public async Task<StatusResponse> PostUser(UserRequest request)
        {

            var exist = await UserExist(request.Email.ToLower());
            var response = new StatusResponse();

            if (exist)
            {
                response.message = "Email Already Exist";
            }
            else
            {
                await UserRegister(request);
                response.message = "User Created";
            }
            return response;
        }

        private async Task<bool> UserExist(String userEmail)
        {
            if (await _context.User.AnyAsync(x => x.UserEmail == userEmail))
            {
                return true;
            }
            return false;
        }

        private async Task<User> UserRegister(UserRequest request)
        {
            DateTime CreatedDate;
            request.Name = request.Name.ToLower();
            request.Lastname = request.Lastname.ToLower();
            request.Email = request.Name.ToLower();
            DateTime.TryParseExact(request.Created, "yyyy/MM/dd", null, DateTimeStyles.None, out CreatedDate);
            var dateCreated = DateTime.Parse(request.Created);

            var serial = _utilities.GenSerial();

            while (await _context.User.AnyAsync(x => x.UserSerial == serial))
            {
                serial = _utilities.GenSerial();
            }

            var user = new User
            {
                UserName = request.Name,
                UserLastname = request.Lastname,
                UserPassword = request.Password,
                UserSerial = serial,
                UserEmail = request.Email,
                UserPhone = request.Phone,
                UserCreated = dateCreated,
                UserStatus = request.Status,
                UserEnable = request.IsEnable,
                PlanId = request.Plan,
                LevelId = request.Level,
            };

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
