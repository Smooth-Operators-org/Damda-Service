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
            var exist = await GroupExist(request.User_Serial, request.Name);
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

        public async Task<object> UpdateGroup(string serial, GroupRequest request)
        {
            var response = new StatusResponse();

            var group = await _context.Group.FirstOrDefaultAsync(x => x.GroupSerial == serial);

            if (group == null)
            {
                response.message = "Group not Found";
                return response;
            }

            var user = await _context.GroupHasUsers.FirstOrDefaultAsync(x => x.UserSerial == request.User_Serial && x.GroupSerial == serial);

            if (user == null || user.RoleId == 2)
            {
                response.message = "Permission Denied";
                return response;
            }

            var query = from gr in _context.Group
                        join gh in _context.GroupHasUsers
                        on gr.GroupSerial equals gh.GroupSerial
                        where gh.UserSerial == serial && gr.GroupName == request.Name
                        select new
                        {
                            Name = gr.GroupName
                        };

            var groups = await query.ToListAsync();

            if (groups.Count > 0)
            {
                response.message = "Group name Already Exist";
                return response;
            }

            var groupSettigns = await _context.GroupSettings.FirstOrDefaultAsync(x => x.GroupSerial == serial);
            var endDate = request.Settings.Begin.AddDays(request.Settings.Timelapse * request.Users.Count());

            group.GroupName = request.Name;
            groupSettigns.GroupSettingsAmount = request.Settings.Amount;
            groupSettigns.GroupSettingsBegin = request.Settings.Begin;
            groupSettigns.GroupSettingsEnd = endDate;
            groupSettigns.GroupSettingsStatus = request.Settings.Status;

            _context.Group.Update(group);
            _context.GroupSettings.Update(groupSettigns);
            await _context.SaveChangesAsync();
            response.message = "Group Updated";

            return response;
        }

        public async Task<StatusResponse> DeleteGroup(string serial)
        {
            var response = new StatusResponse();
            var group = await _context.Group.FirstOrDefaultAsync(x => x.GroupSerial == serial);
            var groupSettings = await _context.GroupSettings.FirstOrDefaultAsync(x => x.GroupSerial == serial);

            if (group != null && groupSettings != null)
            {

                var xs = _context.GroupHasUsers.Where(x => x.GroupSerial == serial).ToList();

                _context.GroupHasUsers.RemoveRange(_context.GroupHasUsers.Where(x => x.GroupSerial == serial));
                _context.GroupSettings.Remove(groupSettings);
                _context.Group.Remove(group);
                await _context.SaveChangesAsync();

                response.message = "Group Deleted";
                return response;
            }

            response.message = "Group Not Found";
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
                UserSerial = request.User_Serial
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
            var users = request.Users.Count();
            var endDate = beginDate.AddDays(request.Settings.Timelapse * users);

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

        private async Task<object> GroupHasUsersRegiter(string serial, Dictionary<int, UserInGroup> users)
        {

            foreach (var user in users)
            {
                var groupHasUsers = new GroupHasUsers
                {
                    GroupSerial = serial,
                    UserSerial = user.Value.Serial,
                    GroupHasUserPosition = user.Key,
                    RoleId = user.Value.Role
                };

                await _context.GroupHasUsers.AddAsync(groupHasUsers);
            }

            await _context.SaveChangesAsync();

            return new StatusResponse { message = "User(s) Added To Group" };

        }

        public async Task<object> GetGroupBySerial(string serial)
        {

            var query = from u in _context.User
                        join g in _context.GroupHasUsers
                        on u.UserSerial equals g.UserSerial
                        join r in _context.Role
                        on g.RoleId equals r.RoleId
                        where g.GroupSerial == serial && u.UserStatus == true && u.UserEnable == true
                        orderby g.GroupHasUserPosition
                        select new
                        {
                            Position = g.GroupHasUserPosition,
                            Serial = u.UserSerial,
                            Name = u.UserName,
                            LastName = u.UserLastname,
                            Role = r.RoleName
                        };

            var users = await query.ToListAsync();
            var usersList = new List<object>();

            if (users.Count > 0)
            {

                foreach (object user in users)
                {
                    usersList.Add(user);
                }

                var groupInfo = new GroupList
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

        private async Task<bool> GroupHasUsersExist(Dictionary<int, UserInGroup> user, string group)
        {
            foreach (var u in user)
            {
                if (await _context.GroupHasUsers.AnyAsync(x => x.UserSerial == u.Value.Serial && x.GroupSerial == group || x.GroupSerial == group && x.GroupHasUserPosition == u.Key))
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
