using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class BuyerTnaTemplateController : BaseController
    {
        private readonly IOmBuyerManager _buyerManager;
        private readonly IBuyerTnaTemplateManager _buyerTnaTemplateManager;

        public BuyerTnaTemplateController(IOmBuyerManager buyerManager,
            IBuyerTnaTemplateManager buyerTnaTemplateManager)
        {
            _buyerManager = buyerManager;
            _buyerTnaTemplateManager = buyerTnaTemplateManager;
        }

        public ActionResult Index(BuyerTnaTemplateViewModel model)
        {
            ModelState.Clear();
            var tempTypes = from TemplateType templateType in Enum.GetValues(typeof(TemplateType))
                select new {Id = (int) templateType, Name = templateType.ToString()};
            model.TemplateTypes = tempTypes;
            model.Buyers = _buyerManager.GetAllBuyers();
            if (!model.IsSearch)
            {
                model.IsSearch = true;
                return View(model);
            }
            List<BuyerTnaTemplateModel> templates = _buyerTnaTemplateManager.GetTemplates(PortalContext.CurrentUser.CompId, model.BuyerRefId,
                    model.TemplateTypeId);
            model.Templates = templates.ToDictionary(x => x.ShortName + x.ActivityId, x => x);
            return View(model);
        }


        public ActionResult Save(BuyerTnaTemplateViewModel model)
        {
            List<OM_BuyerTnaTemplate> buyerTnaLayouts = model.Templates.Where(x => x.Value.Duration > 0)
                .Select(x => x.Value)
                .ToList()
                .ConvertAll(template => new OM_BuyerTnaTemplate()
                {
                    TemplateId = template.TemplateId,
                    CompId = template.CompId,
                    BuyerRefId = template.BuyerRefId,
                    TemplateTypeId = template.TemplateTypeId,
                    ActivityId = template.ActivityId,
                    Duration = template.Duration.GetValueOrDefault(),
                    Remarks = template.Remarks,
                    SerialNo = template.SerialNo,
                    RSerialNo = template.RSerialNo,
                    RType = template.RType,
                    FDuration = template.FDuration,
                    CreatedBy = PortalContext.CurrentUser.UserId,
                    CreatedDate = DateTime.Now
                });

            try
            {
                int saved = _buyerTnaTemplateManager.SaveBuyerTnaTemplateLayout(buyerTnaLayouts);
                if (saved > 0)
                {
                    return ErrorResult("Saved Successfully");
                }
                else
                {
                    return ErrorResult("Fail to save line wise machine plan");
                }

            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }

        }
    }

}