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
    public class PaymentService
    {
        private Utilities _utilities;
        private readonly DataContext _context;
        public PaymentService
            (
            DataContext context,
            Utilities utilities
            )
        {
            _context = context;
            _utilities = utilities;

        }

        public async Task<PaymentList> GetUserPaymentList(string userSerial, string groupSerial)
        {
            var settings = await _context.GroupSettings.FirstOrDefaultAsync(x => x.GroupSerial == groupSerial);
            var user = await _context.GroupHasUsers.FirstOrDefaultAsync(x => x.GroupSerial == groupSerial && x.UserSerial == userSerial);
            var users = await _context.GroupHasUsers.CountAsync(x => x.GroupSerial == groupSerial);

            var date = settings.GroupSettingsBegin;
            var list = new List<PaymentInfo>();
            var item = new PaymentList();


            for (var i = 1; i <= users; i++)
            {
                if (i == user.GroupHasUserPosition)
                {
                    continue;
                }

                var payment = new PaymentInfo
                {
                    Week = i,
                    Date = date.ToString(),
                    Extension = date.AddDays(5).ToString(),
                    Status = false,
                    Amount = settings.GroupSettingsAmount,
                    Received = 0.0,
                };
                list.Add(payment);

                date = date.AddDays(settings.GroupSettingsFrequency);
            }

            item.root = list;

            return item;
        }

    }
}
