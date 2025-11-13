using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model
{
    public class EmployeePersonalInfoViewModel : BaseModel
    {
        public System.Guid EmployeeId { get; set; }
        public string SpousesName { get; set; }

        public MaritalStatusEnum MaritalStatusEnum { get; set; }
        public Nullable<byte> MaritalStatus { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> MarriageAnniversary { get; set; }
       

        public string Nationality { get; set; }

        public string NationalId { get; set; }

        public string BirthRegistrationNo { get; set; }

        public ReligionEnum ReligionEnum { get; set; }
        public Nullable<byte> Religion { get; set; }

        public GenderEnum GenderEnum { get; set; }

        public string TaxIdentificationNo { get; set; }
        public string DrivingLicenseNo { get; set; }

        public string PassportNo { get; set; }

        //public BloodGroupEnum BloodGroupEnum { get; set; }
        public Nullable<byte> BloodGroup { get; set; }

    }
}
