using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMVCContext _context;

        public SalesRecordService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            IQueryable<SalesRecord> _salesRecords = from salesRecord in _context.SalesRecord 
                                                    select salesRecord;

            if (minDate.HasValue)
            {
                _salesRecords = _salesRecords.Where(saleRecord => saleRecord.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                _salesRecords = _salesRecords.Where(saleRecord => saleRecord.Date <= maxDate.Value);
            }

            return await _salesRecords.Include(include => include.Seller)
                                      .Include(include => include.Seller.Department)
                                      .OrderByDescending(saleRecord => saleRecord.Date)
                                      .ToListAsync();
        }
    }
}
