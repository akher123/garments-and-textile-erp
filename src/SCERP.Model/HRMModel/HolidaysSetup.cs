using System;
using System.ComponentModel.DataAnnotations;


namespace SCERP.Model
{
   public class HolidaysSetup
    {
       public int Id { get; set; }
       [ DataType(DataType.Date)]
       [Required]
       public DateTime StartDate { get; set; }
       [DataType(DataType.Date)]
     
       public DateTime EndDate { get; set; }
       [Required]
       public string Title { get; set; }
       public string Description { get; set; }

    }
}
