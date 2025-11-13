using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class LineOvertimeHourRepository : Repository<LineOvertimeHour>, ILineOvertimeHourRepository
    {

        public LineOvertimeHourRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<LineOvertimeHour> GetLineOvertimeHoureByOtDate(DateTime? otDate)
        {
            string sqlQuery = String.Format(@"select * from [dbo].[LineOvertimeHour] where Convert(date,TransactionDate)=convert(date,'{0}') order by DepartmentLineId", otDate);

            return Context.Database.SqlQuery<LineOvertimeHour>(sqlQuery).ToList();
        }
        public DataTable GetOvertimeHoureByOtDate(DateTime? otDate, bool all, bool garments, bool knitting, bool dyeing)
        {
            string sqlQuery = String.Format(@"EXEC SpOTHoureByTransactionDate '{0}','{1}','{2}','{3}','{4}'", otDate, all, garments, knitting, dyeing);
            return ExecuteQuery(sqlQuery);
        }

        public DataTable GetLineWiseEmployeeOTHours(int departmentLineId, DateTime? otDate)
        {
            string sqlQuery = String.Format(@"EXEC SpHrmLineWiseEmployeeOTHours '{0}','{1}'", otDate, departmentLineId);
            return ExecuteQuery(sqlQuery);
        }

        public bool SendOvertimeHour(Guid? prepairedBy, DateTime? otDate)
        {

            var otDateParm = new SqlParameter()
            {
                ParameterName = "OtDate",
                DbType = DbType.DateTime,
                Value = otDate
            };
            var prepairedByParm = new SqlParameter()
            {
                ParameterName = "PrepairedBy",
                DbType = DbType.Guid,
                Value = prepairedBy
            };
            var messageParem = new SqlParameter()
            {
                ParameterName = "Message",
                DbType = DbType.Boolean,
                Value = false,
                Direction = ParameterDirection.Output
            };

            Context.Database.ExecuteSqlCommand("SpSendOovertimeOHourToApproval @OtDate,@PrepairedBy,@Message out", otDateParm, prepairedByParm, messageParem);
            return Convert.ToBoolean(messageParem.Value);
        }
    }
}
