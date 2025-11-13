using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace SCERP.Common
{

    public enum StoreLedgerTransactionType
    {
        
        Receive=1,
        Issue =2,
    }
    public enum InventoryProcessType
    {
        StorePurchaseRequisition=1
    }
    public enum StorePurchaseRequisition
    {
        Store=1,
        Purchases=2,
        Approval=3
    }
    public class InventoryProcess
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ProcessTypeName { get; set; }
        public int ProcessTypeId { get; set; }
    }

    public static class InventoryProcessUtility
    {
        public static List<InventoryProcess> InventoryProcesses { get; set; }
         static InventoryProcessUtility()
         {
             
         }
         private static IEnumerable<InventoryProcess> GetPocessList()
        {
            // start Inventory Store purchase requisition
             InventoryProcesses=new List<InventoryProcess>();
             var storeProcess = new InventoryProcess { ProcessId = (int)StorePurchaseRequisition.Store, ProcessName = StorePurchaseRequisition.Store.ToString(), ProcessTypeId = (int)InventoryProcessType.StorePurchaseRequisition, ProcessTypeName = InventoryProcessType.StorePurchaseRequisition.ToString() };
            InventoryProcesses.Add(storeProcess);
            var purchasesProcess = new InventoryProcess { ProcessId = (int)StorePurchaseRequisition.Purchases, ProcessName =StorePurchaseRequisition.Purchases.ToString(), ProcessTypeId = (int)InventoryProcessType.StorePurchaseRequisition, ProcessTypeName = InventoryProcessType.StorePurchaseRequisition.ToString() };
            InventoryProcesses.Add(purchasesProcess);
            var approvalProcess = new InventoryProcess { ProcessId = (int)StorePurchaseRequisition.Approval, ProcessName = StorePurchaseRequisition.Approval.ToString(), ProcessTypeId = (int)InventoryProcessType.StorePurchaseRequisition, ProcessTypeName = InventoryProcessType.StorePurchaseRequisition.ToString() };
            InventoryProcesses.Add(approvalProcess);
            // end Inventory Store purchase requisition

            return InventoryProcesses;
        }
         public static List<InventoryProcess> GetPocessByProcessType(int processTypeId)
         {
             return GetPocessList().Where(x => x.ProcessTypeId == processTypeId).ToList();
         }

        public static IEnumerable GetProcessTypes()
        {
            return Enum.GetValues(typeof(InventoryProcessType)).Cast<InventoryProcessType>().Select(x=>new {ProcessTypeId=(int)x,ProcessTypeName=x.ToString()});
        }

        public static string GetProcessName(int processId)
        {
            var process = GetPocessList().FirstOrDefault(x => x.ProcessId == processId);
            return process != null ? process.ProcessName : "";
        }
        public static string GetProcessTypeName(int processTypeId)
        {
            var processType = GetPocessList().FirstOrDefault(x => x.ProcessTypeId == processTypeId);
            return processType != null ? processType.ProcessTypeName : "";
        }
    }
}
