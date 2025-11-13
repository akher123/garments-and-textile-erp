
CREATE PROCEDURE [dbo].[SpSewingWIPDetail]
	@ViewDate datetime,
	@CompId varchar(3)
AS
BEGIN
	SET @ViewDate= convert(varchar(10),@ViewDate,120);

	truncate table PROD_SewingWIPDetail;
	INSERT INTO PROD_SewingWIPDetail ( LineId, BuyerRefId, OrderNo, OrderStyleRefId, ColorRefId, IQty, OQty )
SELECT     I.LineId, I.BuyerRefId, I.OrderNo, I.OrderStyleRefId, I.ColorRefId, ISNULL(SUM(D.InputQuantity), 0) AS IQty, 0 AS OQty
FROM         PROD_SewingInputProcess AS I INNER JOIN
                      PROD_SewingInputProcessDetail AS D ON I.SewingInputProcessId = D.SewingInputProcessId AND I.CompId = D.CompId
WHERE     (I.CompId = @CompId ) AND (CONVERT(varchar(10), I.InputDate, 120) <= @ViewDate)
GROUP BY I.LineId, I.BuyerRefId, I.OrderNo, I.OrderStyleRefId, I.ColorRefId ;

INSERT INTO PROD_SewingWIPDetail (LineId, BuyerRefId, OrderNo, OrderStyleRefId, ColorRefId, IQty, OQty)
SELECT     O.LineId, O.BuyerRefId, O.OrderNo, O.OrderStyleRefId, O.ColorRefId, 0 AS IQty, SUM(OD.Quantity) AS OQty
FROM         PROD_SewingOutPutProcessDetail AS OD INNER JOIN
                      PROD_SewingOutPutProcess AS O ON OD.SewingOutPutProcessId = O.SewingOutPutProcessId AND OD.CompId = O.CompId
WHERE     (O.CompId = @CompId) AND (CONVERT(varchar(10), O.OutputDate, 120) <= @ViewDate)
GROUP BY O.LineId, O.OrderStyleRefId, O.ColorRefId, O.BuyerRefId, O.OrderNo;

Select A.LineId, isnull(Production_Machine.Name,'-') as Line ,OM_Buyer.BuyerName, OM_BuyerOrder.RefNo as OrderName ,   isnull(( SELECT     TOP (1) ST.StyleName
FROM         OM_Style AS ST INNER JOIN
                      OM_BuyOrdStyle AS OST ON ST.CompID = OST.CompId AND ST.StylerefId = OST.StyleRefId
WHERE     (OST.OrderStyleRefId = A.OrderStyleRefId)),'-') as StyleName, OM_Color.ColorName, A.WIP From (Select LineId, BuyerRefId, OrderNo, OrderStyleRefId, ColorRefId, Sum(IQty) - Sum(OQty) as WIP
From PROD_SewingWIPDetail Group by LineId, BuyerRefId, OrderNo, OrderStyleRefId, ColorRefId) as A
left join  OM_Buyer on OM_Buyer.CompId=@CompId and OM_Buyer.BuyerRefId=A.BuyerRefID
left join OM_BuyerOrder on OM_BuyerOrder.CompId = @CompId and OM_BuyerOrder.OrderNo=A.OrderNo
left join  Production_Machine on  Production_Machine.CompId=@CompId and  Production_Machine.MachineId=A.LineId
left join OM_Color on OM_Color.CompId=@CompId and OM_Color.ColorRefId=A.ColorRefId
inner join PROD_CuttingProcessStyleActive as SINCUT on A.OrderStyleRefId=SINCUT.OrderStyleRefId and SINCUT.CompId=@CompId
where     WIP <> 0 and Production_Machine.IsActive =1  and       SINCUT.StartDate<=@ViewDate and (SINCUT.EndDate is null or SINCUT.EndDate>=@ViewDate)  and SINCUT.CompId=@CompId 
order by A.LineId
END

--EXEC SpSewingWIPDetail @ViewDate='2016-09-30', @CompId='001'


