-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================

-- SELECT [dbo].[udfDateDiffInMonths] ('2016-09-01','2017-08-31')
CREATE FUNCTION [dbo].[udfDateDiffInMonths] 
(
	@datefrom DATETIME, 
	@dateto DATETIME
)

RETURNS INT
AS
BEGIN

	SET @dateto = DATEADD(DAY, 1, @dateto);

    DECLARE @Years INT, @Months INT, @Days INT
    SET @Years = DATEDIFF(YEAR, @datefrom, @dateto)
    IF DATEADD(YY, @Years, @datefrom) > @dateto
    BEGIN
        SET @Years = @Years - 1
    END
    SET @datefrom = DATEADD(YY, @Years, @datefrom)
    
    SET @Months = DATEDIFF(MM, @datefrom, @dateto)
    IF DATEADD(MM, @Months, @datefrom) > @dateto
    BEGIN
        SET @Months = @Months - 1
    END
    SET @datefrom = DATEADD(MM, @Months, @datefrom)
    
    SET @Days = DATEDIFF(DD, @datefrom, @dateto)

	IF(@Years>=1)

	BEGIN
		SET @Months = @Months + (@Years * 12)
	END

	IF(@Days = 30)
	
	BEGIN
		 SET @Days = 0
		 SET @Months = @Months + 1
	END

	IF(@Days = 31)
	
	BEGIN
		 SET @Days = 0
		 SET @Months = @Months + 1
	END

    RETURN @Months

END
