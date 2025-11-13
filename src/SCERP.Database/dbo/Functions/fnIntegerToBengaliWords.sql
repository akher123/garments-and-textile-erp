
----- SELECT dbo.fnIntegerToWords(3562658) 

CREATE FUNCTION [dbo].[fnIntegerToBengaliWords](@Number AS BIGINT) 
    RETURNS NVARCHAR(1024)
AS

BEGIN

      DECLARE @Below100 TABLE (ID INT IDENTITY(0,1), Word NVARCHAR(1024))
      INSERT @Below100 (Word) VALUES 
                        ( N'জিরো' ), ( N'এক' ),( N'দুই' ), ( N'তিন' ),( N'চার' ), ( N'পাঁচ' ), ( N'ছয়' ), ( N'সাত' ),( N'আট' ), ( N'নয়' ), ( N'দশ' ),
                        ( N'এগারো' ),( N'বার' ), ( N'তের' ), ( N'চৌদ্দ' ),( N'পনেরো' ), ( N'ষোল' ), ( N'সতেরো' ),( N'আঠারো' ), ( N'উনিশ' ),( N'বিশ' ),
						( N'একুশ' ),( N'বাইশ' ),( N'তেইশ' ),( N'চব্বিশ' ),( N'পঁচিশ' ),( N'ছাব্বিশ' ),( N'সাতাশ' ),( N'আটাশ' ),( N'ঊনত্রিশ' ),( N'ত্রিশ' ),						
						( N'একত্রিশ' ),( N'বত্রিশ' ),( N'তেত্রিশ' ),( N'চৌত্রিশ' ),( N'পঁয়ত্রিশ' ),( N'ছয়ত্রিশ' ),( N'সাইত্রিশ' ), ( N'আটত্রিশ' ), ( N'উনচল্লিশ' ), ( N'চল্লিশ' ),  
						( N'একচল্লিশ' ),( N'বিয়াল্লিশ' ),( N'তেতাল্লিশ' ),( N'চুয়াল্লিশ' ),( N'পঁয়তাল্লিশ' ),( N'ছিচল্লিশ' ),( N'সাতচল্লিশ' ), ( N'আটচল্লিশ' ), ( N'উনপঞ্চাশ' ), ( N'পঞ্চাশ' ),
						( N'একান্ন' ),( N'বায়ান্ন' ),( N'তিপান্ন' ),( N'চুয়ান্ন' ),( N'পঞ্চান্ন' ),( N'চাপ্পান্ন' ),( N'সাতান্ন' ), ( N'আটান্ন' ), ( N'উনষাট' ), ( N'ষাট' ),
						( N'একষট্টি' ),( N'বাষট্টি' ),( N'তিষট্টি' ),( N'চুষট্টি' ),( N'পয়ষট্টি' ),( N'ছিষট্টি' ),( N'সাতষট্টি' ), ( N'আটষট্টি' ), ( N'উনসত্তুর' ), ( N'সত্তুর' ),
						( N'একাত্তর' ),( N'বাহাত্তর' ),( N'তিয়াত্তর' ),( N'চুয়াত্তর' ),( N'পঁচাত্তর' ),( N'ছিয়াত্তর' ),( N'সাতাত্তর' ), ( N'আটাত্তর' ), ( N'উনআশি'), ( N'আশি' ),
						( N'একাশি' ),( N'বিরাশি' ),( N'তিরাশি' ),( N'চুরাশি' ),( N'পচাশি' ),( N'ছিয়াশি' ),( N'সাতাশি' ), ( N'আটাশি' ), ( N'উনানব্বই' ), ( N'নব্বই' ),
						( N'একানব্বই' ),( N'বিরানব্বই' ),( N'তিরানব্বই' ),( N'চুরানব্বই' ),( N'পচানব্বই' ),( N'ছিয়ানব্বই' ),( N'সাতানব্বই' ), ( N'আটানব্বই' ), ( N'নিরানব্বই' )



      DECLARE @belowHundred AS NVARCHAR(1024) 

      IF @Number > 99 BEGIN
        SELECT @belowHundred = dbo.fnIntegerToBengaliWords( @Number % 100)
      END

    DECLARE @English NVARCHAR(1024) = 
    (
      SELECT Case 
        WHEN @Number = 0 THEN  ''

        WHEN @Number BETWEEN 1 AND 99
          THEN (SELECT Word FROM @Below100 WHERE ID=@Number)

       WHEN @Number BETWEEN 100 AND 999   
         THEN  (dbo.fnIntegerToBengaliWords( @Number / 100)) +N' শত '+
             Case WHEN @belowHundred <> '' THEN  @belowHundred else @belowHundred end 

       WHEN @Number BETWEEN 1000 AND 99999   
         THEN  (dbo.fnIntegerToBengaliWords( @Number / 1000))+N' হাজার '+
             dbo.fnIntegerToBengaliWords( @Number % 1000)  
	 	   
       WHEN @Number BETWEEN 100000 AND 9999999   
         THEN  (dbo.fnIntegerToBengaliWords( @Number / 100000))+N' লাখ '+
             dbo.fnIntegerToBengaliWords( @Number % 100000) 

       WHEN @Number BETWEEN 10000000 AND 999999999   
         THEN  (dbo.fnIntegerToBengaliWords( @Number / 10000000))+N' কোটি '+
             dbo.fnIntegerToBengaliWords( @Number % 10000000) 

        ELSE ' INVALID INPUT' END
    )


    SELECT @English = RTRIM(@English)

    SELECT @English = RTRIM(LEFT(@English,len(@English)-1))
        WHERE RIGHT(@English,1)='-'


    RETURN (@English)

END 