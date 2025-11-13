using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.CommercialModel;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface ILcStyleRepository : IRepository<COMMLcStyle>
    {
        COMMLcStyle GetLcStyleById(int? id);
        int? GetLcIdByLcNo(string lcNo);
        string GetOrderNoByOrderRefNo(string orderRefNo);
        List<OM_BuyOrdStyle> GetStylesByOrderNo(string orderNo);
        List<COMMLcStyle> GetAllLcStyles();
        List<COMMLcStyle> GetLcStyleByLcId(int lcId);
        List<VwCommLcStyle> GetAllLcStylesByPaging(int startPage, int pageSize, out int totalRecords, COMMLcStyle lcStyle);
        List<COMMLcStyle> GetLcStyleBySearchKey(int searchByCountry, string searchByLcStyle);
        List<VwCommLcStyle> GetLcStyleEditByLcId(COMMLcStyle lcStyle);
    }
}
