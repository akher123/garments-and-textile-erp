using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class BachWiseIssueViewModel:VMaterialIssue
    {
        public List<VMaterialIssue> VMaterialIssues { get; set; }
        public VMaterialIssueDetail IssueDetail { get; set; }
        public Dictionary<string, VMaterialIssueDetail> MaterialIssueDetails { get; set; }
        public string Key { get; set; }
        public IEnumerable ToppingList { get; set; }
        public BachWiseIssueViewModel()
        {
          
            VMaterialIssues = new List<VMaterialIssue>();
            MaterialIssueDetails = new Dictionary<string, VMaterialIssueDetail>();
            IssueDetail = new VMaterialIssueDetail();
            Batches=new List<Pro_Batch>();
        }
        public List<Pro_Batch> Batches { get; set; }
       
        public IEnumerable<SelectListItem> BatcheSelectListItem
        {
            get
            {
                return new SelectList(Batches, "BtRefNo", "BatchNo");
            }
        }
        public IEnumerable<SelectListItem>ToppingSelectListItem
        {
            get
            {

                return new SelectList(ToppingList, "ToppingType", "Value");
            }
        }
        
          
      
    }
}