using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.SqlClient;
using SCERP.Common;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeJobCardRepository : Repository<Employee>, IEmployeeJobCardRepository
    {
        private SqlConnection _connection;
        private readonly SCERPDBContext _context;
        public EmployeeJobCardRepository(SCERPDBContext context)
            : base(context)
        {
            _context = context;
            _connection = (SqlConnection)_context.Database.Connection;
        }

        public DataTable GetEmployeeJobCardInfo(Guid? employeeId,DateTime? startDate, DateTime? endDate)
        {
            var table = new DataTable();
            const string cmdText = @"SPEmployeeJobCard";
            SqlCommand command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@StartDate", endDate);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                if (_connection != null && _connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                table = null;
            }
            finally
            {
                if (_connection != null && _connection.State == ConnectionState.Open) _connection.Close();
            }

            return table;
        }
    }
}
