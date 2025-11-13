CREATE function GetLoandIssueQty

(  
	@SupplierId int,
	@itemId INT
)

RETURNS numeric(18,3) 
AS
BEGIN
	-- Declare the return variable here
	DECLARE @IssuQty numeric(18,3) 

	SELECT 
									
		   @IssuQty=(select  ISNULL(SUM(SLS.Quantity),0)  from Inventory_MaterialIssue as MI
           inner join Inventory_StoreLedger as SLS on MI.MaterialIssueId=SLS.MaterialIssueId 
           where SLS.TransactionType=2 and  MI.IType=3 and MI.SupplierId=@SupplierId  and SLS.ItemId=@itemId
           group by MI.SupplierId ,SLS.ItemId)

	       RETURN @IssuQty

END