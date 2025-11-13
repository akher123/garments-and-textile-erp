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
    public class SalaryIncrementRepository : Repository<Employee>, ISalaryIncrementRepository
    {
        private readonly SqlConnection _connection;
        private readonly SCERPDBContext _context;

        public SalaryIncrementRepository(SCERPDBContext context)
            : base(context)
        {
            _context = context;
            _connection = (SqlConnection) _context.Database.Connection;
        }

        public DataTable GetSalaryIncrementInfo(DateTime fromDate, DateTime toDate, string employeeId, string userName)
        {
            var table = new DataTable();
            const string cmdText = @"SPSalaryIncrementLetter";
            SqlCommand command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@FromDate", fromDate);
            command.Parameters.AddWithValue("@ToDate", toDate);
            command.Parameters.AddWithValue("@EmployeeCardId", employeeId);
            command.Parameters.AddWithValue("@UserName", userName);

            command.CommandType = CommandType.StoredProcedure;

            if (_connection != null && _connection.State == ConnectionState.Closed)
            {
                _connection.Open();
                var adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                if (_connection != null && _connection.State == ConnectionState.Open) _connection.Close();
            }

            return table;
        }
    }
}
