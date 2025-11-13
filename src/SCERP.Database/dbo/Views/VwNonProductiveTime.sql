
CREATE view [dbo].[VwNonProductiveTime]
as
SELECT  npt.NonProductiveTimeId,
 npt.BuyerRefId,
  npt.OrderNo, 
  npt.OrderStyleRefId,
   npt.MachineId,
    npt.NptRefId,
	 npt.StartTime,
	  npt.EndTime,
	   npt.Solution,
	    npt.Supervisor,
		 npt.DownTimeCategoryId,
		 (select Name from Production_Machine where MachineId=npt.MachineId)as MachineName,
		 ( select CategoryName from PROD_DownTimeCategory where DownTimeCategoryId=npt.DownTimeCategoryId) as CatergoryName,
		  npt.EntryDate, 
		    OST.BuyerName, 
			OST.RefNo as OrderName,
			OST.StyleName,
 npt.ResponsibleDepartment,
  npt.Remarks, 
  npt.CompId,
   npt.Manpower
FROM PROD_NonProductiveTime AS npt
 INNER JOIN VOM_BuyOrdStyle AS OST ON npt.OrderStyleRefId = OST.OrderStyleRefId AND npt.CompId = OST.CompId


					
