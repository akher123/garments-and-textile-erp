

--    SELECT [dbo].[fnDateConvert]('25/002/18')

CREATE FUNCTION [dbo].[fnDateConvert] (  

					
				   @UserDate NVARCHAR(20)
)

RETURNS DATE

AS BEGIN
					DECLARE @Day NVARCHAR(5)
					DECLARE @Month NVARCHAR(5)
					DECLARE @Year NVARCHAR(5)
					DECLARE @Result DATETIME = NULL
				

					SET @UserDate = REPLACE(@UserDate,'.','/')
					SET @UserDate = REPLACE(@UserDate,'-','/')
			

					SET @Day = SUBSTRING(@UserDate,0, CHARINDEX('/', @UserDate))
					SET @UserDate = SUBSTRING(@UserDate, CHARINDEX('/', @UserDate) + 1, LEN(@UserDate))

					SET @Month = SUBSTRING(@UserDate,0, CHARINDEX('/', @UserDate))

					SET @Year = SUBSTRING(@UserDate, CHARINDEX('/', @UserDate) + 1, LEN(@UserDate))
					IF(LEN(@Year) = 2)
					BEGIN
						SET @Year = '20' + @Year
					END

					IF(ISDATE(CAST(@Year + '/' + @Month + '/' + @Day AS NVARCHAR(20))) = 1)
					BEGIN
						SET @Result = CAST(@Year + '/' + @Month + '/' + @Day AS DATE)					
					END

					ELSE

					BEGIN
						SET @Result = NULL
					END
					
				RETURN @Result
END






