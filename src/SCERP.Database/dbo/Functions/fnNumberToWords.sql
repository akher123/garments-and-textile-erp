CREATE FUNCTION fnNumberToWords

(

    @Number AS BIGINT

) RETURNS VARCHAR(MAX)

AS

BEGIN

    DECLARE @Below20 TABLE (ID INT IDENTITY(0,1), Word VARCHAR(32))

    DECLARE @Below100 TABLE (ID INT IDENTITY(2,1), Word VARCHAR(32))

    DECLARE @BelowHundred AS VARCHAR(126) 

    

    INSERT @Below20 (Word) VALUES ('ZERO')

    INSERT @Below20 (Word) VALUES ('ONE')

    INSERT @Below20 (Word) VALUES ( 'TWO' )

    INSERT @Below20 (Word) VALUES ( 'THREE')

    INSERT @Below20 (Word) VALUES ( 'FOUR' )

    INSERT @Below20 (Word) VALUES ( 'FIVE' )

    INSERT @Below20 (Word) VALUES ( 'SIX' )

    INSERT @Below20 (Word) VALUES ( 'SEVEN' )

    INSERT @Below20 (Word) VALUES ( 'EIGHT')

    INSERT @Below20 (Word) VALUES ( 'NINE')

    INSERT @Below20 (Word) VALUES ( 'TEN')

    INSERT @Below20 (Word) VALUES ( 'ELEVEN' )

    INSERT @Below20 (Word) VALUES ( 'TWELVE' )

    INSERT @Below20 (Word) VALUES ( 'THIRTEEN' )

    INSERT @Below20 (Word) VALUES ( 'FOURTEEN')

    INSERT @Below20 (Word) VALUES ( 'FIFTEEN' )

    INSERT @Below20 (Word) VALUES ( 'SIXTEEN' )

    INSERT @Below20 (Word) VALUES ( 'SEVENTEEN')

    INSERT @Below20 (Word) VALUES ( 'EIGHTEEN' )

    INSERT @Below20 (Word) VALUES ( 'NINETEEN' )

 

    INSERT @Below100 VALUES ('TWENTY')

    INSERT @Below100 VALUES ('THIRTY')

    INSERT @Below100 VALUES ('FORTY')

    INSERT @Below100 VALUES ('FIFTY')

    INSERT @Below100 VALUES ('SIXTY')

    INSERT @Below100 VALUES ('SEVENTY')

    INSERT @Below100 VALUES ('EIGHTY')

    INSERT @Below100 VALUES ('NINETY')

 

    IF @Number > 99

    BEGIN

        SELECT @belowHundred = dbo.fnNumberToWords( @Number % 100)

    END

 

    DECLARE @NumberInWords VARCHAR(MAX)

    SET @NumberInWords  = 

    (

      SELECT

        CASE 

            WHEN @Number = 0 THEN  ''

 

            WHEN @Number BETWEEN 1 AND 19 

                THEN (SELECT Word FROM @Below20 WHERE ID=@Number)

 

            WHEN @Number BETWEEN 20 AND 99

                THEN (SELECT Word FROM @Below100 WHERE ID=@Number/10)+ '-' + dbo.fnNumberToWords( @Number % 10) 

 

            WHEN @Number BETWEEN 100 AND 999 

                THEN (dbo.fnNumberToWords( @Number / 100)) + ' HUNDRED '+ 

                        CASE

                            WHEN @belowHundred <> '' 

                                THEN 'AND ' + @belowHundred else @belowHundred

                        END

 

            WHEN @Number BETWEEN 1000 AND 999999 

                THEN (dbo.fnNumberToWords( @Number / 1000))+ ' THOUSAND '+ dbo.fnNumberToWords( @Number % 1000)  

 

            WHEN @Number BETWEEN 1000000 AND 999999999 

                THEN (dbo.fnNumberToWords( @Number / 1000000)) + ' MILLION '+ dbo.fnNumberToWords( @Number % 1000000) 

 

            WHEN @Number BETWEEN 1000000000 AND 999999999999 

                THEN (dbo.fnNumberToWords( @Number / 1000000000))+' BILLION '+ dbo.fnNumberToWords( @Number % 1000000000) 

            

            ELSE ' INVALID INPUT'

        END

    )

 

    SELECT @NumberInWords = RTRIM(@NumberInWords)

 

    SELECT @NumberInWords = RTRIM(LEFT(@NumberInWords,LEN(@NumberInWords)-1)) WHERE RIGHT(@NumberInWords,1)='-'

 

    RETURN (@NumberInWords)

 

END