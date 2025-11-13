

-- ==============================================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <31/03/2015>
-- Description:	<> exec [InvSpLoanReportItemLedger] '10504','2016-02-01','2016-06-19',8
-- ==============================================================================================================================================

CREATE PROCEDURE [dbo].[InvSpLoanReportItemLedger]

						@ItemId integer,
                        @FromDate datetime ,
                        @ToDate datetime,
						@SupplierId int 
AS
BEGIN
	
	SET NOCOUNT ON;

		truncate table        Inv_ReportItemLeger

declare @opn as float;
declare @opr as float;
declare @opa as float;

set @opr=0;
set @opa=0;

set @opn=0-(select ( SELECT ISNULL(SUM(ISL.Quantity),0) FROM   Inventory_MaterialIssue AS I INNER JOIN
Inventory_StoreLedger AS ISL ON I.MaterialIssueId = ISL.MaterialIssueId
WHERE        (I.IsActive = 1) AND (I.IType IN (3, 4)) AND (ISL.TransactionDate  < @FromDate) AND (I.SupplierId =@SupplierId) AND (ISL.ItemId =@ItemId)
)

-
(
SELECT         ISNULL(SUM(Quantity), 0) AS TRQty
FROM            VItemReceive
WHERE        (ReceiveType = 3) AND (TransactionDate < @FromDate) and SupplierId=@SupplierId and  ItemId=@ItemId))

INSERT INTO Inv_ReportItemLeger
                      (Ref, TransactionDate, TransactionType, Quantity, UnitPrice, Amount, QuantityL, UnitPriceL, AmountL, QuantityB, UnitPriceB, AmountB, TransactionName)
SELECT     ISNULL(Grn.GRNNumber, '') AS Ref, TransactionDate, TransactionType, Quantity, 
                      UnitPrice, Quantity * UnitPrice AS Amount, 0 AS QuantityL, 0 AS UnitPriceL, 0 AS AmountL, 0 AS QuantityB, 0 AS UnitPriceB, 0 AS AmountB, 
                      'Receive' AS TransactionName
FROM         Inventory_StoreLedger as SL

inner join Inventory_GoodsReceivingNote as Grn on SL.GoodsReceivingNoteId=Grn.GoodsReceivingNotesId
inner join Inventory_QualityCertificate as QC on Grn.QualityCertificateId=QC.QualityCertificateId
inner join Inventory_ItemStore as IST on QC.ItemStoreId=IST.ItemStoreId
WHERE IST.IsActive=1 and Grn.IsActive=1 and IST.SupplierId=@SupplierId and  (SL.ItemId = @ItemId) AND (TransactionType = 1) and IST.ReceiveType=3 AND (TransactionDate >=  @FromDate) AND (TransactionDate <=  @ToDate )
 
INSERT INTO Inv_ReportItemLeger
                      (Ref, TransactionDate, TransactionType, Quantity, UnitPrice, Amount, QuantityL, UnitPriceL, AmountL, QuantityB, UnitPriceB, AmountB, TransactionName)
SELECT     ISNULL(MI.IssueReceiveNo, '') AS Ref, TransactionDate, TransactionType, 0 as  Quantity, 
                      0 as UnitPrice, 0 AS Amount, SL.Quantity  AS QuantityL, UnitPrice AS UnitPriceL, SL.Quantity * UnitPrice AS AmountL, 0 AS QuantityB, 0 AS UnitPriceB, 0 AS AmountB, 
                      'Issue' AS TransactionName
FROM         Inventory_StoreLedger  as SL
inner join Inventory_MaterialIssue as MI on SL.MaterialIssueId=MI.MaterialIssueId
WHERE    MI.SupplierId=@SupplierId and (SL.ItemId = @ItemId) AND (SL.TransactionType = 2) and MI.IType in(3,4) AND TransactionDate >=  @FromDate AND (TransactionDate <=  @ToDate)

--INSERT INTO Inv_ReportItemLeger
-- (Ref, TransactionDate, TransactionType, Quantity,  Amount, UnitPrice, QuantityL, UnitPriceL, AmountL, QuantityB, UnitPriceB, AmountB, TransactionName)
-- values(0,
-- '0001-01-01',
-- 0,
-- ((select isnull(sum(isnull(IR.Quantity,0)),0) from Inventory_StoreLedger as IR
--WHERE IR.TransactionType = 1 AND (IR.TransactionDate <@FromDate)   and IR.ItemId=@ItemId)-((select isnull(sum(isnull(IIS.Quantity,0)),0)   from Inventory_StoreLedger as IIS
--WHERE IIS.TransactionType = 2 AND (IIS.TransactionDate <@FromDate)   and IIS.ItemId=@ItemId))), 

--((select isnull(sum(isnull(IR.UnitPrice,0)*ISNULL(IR.Quantity,0)),0) from Inventory_StoreLedger as IR
--WHERE IR.TransactionType = 1 AND (IR.TransactionDate <@FromDate)   and IR.ItemId=@ItemId)-((select Isnull(sum(isnull(IIS.UnitPrice,0)*ISNULL(IIS.Quantity,0)),0) from Inventory_StoreLedger as IIS
--WHERE IIS.TransactionType = 2 AND (IIS.TransactionDate <@FromDate)   and IIS.ItemId=@ItemId ))) ,

--((select isnull(sum(isnull(IR.UnitPrice,0)*ISNULL(IR.Quantity,0)),0) from Inventory_StoreLedger as IR
--WHERE IR.TransactionType = 1 AND (IR.TransactionDate <@FromDate)   and IR.ItemId=@ItemId)-((select Isnull(sum(isnull(IIS.UnitPrice,0)*ISNULL(IIS.Quantity,0)),0) from Inventory_StoreLedger as IIS
--WHERE IIS.TransactionType = 2 AND (IIS.TransactionDate <@FromDate)   and IIS.ItemId=@ItemId))) /( ((select isnull(sum(isnull(IR.Quantity,0)),0) from Inventory_StoreLedger as IR
--WHERE IR.TransactionType = 1 AND (IR.TransactionDate <@FromDate)   and IR.ItemId=@ItemId)-((select isnull(sum(isnull(IIS.Quantity,0)),0)   from Inventory_StoreLedger as IIS
--WHERE IIS.TransactionType = 2 AND (IIS.TransactionDate <@FromDate)   and IIS.ItemId=@ItemId)))+0.01),

--0,
--0,
--0,
--0,
--0,
--0,
--'Balance')
 
INSERT INTO Inv_ReportItemLeger
 (Ref, TransactionDate, TransactionType, Quantity,  Amount, UnitPrice, QuantityL, UnitPriceL, AmountL, QuantityB, UnitPriceB, AmountB, TransactionName)
 values(0,
 '0001-01-01',
 0,
-- ((select isnull(sum(isnull(IR.Quantity,0)),0) from Inventory_StoreLedger as IR
--WHERE IR.TransactionType = 1 AND (IR.TransactionDate <@FromDate)   and IR.ItemId=@ItemId)-((select isnull(sum(isnull(IIS.Quantity,0)),0)   from Inventory_StoreLedger as IIS
--WHERE IIS.TransactionType = 2 AND (IIS.TransactionDate <@FromDate)   and IIS.ItemId=@ItemId))), 

--((select isnull(sum(isnull(IR.UnitPrice,0)*ISNULL(IR.Quantity,0)),0) from Inventory_StoreLedger as IR
--WHERE IR.TransactionType = 1 AND (IR.TransactionDate <@FromDate)   and IR.ItemId=@ItemId)-((select Isnull(sum(isnull(IIS.UnitPrice,0)*ISNULL(IIS.Quantity,0)),0) from Inventory_StoreLedger as IIS
--WHERE IIS.TransactionType = 2 AND (IIS.TransactionDate <@FromDate)   and IIS.ItemId=@ItemId ))) ,

--((select isnull(sum(isnull(IR.UnitPrice,0)*ISNULL(IR.Quantity,0)),0) from Inventory_StoreLedger as IR
--WHERE IR.TransactionType = 1 AND (IR.TransactionDate <@FromDate)   and IR.ItemId=@ItemId)-((select Isnull(sum(isnull(IIS.UnitPrice,0)*ISNULL(IIS.Quantity,0)),0) from Inventory_StoreLedger as IIS
--WHERE IIS.TransactionType = 2 AND (IIS.TransactionDate <@FromDate)   and IIS.ItemId=@ItemId))) /( ((select isnull(sum(isnull(IR.Quantity,0)),0) from Inventory_StoreLedger as IR
--WHERE IR.TransactionType = 1 AND (IR.TransactionDate <@FromDate)   and IR.ItemId=@ItemId)-((select isnull(sum(isnull(IIS.Quantity,0)),0)   from Inventory_StoreLedger as IIS
--WHERE IIS.TransactionType = 2 AND (IIS.TransactionDate <@FromDate)   and IIS.ItemId=@ItemId)))+0.01),
@opn,
@opr,
@opa,
0,
0,
0,
0,
0,
0,
'Balance')

;WITH CTE AS (
SELECT
rownum = ROW_NUMBER() OVER (ORDER BY p.TransactionDate, p.TransactionType, p.Ref),
 P.Quantity, P.QuantityL, P.QuantityB, p.Amount, p.AmountL, p.AmountB
FROM Inv_ReportItemLeger p 

)
UPDATE CTE SET
QuantityB = isnull(( SELECT
sum(prev.Quantity- prev.QuantityL) 
FROM  CTE prev where prev.rownum <= CTE.rownum ),0) ,
AmountB = isnull(( SELECT
sum(prev.Amount- prev.AmountL) 
FROM  CTE prev where prev.rownum <= CTE.rownum ),0)
  ;

 update Inv_ReportItemLeger set Quantity=0,Amount=0,UnitPrice=0,Ref='Oppering'
  where Inv_ReportItemLeger.TransactionType=0


Update  Inv_ReportItemLeger set Invoice = isnull(( SELECT TOP (1) I.InvoiceNo
 FROM Inventory_GoodsReceivingNote AS G 
 INNER JOIN Inventory_QualityCertificate AS Q ON G.QualityCertificateId = Q.QualityCertificateId 
 INNER JOIN Inventory_ItemStore AS I ON Q.ItemStoreId = I.ItemStoreId WHERE (G.GRNNumber = Inv_ReportItemLeger.Ref and I.ReceiveType=3) ),'-')
  where TransactionType=1 

select ISNULL((Select MIR.IssueReceiveNoteNo as IRNNO from Inventory_MaterialIssue as MI
inner join Inventory_MaterialIssueRequisition as MIR on MI.MaterialIssueRequisitionId=MIR.MaterialIssueRequisitionId

where MI.IssueReceiveNo=IL.Ref 
),IL.Ref) as RefNo ,  IL.*, ITM.ItemCode,ITM.ItemName from Inv_ReportItemLeger as IL
inner join Inventory_Item as ITM on ITM.ItemId=@ItemId

order by TransactionDate,TransactionType, Ref

END




