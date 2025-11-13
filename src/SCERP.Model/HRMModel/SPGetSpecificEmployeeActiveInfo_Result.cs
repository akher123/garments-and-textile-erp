namespace SCERP.Model
{
    using System;

    public partial class SPGetSpecificEmployeeActiveInfo_Result
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string Name { get; set; }
        public string NameInBengali { get; set; }
        public string MothersName { get; set; }
        public string MothersNameInBengali { get; set; }
        public string FathersName { get; set; }
        public string FathersNameInBengali { get; set; }
        public int ReligionId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte GenderId { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public string PhotographPath { get; set; }
        public string MailingAddress { get; set; }
        public string MailingAddressInBengali { get; set; }
        public string MobilePhone { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public int BranchUnitId { get; set; }
        public int BranchUnitDepartmentId { get; set; }
        public int EmployeeTypeId { get; set; }
        public int EmployeeGradeId { get; set; }
        public int EmployeeDesignationId { get; set; }
        public bool IsEligibleForOvertime { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
