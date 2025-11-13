
-- ===========================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <04/11/2015>
-- Description:	<> exec SPIncomeStatementMfgReport 1, '06/30/2016','03/20/2017'
-- ===========================================================================================
 
CREATE PROCEDURE [dbo].[SPIncomeStatementMfgReport]
						
						 		
					    @CompanySectorId	INT				
					   ,@FromDate			DATETIME
					   ,@ToDate				DATETIME
								 
AS

BEGIN
	
						SET NOCOUNT ON;
							
						DECLARE @A DECIMAL(18,2) = 0.00
						DECLARE @B DECIMAL(18,2) = 0.00		
						DECLARE @C DECIMAL(18,2) = 0.00		
						DECLARE @D DECIMAL(18,2) = 0.00	
						DECLARE @E DECIMAL(18,2) = 0.00	
						DECLARE @F DECIMAL(18,2) = 0.00	
						DECLARE @G DECIMAL(18,2) = 0.00	
						DECLARE @H DECIMAL(18,2) = 0.00	
						DECLARE @I DECIMAL(18,2) = 0.00	
						DECLARE @J DECIMAL(18,2) = 0.00	
						DECLARE @K DECIMAL(18,2) = 0.00		
						DECLARE @L DECIMAL(18,2) = 0.00	 
						DECLARE @M DECIMAL(18,2) = 0.00	
						DECLARE @N DECIMAL(18,2) = 0.00	
						DECLARE @O DECIMAL(18,2) = 0.00	
						DECLARE @P DECIMAL(18,2) = 0.00	
						DECLARE @Q DECIMAL(18,2) = 0.00	

						DECLARE @N2 DECIMAL(18,2) = 0.00
						DECLARE @N3 DECIMAL(18,2) = 0.00	

						CREATE TABLE #LocalTempTable(

							 ControlCode NUMERIC
							,ControlName varchar(200)
							,ClosingBalance NUMERIC(18,2)
						)

						INSERT INTO #LocalTempTable (ControlCode,ControlName,ClosingBalance)						
													select SubGrpControlCode,SubGrpControlName,
													case 
													when SUBSTRING(CAST(ClsControlCode as NCHAR) ,1,1) in('1','4') then 
													sum(cast(OpeningBalance as numeric(18,2)))+sum(Debit)-sum(Credit)
													else sum(cast(OpeningBalance as numeric(18,2)))+sum(Credit)-sum(Debit)
													end as ClosingBalance 
													from (
													select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,
													ControlCodeName,AccountCode,AccountName, 
													isnull(OP.OpeningBalance,0) OpeningBalance 
													,sum(Debit) Debit,sum(Credit) Credit
													from VW_ACC_Transactions A left outer join(
													select A.SectorId OpSectorID,A.GLCode OpGLID, 
													case 
													when SUBSTRING(CAST(AccountCode as NCHAR) ,1,1) in('1','4') 
													then (isnull(B.Debit,0)-isnull(B.Credit,0))+(A.Debit-A.Credit)
													else (isnull(B.Credit,0)-isnull(B.Debit,0))+(A.Credit-A.Debit)
													end as OpeningBalance 
													from (
													select A.SectorId,A.GLCode,A.AccountCode, sum(A.Debit) Debit,sum(A.Credit) Credit from VW_ACC_Transactions A where GLID is not null   and CONVERT(datetime,A.VoucherDate, 103) between CONVERT(datetime,'01/01/2013', 103) and CONVERT(datetime,@FromDate, 103)-1  and A.SectorId = @CompanySectorId group by AccountCode,A.GLCode,A.SectorId ) A left outer join Acc_OpeningClosing B on A.GLCode=B.GlId and A.SectorId=B.SectorId 
													) Op on Op.OpGLID=A.GLCode and Op.OpSectorID=A.SectorId
													where GLID is not null  and CONVERT(datetime,VoucherDate, 103) between CONVERT(datetime,@FromDate, 103) and CONVERT(datetime,@ToDate, 103)  and SectorId=@CompanySectorId group by OP.OpeningBalance,ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,
													SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName,AccountCode,AccountName ) A group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName 
						

								

						DECLARE	@ControlCode		NUMERIC
						DECLARE @ControlName		VARCHAR(200)
						DECLARE @ClosingBalance		NUMERIC(18,2)					
						
						DECLARE @MyCursor CURSOR
						SET @MyCursor = CURSOR FAST_FORWARD
						FOR

						SELECT  ControlCode, ControlName, ClosingBalance
						FROM   #LocalTempTable 

						OPEN @MyCursor
						FETCH NEXT FROM @MyCursor
						INTO @ControlCode, @ControlName, @ClosingBalance
						WHILE @@FETCH_STATUS = 0
						BEGIN

						IF(@ControlCode = 30101)
							SET @A = @ClosingBalance

						IF(@ControlCode = 40101)
							SET @B = @ClosingBalance
									
						IF(@ControlCode = 40103)
							SET @C = @ClosingBalance
					
						IF(@ControlCode = 40102)
							SET @E = @ClosingBalance

						IF(@ControlCode = 40301)
							SET @H = @ClosingBalance

						IF(@ControlCode = 40302) 
							SET @I = @ClosingBalance 

						IF(@ControlCode = 40401) 
							SET @M = @ClosingBalance 

						IF(@ControlCode = 30102) 
							SET @N = @ClosingBalance											
 
						IF(@ControlCode = 40201) 
							SET @N3 = @ClosingBalance


						FETCH NEXT FROM @MyCursor
						INTO @ControlCode, @ControlName, @ClosingBalance
						END
						CLOSE @MyCursor
						DEALLOCATE @MyCursor
																																
																																		
						SET @D = @B + @C					
						SET @F = @D + @E
						SET @G = @A - @F										
						SET @J = @H + @I
						SET @K = @G - @J																				
						SET @O = @K - @M - @N3 + @N 
						SET @P = 0
						SET @Q = @O - @P
																																																														
						
						UPDATE Acc_IncomeStatementMgf 
						SET Amount = @A , Percentage = 100
						WHERE g2 = 'A'
						
						IF(@A = NULL OR @A = 0)
						BEGIN
							SET @A = 1	

							UPDATE Acc_IncomeStatementMgf 
							SET Percentage = 0
							WHERE g2 = 'A'
						END
								
						UPDATE Acc_IncomeStatementMgf
						SET Amount = @B, Percentage = @B/@A*100
						WHERE g2 = 'B'
						
						UPDATE Acc_IncomeStatementMgf
						SET Amount = @C, Percentage = @C/@A*100
						WHERE g2 = 'C'	
							
						UPDATE Acc_IncomeStatementMgf
						SET Amount = @D, Percentage = @D/@A*100
						WHERE g2 = 'D'																										
									
						UPDATE Acc_IncomeStatementMgf
						SET Amount = @E, Percentage = @E/@A*100
						WHERE g2 = 'E'				
									
						UPDATE Acc_IncomeStatementMgf
						SET Amount = @F, Percentage = @F/@A*100
						WHERE g2 = 'F'				
									
						UPDATE Acc_IncomeStatementMgf
						SET Amount = @G, Percentage = @G/@A*100
						WHERE g2 = 'G'												
						
						UPDATE Acc_IncomeStatementMgf
						SET Amount = @H, Percentage = @H/@A*100
						WHERE g2 = 'H'

						UPDATE Acc_IncomeStatementMgf
						SET Amount = @I, Percentage = @I/@A*100
						WHERE g2 = 'I'

						UPDATE Acc_IncomeStatementMgf
						SET Amount = @J, Percentage = @J/@A*100
						WHERE g2 = 'J'

						UPDATE Acc_IncomeStatementMgf
						SET Amount = @K, Percentage = @K/@A*100
						WHERE g2 = 'K'

						UPDATE Acc_IncomeStatementMgf
						SET Amount = @M, Percentage = @M/@A*100
						WHERE g2 = 'M'
						
						UPDATE Acc_IncomeStatementMgf
						SET Amount = @N, Percentage = @N/@A*100
						WHERE g2 = 'N1'
						
						UPDATE Acc_IncomeStatementMgf
						SET Amount = @N3, Percentage = @N/@A*100
						WHERE g2 = 'N3'

						UPDATE Acc_IncomeStatementMgf
						SET Amount = @O, Percentage = @O/@A*100
						WHERE g2 = 'O'

						UPDATE Acc_IncomeStatementMgf
						SET Amount = @P, Percentage = @P/@A*100
						WHERE g2 = 'P'
						
						UPDATE Acc_IncomeStatementMgf
						SET Amount = @Q, Percentage = @Q/@A*100
						WHERE g2 = 'Q'
						
						DECLARE @CompanySectorName NVARCHAR(100)	
						
						SELECT @CompanySectorName = Acc_CompanySector.SectorName from Acc_CompanySector WHERE Acc_CompanySector.Id = @CompanySectorId	
															
						SELECT        Id
									 ,g1 
									 ,g2
									 ,Particulars
									 ,Amount
									 ,Percentage
									 ,CompanySectorId
								     ,CreatedDate
									 ,CreatedBy
									 ,EditedDate
								     ,EditedBy
								     ,IsActive
									 ,CONVERT(VARCHAR(10), @FromDate, 103) AS FromDate
									 ,CONVERT(VARCHAR(10), @toDate, 103) AS ToDate
									 ,@CompanySectorName AS CompanySector
						FROM          Acc_IncomeStatementMgf
						WHERE		  IsActive = 1
						ORDER BY g2
																										 													
END