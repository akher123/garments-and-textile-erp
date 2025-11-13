using System;

namespace SCERP.Model.UserRightManagementModel
{
    public class UserMerchandiser
    {
        public int UserMerchandiserId { get; set; }
        public string CompId { get; set; }
        public string MerchandiserRefId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
