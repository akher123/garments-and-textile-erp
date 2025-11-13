using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommonModel
{
   public class Messaging
    {
        public long MessageId { get; set; }
        [Required]
        public string MessageText { get; set; }
        [Required]
        public Guid? SenderId { get; set; }
        [Required]
        public Guid? ReceiverId { get; set; }
        [Required]
        public DateTime? SendTime { get; set; }
        public long? IsViewed { get; set; }
    }
}
