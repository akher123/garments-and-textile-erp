using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using System.Data.SqlClient;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class MrcReportRepository : Repository<Employee>, IMrcReportRepository
    {

        private readonly SCERPDBContext _context;
        private SqlConnection _connection;

        public MrcReportRepository(SCERPDBContext context)
            : base(context)
        {
            this._context = context;
            _connection = (SqlConnection)_context.Database.Connection;
        }

        public List<SpecSheetModel> GetSpecSheetDetail(int id)
        {
            List<SpecSheetModel> specSheet;

            try
            {
                specSheet = _context.Database.
                    SqlQuery<SpecSheetModel>("SPSpecSheetDetailReport @SpecSheetId", new SqlParameter("SpecSheetId", id)).ToList();
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return specSheet;
        }

        public List<SpecSheetModel> GetSpecSheetList(int? buyerId, string styleNo, string jobNo, DateTime? fromDate, DateTime? toDate)
        {
            List<SpecSheetModel> specSheet;

            try
            {
                specSheet = _context.Database.SqlQuery<SpecSheetModel>("SPSpecSheetListReport @BuyerId, @StyleNo, @JobNo, @FromDate, @ToDate", new SqlParameter("BuyerId", buyerId), new SqlParameter("StyleNo", styleNo), new SqlParameter("JobNo", jobNo), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate)).ToList();
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return specSheet;
        }
    }
}
