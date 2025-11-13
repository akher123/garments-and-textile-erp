CREATE PROCEDURE [dbo].[InvSpDyedYarnStockDetail]

						@SubGroupId int,
						@GroupId int,
                        @FromDate datetime ,
                        @ToDate datetime
						as
	truncate table  Inventory_RStock

INSERT INTO Inventory_RStock
(ItemId, ItemCode, OQty, OAmt, RQty, RAmt, IQty, IAmt,SizeRefId,ColorRefId,BrandId,FColorRefId)
select distinct   SR.ItemId, ItemCode, 0 AS OQty, 0 AS OAmt, 0 AS RQty, 0 AS RAmt, 0 AS IQty, 0 AS IAmt,SR.SizeRefId,SR.ColorRefId,C.ColorCode as BrandId ,ISNULL(SR.FColorRefId,'0000')  as FColorRefId from Inventory_MaterialReceiveAgainstPo as MR
inner join Inventory_MaterialReceiveAgainstPoDetail as SR on MR.MaterialReceiveAgstPoId=SR.MaterialReceiveAgstPoId
inner join OM_Color as C on SR.ColorRefId=C.ColorRefId and SR.CompId=C.CompId
inner join OM_Color as DC on SR.FColorRefId=DC.ColorRefId
inner join OM_Size as s on SR.SizeRefId=s.SizeRefId 
inner join Inventory_Item as I on SR.ItemId=I.ItemId
inner join Inventory_SubGroup as Sg on I.SubGroupId=Sg.SubGroupId
inner join Inventory_Group as g on Sg.GroupId=g.GroupId
where MR.StoreId=1  and MR.RType='D' and (Sg.SubGroupId = @SubGroupId or @SubGroupId=-1)and g.GroupId=@GroupId and I.ItemType=1

Update Inventory_RStock Set OQty= isnull(( SELECT SUM(MRD.ReceivedQty-MRD.RejectedQty) 
FROM Inventory_MaterialReceiveAgainstPoDetail as MRD
inner join Inventory_MaterialReceiveAgainstPo as MR on MRD.MaterialReceiveAgstPoId=MR.MaterialReceiveAgstPoId
inner join OM_Color as CLR on MRD.ColorRefId=CLR.ColorRefId
inner join OM_Color as DC on MRD.FColorRefId=DC.ColorRefId
WHERE  (Convert(date,MR.GrnDate) <Convert(date, @FromDate))  AND (ItemId = Inventory_RStock.ItemId) AND (MRD.ColorRefId = Inventory_RStock.ColorRefId) AND (MRD.SizeRefId = Inventory_RStock.SizeRefId)AND (MRD.FColorRefId = Inventory_RStock.FColorRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)


Update Inventory_RStock Set OAmt= isnull(( SELECT SUM(MRD.ReceivedRate*(MRD.ReceivedQty-MRD.RejectedQty))  
FROM Inventory_MaterialReceiveAgainstPoDetail as MRD
inner join Inventory_MaterialReceiveAgainstPo as MR on MRD.MaterialReceiveAgstPoId=MR.MaterialReceiveAgstPoId
inner join OM_Color as CLR on MRD.ColorRefId=CLR.ColorRefId
inner join OM_Color as DC on MRD.FColorRefId=DC.ColorRefId
WHERE  (Convert(date,MR.GrnDate) <Convert(date, @FromDate))  AND (ItemId = Inventory_RStock.ItemId) AND (MRD.ColorRefId = Inventory_RStock.ColorRefId) AND (MRD.SizeRefId = Inventory_RStock.SizeRefId)AND (MRD.FColorRefId = Inventory_RStock.FColorRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)


Update Inventory_RStock Set OQty=OQty- isnull(( SELECT SUM(MID.IssueQty) 
From  Inventory_AdvanceMaterialIssue as MI 
inner join Inventory_AdvanceMaterialIssueDetail as MID on MI.AdvanceMaterialIssueId=MID.AdvanceMaterialIssueId
inner join OM_Color as CLR on MID.ColorRefId=CLR.ColorRefId
inner join OM_Color as DC on MID.FColorRefId=DC.ColorRefId
WHERE  (Convert(Date,MI.IRNoteDate ) < Convert(Date,@FromDate))  AND (ItemId = Inventory_RStock.ItemId) AND (MID.ColorRefId = Inventory_RStock.ColorRefId) AND (MID.SizeRefId = Inventory_RStock.SizeRefId)  AND (MID.FColorRefId = Inventory_RStock.FColorRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)


Update Inventory_RStock Set OAmt=OAmt- isnull(( SELECT SUM(MID.IssueRate *MID.IssueQty) 
From  Inventory_AdvanceMaterialIssue as MI 
inner join Inventory_AdvanceMaterialIssueDetail as MID on MI.AdvanceMaterialIssueId=MID.AdvanceMaterialIssueId
inner join OM_Color as CLR on MID.ColorRefId=CLR.ColorRefId
inner join OM_Color as DC on MID.FColorRefId=DC.ColorRefId
WHERE  (Convert(Date,MI.IRNoteDate ) < Convert(Date,@FromDate))  AND (ItemId = Inventory_RStock.ItemId) AND (MID.ColorRefId = Inventory_RStock.ColorRefId) AND (MID.SizeRefId = Inventory_RStock.SizeRefId)  AND (MID.FColorRefId = Inventory_RStock.FColorRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)

Update Inventory_RStock Set RQty= isnull(( SELECT SUM(MRD.ReceivedQty-MRD.RejectedQty) 
FROM Inventory_MaterialReceiveAgainstPoDetail as MRD
inner join Inventory_MaterialReceiveAgainstPo as MR on MRD.MaterialReceiveAgstPoId=MR.MaterialReceiveAgstPoId
inner join OM_Color as CLR on MRD.ColorRefId=CLR.ColorRefId
inner join OM_Color as DC on MRD.FColorRefId=DC.ColorRefId
WHERE  (Convert(date,MR.GrnDate) >= convert(date,@FromDate)) and (Convert(date,MR.GrnDate) <=convert(date,@ToDate))  AND (ItemId = Inventory_RStock.ItemId) AND (MRD.ColorRefId = Inventory_RStock.ColorRefId) AND (MRD.SizeRefId = Inventory_RStock.SizeRefId) AND (MRD.FColorRefId = Inventory_RStock.FColorRefId)   AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)

Update Inventory_RStock Set RAmt= isnull(( SELECT SUM(MRD.ReceivedRate*(MRD.ReceivedQty-MRD.RejectedQty))
FROM Inventory_MaterialReceiveAgainstPoDetail as MRD
inner join Inventory_MaterialReceiveAgainstPo as MR on MRD.MaterialReceiveAgstPoId=MR.MaterialReceiveAgstPoId
inner join OM_Color as CLR on MRD.ColorRefId=CLR.ColorRefId
inner join OM_Color as DC on MRD.FColorRefId=DC.ColorRefId
WHERE   (Convert(date,MR.GrnDate)  >= Convert(date,@FromDate)) and (Convert(date,MR.GrnDate)  <=Convert(date,@ToDate))  AND (ItemId = Inventory_RStock.ItemId) AND (MRD.ColorRefId = Inventory_RStock.ColorRefId) AND (MRD.SizeRefId = Inventory_RStock.SizeRefId)  AND (MRD.FColorRefId = Inventory_RStock.FColorRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)


Update Inventory_RStock Set IQty= isnull(( SELECT SUM(MID.IssueQty) 
From  Inventory_AdvanceMaterialIssue as MI 
inner join Inventory_AdvanceMaterialIssueDetail as MID on MI.AdvanceMaterialIssueId=MID.AdvanceMaterialIssueId
inner join OM_Color as CLR on MID.ColorRefId=CLR.ColorRefId
inner join OM_Color as DC on MID.FColorRefId=DC.ColorRefId
WHERE  (Convert(Date,MI.IRNoteDate ) >= Convert(Date,@FromDate)) and (Convert(Date,MI.IRNoteDate)  <=Convert(Date,@ToDate))  AND (ItemId = Inventory_RStock.ItemId) AND (MID.ColorRefId = Inventory_RStock.ColorRefId) AND (MID.SizeRefId = Inventory_RStock.SizeRefId)  AND (MID.FColorRefId = Inventory_RStock.FColorRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)


Update Inventory_RStock Set IAmt= isnull(( SELECT SUM(MID.IssueRate *MID.IssueQty) 
From  Inventory_AdvanceMaterialIssue as MI 
inner join Inventory_AdvanceMaterialIssueDetail as MID on MI.AdvanceMaterialIssueId=MID.AdvanceMaterialIssueId
inner join OM_Color as CLR on MID.ColorRefId=CLR.ColorRefId
inner join OM_Color as DC on MID.FColorRefId=DC.ColorRefId
WHERE   (Convert(Date,MI.IRNoteDate ) >= Convert(Date,@FromDate)) and (Convert(Date,MI.IRNoteDate)  <=Convert(Date,@ToDate))  AND (ItemId = Inventory_RStock.ItemId) AND (MID.ColorRefId = Inventory_RStock.ColorRefId) AND (MID.SizeRefId = Inventory_RStock.SizeRefId)  AND (MID.FColorRefId = Inventory_RStock.FColorRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)


select R.ItemId,R.ItemCode, R.OQty, R.OAmt, R.RQty, R.RAmt, R.IQty,IAmt,C.ColorName,SZ.SizeName,BR.Name as Brand, I.ItemName,
(select ColorName from OM_Color where ColorRefId=R.FColorRefId) as GenericName,
 MU.UnitName,SG.SubGroupName,G.GroupName From Inventory_RStock as R
inner join  Inventory_Item as I  on R.ItemId=I.ItemId
inner join  OM_Color as C on R.ColorRefId=C.ColorRefId 

inner join OM_Size as SZ on R.SizeRefId=SZ.SizeRefId
inner join Inventory_Brand as BR on R.BrandId=BR.BrandId 
inner join Inventory_SubGroup as SG on SG.SubGroupId=I.SubGroupId
inner join Inventory_Group as G on G.GroupId=SG.GroupId

left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId

where R.ItemCode=I.ItemCode and I.ItemType=1 
and (R.OQty+R.RQty+ R.IQty>0)
order by I.ItemName

--exec [InvSpDyedYarnStockDetail]23, '19','2017-07-01','2017-08-30'










