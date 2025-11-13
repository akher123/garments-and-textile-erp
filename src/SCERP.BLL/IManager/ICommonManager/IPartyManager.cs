using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface IPartyManager
    {
        List<Party> GetPartiesByPaging(Party model, out int totalRecords);
        Party GetPartyById(long partyId);
        string GetNewPartyRef();
        int SaveParty(Party model);
        int EditParty(Party model);
        List<Party> GetParties(string pType);
        int DeleteParty(long partyId);
        List<Party> GetAllParties(string compId);
        int UpdateParty(int glId, long partyId, PartyType partyType );
        VwParty GetPartyViewById(long partyId);
        List<VwParty> GetVwPartiesByPaging(int pageIndex, string pType, string searchString, out int totalRecords);
    }
}
