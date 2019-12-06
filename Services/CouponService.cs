using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Damda_Service.Data;
using Damda_Service.Models;
using Damda_Service.Utils;
using Microsoft.EntityFrameworkCore;

namespace Damda_Service.Services
{
    public class CouponService
    {
        private readonly CouponContext _context;
        private Utilities _utilities;

        public CouponService(
            CouponContext context,
             Utilities utilities
            )
        {
            _context = context;
            _utilities = utilities;
        }

        public async Task<CouponResponse> PostCoupon(CouponRequest request)
        {
            var response = new CouponResponse();

            var coupon = await CouponRegister(request);
            response.Code = coupon.CouponCode;
            response.Message = "Coupon Created";
            response.Status = 1;

            return response;

        }

        private async Task<Coupon> CouponRegister(CouponRequest request)
        {

            DateTime.TryParseExact(request.Start, "yyyy/MM/dd", null, DateTimeStyles.None, out DateTime Start_Date);
            DateTime.TryParseExact(request.End, "yyyy/MM/dd", null, DateTimeStyles.None, out DateTime End_Date);
            var start = DateTime.Parse(request.Start);
            var end = DateTime.Parse(request.End);

            var platforms = string.Join(",", request.Platform);

            var plans = string.Join(",", request.Plan);

            var code = _utilities.GenCouponCode();

            while (await _context.Coupon.AnyAsync(x => x.CouponCode == code))
            {
                code = _utilities.GenCouponCode();
            }

            var coupon = new Coupon
            {
                CouponCode = code,
                CouponStart_Date = start,
                CouponEnd_Date = end,
                CouponType = request.Type,
                CouponPorcent = request.Porcent,
                CouponAmount = request.Amount,
                CouponPlatform = platforms,
                CouponPlan = plans,
                CouponStatus = true
            };

            await _context.Coupon.AddAsync(coupon);
            await _context.SaveChangesAsync();

            return coupon;
        }

        public async Task<List<CouponInfo>> GetAllCoupons()
        {
            var query = from c in _context.Coupon
                        orderby c.CouponStart_Date
                        select new CouponInfo
                        {
                            CouponCode = c.CouponCode,
                            Start_Date = c.CouponStart_Date,
                            End_Date = c.CouponEnd_Date,
                            Type = c.CouponType,
                            Porcent = c.CouponPorcent,
                            Amount = c.CouponAmount,
                            Platform = c.CouponPlatform,
                            Plan = c.CouponPlan,
                            Status = c.CouponStatus
                        };

            var coupons = await query.ToListAsync();

            return coupons;
        }
    }
}
