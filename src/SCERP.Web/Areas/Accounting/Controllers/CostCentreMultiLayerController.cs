using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.AccountingModel;
using SCERP.Web.Helpers;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class CostCentreMultiLayerController : BaseAccountingController
    {
        public ActionResult CostCentreMultiLayer()
        {
            List<Acc_CostCentreMultiLayer> costCentre = CostCentreManager.GetAllCostCentreMultiLayers();
            List<Acc_CostCentreMultiLayer> costCentreTree = CostCentreTreeBuilder.GetCostCentreTree(costCentre);
            return View(costCentreTree);
        }

        public ActionResult Create(string costCentre)
        {
            int parentId = Convert.ToInt32(costCentre.Split('_')[1]);
            int sortOrder = Convert.ToInt32(costCentre.Split('_')[4]);
            int newParentId = Convert.ToInt32(costCentre.Split('_')[2]);
            int itemLevel = Convert.ToInt32(costCentre.Split('_')[3]);

            var costCentreObj = new Acc_CostCentreMultiLayer();

            if (itemLevel < 3)
            {
                int itemId = CostCentreManager.GetMaxItemId(newParentId);

                costCentreObj.ItemId = itemId;
                costCentreObj.ParentId = newParentId;
                costCentreObj.ItemLevel = itemLevel + 1;
                costCentreObj.SortOrder = sortOrder + 1;
                costCentreObj.IsActive = true;
            }

            else
            {
                return Content("You can not create cost centre at this level !");
            }

            return View(costCentreObj);
        }
         
        [HttpPost]
        public ActionResult SaveCostCentre(Acc_CostCentreMultiLayer costCentre)
        {
            int index = 0;
            costCentre.IsActive = true;
            index = costCentre.Id == 0 ? CostCentreManager.SaveCostCentre(costCentre) : CostCentreManager.EditControlAccount(costCentre);
            return (index > 0) ? Reload() : ErrorMessageResult();
        }

        public ActionResult Edit(string costCentre)
        {
            int id = Convert.ToInt32(costCentre.Split('_')[0]);
            int controleLavel = Convert.ToInt32(costCentre.Split('_')[3]);

            Acc_CostCentreMultiLayer costCentreMultiLayer = CostCentreManager.GetMultiLayerById(id) ?? new Acc_CostCentreMultiLayer();
            return View("Create", costCentreMultiLayer);
        }

        public ActionResult Delete(string costCentre)
        {
            int index = 0;
            int id = Convert.ToInt32(costCentre.Split('_')[0]);
            index = CostCentreManager.DeleteCostCentre(id);
            return index > 0 ? Reload() : ErrorResult();
        }
    }
}