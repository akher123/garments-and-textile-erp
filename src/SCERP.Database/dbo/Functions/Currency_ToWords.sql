Create FUNCTION [dbo].[Currency_ToWords] (
    @Input Numeric (38, 3) -- Input number with as many as 18 digits

) RETURNS VARCHAR(8000)

/*
* Converts a integer number as large as 34 digits into the
* equivalent words.  The first letter is capitalized.
*
* Attribution: Based on NumberToWords by Srinivas Sampath
*        as revised by Nick Barclay
*
* Example:
select dbo.udf_Num_ToWords (1234567890) + CHAR(10)
      +  dbo.udf_Num_ToWords (0) + CHAR(10)
      +  dbo.udf_Num_ToWords (123) + CHAR(10)
select dbo.udf_Num_ToWords(76543210987654321098765432109876543210)
 
DECLARE @i numeric (38,0)
SET @i = 0
WHILE @I <= 1000 BEGIN
    PRINT convert (char(5), @i) 
            + convert(varchar(255), dbo.udf_Num_ToWords(@i))
    SET @I  = @i + 1
END
*
* Published as the T-SQL UDF of the Week Vol 2 #9 2/17/03
****************************************************************/
AS BEGIN
Declare @Number Numeric(38,0)
set @Number = @Input
Declare @Cents as int
set @Cents = 100*Convert(money,(@Input - convert(Numeric(38,3),@Number)))
DECLARE @inputNumber VARCHAR(38)
DECLARE @NumbersTable TABLE (number CHAR(2), word VARCHAR(10))
DECLARE @outputString VARCHAR(8000)
DECLARE @length INT
DECLARE @counter INT
DECLARE @loops INT
DECLARE @position INT
DECLARE @chunk CHAR(3) -- for chunks of 3 numbers
DECLARE @tensones CHAR(2)
DECLARE @hundreds CHAR(1)
DECLARE @tens CHAR(1)
DECLARE @ones CHAR(1)

IF @Number = 0 Return 'Zero'

-- initialize the variables
SELECT @inputNumber = CONVERT(varchar(38), @Number)
     , @outputString = ''
     , @counter = 1
SELECT @length   = LEN(@inputNumber)
     , @position = LEN(@inputNumber) - 2
     , @loops    = LEN(@inputNumber)/3

-- make sure there is an extra loop added for the remaining numbers
IF LEN(@inputNumber) % 3 <> 0 SET @loops = @loops + 1

-- insert data for the numbers and words
INSERT INTO @NumbersTable   SELECT '00', ''
    UNION ALL SELECT '01', 'one'      UNION ALL SELECT '02', 'two'
    UNION ALL SELECT '03', 'three'    UNION ALL SELECT '04', 'four'
    UNION ALL SELECT '05', 'five'     UNION ALL SELECT '06', 'six'
    UNION ALL SELECT '07', 'seven'    UNION ALL SELECT '08', 'eight'
    UNION ALL SELECT '09', 'nine'     UNION ALL SELECT '10', 'ten'
    UNION ALL SELECT '11', 'eleven'   UNION ALL SELECT '12', 'twelve'
    UNION ALL SELECT '13', 'thirteen' UNION ALL SELECT '14', 'fourteen'
    UNION ALL SELECT '15', 'fifteen'  UNION ALL SELECT '16', 'sixteen'
    UNION ALL SELECT '17', 'seventeen' UNION ALL SELECT '18', 'eighteen'
    UNION ALL SELECT '19', 'nineteen' UNION ALL SELECT '20', 'twenty'
    UNION ALL SELECT '30', 'thirty'   UNION ALL SELECT '40', 'forty'
    UNION ALL SELECT '50', 'fifty'    UNION ALL SELECT '60', 'sixty'
    UNION ALL SELECT '70', 'seventy'  UNION ALL SELECT '80', 'eighty'
    UNION ALL SELECT '90', 'ninety'  

WHILE @counter <= @loops BEGIN

    -- get chunks of 3 numbers at a time, padded with leading zeros
    SET @chunk = RIGHT('000' + SUBSTRING(@inputNumber, @position, 3), 3)

    IF @chunk <> '000' BEGIN
        SELECT @tensones = SUBSTRING(@chunk, 2, 2)
             , @hundreds = SUBSTRING(@chunk, 1, 1)
             , @tens = SUBSTRING(@chunk, 2, 1)
             , @ones = SUBSTRING(@chunk, 3, 1)

        -- If twenty or less, use the word directly from @NumbersTable
        IF CONVERT(INT, @tensones) <= 20 OR @Ones='0' BEGIN
            SET @outputString = (SELECT word
                                      FROM @NumbersTable
                                      WHERE @tensones = number)
                   + CASE @counter WHEN 1 THEN '' -- No name
                       WHEN 2 THEN ' thousand ' WHEN 3 THEN ' million '
                       WHEN 4 THEN ' billion '  WHEN 5 THEN ' trillion '
                       WHEN 6 THEN ' quadrillion ' WHEN 7 THEN ' quintillion '
                       WHEN 8 THEN ' sextillion '  WHEN 9 THEN ' septillion '
                       WHEN 10 THEN ' octillion '  WHEN 11 THEN ' nonillion '
                       WHEN 12 THEN ' decillion '  WHEN 13 THEN ' undecillion '
                       ELSE '' END
                               + @outputString
            END
         ELSE BEGIN -- break down the ones and the tens separately

             SET @outputString = ' '
                            + (SELECT word
                                    FROM @NumbersTable
                                    WHERE @tens + '0' = number)
                             + '-'
                             + (SELECT word
                                    FROM @NumbersTable
                                    WHERE '0'+ @ones = number)
                   + CASE @counter WHEN 1 THEN '' -- No name
                       WHEN 2 THEN ' thousand ' WHEN 3 THEN ' million '
                       WHEN 4 THEN ' billion '  WHEN 5 THEN ' trillion '
                       WHEN 6 THEN ' quadrillion ' WHEN 7 THEN ' quintillion '
                       WHEN 8 THEN ' sextillion '  WHEN 9 THEN ' septillion '
                       WHEN 10 THEN ' octillion '  WHEN 11 THEN ' nonillion '
                       WHEN 12 THEN ' decillion '   WHEN 13 THEN ' undecillion '
                       ELSE '' END
                            + @outputString
        END

        -- now get the hundreds
        IF @hundreds <> '0' BEGIN
            SET @outputString  = (SELECT word
                                      FROM @NumbersTable
                                      WHERE '0' + @hundreds = number)
                                + ' hundred '
                                + @outputString
        END
    END

    SELECT @counter = @counter + 1
         , @position = @position - 3

END

-- Remove any double spaces
SET @outputString = LTRIM(RTRIM(REPLACE(@outputString, '  ', ' ')))
SET @outputstring = UPPER(LEFT(@outputstring, 1)) + SUBSTRING(@outputstring, 2, 8000)


RETURN UPPER(@outputString) + ' DOLLARS & ' +convert(Varchar(20),@Cents) + '/100 CENTS'-- return the result
END

--SELECT dbo.[Currency_ToWords] ('4444.44')