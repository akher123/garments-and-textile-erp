

CREATE view [dbo].[VInvItem]
as
select G.GroupId,G.GroupName,G.GroupCode,SG.SubGroupId,SG.SubGroupName,SG.SubGroupCode,
 I.ItemId,I.ItemName,I.ItemCode,I.ReorderLevel,MU.UnitName,MU.UnitId from Inventory_Item as I 

inner join Inventory_SubGroup as SG on I.SubGroupId=SG.SubGroupId
inner join Inventory_Group as G on SG.GroupId=G.GroupId
left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
where G.IsActive=1 and SG.IsActive=1 and I.IsActive=1

