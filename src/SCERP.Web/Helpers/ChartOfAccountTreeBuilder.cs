using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;

namespace SCERP.Web.Helpers
{
    public class ChartOfAccountTreeBuilder
    {
        private List<Acc_ControlAccounts> controlAccounts;
        private List<Acc_GLAccounts> glAccountseList;

        public ChartOfAccountTreeBuilder()
        {
            
        }
        public ChartOfAccountTreeBuilder(List<Acc_ControlAccounts> controlAccounts, List<Acc_GLAccounts> glAccountseList):this()
        {
            this.controlAccounts = controlAccounts;
            this.glAccountseList = glAccountseList;
        }
        public IEnumerable<Acc_ControlAccounts> GetAccountChart(string searchKey)
        {
            List<Acc_ControlAccounts> treeViews = controlAccounts
                .Where(x => x.IsActive == true && x.ParentCode == 0).OrderBy(x => x.ControlCode).ToList();

            foreach (var fch in treeViews)
            {

                fch.Chailds = GetFirstChild(fch.ControlCode, searchKey).ToList();
                if (fch.Chailds.Any())
                {
                    yield return fch;
                }

            }

        }

        private IEnumerable<Acc_ControlAccounts> GetFirstChild(decimal controlCode, string searchKey)
        {
            List<Acc_ControlAccounts> schilds = controlAccounts
                .Where(x => x.IsActive == true && x.ParentCode == controlCode).OrderBy(x => x.ParentCode).ToList();
            foreach (var sch in schilds)
            {
                sch.Chailds = GetSChild(sch.ControlCode, searchKey).ToList();
                if (sch.Chailds.Any())
                {
                    yield return sch;
                }

            }
        }

        private IEnumerable<Acc_ControlAccounts> GetSChild(decimal controlCode, string searchKey)
        {
            List<Acc_ControlAccounts> schilds = controlAccounts
                .Where(x => x.IsActive == true && x.ParentCode == controlCode).OrderBy(x => x.ParentCode).ToList();
            foreach (var sch in schilds)
            {
                sch.Chailds = GetTChild(sch.ControlCode, searchKey).ToList();
                if (sch.Chailds.Any())
                {
                    yield return sch;
                }

            }
        }

        private IEnumerable<Acc_ControlAccounts> GetTChild(decimal controlCode, string searchKey)
        {
            List<Acc_ControlAccounts> schilds = controlAccounts
                .Where(x => x.IsActive == true && x.ParentCode == controlCode).OrderBy(x => x.ParentCode).ToList();
            foreach (var sch in schilds)
            {
                sch.Chailds = GetGlChild(sch);
                if (!String.IsNullOrWhiteSpace(searchKey))
                {
                    if (sch.Chailds.Any())
                    {
                        yield return sch;
                    }
                }
                else
                {
                    yield return sch;
                }
              
               
              
            }
        }

        private List<Acc_ControlAccounts> GetGlChild(Acc_ControlAccounts acc)
        {
            List<Acc_ControlAccounts> schilds = glAccountseList
                .Where(x => x.IsActive == true && x.ControlCode == acc.ControlCode && x.AccountCode % 1000 > 0 ).ToList().Select(
                    x => new Acc_ControlAccounts()
                    {
                        Id = x.Id,
                        ParentCode = acc.ParentCode,
                        ControlCode = x.AccountCode,
                        ControlLevel = acc.ControlLevel + 1,
                        SortOrder = acc.SortOrder,
                        ControlName = x.AccountName,
                        IsActive = acc.IsActive,
                    }).OrderBy(x => x.ControlCode).ToList();
            return schilds;


        }
        public  List<Acc_ControlAccounts> GetCartOfAccountTree(List<Acc_ControlAccounts> controlAccounts,
            List<Acc_GLAccounts> glAccountseList)
        {
            List<Acc_ControlAccounts> accControlAccountses = controlAccounts;
            List<Acc_GLAccounts> glAccountses = glAccountseList;
            List<Acc_ControlAccounts> treeViews = new List<Acc_ControlAccounts>();

            foreach (var accControlAccountse in accControlAccountses)
            {
                if (accControlAccountse != null && accControlAccountse.ParentCode == 0)
                {
                    treeViews.Add(accControlAccountse);
                }
                else
                {
                    foreach (var tree in treeViews)
                    {
                        if (tree.ControlCode == accControlAccountse.ParentCode)
                        {
                            tree.Chailds.Add(accControlAccountse);
                        }
                        else if (tree.Chailds.Count != 0)
                        {
                            foreach (Acc_ControlAccounts chaild1 in tree.Chailds)
                            {
                                if (chaild1.ControlCode == accControlAccountse.ParentCode)
                                {
                                    chaild1.Chailds.Add(accControlAccountse);
                                }
                                else if (chaild1.Chailds.Count != 0)
                                {
                                    foreach (var chaild2 in chaild1.Chailds)
                                    {
                                        if (chaild2.ControlCode == accControlAccountse.ParentCode)
                                        {
                                            chaild2.Chailds.Add(accControlAccountse);
                                        }
                                        if (chaild2.Chailds.Count != 0)
                                        {
                                            foreach (var chaild3 in chaild2.Chailds)
                                            {
                                                if (chaild3.ControlLevel == 4 && glAccountses.Any(  x => x.ControlCode == accControlAccountse.ControlCode))
                                                {
                                                    foreach (var accControlAccounts in glAccountses.Where( x => x.ControlCode == accControlAccountse.ControlCode))
                                                    {
                                                        if (chaild3.ControlCode == accControlAccounts.ControlCode && accControlAccounts.AccountCode % 1000 > 0)
                                                        {

                                                            chaild3.Chailds.Add(new Acc_ControlAccounts()
                                                            {
                                                                Id = accControlAccounts.Id,
                                                                ParentCode = chaild3.ParentCode,
                                                                ControlCode = accControlAccounts.AccountCode,
                                                                ControlLevel = chaild3.ControlLevel + 1,
                                                                SortOrder = chaild3.SortOrder,
                                                                ControlName = accControlAccounts.AccountName,
                                                                IsActive = chaild3.IsActive,
                                                            });
                                                        }
                                                    }


                                                }
                                            }
                                        }

                                    }
                                }

                            }
                        }
                    }
                }

            }
            return treeViews;
        }
    }
}