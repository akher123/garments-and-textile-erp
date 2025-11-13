using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class QuitTypeRepository : Repository<QuitType>, IQuitTypeRepository
    {
        public QuitTypeRepository(SCERPDBContext context)
            : base(context)
        {
        }


        public List<QuitType> GetAllQuitTypes()
        {
            return Context.QuitTypes.Where(x => x.IsActive == true).OrderBy(y => y.Type).ToList();
        }

        public List<QuitType> GetAllQuitTypesByPaging(int startPage, int pageSize, QuitType model, out int totalRecords)
        {
            IQueryable<QuitType> quitTypes;
            try
            {
                quitTypes = Context.QuitTypes.Where(x => x.IsActive == true
                    && ((x.Type.Replace(" ", "").ToLower().Contains(model.Type.Replace(" ", "").ToLower()))
                    || String.IsNullOrEmpty(model.Type)));
                totalRecords = quitTypes.Count();
                switch (model.sortdir)
                {
                    case "DESC":
                        quitTypes = quitTypes
                            .OrderByDescending(r => r.Type)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);

                        break;
                    default:
                        quitTypes = quitTypes
                          .OrderBy(r => r.Type)
                          .Skip(startPage * pageSize)
                          .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return quitTypes.ToList();
        }

        public List<QuitType> GetAllQuitTypesBySearchKey(QuitType model)
        {
            IQueryable<QuitType> quitTypes;
            try
            {
                quitTypes = Context.QuitTypes.Where(x => x.IsActive == true
                    && ((x.Type.Replace(" ", "").ToLower().Contains(model.Type.Replace(" ", "").ToLower()))
                    || String.IsNullOrEmpty(model.Type)));

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return quitTypes.ToList();
        }
    }
}
