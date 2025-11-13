using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface ISeasonManager
    {
        List<OM_Season> GetSeasons();
        List<OM_Season> GetSeasonsByPaging(OM_Season model, out int totalRecords);
        string GetNewSeasonRefId();
        OM_Season GetSeasonById(int seasonId);
        int EditSeason(OM_Season model);
        int SaveSEason(OM_Season model);
        int DeleteSeason(string seasonRefId);

        OM_Season GetSeasonsBySesonRefId(string seasonRefId);
        bool CheckExistingSeason(OM_Season model);
    }
}
