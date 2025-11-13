using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.HRMManager
{
    public class SectionManager : ISectionManager
    {
        private readonly ISectionRepository _sectionRepository;

        public SectionManager(DAL.SCERPDBContext context)
        {
           _sectionRepository=new SectionRepository(context);
        }

        public List<Section> GetAllSections(int startPage, int pageSize, Section model, out int totalRecords)
        {
            List<Section> sections;
            try
            {
                sections = _sectionRepository.GetAllSections(startPage, pageSize, model, out totalRecords);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return sections;
        }

        public Section GetSectionById(int sectionId)
        {
            Section section;
            try
            {
                section =
                    _sectionRepository.FindOne(x => x.IsActive && x.SectionId == sectionId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return section;
        }

        public bool IsExistSection(Section model)
        {
            bool isExist;
            try
            {
                isExist = _sectionRepository.Exists(x => x.IsActive == true
                    && x.SectionId != model.SectionId
                     && (x.Name.Replace(""," ").ToLower() == model.Name.Replace("", " ").ToLower()));
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public int EditSection(Section model)
        {
            var editIndex = 0;
            try
            {
                var sectionObj = _sectionRepository.FindOne(x => x.IsActive && x.SectionId == model.SectionId);
                sectionObj.Name = model.Name;
                sectionObj.NameInBengali = model.NameInBengali;
                sectionObj.Description = model.Description;
                sectionObj.EditedDate = DateTime.Now;
                sectionObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _sectionRepository.Edit(sectionObj);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveSection(Section model)
        {
            var saveIndex = 0;
            try
            {
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                saveIndex = _sectionRepository.Save(model);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public int DeleteSection(int sectionId)
        {

            var deleteIndex = 0;
            try
            {
                var sectonObj = _sectionRepository.FindOne(x => x.IsActive == true && x.SectionId == sectionId);
                sectonObj.IsActive = false;
               
                sectonObj.EditedDate = DateTime.Now;
                sectonObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _sectionRepository.Edit(sectonObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }
        public List<Section> GetAllSectionBySearchKey(string searchKey)
        {
            List<Section> sections;
            try
            {
                sections = _sectionRepository.Filter(x => x.IsActive && ((x.Name.Replace(" ", "")
                        .ToLower().Contains(searchKey.Replace(" ", "")
                            .ToLower())) || String.IsNullOrEmpty(searchKey))).ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return sections;
        }

        public IList<Section> GetListOfSection()
        {
            List<Section> sections;
            try
            {
                sections = _sectionRepository.Filter(x => x.IsActive).OrderBy(x=>x.Name).ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return sections;
        }

        public List<Section> GetSectionByDepartment(int? departmentId)
        {
            List<Section> sections;
            try
            {
                sections = _sectionRepository.Filter(x => x.IsActive).OrderBy(x=>x.Name).ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return sections;
        }
    }
}
