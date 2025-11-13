using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Model.Custom
{
    public class BranchViewModel : Branch
    {

        public BranchViewModel()
        {
            Branches = new List<Branch>();
            IsSearch = true;
        }

        public List<Branch> Branches { get; set; }

        public string SearchByBranchName
        {
            get;
            set;
        }

        public int SearchByCompany
        {
            get;
            set;
        }

        //public IEnumerable Companies { get; set; }
        //public IEnumerable<SelectListItem> CompanySelectListItem
        //{
        //    get { return new SelectList(Companies, "CompanyId", "CompanyName").ToList(); }

        //}

        public IEnumerable Companies { get; set; }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "Id", "Name").ToList(); }

        }

        public List<District> Districts { get; set; }
        public IEnumerable<SelectListItem> DistrictSelectListItem
        {
            get { return new SelectList(Districts, "Id", "Name").ToList(); }

        }

        public List<PoliceStation> PoliceStations { get; set; }
        public IEnumerable<SelectListItem> PoliceStationSelectListItem
        {
            get { return new SelectList(PoliceStations, "Id", "Name").ToList(); }

        }

        public string CompanyName
        {
            get; 
            set;
        }

        public string BranchName
        {
            get;
            set;
        }

        public string PoliceStationName
        {
            get;
            set;
        }

        public string DistrictName
        {
            get;
            set;
        }
    }
}