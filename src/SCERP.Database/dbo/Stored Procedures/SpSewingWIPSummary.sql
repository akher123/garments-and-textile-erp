CREATE PROCEDURE [dbo].[SpSewingWIPSummary]
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

select Production_Machine.Name as LINE, Sum(PROD_SewingWIPDetail.IQty) - Sum(PROD_SewingWIPDetail.OQty) as WIP from PROD_SewingWIPDetail
inner join Production_Machine on PROD_SewingWIPDetail.LineId=Production_Machine.MachineId
inner join PROD_CuttingProcessStyleActive as SINCUT on PROD_SewingWIPDetail.OrderStyleRefId=SINCUT.OrderStyleRefId and SINCUT.CompId=@CompId
where     Production_Machine.IsActive =1  and  SINCUT.StartDate<=@ViewDate and (SINCUT.EndDate is null or SINCUT.EndDate>=@ViewDate)  and SINCUT.CompId=@CompId 
group by Production_Machine.Name,Production_Machine.MachineId
order by Production_Machine.MachineId
end
--EXEC SpSewingWIPSummary @ViewDate='2016-11-01', @CompId='001'


