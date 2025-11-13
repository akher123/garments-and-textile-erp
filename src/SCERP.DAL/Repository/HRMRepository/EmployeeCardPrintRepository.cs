using System.Data.Entity.Core.Objects;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeCardPrintRepository : Repository<Employee>, IEmployeeCardPrintRepository
    {
        public EmployeeCardPrintRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<EmployeeCardPrintModel> GetEmployeeIDCardInfoInEnglishByPaging(int startPage, int pageSize, out int totalRecords, Employee model, SearchFieldModel searchFieldModel)
        {
            try
            {
                var employeeIDCardInfosInEnglish = GetEmployeeIDCardInfoInEnglishBySearchKey(searchFieldModel);
                totalRecords = employeeIDCardInfosInEnglish.Count();


                switch (model.sort)
                {
                    case "Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeIDCardInfosInEnglish = employeeIDCardInfosInEnglish
                                    .OrderByDescending(r => r.Name);
                                break;
                            default:
                                employeeIDCardInfosInEnglish = employeeIDCardInfosInEnglish
                                    .OrderBy(r => r.Name);
                                break;
                        }
                        break;

                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeIDCardInfosInEnglish = employeeIDCardInfosInEnglish
                                    .OrderByDescending(r => r.EmployeeCardId);
                                break;
                            default:
                                employeeIDCardInfosInEnglish = employeeIDCardInfosInEnglish
                                    .OrderBy(r => r.EmployeeCardId);
                                break;
                        }
                        break;
                }

                var listEmployeeCardPrintModel = new List<EmployeeCardPrintModel>();

                foreach (var emplIDCardInfoInEnglish in employeeIDCardInfosInEnglish)
                {
                    string JoiningDate = "DOJ  : " + emplIDCardInfoInEnglish.JoiningDate.Split('/')[0] + " " +
                                         BanglaConversion.ConvertToEnglishMonth(
                                             emplIDCardInfoInEnglish.JoiningDate.Split('/')[1]) + ", " + emplIDCardInfoInEnglish.JoiningDate.Split('/')[2];

                    var employeeCardPrintModel = new EmployeeCardPrintModel
                    {
                        EmployeeId = emplIDCardInfoInEnglish.EmployeeId,
                        EmployeeCardId = "ID No  : " + emplIDCardInfoInEnglish.EmployeeCardId,
                        Name = "Name  : " + emplIDCardInfoInEnglish.Name,
                        Department = "Dept  : " + emplIDCardInfoInEnglish.Department,
                        Section = "Section  : " + emplIDCardInfoInEnglish.Section,
                        Designation = "Desig  : " + emplIDCardInfoInEnglish.Designation,
                        EmployeeJobType = "Job Type : " + emplIDCardInfoInEnglish.EmployeeJobType,
                        JoiningDate = JoiningDate,
                        PhotographPath = emplIDCardInfoInEnglish.PhotographPath
                    };

                    listEmployeeCardPrintModel.Add(employeeCardPrintModel);
                }

                return listEmployeeCardPrintModel;

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
        }

        public List<EmployeeCardPrintModel> GetEmployeeIDCardInfoInBengaliByPaging(int startPage, int pageSize, out int totalRecords, Employee model, SearchFieldModel searchFieldModel)
        {
            try
            {
                var employeeIDCardInfosInBengali = GetEmployeeIDCardInfoInBengaliBySearchKey(searchFieldModel);
                totalRecords = employeeIDCardInfosInBengali.Count();


                switch (model.sort)
                {
                    case "Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeIDCardInfosInBengali = employeeIDCardInfosInBengali
                                    .OrderByDescending(r => r.Name);
                                break;
                            default:
                                employeeIDCardInfosInBengali = employeeIDCardInfosInBengali
                                    .OrderBy(r => r.Name);
                                break;
                        }
                        break;

                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeIDCardInfosInBengali = employeeIDCardInfosInBengali
                                    .OrderByDescending(r => r.EmployeeCardId);
                                break;
                            default:
                                employeeIDCardInfosInBengali = employeeIDCardInfosInBengali
                                    .OrderBy(r => r.EmployeeCardId);
                                break;
                        }
                        break;
                }

                var listEmployeeCardPrintModel = new List<EmployeeCardPrintModel>();

                foreach (var emplIDCardInfoInBengali in employeeIDCardInfosInBengali)
                {

                    string JoiningDate = "যোগদানের তারিখ  : " + BanglaConversion.ConvertToBanglaNumber(emplIDCardInfoInBengali.JoiningDate.Split('/')[0]) + " " +
                                         BanglaConversion.ConvertToBanglaMonth(
                                             emplIDCardInfoInBengali.JoiningDate.Split('/')[1]) + ", " + BanglaConversion.ConvertToBanglaNumber(emplIDCardInfoInBengali.JoiningDate.Split('/')[2]);

                    var employeeCardPrintModel = new EmployeeCardPrintModel
                    {
                        EmployeeId = emplIDCardInfoInBengali.EmployeeId,
                        EmployeeCardId = "আইডি  : " + BanglaConversion.ConvertToBanglaNumber(emplIDCardInfoInBengali.EmployeeCardId),
                        Name = "নাম  : " + emplIDCardInfoInBengali.Name,
                        Department = "বিভাগ  : " + emplIDCardInfoInBengali.Department,
                        Section = "সেকশন  : " + emplIDCardInfoInBengali.Section,
                        Designation = "পদবি  : " + emplIDCardInfoInBengali.Designation,
                        EmployeeJobType = "কাজের ধরন : " + emplIDCardInfoInBengali.EmployeeJobType,
                        JoiningDate = JoiningDate,
                        PhotographPath = emplIDCardInfoInBengali.PhotographPath
                    };

                    listEmployeeCardPrintModel.Add(employeeCardPrintModel);

                }

                return listEmployeeCardPrintModel;

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
        }

        public List<EmployeeCardInfo> GetCardBackInfo(int companyId, int language, int noofCard)
        {
            EmployeeCardInfo cardBackInfo = null;

            if (language == (int) Enum.Parse(typeof (LanguageType), "Bangla"))
                cardBackInfo = Context.EmployeeCardInfoes.FirstOrDefault(p => p.IsActive == true && p.CompanyId == companyId && p.IsBangla);
            else
                cardBackInfo = Context.EmployeeCardInfoes.FirstOrDefault(p => p.IsActive == true && p.CompanyId == companyId && !p.IsBangla);

            List<EmployeeCardInfo> employeeCardInfos = new List<EmployeeCardInfo>();

            for (int i = 0; i < noofCard; i++)
            {
                employeeCardInfos.Add(cardBackInfo);
            }
            return employeeCardInfos;
        }

        public List<EmployeeCardPrintModel> GetEmployeeIDCardInfoInEnglish(List<Guid> employeeIdList, SearchFieldModel searchFieldModel)
        {
            var companyInfo = Context.EmployeeCardInfoes.FirstOrDefault(p => p.IsBangla == false);

            IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);

            var employeeIDCardInfosInEnglish = Context.VEmployeeIDCardInfoInEnglish.Where(x => employeeTypeList.Contains(x.EmployeeTypeId) &&                                  
                    (x.CardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                    (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0) &&
                    (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                    (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0)
                    );

            employeeIDCardInfosInEnglish = from p in employeeIDCardInfosInEnglish
                where employeeIdList.Contains(p.EmployeeId)
                select p;

            var listEmployeeCardPrintModel = new List<EmployeeCardPrintModel>();

            string validDate = DateTime.Now.ToString("dd/MM/yyyy");

            foreach (var empIDCardInfoInEnglish in employeeIDCardInfosInEnglish.ToList())
            {
                string JoiningDate = "DOJ  : " + empIDCardInfoInEnglish.JoiningDate.Split('/')[0] + " " +
                                     BanglaConversion.ConvertToEnglishMonth(
                                         empIDCardInfoInEnglish.JoiningDate.Split('/')[1]) + ", " + empIDCardInfoInEnglish.JoiningDate.Split('/')[2];

                string issueDate = "Issue Date  : " + empIDCardInfoInEnglish.JoiningDate.Split('/')[0] + " " +
                                   BanglaConversion.ConvertToEnglishMonth(
                                       empIDCardInfoInEnglish.JoiningDate.Split('/')[1]) + ", " + empIDCardInfoInEnglish.JoiningDate.Split('/')[2];

                string cardValidity = "Valid By : " + validDate.Split('/')[0] + " " +
                                      BanglaConversion.ConvertToEnglishMonth(
                                          validDate.Split('/')[1]) + ", " + (Convert.ToInt32(validDate.Split('/')[2]) + 2);

                var employeeCardPrintModel = new EmployeeCardPrintModel
                {
                    EmployeeId = empIDCardInfoInEnglish.EmployeeId,
                    EmployeeCardId = "ID No  : " + empIDCardInfoInEnglish.EmployeeCardId,
                    Name = "Name  : " + empIDCardInfoInEnglish.Name,
                    Department = "Dept  : " + empIDCardInfoInEnglish.Department,
                    Section = "Section  : " + empIDCardInfoInEnglish.Section,
                    Line = "Line  : "+ empIDCardInfoInEnglish.Line,
                    Designation = "Desig  : " + empIDCardInfoInEnglish.Designation,
                    EmployeeJobType = "Job Type  : " + empIDCardInfoInEnglish.EmployeeJobType,
                    JoiningDate = JoiningDate,
                    IssueDate = issueDate,
                    PhotographPath = empIDCardInfoInEnglish.PhotographPath,
                    CardValidity = cardValidity,
                    CompanyName = empIDCardInfoInEnglish.CompanyName,
                    CompanyAddress1 = companyInfo.Address1,
                    CompanyAddress2 = companyInfo.Address2,
                    MobileNo = "Mobile : " + companyInfo.Mobile,
                    BloodGroup = "Blood Group : " + empIDCardInfoInEnglish.BloodGroup,
                    FatherName = "C/O : " + empIDCardInfoInEnglish.FathersName,
                    Village = "Village   : " + empIDCardInfoInEnglish.MailingAddress,
                    PostOffice = "Post Office : " + empIDCardInfoInEnglish.PostOffice,
                    PoliceStation = "Thana  : " + empIDCardInfoInEnglish.PoliceStation,
                    District = "District  : " + empIDCardInfoInEnglish.DistrictName,
                    EmergencyContactNo = "Emer. Contact : " + empIDCardInfoInEnglish.EmergencyPhone,
                    NationalIdNo = "National Id No: " + empIDCardInfoInEnglish.NationalIdNo,
                    BirthCertificateNo = "Birth Id No : " + empIDCardInfoInEnglish.BirthRegistrationNo,
                    Notice1 = "If found ownerless please",
                    Notice2 = "return to management Authority",
                    IsBangla = false
                };

                listEmployeeCardPrintModel.Add(employeeCardPrintModel);
            }

            return listEmployeeCardPrintModel;
        }

        public List<EmployeeCardPrintModel> GetEmployeeIDCardInfoInBengali(List<Guid> employeeIdList, SearchFieldModel searchFieldModel)
        {
            var companyInfo = Context.EmployeeCardInfoes.FirstOrDefault(p => p.IsBangla == true);

            IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);

            var employeeIDCardInfosInBengali = Context.VEmployeeIDCardInfoInBengali.Where(x => employeeTypeList.Contains(x.EmployeeTypeId) &&

                                                                                               (x.CardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                                                                                               (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0) &&
                                                                                               (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                                                                                               (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0)
                );

            employeeIDCardInfosInBengali = from p in employeeIDCardInfosInBengali
                where employeeIdList.Contains(p.EmployeeId)
                select p;

            var listEmployeeCardPrintModel = new List<EmployeeCardPrintModel>();

            string validDate = DateTime.Now.ToString("dd/MM/yyyy");

            foreach (var emplIDCardInfoInBengali in employeeIDCardInfosInBengali.ToList())
            {
                string joiningDate = "যোগদানের তারিখ  : " + BanglaConversion.ConvertToBanglaNumber(emplIDCardInfoInBengali.JoiningDate.Split('/')[0]) + " " +
                                     BanglaConversion.ConvertToBanglaMonth(
                                         emplIDCardInfoInBengali.JoiningDate.Split('/')[1]) + ", " + BanglaConversion.ConvertToBanglaNumber(emplIDCardInfoInBengali.JoiningDate.Split('/')[2]);

                string issueDate = "ইস্যুর তারিখ  : " + BanglaConversion.ConvertToBanglaNumber(emplIDCardInfoInBengali.JoiningDate.Split('/')[0]) + " " +
                                   BanglaConversion.ConvertToBanglaMonth(
                                       emplIDCardInfoInBengali.JoiningDate.Split('/')[1]) + ", " + BanglaConversion.ConvertToBanglaNumber(emplIDCardInfoInBengali.JoiningDate.Split('/')[2]);


                string cardValidity = "মেয়াদ : " + BanglaConversion.ConvertToBanglaNumber(validDate.Split('/')[0]) + " "
                                      + BanglaConversion.ConvertToBanglaMonth(validDate.Split('/')[1]) + ", "
                                      + BanglaConversion.ConvertToBanglaNumber((Convert.ToInt32(validDate.Split('/')[2]) + 2).ToString());


                var employeeCardPrintModel = new EmployeeCardPrintModel
                {
                    EmployeeId = emplIDCardInfoInBengali.EmployeeId,
                    EmployeeCardId = "কার্ড নম্বর  : " + BanglaConversion.ConvertToBanglaNumber(emplIDCardInfoInBengali.EmployeeCardId),
                    Name = "নাম  : " + emplIDCardInfoInBengali.Name,
                    Department = "বিভাগ  : " + emplIDCardInfoInBengali.Department,
                    Section = "সেকশন  : " + emplIDCardInfoInBengali.Section,
                    Line = "লাইন  : " + emplIDCardInfoInBengali.Line,
                    Designation = "পদবি  : " + emplIDCardInfoInBengali.Designation,
                    EmployeeJobType = "কাজের ধরন  : " + emplIDCardInfoInBengali.EmployeeJobType,
                    JoiningDate = joiningDate,
                    IssueDate = issueDate,
                    PhotographPath = emplIDCardInfoInBengali.PhotographPath,
                    CardValidity = cardValidity,
                    CompanyName = emplIDCardInfoInBengali.CompanyName,
                    CompanyAddress1 = companyInfo.Address1,
                    CompanyAddress2 = companyInfo.Address2,
                    MobileNo = "মোবাইল : " + companyInfo.Mobile,
                    BloodGroup = "রক্তের গ্রুপ : " + emplIDCardInfoInBengali.BloodGroup,
                    FatherName = "প্রযত্নে : " + emplIDCardInfoInBengali.FathersNameInBengali,
                    Village = "গ্রাম   : " + emplIDCardInfoInBengali.MailingAddressInBengali,
                    PostOffice = "পোষ্ট অফিস : " + emplIDCardInfoInBengali.PostOfficeInBengali,
                    PoliceStation = "থানা  : " + emplIDCardInfoInBengali.PoliceStationInBengali,
                    District = "জেলা  : " + emplIDCardInfoInBengali.DistrictNameInBengali,
                    EmergencyContactNo = "জরুরি যোগাযোগ নম্বর : " + BanglaConversion.ConvertToBanglaNumber(emplIDCardInfoInBengali.EmergencyPhoneInBengali),
                    NationalIdNo = "জাতীয় পরিচয় পত্র: " + BanglaConversion.ConvertToBanglaNumber(emplIDCardInfoInBengali.NationalIdNo),
                    BirthCertificateNo = "জন্ম সনদ পত্র নম্বর : " + BanglaConversion.ConvertToBanglaNumber(emplIDCardInfoInBengali.BirthRegistrationNo),
                    Notice1 = "উক্ত পরিচয়পত্র হারাইয়া গেলে তা্ৎক্ষণিক",
                    Notice2 = "ব্যবস্থাপনা কর্তৃপক্ষকে জানাতে হবে ।",
                    IsBangla = true
                };
                listEmployeeCardPrintModel.Add(employeeCardPrintModel);
            }
            return listEmployeeCardPrintModel;
        }

        public IQueryable<VEmployeeIDCardInfoInEnglish> GetEmployeeIDCardInfoInEnglishBySearchKey(SearchFieldModel searchFieldModel)
        {
            IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);

            var employeeIDCardInfosInEnglish =
                Context.VEmployeeIDCardInfoInEnglish.Where(x =>
                    employeeTypeList.Contains(x.EmployeeTypeId) &&
                    (x.CardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                    (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0) &&
                    (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0) &&
                    (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                    (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0)
                    );

            return employeeIDCardInfosInEnglish.OrderBy(x => x.EmployeeCardId);
        }

        public IQueryable<VEmployeeIDCardInfoInBengali> GetEmployeeIDCardInfoInBengaliBySearchKey(SearchFieldModel searchFieldModel)
        {

            IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);

            var employeeIDCardInfosInBengali =
                Context.VEmployeeIDCardInfoInBengali.Where(x =>
                    employeeTypeList.Contains(x.EmployeeTypeId) &&
                    (x.CardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                    (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0) &&
                    (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                    (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0)
                    );

            return employeeIDCardInfosInBengali.OrderBy(x => x.EmployeeCardId);
        }
    }
}
