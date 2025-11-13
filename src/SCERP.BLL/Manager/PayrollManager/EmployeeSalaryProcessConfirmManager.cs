using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.BLL.Manager.AccountingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.DAL.Repository.AccountingRepository;
using SCERP.DAL.Repository.PayrollRepository;
using SCERP.Model;
using System.Globalization;

namespace SCERP.BLL.Manager.PayrollManager
{
    public class EmployeeSalaryProcessConfirmManager : BaseManager, IEmployeeSalaryProcessConfirmManager
    {
        protected readonly IEmployeeSalaryProcessConfirmRepository EmployeeSalaryConfirmProcessRepository = null;

        public EmployeeSalaryProcessConfirmManager(SCERPDBContext context)
        {
            EmployeeSalaryConfirmProcessRepository = new EmployeeSalaryProcessConfirmRepository(context);
        }

        public string ConfirmSalary(Acc_VoucherMaster vmMaster)
        {
            var empsal = EmployeeSalaryConfirmProcessRepository.GetEmployeeSalaryTemp();
            decimal? totalGross = 0;
            decimal? totalNetSalary = 0;

            try
            {
                foreach (var t in empsal)
                {
                    var esp = new EmployeeSalary_Processed
                    {
                        Id = 0,
                        EmployeeId = t.EmployeeId,
                        //Month = t.Month,
                        //Year = t.Year,
                        GrossSalary = t.GrossSalary,
                        BasicSalary = t.BasicSalary,
                        HouseRent = t.HouseRent,
                        MedicalAllowance = t.MedicalAllowance,
                        Conveyance = t.Conveyance,
                        FoodAllowance = t.FoodAllowance,
                        Tax = t.Tax,
                        ProvidentFund = t.ProvidentFund,
                        NetSalaryPaid = t.NetSalaryPaid,
                        CreatedDate = DateTime.Now,
                        CreatedBy = t.CreatedBy,
                        IsActive = true,
                    };

                    totalGross += t.GrossSalary;
                    totalNetSalary += t.NetSalaryPaid;

                    EmployeeSalaryConfirmProcessRepository.Add(esp);
                }
            
                string str = EmployeeSalaryConfirmProcessRepository.SaveMaster(vmMaster);
                long RefId = 0;
                long.TryParse(str, out RefId);

                Acc_VoucherDetail[] voucherDetail = new Acc_VoucherDetail[2];

                voucherDetail[0] = new Acc_VoucherDetail(); // Bank Head
                voucherDetail[0].Id = 0;
                voucherDetail[0].RefId = RefId;
                voucherDetail[0].GLID = 291;
                voucherDetail[0].Particulars = "test particulars";
                voucherDetail[0].Debit = 0;
                voucherDetail[0].Credit = totalNetSalary;

                voucherDetail[1]= new Acc_VoucherDetail(); // Payable Salary
                voucherDetail[1].Id = 0;
                voucherDetail[1].RefId = RefId;
                voucherDetail[1].GLID = 369;
                voucherDetail[1].Particulars = "test particulars";
                voucherDetail[1].Debit = totalNetSalary;
                voucherDetail[1].Credit = 0;
            
                for (int i = 0; i < voucherDetail.Count(); i++)
                {
                    EmployeeSalaryConfirmProcessRepository.SaveDetail(voucherDetail[i]);
                }

                EmployeeSalaryConfirmProcessRepository.TruncateTempTable();
                return "Salary Processed Successfully !";
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return exception.Message;
            }
        }
    }
}
