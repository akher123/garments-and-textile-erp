using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model.Production;

namespace SCERP.Model.CommonModel
{
    public class Color:SearchModel<Color>
    {
        public Color()
        {
            this.Pro_Batch = new HashSet<Pro_Batch>();
        }
        public long ColorId { get; set; }
        public string ColorRef { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        public virtual ICollection<Pro_Batch> Pro_Batch { get; set; }
    }
}
