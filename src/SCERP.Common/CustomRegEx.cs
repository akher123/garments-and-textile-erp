using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common
{
    public sealed class CustomRegEx
    {

        public const string CountryNameValidationExpression = @"^[a-zA-Z\s]+[\&]{0,1}[a-zA-Z\s]+$";

        public const string CountryCodeValidationExpression = @"[\+]{0,1}[0-9\s]+[\-]{0,1}[0-9\s]+[0-9\s]+$";

        public const string CompanyNameValidationExpression = @"^[a-zA-Z0-9\s]+[\-]{0,1}[a-zA-Z0-9\s]+[&]{0,1}[a-zA-Z0-9\s]+[.]{0,1}[\s]{0,1}[(]{0,1}[a-zA-Z0-9\s]*[)]{0,1}";

        public const string ContactPersonDesignationValidationExpression = @"^[a-zA-Z\s]+[\.]{0,1}[a-zA-Z\s]+[.]{0,1}[a-zA-Z\s]+$";

        public const string UnitNameValidationExpression = @"^[a-zA-Z\s]+[\&]{0,1}[a-zA-Z\s]+[(]{0,1}[a-zA-Z\s]+[)]{0,1}[a-zA-Z\s]+$";

        public const string DepartmentNameValidationExpression = @"^[a-zA-Z\s]+[\-]{0,1}[a-zA-Z\s]+[&]{0,1}[a-zA-Z\s]+[(]{0,1}[a-zA-Z\s]+[)]{0,1}";

        public const string SectionNameValidationExpression = @"^[a-zA-Z0-9\s]+[\-]{0,1}[a-zA-Z0-9\s]+[&]{0,1}[a-zA-Z0-9\s]+[(]{0,1}[a-zA-Z0-9\s]+[)]{0,1}";

        public const string DriviningLicenceValidationExpression = @"^([\d])+$";

        public const string PassportNoValidationExpression = @"^([\d])+$";

        public const string LineNameValidationExpression = @"^[a-zA-Z0-9\s]+[\-]{0,1}[a-zA-Z0-9\s]+[&]{0,1}[a-zA-Z0-9\s]+[(]{0,1}[a-zA-Z0-9\s]+[)]{0,1}";

        public const string EmployeeTypeValidationExpression = @"^[a-zA-Z\s]+[\-]{0,1}[a-zA-Z\s]+$";

        public const string EmployeeDesignationValidationExpression = @"^[a-zA-Z\s\.\(\)\-]+$";

        public const string BankNameValidationExpression = @"^([a-zA-Z\s\-\,\.\&])+$";

        public const string AccountNameValidationExpression = @"^([a-zA-Z\s\-\,\.\&])+$";

        public const string SkillSetTitleValidationExpression = @"^[a-zA-Z\[\]\^\$\.\|\?\*\+\(\\~`\!@#%&\-_+={}'""<>:;,\s]+$";

        public const string BenefitTitleValidationExpression = @"^[a-zA-Z\s]+[\(]{0,1}[a-zA-Z\s]+[)]{0,1}[a-zA-Z\s]+$";

        public const string LeaveTypeValidationExpression = @"^[a-zA-Z\s]+[\&]{0,1}[a-zA-Z\s]+[,]{0,1}[a-zA-Z\s]+$";

        public const string VatValidationException = @"^([\d\s\-\(\)])+$";

        public const string WorkShiftNameValidationException = @"^[a-zA-Z\s]+[\,]{0,1}[a-zA-Z\s]+[&]{0,1}[a-zA-Z\s]+$";

        public const string WorkGroupNameValidationException = @"^[a-zA-Z\s\&\,\-]+$";

        public const string ReligionNameValidationException = @"^[a-zA-Z\s]+[\(]{0,1}[a-zA-Z\s]+[)]{0,1}";

        public const string MaritialStatusValidationException = @"^[a-zA-Z\s]+$";

        public const string BranchNameValidationExpression = @"^([a-zA-Z\s\-\,\.\&])+$";

        public const string PersonNameValidationException = @"^[a-zA-Z\s]+[\.]{0,1}[a-zA-Z\s]+[.]{0,1}[a-zA-Z\s]+$";

        public const string MobileNoValidationException = @"^[0-9\,\-\s\+]+$"; 

        public const string PhoneNoValidationException = @"^[0-9\,\-\s\+]+$"; 

        public const string AuthorizationTypeValidationException = @"^[a-zA-Z\s]+[\.]{0,1}[a-zA-Z\s]+[.]{0,1}[a-zA-Z]+$";

        public const string BranchNameValidationException = @"^([a-zA-Z\d\s\-\(\)\,\.\'])+$";

        public const string FullAddressValidationException = @"^([a-zA-Z\d\s\-\(\)\,\.\/\'\&\#])+$";

        public const string PostOfficeValidationException = @"^([a-zA-Z\s\-\(\)])+[\.]{0,1}[a-zA-Z\s\-\(\)]+[.]{0,1}[a-zA-Z\s\-\(\)]+$";

        public const string PostCodeValidationException = @"^([\d\s\-\(\)])+$";

        public const string EmailValidationException = @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$";

        public const string FaxValidationException = @"^((\+)?([\(]?[\s]?[\d][\s]?[\)]?)?[0-9\-\s])+$";

        public const string WebSiteValidationException = @"^(http:\/\/|https:\/\/)?(www.)?([a-zA-Z0-9]+).[a-zA-Z0-9]*.[a-z]{3}.?([a-z]+)?$";

        public const string TinValidationException = @"^([\d\s\-\(\)])+$";

        public const string InstitutionValidationExpression = @"^([a-zA-Z\d\s\-\(\)\,\.\'\&])+$";

        public const string ResultValidationExpression = @"^([a-zA-Z\d\s\-\(\)\.\+\-])+$";

        public const string DistricNameValidationExpression = @"^[a-zA-Z\s]+[\&]{0,1}[a-zA-Z\s]+$";

        public const string EducationLavelValidationExpression = @"^([a-zA-Z\s])+$";

        public const string EmployeeCardIdlValidationExpression = @"^([a-zA-Z\d\s\-\.])+$";

        public const string NationalityValidationExpression = @"^[a-zA-Z\s]+[\&]{0,1}[a-zA-Z\s]+$";

        public const string NationalIdCardValidationExpression = @"^([\d])+$";

        public const string BirthRegiNoValidationExpression = @"^([\d])+$";

        public const string TaxIdentificationNoValidationExpression = @"^([\d])+$";

        public const string NoOfDaysValidationExpression = @"^([\d])+$";

        public const string ResponsibilityValidationExpression = @"^([a-zA-Z\s\-\(\)\&])+$";

        public const string AccountNumberValidationExpression = @"^([\d\s\-\(\)\.])+$";

        public const string ExamTitleValidationExpression = @"^([a-zA-Z\d\s\-\(\)\,\.\'\&])+$";

        public const string PassingYearValidationExpression = @"^([\d])+$";



        public const string LocationValidationException = @"^([a-zA-Z\d\s\-\(\)\,\.\/\'\&])+$";
        public const string PurposeValidationException = @"^([a-zA-Z\d\s\-\(\)\,\.\/\'\&])+$";
        public const string PunchCardValidationExpression = @"^([\d\s\,\.])+$";


       

    }
}
