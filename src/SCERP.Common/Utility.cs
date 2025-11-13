using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Remoting.Messaging;


namespace SCERP.Common
{
    public static class PhoneNumbers
    {
        public  static List<string> GetPhoneNumes()
        {
            return new List<string>{"01912657743","01711536346","01720518167","01711536184","01912297684"};
        }
    }


    public static class EmailTemplateRefId
    {
        public const string FABRICSTORE = "001";
        public const string GENERELGETEPASS = "002";
        public const string CHAMICALE_ISSUE = "003";
        public const string PREE_COST_SHEET = "004";
        public const string MANAGEMENT_TNA = "005";
    }

    public static class ColorType
    {
        public const string COLOR = "01";
        public const string LOT = "02";

    }

    public static class ChallanType
    {
        public const string Maintenance = "M";
        public const string Fabric = "F";

    }
    public static class RType
    {
        public const string RECEIVEWITHOUTBOOKIN = "N";
        public const string BOOKING = "B";
        public const string YARNDYED = "D";
        public const string KNITTING_PROGRAMWISEYARNRETURN = "R";
        public const string COLLAR_CUTT_PROGRAMWISEYARNRETURN = "C";
        public const string PI = "P";
    }
    public enum RegisterType
    {
        GENERALREGISTER,
        DYESCHEMICALREGISTER,
        YARNREGISTER,
        GREYFABRIC,
        FINISHFABRIC
    }
    public enum EmblishmentStatus
    {
        Print = 1,
        Embroidery = 2
    }
    public enum ActionType
    {
        YarnIssue = 1,
        YarnDelivery = 2,
        YarnReceive = 3,
        YarnReturn = 6,
        AccessoriesReceive = 4,
        AccessoriesIssue = 5,
    }

    public enum StoreType
    {
        Yarn = 1,
        Acessories = 2,
        Fabric = 3,
        Others = 4
    }

    public enum MaterialIssueType
    {
        GeneralIssue = 1,
        BatchWiseIssue = 2,
        LoanReturn = 3,
        LaonGiven = 4
    }

    public enum TaskStatus
    {
        NotStarted = 1,
        NotAssigned = 2,
        Running = 3,
        Done = 4
    }

    public enum Assignee
    {
        Rabbi = 1,
        Yasir = 2,
        Akher = 3,
        Sayeed = 4
    }

    public enum WorkingDayStatus
    {

        [Description("Working Day")]
        WorkingDay = 1,
        [Description("Holiday")]
        Holiday = 2,

    }

    public enum MachineLogStatus
    {
        Stop = 1,
        Start = 2,
        Running = 3
    }

    public enum CheckOutStatus
    {
        CheckIn = 1,
        CheckOut = 2,

    }

    public enum ItemType
    {

        Compliance = 1,
        NoCompliance = 2

    }

    public enum BatchToping
    {
        General = 1,
        Topping = 2,
        ReDyeing = 3,
        ReWashing = 4
    }

    public enum ConsType
    {
        General = 1,
        ColorSize = 2,
        Color = 3,
        Size = 4
    }

    public enum MaterialReceiveType
    {
        WithSpr = 1,
        WithoutSpr = 2,
        Loan = 3
    }

    public enum ProcessKeyEnum /*DO NOT MODIFY THESE*/
    {
        HRM_Leave = 1,
        INVENTORY_Requisition = 2,
        INVENTORY_Purchase = 3
    }

    public enum GenderEnum
    {
        Male = 1,
        Female = 2,
        Others = 3
    }

    public enum StatusValue
    {
        Active = 1,
        InActive = 2,
    }

    public enum QCPassStatus
    {
        Passed = 1,
        Pass = 0,
        All = 2
    }

    public enum MaritalStatusEnum
    {
        Married = 1,
        Unmarried = 2,
        Complicated = 3
    }

    public enum ReligionEnum
    {
        Muslim = 1,
        Hindu = 2,
        Buddhist = 3,
        Christian = 4,
        Others = 5
    }

    public enum PaymentStatusEnum
    {
        Unpaid = 1,
        PartialPaid = 2,
        FullyPaid = 3
    }

    public static class ProcessKey
    {
        public static string SampleDevelopment = "SAMPLE_DEVELOPMENT";
        public static string SampleSubmission = "SAMPLE_SUBMISSION";
        public static string SampleApproval = "SAMPLE_APPROVAL";
        public static string TrimsAndAccessories = "TRIMS_ACCESSORIES";
        public static string Embellishment = "EMBELLISHMENT";
        public static string LabDipDevelopment = "LABDIP_DEVELOPMENT";
        public static string LabDipSubmission = "LABDIP_SUBMISSION";
        public static string LabDipApproval = "LABDIP_APPROVAL";
    }

    public enum MonthEnum
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public class EmployeePrintData
    {
        public string CompanyName { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string BloodGroup { get; set; }
        public string IdCardNo { get; set; }
        public string BackPageInfo { get; set; }
        public string PdfPath { get; set; }
        public string ImagePath { get; set; }
        public string CompanyLogoUrl { get; set; }
        public string CardBackCaution { get; set; }
        public string CardAddress { get; set; }
        public string EmployeeSignature { get; set; }
        public string AuthorizedSignature { get; set; }
    }

    public static class CompanyInfo
    {
        public static string CompanyAddress = "Plummy Fashions Ltd." + "<br>" +
                                              "Unit -502, Concord Tower" + "<br>" +
                                              "113 Kazi Nazrul Islam Avenue," + "<br>" +
                                              "Dhaka - 1000" + "<br>" +
                                              "Phone : +880 2 8317240, 9346944" + "<br>" +
                                              "Fax : +880 2 9347851" + "<br>" +
                                              "Email: info@plummyfashions.com";

        public static string CompanyAddressforPdf = "Plummy Fashions Ltd." + Environment.NewLine +
                                                    "Unit -502, Concord Tower" + Environment.NewLine +
                                                    "113 Kazi Nazrul Islam Avenue," + Environment.NewLine +
                                                    "Dhaka - 1000" + Environment.NewLine +
                                                    "Phone : +880 2 8317240, 9346944" + Environment.NewLine +
                                                    "Fax : +880 2 9347851" + Environment.NewLine +
                                                    "Email: info@plummyfashions.com";

        public static string CardBackCaution = "This Card should be used by card holder only. If this card is found ownerless, Please return it to the issuing authority. This card is not transferable to anybody.";
    }

    public enum LeaveStatusHR
    {
        Approved = 1,
        Pending = 2,
        Cancel = 3
    }

    public enum LeaveStatusWorker
    {
        Approved = 1
    }

    public enum LeaveRecommendation
    {
        Recommended = 1,
        Pending = 2,
        Cancel = 3
    }

    public enum AuthorizationType /*DO NOT MODIFY THESE*/
    {
        LeaveRecommendation = 1,
        LeaveApproval = 2,
        RequisitionPreparation = 3,
        RequisitionIssue = 4,
        RequisitionApproval = 5,
        RequisitionPurchase = 6,
        PurchaseRequisitionPreparation = 7,
        PurchaseRequisitionApproval = 8
    }

    public enum MerchandisingTypes
    {
        Merchandiser = 1
    }

    public enum ReasonType
    {
        Personal = 1,
        Official = 2
    }

    public enum LanguageType
    {
        Bangla = 1,
        English = 2
    }

    public enum AttendanceStatus
    {
        Present = 1,
        Absent = 2,
        Late = 3,
        Leave = 4,
        OSD = 5
    }

    public enum EmployeeTypeId
    {
        ManagementCommittee = 1,
        Management = 2,
        MiddleManagement = 3,
        TeamMemberA = 4,
        TeamMemberB = 5
    }

    public enum CurrencyType
    {
        BDT = 1,
        EUR = 2,
        USD = 3,
    }

    public enum PrintFormatType
    {
        PDF = 1,
        Excel = 2
    }

    public enum EmployeeCategoryValue
    {
        Regular = 1,
        Quit = 2,
        NewJoining = 3,
        NewJoiningAndQuit = 4
    }

    public enum FeedBackStatus
    {
        Waiting = 1,
        Open = 2,
        Closed = 3
    }

    public enum LcType
    {
        Export_Master = 1,
        GROUP_LC = 2,
    }

    public enum PartialShipment
    {
        Allowed = 1,
        Prohibited = 2
    }

    public enum LcOrderSearch
    {
        Shipment_Date = 1,
        LC_Date = 2,
        Exp_Date = 3
    }

    public enum EmployeeCategory
    {
        REGULAR = 1,
        QUIT = 2,
        NEW_JOINING = 3,
        New_JOINING_AND_QUIT = 4
    }

    public enum EmployeeTypeReport
    {
        TeamMember = 1
       , Staff = 2
    }

    public static class TnaActivityKeyValue
    {
        public const string BULKCUTTING = "BCUT";
        public const string BULKSEWING = "BSW";
        public const string PRINT_EM_SEND ="PES";
        public const string PRINT_EM_RCV = "PER";
        public const string SEWING_FINISHING = "FISH";
        public const string Bulk_dyeing_Sloid_Fabric= "BDY";
        public const string Bulk_Knitting_Solid_Fabric ="KNIT";
        public const string FINISH_FABRICS_DELIVERY = "FFD";

     
    }

}
