
CREATE procedure [dbo].[spTnaHorizontal]
@BuyerRefId varchar(10), 
@OrderNo varchar(12) ,
@OrderStyleRefId varchar(12),
@CompId varchar(3) 
as
DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX);
SET @cols = STUFF((SELECT ',' + QUOTENAME(c.ShortName) 
            FROM     OM_TnaActivity c 
	
			order by c.SLNo
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')
	

set @query = 'SELECT  p.Merchandiser, p.Buyer , p.OrderName as [Order] , p.StyleName as [Style], p.Quantity , p.ShipDate ,' + @cols + ' from 
            (
               select
			    '' <a style="color: purple;cursor: pointer" onclick="AddNewClick(this)" action="/MIS/MisDashboard/TnaDetail?orderStyleRefId=''+VwTnaCTE.OrderStyleRefId+''">''+ VOM_BuyOrdStyle.Merchandiser+''</a>'' as  Merchandiser,
			   
				 VOM_BuyOrdStyle.BuyerName as  Buyer,
			     VOM_BuyOrdStyle.RefNo as  OrderName,
				 	''<a title="TNA REPORT" href="/Planning/TimeAndAction/TnAReport?orderStyleRefId=''+VwTnaCTE.OrderStyleRefId+''" target="_blank">''+ VOM_BuyOrdStyle.StyleName +''</a>''  as  StyleName,
			    VOM_BuyOrdStyle.Quantity as  Quantity,
				Convert(varchar,VOM_BuyOrdStyle.EFD,103)  as  ShipDate,
			    ''<div data-toggle="tooltip" title="''+ ISNULL(VwTnaCTE.UpdateRemarks,''Plan date missing'')+''"> <b style="color:''+VwTnaCTE.AlertColor+''" ><i class="fas fa-circle"></i></b></div>''  as ASDate
                , VwTnaCTE.ShortName
		
                from  VwTnaCTE 
				inner join VOM_BuyOrdStyle on VwTnaCTE.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId
				where VwTnaCTE.ShortName is not null  and (VOM_BuyOrdStyle.BuyerRefId='''+@BuyerRefId+''' or '''+@BuyerRefId+'''=''-1'') and (VOM_BuyOrdStyle.OrderNo='''+@OrderNo+''' or '''+@OrderNo+'''=''-1'') and (VOM_BuyOrdStyle.OrderStyleRefId='''+@OrderStyleRefId+''' or '''+@OrderStyleRefId+'''=''-1'') and VwTnaCTE.CompId='''+@CompId+'''
          
		   ) x
            pivot 
            (
                min(ASDate)
                for ShortName in (' + @cols + ')
            ) p '


execute(@query)






---exec [dbo].[spTnaHorizontal] '090', 'PFL/ORD00817','ST01475' ,'001'


