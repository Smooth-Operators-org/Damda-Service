﻿using Damda_Service.Data;
using Damda_Service.Models;
using Damda_Service.Utils;
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
            request.Email = request.Email.ToLower();
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

        public async Task<List<UserInfo>> GetAllUsers()
        {
            var query = from u in _context.User
                        orderby u.UserName
                        where u.UserStatus == true
                        select new UserInfo
                        {
                            Name = u.UserName,
                            Lastname = u.UserLastname,
                            Email = u.UserEmail,
                            Phone = u.UserPhone,
                            Plan = u.PlanId,
                            Level = u.LevelId,
                            Status = u.UserStatus,
                            IsEnable = u.UserEnable
                        };

            var users = await query.ToListAsync();

            return users;
        }

        public async Task<object> GetUserGroupList(string userSerial, string groupSerial)
        {
            var settigs = await _context.GroupSettings.FirstOrDefaultAsync(x => x.GroupSerial == groupSerial);
            var user = await _context.GroupHasUsers.FirstOrDefaultAsync(x => x.GroupSerial == groupSerial && x.UserSerial == userSerial);
            var payment = await _context.Payment.Where(x => x.GroupHasUsersId == user.GroupHasUsersId).ToListAsync();

            return {};

        }

        public async Task<StatusResponse> DeleteUser(string serial)
        {
            var response = new StatusResponse();
            var user = await _context.User.FirstOrDefaultAsync(x => x.UserSerial == serial);

            if (user != null)
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();

                response.message = "User Deleted";
                return response;
            }

            response.message = "User Not Found";
            return response;
        }

        public async Task<StatusResponse> UpdateUser(string serial, UserInfo userinfo)
        {
            var response = new StatusResponse();
            var user = await _context.User.FirstOrDefaultAsync(x => x.UserSerial == serial);

            if (user == null)
            {
                response.message = "User not Found";
                return response;
            }

            if (user.UserEmail != userinfo.Email)
            {
                var exist = await UserExist(userinfo.Email);

                if (exist)
                {
                    response.message = "Email Already Exist";
                    return response;
                }
            }

            user.UserName = userinfo.Name.ToLower();
            user.UserLastname = userinfo.Lastname.ToLower();
            user.UserEmail = userinfo.Email.ToLower();
            user.UserPhone = userinfo.Phone;
            user.PlanId = userinfo.Plan;
            user.LevelId = userinfo.Level;
            user.UserStatus = userinfo.Status;
            user.UserEnable = userinfo.IsEnable;


            _context.User.Update(user);
            await _context.SaveChangesAsync();
            response.message = "User Updated";

            return response;

        }

        public async Task<object> GetUserGroups(string serial)
        {

            var query = from gr in _context.Group
                        join gh in _context.GroupHasUsers
                        on gr.GroupSerial equals gh.GroupSerial
                        join gs in _context.GroupSettings
                        on gh.GroupSerial equals gs.GroupSerial
                        where gh.UserSerial == serial
                        orderby gs.GroupSettingsBegin
                        select new GroupInfo
                        {
                            Group = gr.GroupSerial,
                            Name = gr.GroupName,
                            Begin = gs.GroupSettingsBegin,
                            Amount = gs.GroupSettingsAmount,
                            Status = gs.GroupSettingsStatus
                        };

            var groups = await query.ToListAsync();
            var groupsList = new List<GroupInfo>();

            if (groups.Count > 0)
            {

                foreach (GroupInfo group in groups)
                {
                    groupsList.Add(group);
                }

                var userHasGrouo = new UserHasGroups
                {
                    Serial = serial,
                    List = groupsList
                };

                return userHasGrouo;
            }

            var statusResponse = new StatusResponse
            {
                message = "User Not Found"
            };

            return statusResponse;
        }

        public async Task<object> GetUserBySerial(string serial)
        {

            var user = await _context.User.FirstOrDefaultAsync(x => x.UserSerial == serial);

            if (user != null)
            {

                var userInfo = new UserInfo
                {
                    Name = user.UserName,
                    Lastname = user.UserLastname,
                    Email = user.UserEmail,
                    Serial = user.UserSerial,
                    Phone = user.UserPhone,
                    Plan = user.PlanId,
                    Level = user.LevelId,
                    Status = user.UserStatus,
                    IsEnable = user.UserEnable
                };

                return userInfo;

            }
            else
            {

                return new StatusResponse
                {
                    message = "User Not Found"
                };

            }
        }
    }
}
