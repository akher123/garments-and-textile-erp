CREATE procedure SpGetBuyerOrderShipPivot
@CompId varchar(3),
@OrderShipRefId varchar(8)
as

DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX)

select @cols = STUFF((SELECT distinct ',' + QUOTENAME(S.SizeName) 
                                        from OM_BuyOrdShip as SH
inner join OM_BuyOrdShipDetail as SHD on SH.OrderShipRefId=SHD.OrderShipRefId and SH.CompId=SHD.CompId
inner join OM_Size as S on SHD.SizeRefId=S.SizeRefId and SHD.CompId=S.CompId
inner join OM_Color as C on SHD.ColorRefId=C.ColorRefId and SHD.CompId=C.CompId
where SH.OrderShipRefId=@OrderShipRefId  and SH.CompId=@CompId

            FOR 
			
			XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

set @query = 'SELECT ColorName,' + @cols + '
             from 
             (
                select S.SizeName, C.ColorName, SHD.Quantity
                                                  from OM_BuyOrdShip as SH
inner join OM_BuyOrdShipDetail as SHD on SH.OrderShipRefId=SHD.OrderShipRefId and SH.CompId=SHD.CompId
inner join OM_Size as S on SHD.SizeRefId=S.SizeRefId and SHD.CompId=S.CompId
inner join OM_Color as C on SHD.ColorRefId=C.ColorRefId and SHD.CompId=C.CompId
where SH.OrderShipRefId=''SH/00438''  
            ) x
            pivot 
            (
                max(Quantity)
                for SizeName in (' + @cols + ')
            ) p '

execute(@query)


