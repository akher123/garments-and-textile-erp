-- =================================================================================
-- Author:		<Shahanur Islam>
-- Create date: <2016-05-01>
-- Description:	<> EXEC SPTB  '1','2016-07-01','2017-06-30'
-- =================================================================================

CREATE PROCEDURE [dbo].[SPTB]

						@CompID char(3),
						@SDate DATETIME,
						@EDate DATETIME

AS
BEGIN
	
	SET NOCOUNT ON;
		
		DECLARE  @yDate DATETIME
		set @yDate = @SDate + 1


		Delete From RTrialBalance where SectorId = @CompID

		INSERT INTO RTrialBalance ( SectorId, AccountCode, AccountName, OpAmt, DrAmt, CrAmt, GrID ) 
		SELECT M.SectorId, A.AccountCode, A.AccountName, 0.00 AS OpAmt, ISNULL(SUM(D.Debit), 0) AS DrAmt, 
		ISNULL(SUM(D.Credit), 0) AS CrAmt, '5' AS GrID FROM Acc_VoucherDetail AS D INNER JOIN Acc_VoucherMaster 
		AS M ON D.RefId = M.Id INNER JOIN Acc_GLAccounts AS A ON D.GLID = A.Id WHERE M.SectorId = @CompID AND 
		M.VoucherDate >= @SDate AND M.VoucherDate <= @EDate GROUP BY M.SectorId, A.AccountCode, A.AccountName


		INSERT INTO RTrialBalance ( SectorId, AccountCode, AccountName, OpAmt, DrAmt, CrAmt, GrID ) 
		SELECT 1 as SectorId, AccountCode, AccountName,  0 AS OpAmt, 0 AS DrAmt, 0 AS CrAmt, '5' as GrID FROM 
		Acc_GLAccounts WHERE  AccountCode NOT IN ( SELECT AccountCode FROM RTrialBalance AS 
		RTrialBalance_1 WHERE SectorId = @CompID  )

		Update RTrialBalance Set OpAmt = OpAmt + isnull(( SELECT ISNULL(SUM(D.Debit), 0) - ISNULL(SUM(D.Credit), 0) AS OpAmt
		FROM Acc_VoucherDetail AS D INNER JOIN Acc_VoucherMaster AS M ON D.RefId = M.Id INNER JOIN Acc_GLAccounts AS A 
		ON D.GLID = A.Id WHERE M.SectorId = @CompID AND M.VoucherDate < @SDate AND M.SectorId = @CompID AND 
		A.AccountCode = RTrialBalance.AccountCode ),0) where SectorId=@CompID
		
		Delete From RTrialBalance where OpAmt = 0 and DrAmt=0 and CrAmt=0 and SectorId= @CompID
		
		INSERT INTO RTrialBalance ( SectorId, AccountCode, AccountName, OpAmt, DrAmt, CrAmt, GrID ) 
		SELECT R.SectorId, G.ControlCode, '-' as ControlName, ISNULL(SUM(R.OpAmt), 0) AS OpAmt, 
		ISNULL(SUM(R.DrAmt), 0) AS DrAmt, ISNULL(SUM(R.CrAmt), 0) AS CrAmt, '4' AS GrID FROM RTrialBalance AS R 
		INNER JOIN Acc_GLAccounts AS G ON R.AccountCode = G.AccountCode INNER JOIN Acc_ControlAccounts AS A 
		ON G.Id = A.Id where R.SectorId = @CompID GROUP BY R.SectorId, G.ControlCode 

		INSERT INTO RTrialBalance ( SectorId, AccountCode, AccountName, OpAmt, DrAmt, CrAmt, GrID ) 
		SELECT R.SectorId, A.ParentCode AS AccountCode, '-' AS AccountName, ISNULL(SUM(R.OpAmt), 0) AS OpAmt, 
		ISNULL(SUM(R.DrAmt), 0) AS DrAmt, ISNULL(SUM(R.CrAmt), 0) AS CrAmt, '3' AS GrID FROM RTrialBalance AS R 
		INNER JOIN Acc_ControlAccounts AS A ON R.AccountCode = A.ControlCode WHERE R.SectorId = @CompID AND 
		R.GrID = '4' GROUP BY R.SectorId, A.ParentCode

		INSERT INTO RTrialBalance ( SectorId, AccountCode, AccountName, OpAmt, DrAmt, CrAmt, GrID ) 
		SELECT R.SectorId, A.ParentCode AS AccountCode, '-' AS AccountName, ISNULL(SUM(R.OpAmt), 0) AS OpAmt, 
		ISNULL(SUM(R.DrAmt), 0) AS DrAmt, ISNULL(SUM(R.CrAmt), 0) AS CrAmt, '2' AS GrID FROM RTrialBalance AS R 
		INNER JOIN Acc_ControlAccounts AS A ON R.AccountCode = A.ControlCode WHERE R.SectorId = @CompID AND 
		R.GrID = '3' GROUP BY R.SectorId, A.ParentCode

		INSERT INTO RTrialBalance ( SectorId, AccountCode, AccountName, OpAmt, DrAmt, CrAmt, GrID ) 
		SELECT R.SectorId, A.ParentCode AS AccountCode, '-' AS AccountName, ISNULL(SUM(R.OpAmt), 0) AS OpAmt, 
		ISNULL(SUM(R.DrAmt), 0) AS DrAmt, ISNULL(SUM(R.CrAmt), 0) AS CrAmt, '1' AS GrID FROM RTrialBalance AS R 
		INNER JOIN Acc_ControlAccounts AS A ON R.AccountCode = A.ControlCode WHERE R.SectorId = @CompID AND 
		R.GrID = '2' GROUP BY R.SectorId, A.ParentCode


		Update RTrialBalance Set AccountName= isnull(( SELECT TOP (1) ControlName FROM Acc_ControlAccounts
		WHERE ControlCode = RTrialBalance.AccountCode ),'-')  where SectorId = @CompID and GrID <= '4'

		update RTrialBalance set AccountCode=AccountCode*100 where SectorId = @CompID and GrID = '1'

		update RTrialBalance set AccountCode=AccountCode*100 where SectorId = @CompID and GrID <= '2'
		update RTrialBalance set AccountCode=AccountCode*100 where SectorId = @CompID and GrID <= '3'
		update RTrialBalance set AccountCode=AccountCode*1000 where SectorId = @CompID and GrID <= '4'

		--INSERT INTO RTrialBalance ( CompCode, AcCode, MainCode, SubCode, MCode, Particular, GrID, OpAmt, DrAmt, CrAmt) 
		--SELECT CompCode, '99' AS AcCode, '--' AS MainCode, '---' AS SubCode, '000' AS MCode, 'Summary' AS Particular, 
		--'3' AS GrID, ISNULL(SUM(OpAmt), 0) AS OpAmt, ISNULL(SUM(DrAmt), 0) AS DrAmt, ISNULL(SUM(CrAmt), 0) AS CrAmt 
		--FROM RTrialBalance AS RTrialBalance_1 WHERE     (CompCode = @CompID ) AND (GrID = '1') GROUP BY CompCode


END

--Select * From RTrialBalance where SectorId='1' order by AccountCode, GrID