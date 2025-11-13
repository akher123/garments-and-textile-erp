using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Web.Controllers;
namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class BaseInventoryController : BaseController
    {
        #region Inventory
        public IMaterialIssueManager MaterialIssueManager
        {
            get
            {
                return Manager.MaterialIssueManager;
            }
        }
        public IInventoryAuthorizedPersonManager InventoryAuthorizedPersonManager
        {
            get
            {
                return Manager.InventoryAuthorizedPersonManager;
            }
        }
        public IMaterialRequisitionManager MaterialRequisitionManager
        {
            get
            {
                return Manager.MaterialRequisitionManager;
            }
        }
        public IStoreLedgerManager StoreLedgerManager
        {
            get
            {
                return Manager.StoreLedgerManager;
            }
        }
        
        public IStorePurchaseRequisitionManager StorePurchaseRequisitionManager
        {
            get
            {
                return Manager.StorePurchaseRequisitionManager;
            }
        }
        public IInventoryApprovalStatusManager InventoryApprovalStatusManager
        {
            get
            {
                return Manager.InventoryApprovalStatusManager;
            }
        }
        public IPurchaseTypeManager PurchaseTypeManager
        {
            get
            {
                return Manager.PurchaseTypeManager;
            }
        }

        public IRequisitiontypeManager RequisitiontypeManager
        {
            get
            {
                return Manager.RequisitiontypeManager;
            }
        }
        public ISizeManager SizeManager
        {
            get
            {
                return Manager.SizeManager;
            }
        }
        public IInventoryGroupManager InventoryGroupManager
        {
            get
            {
                return Manager.InventoryGroupManager;
            }
        }
        public IInventorySubGroupManager InventorySubGroupManager
        {
            get
            {
                return Manager.InventorySubGroupManager;
            }
        }
        public IInventoryItemManager InventoryItemManager
        {
            get
            {
                return Manager.InventoryItemManager;
            }
        }
        public IItemStoreManager ItemStoreManager
        {
            get
            {
                return Manager.ItemStoreManager;
            }
        }

        public IQualityCertificateManager QualityCertificateManager
        {
            get
            {
                return Manager.QualityCertificateManager;
            }
        }

        public IGoodsReceivingNoteManager GoodsReceivingNoteManager
        {
            get
            {
                return Manager.GoodsReceivingNoteManager;
            }
        }
        public IBrandManager BrandManager
        {
            get
            {
                return Manager.BrandManager;
            }
        }

 
        public IMaterialIssueRequisitionManager MaterialIssueRequisitionManager
        {
            get
            {
                return Manager.MaterialIssueRequisitionManager;
            }
        }
        

        #endregion
	}
}