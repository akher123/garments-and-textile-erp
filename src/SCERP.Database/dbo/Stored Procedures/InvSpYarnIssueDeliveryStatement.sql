CREATE procedure [dbo].[InvSpYarnIssueDeliveryStatement]
@PartyId bigint,
@FromDate date=NULL,
@ToDate date =NULL,
@CompId varchar(3)
as 


select MI.IType , SR.TransactionDate as IDate, P.Name as Party, B.BuyerName, MI.OrderNo,MI.StyleNo,  MI.IRefId ,I.ItemName,I.UnitName,REPLACE(I.SubGroupName,' ','') as SubGroupName,C.ColorName as LotNo,S.SizeName as YarnCount,Br.Name as Brand, SR.Quantity,SR.Rate,(SR.Quantity*SR.Rate) as Amount,CP.Name as CompanyName,CP.FullAddress,CP.ImagePath,ISNULL(MI.ProgramRefId,'--') as ProgramRefId  
,(select top(1) CLR.ColorName 

from Inventory_AdvanceMaterialIssueDetail AS DTL
INNER JOIN OM_Color AS CLR ON DTL.FColorRefId=CLR.ColorRefId AND DTL.CompId=CLR.CompId
where AdvanceMaterialIssueId=MI.AdvanceMaterialIssueId and DTL.ColorRefId=SR.ColorRefId and DTL.SizeRefId=SR.SizeRefId ) AS ColorName
 from Inventory_AdvanceMaterialIssue as MI 
inner join Inventory_StockRegister as SR on MI.AdvanceMaterialIssueId=SR.SourceId
inner join Party as P on MI.PartyId=P.PartyId
inner join VInvItem as I on SR.ItemId=I.ItemId 
inner join OM_Color as C on SR.ColorRefId=C.ColorRefId and SR.CompId=C.CompId
inner join Inventory_Brand as Br on C.ColorCode=Br.BrandId
inner join OM_Size as S on SR.SizeRefId=S.SizeRefId and SR.CompId=S.CompId
left join OM_Buyer as B on MI.BuyerRefId=B.BuyerRefId and MI.CompId=B.CompId
inner join Company as CP on MI.CompId=CP.CompanyRefId
--where P.PartyId=1 and  SR.TransactionType=2 and MI.IType in(1,2,6) and SR.StoreId=1 and MI.StoreId=1 and (MI.PartyId=@PartyId or @PartyId='-1')  and (SR.TransactionDate>=@FromDate and SR.TransactionDate<=@ToDate)
where MI.CompId=@CompId and  SR.TransactionType=2 and MI.IType in(1,2,6) and SR.StoreId=1 and MI.StoreId=1 and (MI.PartyId=@PartyId or @PartyId='-1')  and ( CAST(SR.TransactionDate AS DATE) >= ISNULL(@FromDate,'1900-01-01' )) and (CAST(SR.TransactionDate AS DATE )<= ISNULL(@ToDate,GETDATE()) )
order by MI.IRefId








