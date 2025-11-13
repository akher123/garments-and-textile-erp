using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class CuttingSequenceController : BaseController
    {
        private readonly ICuttingSequenceManager _cuttingSequenceManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IComponentManager _componentManager;

        public CuttingSequenceController(ICuttingSequenceManager cuttingSequenceManager, IOmBuyerManager buyerManager,  IComponentManager componentManager)
        {
            _cuttingSequenceManager = cuttingSequenceManager;
            _buyerManager = buyerManager;
            _componentManager = componentManager;
        }
           [AjaxAuthorize(Roles = "cuttingsequence-1,cuttingsequence-2,cuttingsequence-3")]
        public ActionResult Index(CuttingSequenceViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            return View(model);
        }
           [AjaxAuthorize(Roles = "cuttingsequence-2,cuttingsequence-3")]
        public ActionResult Save(CuttingSequenceViewModel model)
         {
            var compid = PortalContext.CurrentUser.CompId;
            List<PROD_CuttingSequence> cuttingSequences;
            cuttingSequences = model.CuttingSequenceDictionary.Select(x => x.Value).ToList().Select(x => new PROD_CuttingSequence()
            {
                CuttingSequenceRefId = "00000000",
                BuyerRefId = model.CuttingSequence.BuyerRefId,
                OrderNo = model.CuttingSequence.OrderNo,
                OrderStyleRefId = model.CuttingSequence.OrderStyleRefId,
                ColorRefId = model.CuttingSequence.ColorRefId,
                ComponentRefId = x.ComponentRefId,
                CuttingSequenceId = x.CuttingSequenceId,
                SlNo = x.SlNo,
                CompId = compid
            }).ToList();
       
            int saveIndex = 0;
            if (cuttingSequences.Any())
            {
                saveIndex = _cuttingSequenceManager.SaveCuttingSequenceLis(cuttingSequences, model.CuttingSequence.CuttingSequenceId, model.CuttingSequence.ColorRefId);
            }
            else
            {
                return ErrorResult("Add at list one sequence ");
            }
            return saveIndex > 0 ? Reload() : ErrorResult("Failed To Save Sequence");

        
           
        }
        [AjaxAuthorize(Roles = "cuttingsequence-2,cuttingsequence-3")]
        public ActionResult Find(CuttingSequenceViewModel model)
        {
            ModelState.Clear();
            List<VwCuttingSequence> cuttingSequences = _cuttingSequenceManager.GetCuttingSequenceByParam(model.CuttingSequence.ColorRefId, model.CuttingSequence.OrderNo, model.CuttingSequence.BuyerRefId, model.CuttingSequence.OrderStyleRefId);
            if (cuttingSequences.Any())
            {
               model.CuttingSequence = cuttingSequences.First();
            }

            model.CuttingSequenceDictionary = cuttingSequences.ToDictionary(x => Convert.ToString(x.SlNo + "-" + x.CuttingSequenceId + x.ColorRefId), x => x);
            model.Components = _componentManager.GetComponents();
            return PartialView("~/Areas/Production/Views/CuttingSequence/_Part.cshtml", model);
        }
          [AjaxAuthorize(Roles = "cuttingsequence-2,cuttingsequence-3")]
        public ActionResult AddNewPart([Bind(Include = "CuttingSequenceDictionary,CuttingSequence")]CuttingSequenceViewModel model)
        {
            ModelState.Clear();
            string key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            if (String.IsNullOrEmpty(model.CuttingSequence.ComponentRefId))
            {
                return ErrorResult("Please Select Part From Dropdown.");
            }
            
            model.Components = _componentManager.GetComponents();
            foreach (var item in model.CuttingSequenceDictionary.Where(kvp => kvp.Value.ComponentRefId == model.CuttingSequence.ComponentRefId).ToList())
            {
                model.CuttingSequenceDictionary.Remove(item.Key);
            }
            model.CuttingSequenceDictionary.Add(key, model.CuttingSequence);
            var partDictionary = new Dictionary<string, VwCuttingSequence>();
            int index = 1;
            foreach (var part in model.CuttingSequenceDictionary)
            {
                part.Value.SlNo = index;
                OM_Component firstOrDefault = model.Components.FirstOrDefault(x => x.ComponentRefId == part.Value.ComponentRefId);
                if (firstOrDefault != null)
                    part.Value.ComponentName =
                        firstOrDefault.ComponentName;
                partDictionary.Add(part.Key, part.Value);
                index++;
            }
            model.CuttingSequenceDictionary = partDictionary;
        
            return PartialView("~/Areas/Production/Views/CuttingSequence/_Part.cshtml", model);
        }
        [AjaxAuthorize(Roles = "cuttingsequence-3")]
        public ActionResult DeletePart([Bind(Include = "CuttingSequenceDictionary,CuttingSequence")]CuttingSequenceViewModel model)
        {
            ModelState.Clear();
            var partDictionary = new Dictionary<string, VwCuttingSequence>();
            int index = 1;
            foreach (var part in model.CuttingSequenceDictionary)
            {
                part.Value.SlNo = index;
                partDictionary.Add(part.Key, part.Value);
                index++;
            }
            model.CuttingSequenceDictionary = partDictionary;
            model.Components = _componentManager.GetComponents();
            return PartialView("~/Areas/Production/Views/CuttingSequence/_Part.cshtml", model);
        }
        [AjaxAuthorize(Roles = "cuttingsequence-3")]
        public ActionResult Delete(long cuttingSequenceId)
        {
            int deleteIndex = _cuttingSequenceManager.DeleteCuttingSequence(cuttingSequenceId);

            return deleteIndex>0 ? Reload() : ErrorResult("Delete Failed");
        }
    }
}