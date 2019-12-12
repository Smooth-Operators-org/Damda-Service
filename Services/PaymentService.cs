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

        public async Task<object> GetUserPaymentList(string userSerial, string groupSerial)
        {
            var user = await _context.GroupHasUsers.FirstOrDefaultAsync(x => x.GroupSerial == groupSerial && x.UserSerial == userSerial);

            if (user != null)
            {
                var query = from p in _context.Payment
                            where p.GroupHasUsersId == user.GroupHasUsersId
                            select new PaymentInfo
                            {
                                Week = p.PaymentWeek,
                                Date = p.PaymentDate.ToString(),
                                Extension = p.PaymentExtension.ToString(),
                                Status = p.PaymentStatus,
                                Amount = p.PaymentAmount,
                                Received = p.PaymentReceived,
                            };

                var payments = await query.ToListAsync();

                var root = new Dictionary<int, PaymentInfo>();

                foreach (var payment in payments)
                {
                    root.Add(payment.Week, payment);
                }

                var settings = await _context.GroupSettings.FirstOrDefaultAsync(x => x.GroupSerial == groupSerial);
                var users = await _context.GroupHasUsers.CountAsync(x => x.GroupSerial == groupSerial);

                var date = settings.GroupSettingsBegin;
                var item = new PaymentList();


                for (var i = 1; i <= users; i++)
                {
                    if (i == user.GroupHasUserPosition || root.ContainsKey(i))
                    {
                        date = date.AddDays(settings.GroupSettingsFrequency);
                        continue;
                    }

                    var payment = new PaymentInfo
                    {
                        Week = i,
                        Date = date.ToString("yyyy-MM-dd"),
                        Extension = date.AddDays(5).ToString("yyyy-MM-dd"),
                        Status = false,
                        Amount = settings.GroupSettingsAmount,
                        Received = 0.0,
                    };
                    root.Add(i, payment);
                    date = date.AddDays(settings.GroupSettingsFrequency);
                }
                item.root = root;
                return item;
            }
            return (new StatusResponse
            {
                message = "User Not Found"
            });

        }

        public async Task<object> PostPayment(PaymentRequest request)
        {
            if (DateTime.Today <= DateTime.Parse(request.Payment_Date))
            {
                var user = await _context.GroupHasUsers.FirstOrDefaultAsync(x => x.GroupSerial == request.Group_Serial && x.UserSerial == request.User_Serial);

                if (user != null)
                {
                    var payment = await _context.Payment.FirstOrDefaultAsync(x => x.GroupHasUsersId == user.GroupHasUsersId && x.PaymentWeek == request.Week);

                    if (payment != null)
                    {
                        await UpdatePayment(request, user, payment);
                    }
                    else
                    {
                        var date = DateTime.Parse(request.Payment_Date);
                        var extension = DateTime.Parse(request.Extension_Date);
                        var payed = DateTime.Parse(request.Payed_Date);
                        var amount = request.Amount;
                        var receivedAmount = request.Received;
                        var status = false;

                        if (payed <= extension && receivedAmount >= amount)
                        {
                            status = true;
                        }

                        var payment = new Payment
                        {
                            PaymentDate = date,
                            PaymentStatus = status,
                            PaymentAmount = amount,
                            PaymentReceived = receivedAmount,
                            PaymentExtension = extension,
                            GroupHasUsersId = user.GroupHasUsersId,
                            PaymentWeek = request.Week
                        };

                        await _context.Payment.AddAsync(payment);
                        await _context.SaveChangesAsync();

                        return (new StatusResponse
                        {
                            message = "Payment Successful"
                        });
                    }
                }
                return (new StatusResponse
                {
                    message = "User Not Found"
                });
            }
            return (new StatusResponse
            {
                message = "Payment expired"
            });
        }
        public async Task<object> UpdatePayment(PaymentRequest request, GroupHasUsers user, Payment payment)
        {

            if (exist == null)
            {

                var date = DateTime.Parse(request.Payment_Date);
                var extension = DateTime.Parse(request.Extension_Date);
                var payed = DateTime.Parse(request.Payed_Date);
                var amount = request.Amount;
                var receivedAmount = request.Received;
                var status = false;

                if (payed <= extension && receivedAmount >= amount)
                {
                    status = true;
                }

                var payment = new Payment
                {
                    PaymentDate = date,
                    PaymentStatus = status,
                    PaymentAmount = amount,
                    PaymentReceived = receivedAmount,
                    PaymentExtension = extension,
                    GroupHasUsersId = user.GroupHasUsersId,
                    PaymentWeek = request.Week
                };

                await _context.Payment.AddAsync(payment);
                await _context.SaveChangesAsync();

                return (new StatusResponse
                {
                    message = "Payment Successful"
                });
            }

            return (new StatusResponse
            {
                message = "User Not Found"
            });
        }
    }
}
