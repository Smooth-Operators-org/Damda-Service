using Damda_Service.Data;
using Damda_Service.Models;
using Damda_Service.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Services
{
    public class GroupService
    {
        private Utilities _utilities;
        private readonly DataContext _context;
        public GroupService
            (
            DataContext context,
            Utilities utilities
            )
        {
            _context = context;
            _utilities = utilities;
        }

        public async Task<StatusResponse> PostGroup(GroupRequest request)
        {
            request.Name.ToLower();
            var exist = await GroupExist(request.Creator, request.Name);
            var response = new StatusResponse();

            if (exist)
            {
                response.message = "Group Name Already Exist";
            }
            else
            {
                await GroupRegiter(request);
                response.message = "Group Created";
            }
            return response;
        }

        public async Task<StatusResponse> PostGroupHasUsers(GroupHasUsersRequest request)
        {
            var exist = await GroupHasUsersExist(request.User, request.Group);
            var response = new StatusResponse();

            if (exist)
            {
                response.message = "User Already Has Group Or Position Is Taken";
            }
            else
            {
                await GroupHasUsersRegiter(request.Group, request.User);
                response.message = "User Added to Group";
            }
            return response;
        }

        private async Task<Group> GroupRegiter(GroupRequest request)
        {
            var serial = _utilities.GenSerial();

            while (await _context.User.AnyAsync(x => x.UserSerial == serial))
            {
                serial = _utilities.GenSerial();
            }

            var group = new Group
            {
                GroupSerial = serial,
                GroupName = request.Name,
                UserSerial = request.Creator
            };

            await _context.Group.AddAsync(group);
            await _context.SaveChangesAsync();

            await GroupSettingsRegiter(serial, request);
            await GroupHasUsersRegiter(serial, request.Users);

            return group;

        }
        private async Task<GroupSettings> GroupSettingsRegiter(string serial, GroupRequest request)
        {

            var beginDate = Convert.ToDateTime(request.Settings.Begin);

            var endDate = beginDate.AddDays(request.Settings.Timelapse);

            var groupSettings = new GroupSettings
            {
                GroupSettingsAmount = request.Settings.Amount,
                GroupSettingsBegin = beginDate,
                GroupSettingsEnd = endDate,
                GroupSettingsStatus = request.Settings.Status,
                GroupSerial = serial,

            };

            await _context.GroupSettings.AddAsync(groupSettings);
            await _context.SaveChangesAsync();

            return groupSettings;

        }

        private async Task<object> GroupHasUsersRegiter(string serial, Dictionary<int, string> users)
        {

            foreach (var user in users)
            {
                var groupHasUsers = new GroupHasUsers
                {
                    GroupSerial = serial,
                    UserSerial = user.Value,
                    GroupHasUserPosition = user.Key
                };

                await _context.GroupHasUsers.AddAsync(groupHasUsers);
            }

            await _context.SaveChangesAsync();

            return new StatusResponse { message = "User(s) Added To Group"};

        }

        public async Task<object> GetGroupBySerial(string serial)
        {

            var query = from u in _context.User
                        join g in _context.GroupHasUsers
                        on u.UserSerial equals g.UserSerial
                        where g.GroupSerial == serial && u.UserStatus == true && u.UserEnable == true
                        orderby g.GroupHasUserPosition
                        select new
                        {
                            Position = g.GroupHasUserPosition,
                            Serial = u.UserSerial,
                            Name = u.UserName,
                            LastName = u.UserLastname,
                        };

            var users = await query.ToListAsync();
            var usersList = new List<object>();

            if (users.Count > 0)
            {

                foreach (object user in users)
                {
                    usersList.Add(user);
                }

                var groupInfo = new GroupInfo
                {
                    Serial = serial,
                    List = usersList
                };

                return groupInfo;
            }

            var statusResponse = new StatusResponse
            {
                message = "Group Not Found"
            };

            return statusResponse;

        }

        private async Task<bool> GroupHasUsersExist(Dictionary<int, string> user, string group)
        {
            foreach (var u in user)
            {
                if (await _context.GroupHasUsers.AnyAsync(x => x.UserSerial == u.Value && x.GroupSerial == group || x.GroupSerial == group && x.GroupHasUserPosition == u.Key))
                {
                    return true;
                }
            }
            return false;

        }

        private async Task<bool> GroupExist(string userserial, string name)
        {
            if (await _context.Group.AnyAsync(x => x.UserSerial == userserial && x.GroupName == name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
