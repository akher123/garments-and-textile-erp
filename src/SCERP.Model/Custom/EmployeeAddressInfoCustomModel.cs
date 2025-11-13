using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;

namespace SCERP.Model.Custom
{
    public class EmployeeAddressInfoCustomModel  
    {
        public EmployeeAddressInfoCustomModel()
        {
            EmployeePresentAddress = new EmployeePresentAddress();
            EmployeePermanentAddress=new EmployeePermanentAddress();

            EmployeePresentAddresses=new List<EmployeePresentAddress>();
            EmployeePermanentAddress=new EmployeePermanentAddress();
        }

        public int EmployeePresentAddressId { get; set; }

        public int EmployeePermanentAddressId { get; set; }

        public EmployeePresentAddress EmployeePresentAddress { get; set; }
        public EmployeePermanentAddress EmployeePermanentAddress { get; set; }

        public List<EmployeePresentAddress> EmployeePresentAddresses { get; set; }
        public List<EmployeePermanentAddress> EmployeePermanentAddresses { get; set; }



        public List<Country> PresentCountries { get; set; }
        public List<SelectListItem> PresentCountrySelectListItem
        {
            get { return new SelectList(PresentCountries, "Id", "CountryName").ToList(); }

        }

        public List<District> PresentDistricts { get; set; }
        public List<SelectListItem> PresentDistrictSelectListItem
        {
            get { return new SelectList(PresentDistricts, "Id", "Name").ToList(); }

        }

        public List<PoliceStation> PresentPoliceStations { get; set; }
        public List<SelectListItem> PresentPoliceStationsSelectListItem
        {
            get { return new SelectList(PresentPoliceStations, "Id", "Name").ToList(); }

        }

        public List<Country> PermanentCountries { get; set; }
        public List<SelectListItem> PermanentCountrySelectListItem
        {
            get { return new SelectList(PermanentCountries, "Id", "CountryName").ToList(); }

        }

        public List<District> PermanentDistricts { get; set; }
        public List<SelectListItem> PermanentDistrictSelectListItem
        {
            get { return new SelectList(PermanentDistricts, "Id", "Name").ToList(); }

        }

        public List<PoliceStation> PermanentPoliceStations { get; set; }
        public List<SelectListItem> PermanentPoliceStationsSelectListItem
        {
            get { return new SelectList(PermanentPoliceStations, "Id", "Name").ToList(); }

        }
       
        public int? EmployeePresentCountryId { get; set; }

        public int? EmployeePresentDistrictId { get; set; }

        public int? EmployeePresentPoliceStationId { get; set; }



        public int? EmployeePermanentCountryId { get; set; }

        public int? EmployeePermanentDistrictId { get; set; }

        public int? EmployeePermanentPoliceStationId { get; set; }


    }
}