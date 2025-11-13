using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SCERP.Model
{
    public  class Acc_ControlAccounts:BaseModel
    {
        public int Id { get; set; }
        public decimal ParentCode { get; set; }
        public decimal ControlCode { get; set; }
        public string ControlName { get; set; }
        public Nullable<int> ControlLevel { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public ICollection<Acc_ControlAccounts> Chailds { get; set; }
        public Acc_ControlAccounts()
        {
            Chailds = new Collection<Acc_ControlAccounts>();
        }
    }
}
