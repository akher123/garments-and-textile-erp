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
    public class EmployeeAppointmentManager : BaseManager, IEmployeeAppointmentManager
    {

        private readonly IEmployeeAppointmentRepository _employeeAppointmentRepository = null;

        public EmployeeAppointmentManager(SCERPDBContext context)
        {
            _employeeAppointmentRepository = new EmployeeAppointmentRepository(context);
        }

        public string GetEmployeeAppointmentInfo(Guid employeeId, string userName, DateTime prepareDate)
        {
            DataRow emp;
            DataTable table = _employeeAppointmentRepository.GetEmployeeAppointmentInfo(employeeId, userName, prepareDate);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            string DateToday = DateTime.Now.ToString("dd/MM/yyyy"); // Replace with date input from controller
            DateTime joinDate = emp.Field<DateTime>("JoinDateCalculation");

            //string ProbationEndDate = joinDate.AddMonths(3).ToString("dd/MM/yyyy");

            DateTime confirmationDate = emp.Field<DateTime>("ConfirmationDate");
            string ProbationEndDate = confirmationDate.AddDays(-1).ToString("dd/MM/yyyy");

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/AppointmentLetter.xml"));

            var appointmentLetterBuilder = new StringBuilder();
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{DATE_TODAY}", BanglaConversion.ConvertToBanglaNumber(DateToday));
                appointmentLetterBuilder.Replace("{PREPARE_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PrepareDate")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{EMP_FATHER_NAME}", emp.Field<string>("FathersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_MOTHER_NAME}", emp.Field<string>("MothersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_CARDNO}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_GRADE}", emp.Field<string>("Grade"));
                appointmentLetterBuilder.Replace("{EMP_SPOUSE_NAME}", emp.Field<string>("SpousesNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_PRE_ADDRESS}", emp.Field<string>("PreMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POST_OFFICE}", emp.Field<string>("PrePostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POLICE_STATION}", emp.Field<string>("PrePolice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_DISTRICT}", emp.Field<string>("PreDist"));
                appointmentLetterBuilder.Replace("{EMP_PER_ADDRESS}", emp.Field<string>("PerMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PER_POST_OFFICE}", emp.Field<string>("PerPostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PER_POLICE_STATION}", emp.Field<string>("PerPolice"));
                appointmentLetterBuilder.Replace("{EMP_PER_DISTRICT}", emp.Field<string>("PerDist"));
                appointmentLetterBuilder.Replace("{PROBATION_END_DATE}", BanglaConversion.ConvertToBanglaNumber(ProbationEndDate));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{HOUSE_RENT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("HouseRent").ToString()));
                appointmentLetterBuilder.Replace("{MEDICAL_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("MedicalAllowance").ToString()));
                appointmentLetterBuilder.Replace("{FOOD_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("FoodAllowance").ToString()));
                appointmentLetterBuilder.Replace("{TRANSPORT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Conveyance").ToString()));
                appointmentLetterBuilder.Replace("{WeekEnd}", BanglaConversion.ConvertToBanglaNumber(emp.Field<int>("Weekend").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{EMPLOYEE_OT_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EmployeeOTRate").ToString()));
                appointmentLetterBuilder.Replace("{AMOUNT_IN_WORDS}", emp.Field<string>("AmountInWords"));
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public string GetEmployeeAppointmentInfoNew(Guid employeeId, string userName, DateTime prepareDate)
        {
            DataRow emp;
            DataTable table = _employeeAppointmentRepository.GetEmployeeAppointmentInfoNew(employeeId, userName, prepareDate);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            string DateToday = DateTime.Now.ToString("dd/MM/yyyy"); // Replace with date input from controller
            DateTime joinDate = emp.Field<DateTime>("JoinDateCalculation");

            //string ProbationEndDate = joinDate.AddMonths(3).ToString("dd/MM/yyyy");

            DateTime confirmationDate = emp.Field<DateTime>("ConfirmationDate");
            string ProbationEndDate = confirmationDate.AddDays(-1).ToString("dd/MM/yyyy");

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/AppointmentLetterNew.xml"));

            var appointmentLetterBuilder = new StringBuilder();
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{DATE_TODAY}", BanglaConversion.ConvertToBanglaNumber(DateToday));
                appointmentLetterBuilder.Replace("{PREPARE_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PrepareDate")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{EMP_FATHER_NAME}", emp.Field<string>("FathersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_MOTHER_NAME}", emp.Field<string>("MothersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_CARDNO}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_GRADE}", emp.Field<string>("Grade"));
                appointmentLetterBuilder.Replace("{EMP_SPOUSE_NAME}", emp.Field<string>("SpousesNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_PRE_ADDRESS}", emp.Field<string>("PreMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POST_OFFICE}", emp.Field<string>("PrePostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POLICE_STATION}", emp.Field<string>("PrePolice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_DISTRICT}", emp.Field<string>("PreDist"));
                appointmentLetterBuilder.Replace("{EMP_PER_ADDRESS}", emp.Field<string>("PerMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PER_POST_OFFICE}", emp.Field<string>("PerPostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PER_POLICE_STATION}", emp.Field<string>("PerPolice"));
                appointmentLetterBuilder.Replace("{EMP_PER_DISTRICT}", emp.Field<string>("PerDist"));
                appointmentLetterBuilder.Replace("{PROBATION_END_DATE}", BanglaConversion.ConvertToBanglaNumber(ProbationEndDate));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{HOUSE_RENT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("HouseRent").ToString()));
                appointmentLetterBuilder.Replace("{MEDICAL_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("MedicalAllowance").ToString()));
                appointmentLetterBuilder.Replace("{FOOD_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("FoodAllowance").ToString()));
                appointmentLetterBuilder.Replace("{TRANSPORT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Conveyance").ToString()));
                appointmentLetterBuilder.Replace("{WeekEnd}", BanglaConversion.ConvertToBanglaNumber(emp.Field<int>("Weekend").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{EMPLOYEE_OT_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EmployeeOTRate").ToString()));
                appointmentLetterBuilder.Replace("{AMOUNT_IN_WORDS}", emp.Field<string>("AmountInWords"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_SKILL_TYPE}", emp.Field<string>("SkillType"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_PHOTOGRAPH_PATH}", emp.Field<string>("PhotographPath"));
                appointmentLetterBuilder.Replace("{APPLICATION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));

                if (emp.Field<string>("EmployeeType") == "Team Member-A")
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৩ (তিন)");
                }
                else
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৬ (ছয়)");
                }
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public string GetEmployeeAppointmentInfoStaffNew(Guid employeeId, string userName, DateTime prepareDate)
        {
            DataRow emp;
            DataTable table = _employeeAppointmentRepository.GetEmployeeAppointmentInfoNew(employeeId, userName, prepareDate);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            string DateToday = DateTime.Now.ToString("dd/MM/yyyy"); // Replace with date input from controller
            DateTime joinDate = emp.Field<DateTime>("JoinDateCalculation");

            //string ProbationEndDate = joinDate.AddMonths(3).ToString("dd/MM/yyyy");

            DateTime confirmationDate = emp.Field<DateTime>("ConfirmationDate");
            string ProbationEndDate = confirmationDate.AddDays(-1).ToString("dd/MM/yyyy");

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/AppointmentLetterStaffNew.xml"));

            var appointmentLetterBuilder = new StringBuilder();
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{DATE_TODAY}", BanglaConversion.ConvertToBanglaNumber(DateToday));
                appointmentLetterBuilder.Replace("{PREPARE_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PrepareDate")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{EMP_FATHER_NAME}", emp.Field<string>("FathersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_MOTHER_NAME}", emp.Field<string>("MothersNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_CARDNO}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_GRADE}", emp.Field<string>("Grade"));
                appointmentLetterBuilder.Replace("{EMP_SPOUSE_NAME}", emp.Field<string>("SpousesNameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_PRE_ADDRESS}", emp.Field<string>("PreMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POST_OFFICE}", emp.Field<string>("PrePostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_POLICE_STATION}", emp.Field<string>("PrePolice"));
                appointmentLetterBuilder.Replace("{EMP_PRE_DISTRICT}", emp.Field<string>("PreDist"));
                appointmentLetterBuilder.Replace("{EMP_PER_ADDRESS}", emp.Field<string>("PerMailingAddress"));
                appointmentLetterBuilder.Replace("{EMP_PER_POST_OFFICE}", emp.Field<string>("PerPostOffice"));
                appointmentLetterBuilder.Replace("{EMP_PER_POLICE_STATION}", emp.Field<string>("PerPolice"));
                appointmentLetterBuilder.Replace("{EMP_PER_DISTRICT}", emp.Field<string>("PerDist"));
                appointmentLetterBuilder.Replace("{PROBATION_END_DATE}", BanglaConversion.ConvertToBanglaNumber(ProbationEndDate));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{HOUSE_RENT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("HouseRent").ToString()));
                appointmentLetterBuilder.Replace("{MEDICAL_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("MedicalAllowance").ToString()));
                appointmentLetterBuilder.Replace("{FOOD_ALLOWANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("FoodAllowance").ToString()));
                appointmentLetterBuilder.Replace("{TRANSPORT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Conveyance").ToString()));
                appointmentLetterBuilder.Replace("{ENTERTAINMENT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EntertainmentAllowance").ToString()));
                appointmentLetterBuilder.Replace("{WeekEnd}", BanglaConversion.ConvertToBanglaNumber(emp.Field<int>("Weekend").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{EMPLOYEE_OT_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EmployeeOTRate").ToString()));
                appointmentLetterBuilder.Replace("{AMOUNT_IN_WORDS}", emp.Field<string>("AmountInWords"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_SKILL_TYPE}", emp.Field<string>("SkillType"));
                appointmentLetterBuilder.Replace("{EMPLOYEE_PHOTOGRAPH_PATH}", emp.Field<string>("PhotographPath"));
                appointmentLetterBuilder.Replace("{APPLICATION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));

                if (emp.Field<string>("EmployeeType") == "Team Member-A")
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৩ (তিন)");
                }
                else
                {
                    appointmentLetterBuilder.Replace("{MONTH}", "০৬ (ছয়)");
                }
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public string GetFinalSettlementInfo(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction)
        {
            DataRow emp;
            var appointmentLetterBuilder = new StringBuilder();
            DataTable table = _employeeAppointmentRepository.GetFinalSettlementInfo(employeeId, userName, prepareDate, othersDeduction);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/HRM/FinalSettlement.xml"));           
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{COMPANY_NAME}", emp.Field<string>("CompanyName"));
                appointmentLetterBuilder.Replace("{COMPANY_ADDRESS}", emp.Field<string>("CompanyAddress"));
                appointmentLetterBuilder.Replace("{MONTH}", BanglaConversion.ConvertEnglishMonthtoBanglaMonth(emp.Field<string>("Month")));
                appointmentLetterBuilder.Replace("{YEAR}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("Year")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_CARDID}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{QUIT_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("QuitDate")));
                appointmentLetterBuilder.Replace("{SERVICE_YEAR}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[0]));
                appointmentLetterBuilder.Replace("{SERVICE_MONTH}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[1]));
                appointmentLetterBuilder.Replace("{SERVICE_DAY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[2]));

                appointmentLetterBuilder.Replace("{EARN_LEAVE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal>("EarnLeave").ToString()));
                appointmentLetterBuilder.Replace("{DAILY_GROSS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("DailyGrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{SERVICE_BENEFIT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("ServiceBenefit").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_WORKDAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("TotalWorkingDays").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_OTHOURS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalOTHours").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_PAYDAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("TotalPayDays").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{DAILY_BASIC}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("DailyBasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{ABSENT_DAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("AbsentDays").ToString()));
                appointmentLetterBuilder.Replace("{BENEFIT_GIVEN}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BenefitGiven").ToString()));
                appointmentLetterBuilder.Replace("{EARN_LEAVE_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EarnLeaveAmount").ToString()));
                appointmentLetterBuilder.Replace("{SERVICE_BENEFIT_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("ServiceBenefitAmount").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_PAID_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalAmountPaid").ToString()));
                appointmentLetterBuilder.Replace("{ATTENDANCE_BONUS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("AttendanceBonus").ToString()));
                appointmentLetterBuilder.Replace("{ABSENT_FEE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("AbsentFee").ToString()));
                appointmentLetterBuilder.Replace("{ADVANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Advance").ToString()));
                appointmentLetterBuilder.Replace("{OTHER_DEDUCTION}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OtherDeduction").ToString()));
                appointmentLetterBuilder.Replace("{STAMP_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("StampAmount").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_DEDUCTION}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalDeduction").ToString()));
                appointmentLetterBuilder.Replace("{NET_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32>("NetAmount").ToString()));
                appointmentLetterBuilder.Replace("{NET_AMOUNT_INWORD}", emp.Field<string>("NetAmountInWord"));
                appointmentLetterBuilder.Replace("{PREPARARION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PreparationDate")));
                
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public string GetFinalSettlementInfo08PM(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction)
        {
            DataRow emp;
            var appointmentLetterBuilder = new StringBuilder();
            DataTable table = _employeeAppointmentRepository.GetFinalSettlementInfo08PM(employeeId, userName, prepareDate, othersDeduction);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/HRM/FinalSettlement.xml"));
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{COMPANY_NAME}", emp.Field<string>("CompanyName"));
                appointmentLetterBuilder.Replace("{COMPANY_ADDRESS}", emp.Field<string>("CompanyAddress"));
                appointmentLetterBuilder.Replace("{MONTH}", BanglaConversion.ConvertEnglishMonthtoBanglaMonth(emp.Field<string>("Month")));
                appointmentLetterBuilder.Replace("{YEAR}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("Year")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_CARDID}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{QUIT_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("QuitDate")));
                appointmentLetterBuilder.Replace("{SERVICE_YEAR}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[0]));
                appointmentLetterBuilder.Replace("{SERVICE_MONTH}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[1]));
                appointmentLetterBuilder.Replace("{SERVICE_DAY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[2]));
                appointmentLetterBuilder.Replace("{EARN_LEAVE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal>("EarnLeave").ToString()));
                appointmentLetterBuilder.Replace("{DAILY_GROSS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("DailyGrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{SERVICE_BENEFIT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("ServiceBenefit").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_WORKDAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("TotalWorkingDays").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_OTHOURS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalOTHours").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_PAYDAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("TotalPayDays").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{DAILY_BASIC}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("DailyBasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{ABSENT_DAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("AbsentDays").ToString()));
                appointmentLetterBuilder.Replace("{BENEFIT_GIVEN}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BenefitGiven").ToString()));
                appointmentLetterBuilder.Replace("{EARN_LEAVE_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EarnLeaveAmount").ToString()));
                appointmentLetterBuilder.Replace("{SERVICE_BENEFIT_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("ServiceBenefitAmount").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_PAID_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalAmountPaid").ToString()));
                appointmentLetterBuilder.Replace("{ATTENDANCE_BONUS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("AttendanceBonus").ToString()));
                appointmentLetterBuilder.Replace("{ABSENT_FEE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("AbsentFee").ToString()));
                appointmentLetterBuilder.Replace("{ADVANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Advance").ToString()));
                appointmentLetterBuilder.Replace("{OTHER_DEDUCTION}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OtherDeduction").ToString()));
                appointmentLetterBuilder.Replace("{STAMP_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("StampAmount").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_DEDUCTION}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalDeduction").ToString()));
                appointmentLetterBuilder.Replace("{NET_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32>("NetAmount").ToString()));
                appointmentLetterBuilder.Replace("{NET_AMOUNT_INWORD}", emp.Field<string>("NetAmountInWord"));
                appointmentLetterBuilder.Replace("{PREPARARION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PreparationDate")));
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public string GetFinalSettlementInfo10PMNoWeekend(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction)
        {
            DataRow emp;
            var appointmentLetterBuilder = new StringBuilder();
            DataTable table = _employeeAppointmentRepository.GetFinalSettlementInfo10PMNoWeekend(employeeId, userName, prepareDate, othersDeduction);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/HRM/FinalSettlement.xml"));
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{COMPANY_NAME}", emp.Field<string>("CompanyName"));
                appointmentLetterBuilder.Replace("{COMPANY_ADDRESS}", emp.Field<string>("CompanyAddress"));
                appointmentLetterBuilder.Replace("{MONTH}", BanglaConversion.ConvertEnglishMonthtoBanglaMonth(emp.Field<string>("Month")));
                appointmentLetterBuilder.Replace("{YEAR}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("Year")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_CARDID}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{QUIT_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("QuitDate")));
                appointmentLetterBuilder.Replace("{SERVICE_YEAR}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[0]));
                appointmentLetterBuilder.Replace("{SERVICE_MONTH}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[1]));
                appointmentLetterBuilder.Replace("{SERVICE_DAY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[2]));
                appointmentLetterBuilder.Replace("{EARN_LEAVE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal>("EarnLeave").ToString()));
                appointmentLetterBuilder.Replace("{DAILY_GROSS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("DailyGrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{SERVICE_BENEFIT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("ServiceBenefit").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_WORKDAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("TotalWorkingDays").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_OTHOURS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalOTHours").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_PAYDAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("TotalPayDays").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{DAILY_BASIC}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("DailyBasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{ABSENT_DAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("AbsentDays").ToString()));
                appointmentLetterBuilder.Replace("{BENEFIT_GIVEN}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BenefitGiven").ToString()));
                appointmentLetterBuilder.Replace("{EARN_LEAVE_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EarnLeaveAmount").ToString()));
                appointmentLetterBuilder.Replace("{SERVICE_BENEFIT_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("ServiceBenefitAmount").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_PAID_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalAmountPaid").ToString()));
                appointmentLetterBuilder.Replace("{ATTENDANCE_BONUS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("AttendanceBonus").ToString()));
                appointmentLetterBuilder.Replace("{ABSENT_FEE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("AbsentFee").ToString()));
                appointmentLetterBuilder.Replace("{ADVANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Advance").ToString()));
                appointmentLetterBuilder.Replace("{OTHER_DEDUCTION}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OtherDeduction").ToString()));
                appointmentLetterBuilder.Replace("{STAMP_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("StampAmount").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_DEDUCTION}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalDeduction").ToString()));
                appointmentLetterBuilder.Replace("{NET_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32>("NetAmount").ToString()));
                appointmentLetterBuilder.Replace("{NET_AMOUNT_INWORD}", emp.Field<string>("NetAmountInWord"));
                appointmentLetterBuilder.Replace("{PREPARARION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PreparationDate")));
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public string GetFinalSettlementInfo10PM(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction)
        {
            DataRow emp;
            var appointmentLetterBuilder = new StringBuilder();
            DataTable table = _employeeAppointmentRepository.GetFinalSettlementInfo10PM(employeeId, userName, prepareDate, othersDeduction);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/HRM/FinalSettlement.xml"));
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{COMPANY_NAME}", emp.Field<string>("CompanyName"));
                appointmentLetterBuilder.Replace("{COMPANY_ADDRESS}", emp.Field<string>("CompanyAddress"));
                appointmentLetterBuilder.Replace("{MONTH}", BanglaConversion.ConvertEnglishMonthtoBanglaMonth(emp.Field<string>("Month")));
                appointmentLetterBuilder.Replace("{YEAR}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("Year")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_CARDID}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{QUIT_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("QuitDate")));
                appointmentLetterBuilder.Replace("{SERVICE_YEAR}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[0]));
                appointmentLetterBuilder.Replace("{SERVICE_MONTH}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[1]));
                appointmentLetterBuilder.Replace("{SERVICE_DAY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[2]));
                appointmentLetterBuilder.Replace("{EARN_LEAVE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal>("EarnLeave").ToString()));
                appointmentLetterBuilder.Replace("{DAILY_GROSS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("DailyGrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{SERVICE_BENEFIT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("ServiceBenefit").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_WORKDAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("TotalWorkingDays").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_OTHOURS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalOTHours").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_PAYDAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("TotalPayDays").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{DAILY_BASIC}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("DailyBasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{ABSENT_DAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("AbsentDays").ToString()));
                appointmentLetterBuilder.Replace("{BENEFIT_GIVEN}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BenefitGiven").ToString()));
                appointmentLetterBuilder.Replace("{EARN_LEAVE_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EarnLeaveAmount").ToString()));
                appointmentLetterBuilder.Replace("{SERVICE_BENEFIT_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("ServiceBenefitAmount").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_PAID_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalAmountPaid").ToString()));
                appointmentLetterBuilder.Replace("{ATTENDANCE_BONUS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("AttendanceBonus").ToString()));
                appointmentLetterBuilder.Replace("{ABSENT_FEE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("AbsentFee").ToString()));
                appointmentLetterBuilder.Replace("{ADVANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Advance").ToString()));
                appointmentLetterBuilder.Replace("{OTHER_DEDUCTION}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OtherDeduction").ToString()));
                appointmentLetterBuilder.Replace("{STAMP_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("StampAmount").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_DEDUCTION}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalDeduction").ToString()));
                appointmentLetterBuilder.Replace("{NET_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32>("NetAmount").ToString()));
                appointmentLetterBuilder.Replace("{NET_AMOUNT_INWORD}", emp.Field<string>("NetAmountInWord"));
                appointmentLetterBuilder.Replace("{PREPARARION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PreparationDate")));
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }

        public string GetFinalSettlementInfoOriginalNoWeekend(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction)
        {
            DataRow emp;
            var appointmentLetterBuilder = new StringBuilder();
            DataTable table = _employeeAppointmentRepository.GetFinalSettlementInfoOriginalNoWeekend(employeeId, userName, prepareDate, othersDeduction);

            if (table == null) return null;

            if (table.Rows.Count > 0)
                emp = table.Rows[0];
            else
                return null;

            var appointmentInfo = AppointmentLetter.Create(HostingEnvironment.MapPath("~/Content/HRM/FinalSettlement.xml"));
            appointmentLetterBuilder.AppendLine(appointmentInfo.EmployeeSpecificInfo);

            if (emp != null)
            {
                appointmentLetterBuilder.Replace("{COMPANY_NAME}", emp.Field<string>("CompanyName"));
                appointmentLetterBuilder.Replace("{COMPANY_ADDRESS}", emp.Field<string>("CompanyAddress"));
                appointmentLetterBuilder.Replace("{MONTH}", BanglaConversion.ConvertEnglishMonthtoBanglaMonth(emp.Field<string>("Month")));
                appointmentLetterBuilder.Replace("{YEAR}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("Year")));
                appointmentLetterBuilder.Replace("{EMP_NAME}", emp.Field<string>("NameInBengali"));
                appointmentLetterBuilder.Replace("{EMP_CARDID}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("EmployeeCardId")));
                appointmentLetterBuilder.Replace("{EMP_DESIGNATION}", emp.Field<string>("Designation"));
                appointmentLetterBuilder.Replace("{EMP_SECTION}", emp.Field<string>("Section"));
                appointmentLetterBuilder.Replace("{EMP_DEPARTMENT}", emp.Field<string>("Department"));
                appointmentLetterBuilder.Replace("{JOINING_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("JoiningDate")));
                appointmentLetterBuilder.Replace("{QUIT_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("QuitDate")));
                appointmentLetterBuilder.Replace("{SERVICE_YEAR}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[0]));
                appointmentLetterBuilder.Replace("{SERVICE_MONTH}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[1]));
                appointmentLetterBuilder.Replace("{SERVICE_DAY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("ServiceDuration").Split('-')[2]));
                appointmentLetterBuilder.Replace("{EARN_LEAVE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal>("EarnLeave").ToString()));
                appointmentLetterBuilder.Replace("{DAILY_GROSS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("DailyGrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{SERVICE_BENEFIT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("ServiceBenefit").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_WORKDAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("TotalWorkingDays").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_OTHOURS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalOTHours").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_PAYDAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("TotalPayDays").ToString()));
                appointmentLetterBuilder.Replace("{OVERTIME_RATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OverTimeRate").ToString()));
                appointmentLetterBuilder.Replace("{GROSS_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("GrossSalary").ToString()));
                appointmentLetterBuilder.Replace("{DAILY_BASIC}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("DailyBasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{BASIC_SALARY}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BasicSalary").ToString()));
                appointmentLetterBuilder.Replace("{ABSENT_DAYS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32?>("AbsentDays").ToString()));
                appointmentLetterBuilder.Replace("{BENEFIT_GIVEN}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("BenefitGiven").ToString()));
                appointmentLetterBuilder.Replace("{EARN_LEAVE_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("EarnLeaveAmount").ToString()));
                appointmentLetterBuilder.Replace("{SERVICE_BENEFIT_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("ServiceBenefitAmount").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_PAID_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalAmountPaid").ToString()));
                appointmentLetterBuilder.Replace("{ATTENDANCE_BONUS}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("AttendanceBonus").ToString()));
                appointmentLetterBuilder.Replace("{ABSENT_FEE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("AbsentFee").ToString()));
                appointmentLetterBuilder.Replace("{ADVANCE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("Advance").ToString()));
                appointmentLetterBuilder.Replace("{OTHER_DEDUCTION}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("OtherDeduction").ToString()));
                appointmentLetterBuilder.Replace("{STAMP_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("StampAmount").ToString()));
                appointmentLetterBuilder.Replace("{TOTAL_DEDUCTION}", BanglaConversion.ConvertToBanglaNumber(emp.Field<decimal?>("TotalDeduction").ToString()));
                appointmentLetterBuilder.Replace("{NET_AMOUNT}", BanglaConversion.ConvertToBanglaNumber(emp.Field<Int32>("NetAmount").ToString()));
                appointmentLetterBuilder.Replace("{NET_AMOUNT_INWORD}", emp.Field<string>("NetAmountInWord"));
                appointmentLetterBuilder.Replace("{PREPARARION_DATE}", BanglaConversion.ConvertToBanglaNumber(emp.Field<string>("PreparationDate")));
            }
            return appointmentLetterBuilder.Replace("\n", "").Replace("\r", "").ToString().Trim();
        }
    }
}