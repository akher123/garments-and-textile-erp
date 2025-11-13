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
    public class EmployeeAppointmentRepository : Repository<Employee>, IEmployeeAppointmentRepository
    {
        private readonly SqlConnection _connection;
        private readonly SCERPDBContext _context;
        public EmployeeAppointmentRepository(SCERPDBContext context)
            : base(context)
        {
            _context = context;
            _connection = (SqlConnection)_context.Database.Connection;
        }

        public DataTable GetEmployeeAppointmentInfo(Guid employeeId, string userName, DateTime prepareDate)
        {
            var table = new DataTable();
            const string cmdText = @"SPAppointmentLetter";
            SqlCommand command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);

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

        public DataTable GetEmployeeAppointmentInfoNew(Guid employeeId, string userName, DateTime prepareDate)
        {
            var table = new DataTable();
            const string cmdText = @"SPAppointmentLetterNew";
            SqlCommand command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);

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

        public DataTable GetFinalSettlementInfo(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction)
        {
            var table = new DataTable();
            const string cmdText = @"SPFinalSettlement";
            SqlCommand command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);
            command.Parameters.AddWithValue("@OtherDeduction", othersDeduction);
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

        public DataTable GetFinalSettlementInfo08PM(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction)
        {
            var table = new DataTable();
            const string cmdText = @"SPFinalSettlement_08PM";
            SqlCommand command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);
            command.Parameters.AddWithValue("@OtherDeduction", othersDeduction);
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

        public DataTable GetFinalSettlementInfo10PMNoWeekend(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction)
        {
            var table = new DataTable();
            const string cmdText = @"SPFinalSettlement_10PMNoWeekend";
            SqlCommand command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);
            command.Parameters.AddWithValue("@OtherDeduction", othersDeduction);
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

        public DataTable GetFinalSettlementInfo10PM(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction)
        {
            var table = new DataTable();
            const string cmdText = @"SPFinalSettlement_10PM";
            SqlCommand command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);
            command.Parameters.AddWithValue("@OtherDeduction", othersDeduction);
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

        public DataTable GetFinalSettlementInfoOriginalNoWeekend(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction)
        {
            var table = new DataTable();
            const string cmdText = @"SPFinalSettlement_OriginalNoWeekend";
            SqlCommand command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@prepareDate", prepareDate);
            command.Parameters.AddWithValue("@OtherDeduction", othersDeduction);
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