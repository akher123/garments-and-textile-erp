using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
   public class SampleStyleManager:ISampleStyleManager
   {
       private readonly ISampleStyleRepository _sampleStyleRepository;

       public SampleStyleManager(ISampleStyleRepository sampleStyleRepository)
       {
           _sampleStyleRepository = sampleStyleRepository;
       }


       public List<OM_SampleStyle> GetSampleStyles(int sampleOrderId)
       {
           var sampleStyles = _sampleStyleRepository.Filter(x => x.SampleOrderId == sampleOrderId);
           return sampleStyles.ToList();
       }

       public OM_SampleStyle GetSampleStyleById(int sampleStyleId)
       {
           return _sampleStyleRepository.FindOne(x=>x.SampleStyleId==sampleStyleId);
       }

   
       public int EditSampleStyle(OM_SampleStyle model)
       {
            OM_SampleStyle sampleStyle= _sampleStyleRepository.FindOne(x => x.SampleStyleId == model.SampleStyleId);
           sampleStyle.EditedBy = PortalContext.CurrentUser.UserId;
           sampleStyle.EditedDate = DateTime.Now;
           sampleStyle.StyleNo = model.StyleNo;
           sampleStyle.StyleQty = model.StyleQty;
           sampleStyle.SampleDate = model.SampleDate;
           sampleStyle.EFDate = model.EFDate;
           sampleStyle.ItemName = model.ItemName;
           sampleStyle.Fabrication = model.Fabrication;
           sampleStyle.FabQty = model.FabQty;
           sampleStyle.Gsm = model.Gsm;
           sampleStyle.FinishDia = model.FinishDia;
           sampleStyle.ColorName = model.ColorName;
           sampleStyle.SizeName = model.SizeName;
           sampleStyle.SampleType = model.SampleType;
           sampleStyle.Remarks = model.Remarks;

           sampleStyle.RibFab = model.RibFab;
           sampleStyle.RibFab = model.RibFab;
           sampleStyle.ContasFab = model.ContasFab;
           sampleStyle.ContasQty = model.ContasQty;

           return _sampleStyleRepository.Edit(sampleStyle);
       }

       public int SaveSampleStyle(OM_SampleStyle sampleStyle)
       {
           sampleStyle.CompId = PortalContext.CurrentUser.CompId;
           sampleStyle.CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
           sampleStyle.CreatedDate = DateTime.Now;
           sampleStyle.StyleRefId = GetNewRefId(sampleStyle.CompId);
           return _sampleStyleRepository.Save(sampleStyle);
       }

       private string GetNewRefId(string compId)
       {
           var maxRefId = _sampleStyleRepository.Filter(x => x.CompId == compId).Max(x => x.StyleRefId);
           return maxRefId.IncrementOne().PadZero(5);
       }

       public int DeleteSampleStyle(OM_SampleStyle sampleStyle)
       {
                 return _sampleStyleRepository.DeleteOne(sampleStyle);
       }

    
   }
}
