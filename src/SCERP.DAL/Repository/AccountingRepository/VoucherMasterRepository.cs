using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;
using SCERP.Model.AccountingModel;
using System.Data.SqlClient;
using System.Data;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class VoucherMasterRepository : Repository<Acc_VoucherMaster>, IVoucherMasterRepository
    {
        public VoucherMasterRepository(SCERPDBContext context) : base(context)
        {

        }

        public IQueryable<VAccVoucherMaster> GetVoucherList(Expression<Func<VAccVoucherMaster, bool>> predicate)
        {
            var dbRawSqlQuery = Context.VAccVoucherMasters.Where(predicate);
            return dbRawSqlQuery;
        }

        public int SaveVoucherToCostCentre(Acc_VoucherToCostcentre voucherToCostcentre)
        {
            try
            {
                Context.Acc_VoucherToCostcentre.Add(voucherToCostcentre);
                Context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }

        public int DeleteVouchertoCostCentre(Acc_VoucherToCostcentre voucherToCostcentre)
        {
            try
            {
                var temp = from p in Context.Acc_VoucherToCostcentre.Where(p => p.IsActive && p.VoucherNo == voucherToCostcentre.VoucherNo)
                           select p;

                foreach (var t in temp)
                {
                    t.IsActive = false;
                }

                Context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }

        public IQueryable<Acc_VoucherToCostcentre> GetVoucherToCostCentre(long Id)
        {
            long voucherNo = 0;
            var accVoucherToCostcentre = Context.Acc_VoucherMaster.FirstOrDefault(p => p.Id == Id);

            if (accVoucherToCostcentre != null)
            {
                voucherNo = accVoucherToCostcentre.VoucherNo;
            }

            return Context.Acc_VoucherToCostcentre.Where(p => p.VoucherNo == voucherNo && p.IsActive).OrderBy(p => p.Id);
        }

        public string GetAccountNameByCode(decimal accountCode)
        {
            var accGlAccounts = Context.Acc_GLAccounts.FirstOrDefault(p => p.AccountCode == accountCode);
            if (accGlAccounts != null) return accGlAccounts.AccountName;
            return "Not found !";
        }

        public string GetVoucherNoByType(string type, DateTime voucherDate)
        {
            var voucherNo = "";

            Acc_FinancialPeriod period = Context.Acc_FinancialPeriod.SingleOrDefault(p => p.PeriodStartDate <= voucherDate.Date && p.PeriodEndDate >= voucherDate.Date);

            var temp = Context.Acc_VoucherMaster.Where(p => p.VoucherType == type && p.VoucherDate >= period.PeriodStartDate && p.VoucherDate <= period.PeriodEndDate);

            if (temp.Any())
            {

                int RefNo = 1;

                var connectionString = Context.Database.Connection.ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand("dbo.SPAccountReferenceNo", conn);
                cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter voucherTypeParam = cmd.Parameters.AddWithValue("@VoucherType", type);
                voucherTypeParam.SqlDbType = SqlDbType.NVarChar;


                DateTime Date = new DateTime(1900, 01, 01);
                Date = voucherDate;
                SqlParameter dateParam = cmd.Parameters.AddWithValue("@Date", Date);
                dateParam.SqlDbType = SqlDbType.DateTime;


                SqlParameter actionTypeParam = cmd.Parameters.AddWithValue("@ActionType", "GET");
                actionTypeParam.SqlDbType = SqlDbType.NVarChar;

                conn.Open();
                cmd.CommandTimeout = 36000;
                RefNo = Convert.ToInt16(cmd.ExecuteScalar());
                conn.Close();


                if (period != null) voucherNo = type + "-" + RefNo + "/" + period.PeriodName;
            }
            else
            {
                voucherNo = type + "-" + 1 + "/" + period.PeriodName;
            }
            return voucherNo;
        }

        public string GetCurrencyById(int id)
        {
            var result = Context.Acc_Currency.SingleOrDefault(p => p.ActiveStatus == true);

            if (result != null)
            {
                if (id == 1)
                    return result.FirstCurValue.ToString(CultureInfo.InvariantCulture);
                if (id == 2)
                    return result.SecendCurValue.ToString(CultureInfo.InvariantCulture);
                if (id == 3)
                    return result.ThirdCurValue.ToString(CultureInfo.InvariantCulture);
            }
            return "1";
        }



        public int ChangeGlHeadGroup(string sectorId, string glId, string glIdNew)
        {
            try
            {
                int glCode = 0;
                int glCodeNew = 0;

                int? sector = Convert.ToInt32(!string.IsNullOrEmpty(sectorId));

                if (!string.IsNullOrEmpty(glId))
                {
                    decimal gl = Convert.ToDecimal(glId);

                    var accGlAccounts = Context.Acc_GLAccounts.FirstOrDefault(p => p.AccountCode == gl);

                    if (accGlAccounts != null)
                    {
                        glCode = accGlAccounts.Id;
                    }
                }

                if (!string.IsNullOrEmpty(glIdNew))
                {
                    decimal glNew = Convert.ToDecimal(glIdNew);

                    var accGlAccountsNew = Context.Acc_GLAccounts.FirstOrDefault(p => p.AccountCode == glNew);

                    if (accGlAccountsNew != null)
                    {
                        glCodeNew = accGlAccountsNew.Id;
                    }
                }

                var temp = from p in Context.Acc_VoucherMaster
                           join q in Context.Acc_VoucherDetail on p.Id equals q.RefId
                           where p.SectorId == sector && q.GLID == glCode
                           select q;

                foreach (var t in temp)
                {
                    t.GLID = glCodeNew;
                }

                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return 1;
        }


        public int ChangeGlHeadByParent(string sectorId, string glHead, string glHeadParent)
        {
            string glCode = glHead.Substring(glHead.Length - 10, 10);
            string glName = glHead.Substring(0, glHead.Length - 12);
            string groupCode = glHeadParent.Substring(glHeadParent.Length - 7, 7);



            return 1;
        }



        public Acc_Currency GetCurrency()
        {
            return Context.Acc_Currency.FirstOrDefault();
        }

        public decimal GetConversionValueByVoucherId(long id)
        {
            var activeCurrencyId = 0;
            var activeCurrencyValue = 1;

            var accVoucherMaster = Context.Acc_VoucherMaster.SingleOrDefault(p => p.Id == id);
            if (accVoucherMaster != null)
            {
                activeCurrencyId = (int)accVoucherMaster.ActiveCurrencyId;
            }

            var voucherdetail = from p in Context.Acc_VoucherDetail
                                join q in Context.Acc_VoucherMaster on p.RefId equals q.Id
                                where q.Id == id
                                select p;

            foreach (var t in voucherdetail)
            {
                if (activeCurrencyId == 1)
                    return t.FirstCurValue;
                else if (activeCurrencyId == 2)
                    return t.SecendCurValue;
                else if (activeCurrencyId == 3)
                    return t.ThirdCurValue;
            }
            return activeCurrencyValue;
        }

        public int? GetCostCentreByEmployeeId(Guid? employeeId)
        {
            var accActiveCompanySector = Context.Acc_ActiveCompanySector.FirstOrDefault(p => p.EmployeeId == employeeId);
            if (accActiveCompanySector != null) return accActiveCompanySector.CompanyId;
            else
                return 0;
        }

        public int IncreaseRefno(string type, DateTime voucherDate)
        {
            int RefNo = 1;

            var connectionString = Context.Database.Connection.ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("dbo.SPAccountReferenceNo", conn);
            cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter voucherTypeParam = cmd.Parameters.AddWithValue("@VoucherType", type);
            voucherTypeParam.SqlDbType = SqlDbType.NVarChar;


            DateTime Date = new DateTime(1900, 01, 01);
            Date = voucherDate;
            SqlParameter dateParam = cmd.Parameters.AddWithValue("@Date", Date);
            dateParam.SqlDbType = SqlDbType.DateTime;


            SqlParameter actionTypeParam = cmd.Parameters.AddWithValue("@ActionType", "SAVE");
            actionTypeParam.SqlDbType = SqlDbType.NVarChar;

            conn.Open();
            cmd.CommandTimeout = 36000;
            RefNo = Convert.ToInt16(cmd.ExecuteScalar());
            conn.Close();
            return 1;
        }
    }
}