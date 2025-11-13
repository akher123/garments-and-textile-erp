using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.AccountingModel
{
    public partial class Acc_CostCentreMultiLayer : BaseModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> ItemLevel { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Acc_CostCentreMultiLayer> Chailds { get; set; }

        public Acc_CostCentreMultiLayer()
        {
            Chailds = new Collection<Acc_CostCentreMultiLayer>();
        }
    }
}
