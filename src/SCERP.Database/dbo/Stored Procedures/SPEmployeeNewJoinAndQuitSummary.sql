-- ====================================================================================================================
-- Author	   :  Yasir
-- Create date :  2017-11-05
-- Description :  EXEC [SPEmployeeNewJoinAndQuitSummary] 1, 1, 2017, 2017, 09, 10, '2017-08-26', '2017-09-25', ''
-- ====================================================================================================================

CREATE PROCEDURE [dbo].[SPEmployeeNewJoinAndQuitSummary] 


								 @CompanyId			INT
								,@BranchId			INT	
								,@FromYear			INT
								,@ToYear			INT
								,@FromMonth			INT
								,@ToMonth			INT
								,@FromDate			DATETIME
								,@ToDate			DATETIME
								,@UserName			NVARCHAR(100) = ''	

AS
BEGIN

	SET NOCOUNT ON;

						  CREATE TABLE #TempTable  
					      ( 
								Year				INT
							   ,Month				INT
							   ,MonthName			NVARCHAR(100)								   						
							   ,Section				NVARCHAR(100)
							   ,SectionId			INT
							   ,NewJoin				INT
							   ,NewJoinAmount		DECIMAL(18,2)
							   ,Quit				INT
							   ,QuitAmount			DECIMAL(18,2)
							   ,Variance			INT	
							   ,AmountVariance		DECIMAL(18,2)
					       )

						INSERT INTO #TempTable(Year, Month, MonthName, Section, SectionId)
							   SELECT 		
							   Year
							  ,Month
							  ,MonthName
							  ,Section
							  ,SectionId													 
						  FROM [dbo].[EmployeeProcessedSalary]
			  
						  WHERE IsActive = 1
						  AND (Year BETWEEN @FromYear AND @FromYear)
						  AND (Month BETWEEN @FromMonth AND @ToMonth)
						  AND EmployeeCategoryId IN (2,3,4)

						  GROUP BY  Year, Month, MonthName, Section, SectionId	
			  
			  														  											  
						  ----------------------- NOW  UPDATE Each Coloumn -----------------------------	
						  
							UPDATE #TempTable 
							SET #TempTable.NewJoin = NewTable.NewJoin 
								,#TempTable.NewJoinAmount = NewTable.NewGross
							FROM #TempTable INNER JOIN 
							(	   								 
							    SELECT	 Year
										,Month
										,SectionId
										,COUNT(1) AS NewJoin
										,SUM(GrossSalary) AS NewGross
								FROM EmployeeProcessedSalary
								WHERE EmployeeCategoryId IN (3,4)
								AND CAST(JoiningDate AS DATE) BETWEEN CAST(FromDate AS DATE) AND CAST(ToDate AS DATE)								
								GROUP BY Year,Month,SectionId
							) AS NewTable ON #TempTable.SectionId = NewTable.SectionId 
							AND #TempTable.Year = NewTable.Year
							AND #TempTable.Month = NewTable.Month


							UPDATE #TempTable 
							SET  #TempTable.Quit = NewTable.Quit 
								,#TempTable.QuitAmount = NewTable.QuitGross
							FROM #TempTable INNER JOIN 
							(	   								 
							    SELECT	 Year
										,Month
										,SectionId
										,COUNT(1) AS Quit
										,SUM(GrossSalary) AS QuitGross
								FROM EmployeeProcessedSalary
								WHERE EmployeeCategoryId IN (2,4)
								AND CAST(QuitDate AS DATE) BETWEEN CAST(FromDate AS DATE) AND CAST(ToDate AS DATE)
								GROUP BY Year, Month, SectionId
							) AS NewTable ON #TempTable.SectionId = NewTable.SectionId
								AND #TempTable.Year = NewTable.Year
								AND #TempTable.Month = NewTable.Month

	  					  SELECT * FROM #TempTable		
						  ORDER BY Year, Month, Section	  
		
END