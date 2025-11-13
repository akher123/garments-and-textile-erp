using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
   public class TempGroupManager: ITempGroupManager
   {
       private readonly ITempGroupRepository _tempGroupRepository;

       public TempGroupManager(ITempGroupRepository tempGroupRepository)
       {
           _tempGroupRepository = tempGroupRepository;
       }
       public List<OM_TempGroup> GetTempGroupByPaging(string tempGroupName, int pageIndex, string sort, string sortdir, out int totalRecords)
       {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
           var tempGrouplist =_tempGroupRepository.Filter(x => x.TempGroupName.Trim().Contains(tempGroupName) || String.IsNullOrEmpty(tempGroupName)).Include(x=>x.OM_CostSheetTemplate);
            totalRecords = tempGrouplist.Count();
            switch (sort)
            {
                case "TempGroupName":
                    switch (sortdir)
                    {
                        case "DESC":
                            tempGrouplist = tempGrouplist
                                 .OrderByDescending(r => r.TempGroupName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            tempGrouplist = tempGrouplist
                                 .OrderBy(r => r.TempGroupId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    tempGrouplist = tempGrouplist
                        .OrderByDescending(r => r.TempGroupId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return tempGrouplist.ToList();
        }

       public OM_TempGroup GeTempGroupById(int tempGroupId, string compId)
       {
           return _tempGroupRepository.FindOne(x =>x.CompId==compId &&  x.TempGroupId == tempGroupId);
       }

       public int DeleteTemplateGroup(int tempGroupId, string compId)
       {
           return _tempGroupRepository.Delete(x => x.CompId == compId && x.TempGroupId == tempGroupId);
       }

       public bool IsTempGroupExist(OM_TempGroup model)
       {
            return _tempGroupRepository.Exists(x => x.CompId == PortalContext.CurrentUser.CompId && x.TempGroupId != model.TempGroupId && x.TempGroupName == model.TempGroupName);
        }

       public int EditTempGroup(OM_TempGroup model)
       {
           OM_TempGroup tempGroup =
               _tempGroupRepository.FindOne(
                   x => x.CompId == PortalContext.CurrentUser.CompId && x.TempGroupId == model.TempGroupId);
           tempGroup.TempGroupName = model.TempGroupName;
           return _tempGroupRepository.Edit(tempGroup);
       }

       public int SaveTempGroup(OM_TempGroup model)
       {
           model.CompId = PortalContext.CurrentUser.CompId;
           return _tempGroupRepository.Save(model);
       }

       public List<OM_TempGroup> GetAllTempGroup(string compId)
       {
           return _tempGroupRepository.Filter(x => x.CompId == compId).ToList();
       }
   }
}
