using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System.Linq;
using System.Data;
using SCERP.BLL.Process;
using System.Web.Hosting;

namespace SCERP.BLL.Manager.HRMManager
{
    public class SalaryIncrementManager : BaseManager, ISalaryIncrementManager
    {
        private readonly ISalaryIncrementRepository _salaryIncrementRepository = null;

        public SalaryIncrementManager(SCERPDBContext context)
        {
            _salaryIncrementRepository = new SalaryIncrementRepository(context);
        }

        public string GetSalaryIncrementInfo(DateTime fromDate, DateTime toDate, string employeeId, string userName)
        {
            DataRow emp;
            DataTable table = _salaryIncrementRepository.GetSalaryIncrementInfo(fromDate, toDate, employeeId, userName);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            string DateToday = DateTime.Now.ToString("dd/MM/yyyy"); // Replace with date input from controller      

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/SalaryIncrement.xml"));

            var appointmentLetterBuilder = new StringBuilder();
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{DATE_TODAY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("DateToday")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("EmployeeName"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("DepartmentName"));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("DesignationName"));
                appointmentLetterBuilder.Replace("{EMP_CARDNO}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_GRADE}", emp.Field<string>("GradeName"));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{NEW_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("NewGross").ToString()));
                appointmentLetterBuilder.Replace("{NEW_BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("NewBasic").ToString()));
                appointmentLetterBuilder.Replace("{NEW_HOUSE_RENT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("NewHouseRent").ToString()));
                appointmentLetterBuilder.Replace("{MEDICINE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("MedicalAllowance").ToString()));
                appointmentLetterBuilder.Replace("{FOOD}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("FoodAllowance").ToString()));
                appointmentLetterBuilder.Replace("{TRANSPORT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Conveyance").ToString()));
                appointmentLetterBuilder.Replace("{NEW_GROSS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("NewGross").ToString()));
                appointmentLetterBuilder.Replace("{INCREMENT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalIncrement").ToString()));
                appointmentLetterBuilder.Replace("{EFFECTIVE_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("LastIncrementDate")));
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }
    }
}
