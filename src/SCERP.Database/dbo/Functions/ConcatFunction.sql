CREATE FUNCTION dbo.ConcatFunction
(
  @PartyId bigint
)
RETURNS NVARCHAR(MAX)
WITH SCHEMABINDING 
AS 
BEGIN
  DECLARE @s NVARCHAR(MAX);
 
  SELECT @s = COALESCE(@s + N', ', N'') + Name
    FROM dbo.Party
	WHERE PartyId = @PartyId
	ORDER BY Name;
 
  RETURN (@s);
END
