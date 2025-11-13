using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.CommercialModel;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface ILcStyleManager
    {
        List<VwCommLcStyle> GetAllLcStylesByPaging(int startPage, int pageSize, out int totalRecords, COMMLcStyle lcStyle);

        List<COMMLcStyle> GetAllLcStyles();

        COMMLcStyle GetLcStyleById(int? id);

        string GetOrderNoByOrderRefNo(string orderRefNo);

        int? GetLcIdByLcNo(string lcNo);

        int SaveLcStyle(COMMLcStyle lcStyle);

        List<OM_BuyOrdStyle> GetStylesByOrderNo(string orderNo);

        int EditLcStyle(COMMLcStyle lcStyle);

        List<COMMLcStyle> GetLcStyleByLcId(int lcId);

        int DeleteLcStyle(COMMLcStyle commLcStyle);

        bool CheckExistingLcStyle(COMMLcStyle lcStyle);

        List<COMMLcStyle> GetLcStyleBySearchKey(int searchByCountry, string searchByCommLcStyle);

        List<VwCommLcStyle> GetLcStyleEditByLcId(COMMLcStyle lcStyle);
    }
}