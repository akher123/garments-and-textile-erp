using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.PayrollModel
{
    public class BonusRule
    {
        public int BonusRuleId { get; set; }
        public string BonusRoleRefId { get; set; }
		[Required]
		public string CompId { get; set; }
		[Required]
		public DateTime EffectiveDate { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public double MoreThanOneYear { get; set; }
		[Required]
		public double MoreThanSixLessOneYear { get; set; }
		public string Remarks { get; set; }

		public DateTime CreatedDate { get; set; }
		public Guid CreatedBy { get; set; }
		public DateTime? EditedDate { get; set; }
		public Guid? EditedBy { get; set; }
		public string IsProcessed { get; set; }

		

	}
}
