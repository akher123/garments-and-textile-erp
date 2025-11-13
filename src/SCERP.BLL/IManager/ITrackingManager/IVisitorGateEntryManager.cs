using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TrackingModel;

namespace SCERP.BLL.IManager.ITrackingManager
{
   public interface IVisitorGateEntryManager
    {
       List<TrackVisitorGateEntry> GetVisitorGateEntryByPaging(TrackVisitorGateEntry model, out int totalRecords);
       int SaveVisitorGateEntry(TrackVisitorGateEntry model);
       int EditVisitorGateEntry(TrackVisitorGateEntry model);
       int DeleteVisitorGateEntry(long visitorGateEntryId);
       TrackVisitorGateEntry GetVisitorGateEntryByPhone(string phone);
       TrackVisitorGateEntry GetVisitorGateEntryById(long visitorGateEntryId);
       bool IsVitorGateEntryExist(TrackVisitorGateEntry model);
    }
}
