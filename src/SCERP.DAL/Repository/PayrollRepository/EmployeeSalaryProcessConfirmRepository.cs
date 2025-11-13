using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.PayrollRepository
{
    public class EmployeeSalaryProcessConfirmRepository : Repository<EmployeeSalary_Processed>, IEmployeeSalaryProcessConfirmRepository
    {
        public EmployeeSalaryProcessConfirmRepository(SCERPDBContext context)
            : base(context)
        {
        }


        public IQueryable<EmployeeSalary_Processed_Temp> GetEmployeeSalaryTemp()
        {
            return Context.EmployeeSalary_Processed_Temp.Where(p => p.IsActive == true);
        }

        public string SaveMaster(Acc_VoucherMaster voucherMaster)
        {
            try
            {
                Context.Acc_VoucherMaster.Add(voucherMaster);
                var saved = Context.SaveChanges();

                string temp =
                    Context.Acc_VoucherMaster.Where(p => p.IsActive == true && p.VoucherType == "JV")
                        .Max(p => p.Id)
                        .ToString();

                return temp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string SaveDetail(Acc_VoucherDetail voucherDetail)
        {
            try
            {
                long? Id = 1;

                var temp =
                    Context.Acc_VoucherMaster.Where(
                        p => p.IsActive == true)
                        .OrderByDescending(p => p.Id)
                        .FirstOrDefault();

                if (temp == null)
                    Id = 1;
                else
                    Id = temp.Id;

                voucherDetail.RefId = Id;

                Context.Acc_VoucherDetail.Add(voucherDetail);
                Context.SaveChanges();

                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public int TruncateTempTable()
        {
            return Context.Database.ExecuteSqlCommand("TRUNCATE TABLE [EmployeeSalary_Processed_Temp]");
        }
    }
}
