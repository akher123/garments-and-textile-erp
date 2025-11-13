using System;
using System.Collections.Generic;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.DAL.Repository.PlanningRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.PlanningManager
{
    public class ProductionLineManager : IProductionLineManager
    {
        private readonly IProductionLineRepository _productionLineRepository = null;

        public ProductionLineManager(IProductionLineRepository productionLineRepository)
        {
            _productionLineRepository = productionLineRepository;
        }

        public List<PLAN_ProductionLine> GetProductionLine(int startPage, int pageSize, out int totalRecords, PLAN_ProductionLine model,
            SearchFieldModel searchFieldModel)
        {
            List<PLAN_ProductionLine> productionLines;
            try
            {
                productionLines = _productionLineRepository.GetProductionLine(startPage, pageSize, out totalRecords,
                    model, searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return productionLines;
        }


        public List<PLAN_ProductionLine> GetProductionLineBySearchKey(SearchFieldModel searchFieldModel)
        {
            List<PLAN_ProductionLine> productionLines;
            try
            {
                productionLines = _productionLineRepository.GetProductionLineBySearchKey(searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return productionLines;
        }


        public int DeleteProductionLine(int productionLineId)
        {
            var deleteIndex = 0;
            try
            {
                var productionLineObj = _productionLineRepository.FindOne(x => x.IsActive && x.ProductionLineId == productionLineId);
                productionLineObj.IsActive = false;
                productionLineObj.EditedDate = DateTime.Now;
                productionLineObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _productionLineRepository.Edit(productionLineObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }


        public PLAN_ProductionLine GetProductionLineById(int productionLineId)
        {
            PLAN_ProductionLine productionLine;
            try
            {
                productionLine = _productionLineRepository.GetProductionLineById(productionLineId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return productionLine;
        }


        public int EditProductionLine(PLAN_ProductionLine model)
        {
            var editIndex = 0;
            try
            {
                var productionLineObj = _productionLineRepository.FindOne(x => x.IsActive && x.ProductionLineId == model.ProductionLineId);
                productionLineObj.BranchUnitDepartmentId = model.BranchUnitDepartmentId;
                productionLineObj.LineId = model.LineId;
                productionLineObj.NoOfOperator = model.NoOfOperator;
                productionLineObj.LineEfficiency = model.LineEfficiency;
                productionLineObj.CapacityAvailable = model.CapacityAvailable;
                productionLineObj.FromDate = model.FromDate;
                productionLineObj.ToDate = model.ToDate;
                productionLineObj.CreatedDate = model.CreatedDate;
                productionLineObj.CreatedBy = model.CreatedBy;
                productionLineObj.EditedDate = DateTime.Now;
                productionLineObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _productionLineRepository.Edit(productionLineObj);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return editIndex;
        }


        public int SaveProductionLine(PLAN_ProductionLine model)
        {
            var saveIndex = 0;
            var productionLine = new PLAN_ProductionLine()
            {
                BranchUnitDepartmentId = model.BranchUnitDepartmentId,
                LineId = model.LineId,
                NoOfOperator = model.NoOfOperator,
                LineEfficiency = model.LineEfficiency,
                CapacityAvailable = model.CapacityAvailable,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                CreatedBy = PortalContext.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                IsActive = true,
            };
            try
            {
                saveIndex = _productionLineRepository.Save(productionLine);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public List<PLAN_ProductionLine> GetProductionLineByBranchUnitDepartmentId(int productionLineId)
        {

            List<PLAN_ProductionLine> productionLines;
            try
            {
                productionLines = _productionLineRepository.GetProductionLineByBranchUnitDepartmentId(productionLineId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return productionLines;
        }
   
    }
}
