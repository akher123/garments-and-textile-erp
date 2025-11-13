
CREATE procedure [dbo].[SpGatePass]
@GatePassId int
as
--select 
--gp.RefId,
--gp.BillNo,
--gp.ChallanNo
--,gp.ChallanDate
--,gp.PartyName
--,gp.Through,
--gp.Designation,
--gp.[Address],
--gp.Remarks,
--(select Name from Employee where EmployeeId=gp.CreatedBy) as PreparedByName,
--ISNULL((select Name from Employee where EmployeeId=gp.ApprovedBy),'Not Approved') as ApprovedByName,
--gpd.Item,
--gpd.[Description],
--gpd.Unit,
--gpd.Quantity,
--gpd.Remarks as DtlRemarks
--from GatePass as gp
--inner join GatePassDetail  as gpd on gp.GatePassId=gpd.GatePassId
--where gp.GatePassId=@GatePassId
--order by gpd.GatePassDetailId  

select 
gp.RefId,
gp.BillNo,
gp.ChallanNo
,gp.ChallanDate
,gp.PartyName
,gp.Through,
gp.Designation,
gp.[Address],
gp.Remarks,
(select Name from Employee where EmployeeId=gp.CreatedBy) as PreparedByName,
ISNULL((select Name from Employee where EmployeeId=gp.ApprovedBy),'Not Approved') as ApprovedByName,
gpd.Item,
gpd.[Description],
gpd.Unit,
gpd.Quantity,
gpd.Remarks as DtlRemarks,
gpd.Color,
gpd.FColorName,
gpd.Size,
gpd.Wrapper
,gpd.WrappingQty
,gp.BuyerName,
gp.OrderName,
gp.StyleName,
gp.DriverName,
gp.VehicleNo
from GatePass as gp
inner join GatePassDetail  as gpd on gp.GatePassId=gpd.GatePassId
where gp.GatePassId=@GatePassId
order by gpd.GatePassDetailId  
