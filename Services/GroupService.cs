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
            var exist = await GroupHasUsersExist(request);
            var response = new StatusResponse();

            if (exist)
            {
                response.message = "User Already Has Group";
            }
            else
            {
                await GroupHasUsersRegiter(request);
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

            var groupHasUsersRequest = new GroupHasUsersRequest
            {
                Group = serial,
                User = request.Creator
            };

            await _context.Group.AddAsync(group);
            await _context.SaveChangesAsync();

            await GroupSettingsRegiter(serial, request);
            await PostGroupHasUsers(groupHasUsersRequest);

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

        private async Task<GroupHasUsers> GroupHasUsersRegiter(GroupHasUsersRequest groupHasUsersRequest)
        {

            var groupHasUsers = new GroupHasUsers
            {
                GroupSerial = groupHasUsersRequest.Group,
                UserSerial = groupHasUsersRequest.User
            };

            await _context.GroupHasUsers.AddAsync(groupHasUsers);
            await _context.SaveChangesAsync();

            return groupHasUsers;

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

        private async Task<bool> GroupHasUsersExist(GroupHasUsersRequest request)
        {
            if (await _context.GroupHasUsers.AnyAsync(x => x.UserSerial == request.User && x.GroupSerial == request.Group))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*
        public async Task<UserInfo> GetGroupBySerial(string serial)
        {

            var user = await _context.Group.FirstOrDefaultAsync(x => x.UserSerial == serial);

            var userInfo = new GroupInfo
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
        */

    }
}
