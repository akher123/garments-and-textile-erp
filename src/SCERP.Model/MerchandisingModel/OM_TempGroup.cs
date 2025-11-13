using System;
using System.Collections.Generic;

namespace SCERP.Model
{
    public partial class OM_TempGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OM_TempGroup()
        {
            this.OM_CostSheetTemplate = new HashSet<OM_CostSheetTemplate>();
        }
    
        public int TempGroupId { get; set; }
        public string TempGroupName { get; set; }
        public string CompId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OM_CostSheetTemplate> OM_CostSheetTemplate { get; set; }
    }
}
