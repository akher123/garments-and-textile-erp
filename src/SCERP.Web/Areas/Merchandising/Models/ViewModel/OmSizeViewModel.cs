using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class OmSizeViewModel : OM_Size
    {
        public List<OM_Size> Sizes { get; set; }

        public OmSizeViewModel()
        {
            Sizes = new List<OM_Size>();
        }

        public IEnumerable<Dropdown> ItemTypes
        {
            get
            {
                return new[]
                {
                      new Dropdown {Id = "01", Value = "GARMENT"}
                     , new Dropdown {Id ="02", Value = "FABRIC"}
                     , new Dropdown{Id = "03", Value = "ACCESSORY"}
                     , new Dropdown {Id = "04", Value = "PACKING"}
                     , new Dropdown {Id = "05", Value = "YARN"}
                     , new Dropdown{Id = "06", Value = "OTHERS"}

                };
            }
        }

        public IEnumerable<SelectListItem> ItemTypesSelectListItem
        {
            get
            {

                return new SelectList(ItemTypes, "Id", "Value");
            }
        }

        public string TypeName
        {
            get
            {
                return ItemTypes.First(x => x.Id == TypeId).Value.ToUpper();
            }
        }
    }
}