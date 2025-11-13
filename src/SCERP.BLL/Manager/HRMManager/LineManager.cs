using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.HRMManager
{
    public class LineManager : ILineManager
    {
        private readonly ILineRepository _lineRepository = null;
        public LineManager(SCERPDBContext context)
        {
            _lineRepository = new LineRepository(context);
        }

        public List<Line> GetAllLinesByPaging(int startPage, int pageSize, Line line, out int totalRecords)
        {
            List<Line> lines;
            try
            {
                lines = _lineRepository.GetAllLinesByPaging(startPage, pageSize, line, out totalRecords);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);

            }
            return lines;
        }

        public Line GetLineById(int lineId)
        {
            Line line = null;
            try
            {
                line =
                    _lineRepository.GetLineById(lineId);

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return line;
        }

        public bool CheckExistingLine(Line model)
        {
            bool isExist;
            try
            {
                isExist = _lineRepository.Exists((x => x.IsActive == true
                          && (x.LineId != model.LineId)
                          && (x.Name.Replace(" ", "").ToLower() == model.Name.Replace(" ", "").ToLower())));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public int EditLine(Line model)
        {
            var editIndex = 0;
            try
            {
                var line = _lineRepository.FindOne(x => x.IsActive && x.LineId == model.LineId);
                line.Name = model.Name;
                line.NameInBengali = model.NameInBengali;
                line.Description = model.Description;
                line.EditedDate = DateTime.Now;
                line.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _lineRepository.Edit(line);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveLine(Line model)
        {
            var saveIndex = 0;
            try
            {
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                saveIndex = _lineRepository.Save(model);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public int DeleteLine(int lineId)
        {
            var deleteIndex = 0;
            try
            {
                var line = _lineRepository.FindOne(x => x.IsActive == true && x.LineId == lineId);
                line.IsActive = false;
                deleteIndex = _lineRepository.Edit(line);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

        public List<Line> GetAllLinesBySearchKey(string lineName)
        {
            List<Line> lines;
            try
            {
                lines = _lineRepository.Filter(x => x.IsActive
                         && ((x.Name.ToLower().Contains(lineName.ToLower())) || (String.IsNullOrEmpty(lineName))))
                         .ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return lines;
        }

        public List<Line> GetAllLines()
        {
            List<Line> lines;
            try
            {
                lines = _lineRepository.Filter(x => x.IsActive).OrderBy(x=>x.Name).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return lines;
        }
    }
}
