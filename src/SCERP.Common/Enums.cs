using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common
{

    public enum POSTED
    {
        Y =1,
        N =2
    }
    public enum BillTable
    {
        PROD_KnittingRollIssue=1,
        Inventory_GreyIssue = 2,
        Inventory_FinishFabricIssue = 3,
        PROD_ProcessReceive=4
    }

    public enum AccountRevenue
    {
        [Display(Name = "Knitting Charge Income")]
        KnittingChargeIncome = 5918,
        [Display(Name = " Dyeing Charge Income")]
        DyeingChargeIncome =165
    }
    public enum AccountExpenses
    {
        [Display(Name = "Knitting Charge Expenses")]
        KnittingChargeExpenses = 2788,
        [Display(Name = "Printing Charge Expenses")]
        PrintingChargeExpenses =1,
        [Display(Name = "Embroider Charge Expenses")]
        EmbroiderChargeExpenses = 2
    }
    public enum PartyType
    {
        
        K,R,D,P,E
    }
    public enum FileSource
    {
        BuyerOrder = 1,
        SalceContact = 2,
        ExportLc = 3,
        BbLc = 4,
        Pc = 5,
        Export = 6,
        CashLc =7,
        OrderStyle=8
    }

    public enum ReportType
    {
        PDF = 1,
        //DOC = 2,
        Excel = 3,
        Word = 4
    }
    public enum ComponentType
    {
        Body = 1,
        Collar = 2,
        Cuff = 3,
    }

    public enum BatchType
    {
        Internal=1,
        External=2,
    
    }
    public enum FinishType
    {
        Iron = 1,
        Poly = 2,
    }
    public enum BatchStatus
    {
        Pending=1,
        Running=2,
        Closed=3
   }

    public enum TemplateType
    {
        Template1=1,
        Template2=2
    }
}
