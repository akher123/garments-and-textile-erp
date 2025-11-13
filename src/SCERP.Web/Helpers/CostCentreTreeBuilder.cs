using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using SCERP.Model.AccountingModel;

namespace SCERP.Web.Helpers
{
    public class CostCentreTreeBuilder
    {
        public static List<Acc_CostCentreMultiLayer> GetCostCentreTree(List<Acc_CostCentreMultiLayer> costCentres)
        {

            List<Acc_CostCentreMultiLayer> costCentreList = costCentres;
            List<Acc_CostCentreMultiLayer> treeViews = new List<Acc_CostCentreMultiLayer>();

            foreach (var costCentre in costCentreList)
            {
                if (costCentre != null && costCentre.ParentId == 0)
                {
                    treeViews.Add(costCentre);
                }
                else
                {
                    foreach (var tree in treeViews)
                    {
                        if (tree.ItemId == costCentre.ParentId)
                        {
                            tree.Chailds.Add(costCentre);
                        }
                        else if (tree.Chailds.Count != 0)
                        {
                            foreach (Acc_CostCentreMultiLayer chaild1 in tree.Chailds)
                            {
                                if (chaild1.ItemId == costCentre.ParentId)
                                {
                                    chaild1.Chailds.Add(costCentre);
                                }
                                else if (chaild1.Chailds.Count != 0)
                                {
                                    foreach (var chaild2 in chaild1.Chailds)
                                    {
                                        if (chaild2.ItemId == costCentre.ParentId)
                                        {
                                            chaild2.Chailds.Add(costCentre);
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