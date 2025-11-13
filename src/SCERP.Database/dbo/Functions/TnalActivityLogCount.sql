CREATE FUNCTION TnalActivityLogCount
(
    @id int,
	@KeyName varchar(50)
)
 RETURNS varchar(100) 
AS
BEGIN
    Declare @count int;
    SELECT @count =  Count(1) from OM_TnaActivityLog where TnaId=@id and KeyName=@KeyName
	if @count>0
      BEGIN
	  RETURN CAST( @count as varchar)+'*';
      END
	  RETURN  '';

END
