CREATE procedure [dbo].[SpItemRateUpdate]
@Date datetime,
@ItmeId int 
as
declare 
@RQty numeric(18,5),
@RAmt numeric(18,5),
@IQty numeric(18,5),
@IAmt numeric(18,5),
@TQty numeric(18,5),
@TAmt numeric(18,5),
@Rate numeric(18,5)

--declare @xdt as datetime
--set @xdt=@Date
set @Date=DATEADD(DAY, -day(@Date)+1, @Date)
--select @xdt

DECLARE @a INT = 0

WHILE @a <= 30
BEGIN
    
set @Date=DATEADD(DAY, 1, @Date)


select  @IQty=sum(ISNULL(Inventory_StoreLedger.Quantity,0)),@IAmt=sum(ISNULL(Inventory_StoreLedger.Amount,0)) from Inventory_StoreLedger
where Inventory_StoreLedger.TransactionType=2 and Inventory_StoreLedger.ItemId=@ItmeId  and Inventory_StoreLedger.TransactionDate<@Date

select @RQty= sum(ISNULL(Inventory_StoreLedger.Quantity,0)) ,@RAmt=sum(ISNULL(Inventory_StoreLedger.Amount,0)) from Inventory_StoreLedger
where Inventory_StoreLedger.TransactionType=1 and Inventory_StoreLedger.ItemId=@ItmeId  and Inventory_StoreLedger.TransactionDate<=@Date
set @TQty=@RQty-@IQty 
set @TAmt=@RAmt-@IAmt
set @Rate=0
if @TQty <> 0 
BEGIN
--set @Rate=@TAmt/iif(@TQty=0,1,@TQty)

set @Rate=@TAmt/@TQty
update  Inventory_StoreLedger set UnitPrice=@Rate ,Amount=Quantity*@Rate
where Inventory_StoreLedger.TransactionType=2 and Inventory_StoreLedger.TransactionDate=@Date and ItemId=@ItmeId 
SET @a = @a + 1
--print @a
--print '@RQty  ' + cast(@RQty as varchar(10))
--print '@IQty  ' + cast(@IQty as varchar(10))
--print '@RAmt  ' + cast(@RAmt as varchar(10))
--print '@IAmt  ' + cast(@IAmt as varchar(10))
--print '@Rate  ' + cast(@Rate as varchar(10))
--print '@Date  ' + cast(@Date as varchar(12))
--print '------------'
END
ELSE

BEGIN
update  Inventory_StoreLedger set UnitPrice=@Rate ,Amount=Quantity*@Rate
where Inventory_StoreLedger.TransactionType=2 and Inventory_StoreLedger.TransactionDate=@Date and ItemId=@ItmeId 
SET @a = @a + 1
--print @a
--print '@RQty  ' + cast(@RQty as varchar(10))
--print '@IQty  ' + cast(@IQty as varchar(10))
--print '@RAmt  ' + cast(@RAmt as varchar(10))
--print '@IAmt  ' + cast(@IAmt as varchar(10))
--print '@Rate  ' + cast(@Rate as varchar(10))
--print '@Date  ' + cast(@Date as varchar(12))
--print '------------'
END







    --SET @a = @a + 1
	
END



--DECLARE @i INT = 0

--WHILE @i <= 30
--BEGIN
--	print @i
--	set @i=@i+1
--END
