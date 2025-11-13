using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeSkillManager : BaseManager, IEmployeeSkillManager
    {
        private readonly IEmployeeSkillRepository _employeeSkillRepository = null;

        public EmployeeSkillManager(SCERPDBContext context)
        {
            _employeeSkillRepository = new EmployeeSkillRepository(context);
        }


        public EmployeeSkill GetEmployeeSkillById(int employeeSkillId)
        {

            EmployeeSkill employeeSkill = null;

            try
            {
              
                employeeSkill = _employeeSkillRepository.GetEmployeeSkillById(employeeSkillId);


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeSkill;
        }



        public int SaveEmployeeSkill(EmployeeSkill employeeSkill)
        {
            var savedemployeeSkill = 0;
            try
            {
                employeeSkill.CreatedDate = DateTime.Now;
                employeeSkill.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeSkill.IsActive = true;
                savedemployeeSkill = _employeeSkillRepository.Save(employeeSkill);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return savedemployeeSkill;
        }

        public List<VEmployeeSkillDetail> GetAllEmployeeSkillDetails(int startPage, int pageSize, EmployeeSkill model, SearchFieldModel searchFieldModel,
            out int totalRecords)
        {
            var vEmployeeSkillDetails = new List<VEmployeeSkillDetail>();
            try
            {
                vEmployeeSkillDetails = _employeeSkillRepository.GetAllEmployeeSkillDetails(startPage, pageSize, model, searchFieldModel, out  totalRecords);
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.InnerException.Message);
            }

            return vEmployeeSkillDetails;
        }

        public int EditEmployeeSkill(EmployeeSkill employeeSkill)
        {
            var editIndex = 0;

            try
            {
                var employeeskill = _employeeSkillRepository.FindOne(x => x.EmployeeSkillId == employeeSkill.EmployeeSkillId);
                employeeskill.EditedDate = DateTime.Now;
                employeeSkill.EditedBy = PortalContext.CurrentUser.UserId;
                employeeSkill.SkillOperationId = employeeSkill.SkillOperationId;
                employeeSkill.Efficiency = employeeSkill.Efficiency;
                employeeSkill.FromDate = employeeSkill.FromDate;
                employeeSkill.ToDate = employeeSkill.ToDate;
                editIndex = _employeeSkillRepository.Edit(employeeSkill);

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }

            return editIndex;
        }

        public int DeleteEmployeeSkillById(int employeeSkillId)
        {

            var deleteIndex = 0;

            try
            {
                var employeeSkill = _employeeSkillRepository.FindOne(x => x.IsActive && x.EmployeeSkillId == employeeSkillId);
                employeeSkill.EditedDate = DateTime.Now;
                employeeSkill.EditedBy = PortalContext.CurrentUser.UserId;
                employeeSkill.IsActive = false;
                deleteIndex = _employeeSkillRepository.Edit(employeeSkill);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);

            }
            return deleteIndex;


        }
    }
}
