using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class MCostingManager : IMCostingManager
    {


        private readonly IMCostingRepository _costingRepository;
   

        public MCostingManager(IMCostingRepository costingRepository)
        {
            _costingRepository = costingRepository;
        }

        public int SaveCost(OM_Costing model)
        {
            return _costingRepository.Save(model);
        }

        public List<OM_Costing> GetCostByPaging(OM_Costing model,int pageIndex,string sort,string sortdir, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var costList =
              _costingRepository.All();
            totalRecords = costList.Count();

            if (totalRecords > 0)
            {
                switch (sort)
                {
                    case "BuyerName":
                        switch (sortdir)
                        {
                            case "DESC":
                                costList = costList
                                  .OrderByDescending(r => r.BuyerName)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);

                                break;
                            default:
                                costList = costList
                                  .OrderBy(r => r.BuyerName)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                                break;
                        }
                        break;
                    case "BuyingHouse":
                        switch (sortdir)
                        {
                            case "DESC":
                                costList = costList
                                  .OrderByDescending(r => r.BuyingHouse)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);

                                break;
                            default:
                                costList = costList
                                  .OrderBy(r => r.BuyingHouse)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                                break;
                        }

                        break;
                    default:
                        costList = costList
                          .OrderByDescending(r => r.CostingId)
                          .Skip(index * pageSize)
                          .Take(pageSize);
                        break;
                }
            }
            return costList.ToList();
        }

        public OM_Costing GetCostById(int costingId)
        { 
            return _costingRepository.FindOne(x => x.CostingId == costingId);
        }

        public int DeleteCost(int costingId)
        {
            var costing = _costingRepository.GetById(costingId);
            return _costingRepository.DeleteOne(costing);
        }

        public int UpdateCosting(int costingId, string fieldName, string value)
        { 
            int updated = _costingRepository.UpdateCosting(costingId, fieldName, value);
            return updated;
        }

        public OM_Costing GetUpdatedCosting(int costingId)
        {
          return  _costingRepository.GetUpdatedCosting(costingId);
        }
    }
}





