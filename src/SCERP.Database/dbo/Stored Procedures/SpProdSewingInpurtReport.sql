CREATE procedure [dbo].[SpProdSewingInpurtReport]
@SewingInputProcessId bigint,
@CompId varchar(3)
as 
select IP.SewingInputProcessRefId ,
 OST.BuyerName,
 OST.RefNo as OrderName,
 OST.StyleName,
 IP.InputDate,
 IP.BatchNo,
 IP.JobNo,
 H.HourName,
 IP.Remarks,
 M.Name as Line,
 C.ColorName,
 (select top(1)  ct.CountryName from OM_BuyOrdShip as sh
 inner join Country as ct on sh.CountryId=ct.Id
    where sh.OrderShipRefId=IP.OrderShipRefId and CompId=IP.CompId ) as WIP,
   IIF(len(BSS.SizeRow)=1,'0'+cast(BSS.SizeRow as char(2)),cast(BSS.SizeRow as char(2))) +'---'+S.SizeName as SizeName, 
 IPD.InputQuantity,
 E.Name as IssuedByName ,
 CP.Name as CompanyName,
 CP.FullAddress
 from PROD_SewingInputProcess as IP
inner join PROD_SewingInputProcessDetail as IPD on IP.SewingInputProcessId=IPD.SewingInputProcessId
inner join OM_Color as C on IP.ColorRefId=C.ColorRefId and IP.CompId=C.CompId
inner join OM_Size as S on IPD.SizeRefId=S.SizeRefId and IP.CompId=S.CompId
inner join Production_Machine as M on IP.LineId=M.MachineId 
inner join PROD_Hour as H on IP.HourId=H.HourId
inner join VOM_BuyOrdStyle as OST on IP.OrderStyleRefId=OST.OrderStyleRefId and IP.CompId=OST.CompId
inner join Employee as E on IP.PreparedBy=E.EmployeeId 
inner join Company as CP on IP.CompId=CP.CompanyRefId
inner join OM_BuyOrdStyleSize as BSS on IP.OrderStyleRefId=BSS.OrderStyleRefId and IPD.SizeRefId=BSS.SizeRefId  and IP.CompId=BSS.CompId
where IP.SewingInputProcessId=@SewingInputProcessId and IP.CompId=@CompId




