-- =======================================================================================================================================
-- Author:		Golam Rabbi
-- Create date: 2015.04.18
-- Description:	To Get All Branch Info
-- Exec SPGetAllBranchInfo null, null, 'superadmin', 0, 2, 1,2,1
-- =======================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetAllBranchInfo]
	-- Add the parameters for the stored procedure here
	 @CompanyID INT = NULL,
	 @BranchName NVARCHAR(100) = NULL,
	 @UserName NVARCHAR(100),
	 
	 @StartRowIndex INT = NULL,
	 @MaxRows INT = NULL,
	 @SortField INT = NULL,
	 @SortDiriection INT = NULL,
	 @RowCount INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SQL AS NVARCHAR(1000)
	DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
    DECLARE @ListOfBranchIds TABLE(BranchIDs INT);

    -- Insert statements for procedure here

	IF(@CompanyID IS NULL)
	BEGIN
	   INSERT INTO @ListOfCompanyIds
	   SELECT DISTINCT CompanyId FROM UserPermissionForDepartmentLevel
	   WHERE UserName = @UserName;
	END  
	ELSE
	BEGIN
	   INSERT INTO @ListOfCompanyIds VALUES (@CompanyID)
	END


	INSERT INTO @ListOfBranchIds
	SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
	WHERE UserName = @UserName;
 

	DECLARE @ReturnTable TABLE (
								RowID INT NULL,
								BranchId INT NULL,
								BranchName NVARCHAR(100) NULL,
								CompanyName NVARCHAR(100) NULL,
								BranchAddress NVARCHAR(100) NULL,
								PoliceStation NVARCHAR(100) NULL,
								District NVARCHAR(100) NULL,
								PostOffice NVARCHAR(100) NULL,
								PostCode NVARCHAR(100) NULL,
								Phone NVARCHAR(100) NULL,
								Email NVARCHAR(100) NULL,
								Fax NVARCHAR(100) NULL
								);

	INSERT INTO @ReturnTable
	SELECT DISTINCT
				    --ROW_NUMBER() OVER (ORDER BY
								--			 CASE @SortField
								--				WHEN 1 THEN branch.Name 
								--				WHEN 2 THEN company.Name 
								--			 END 
								--			 CASE @SortDir
								--				WHEN 1 THEN ASC
								--				WHEN 2 THEN DESC
								--			 END
								--	  ) AS RowNumber,

					ROW_NUMBER() OVER (ORDER BY
											 CASE WHEN (@SortField = 1 AND @SortDiriection = 1)
												       THEN branch.Name
											 END ASC, 
											 CASE WHEN (@SortField = 1 AND @SortDiriection = 2)
												       THEN branch.Name
											 END DESC,
											 CASE WHEN (@SortField = 2 AND @SortDiriection = 1)
												       THEN company.Name
											 END ASC, 
											 CASE WHEN (@SortField = 2 AND @SortDiriection = 2)
												       THEN company.Name
											 END DESC
									  ) AS RowNumber,
					branch.Id AS BranchId,
					branch.Name AS BranchName,
					company.Name AS CompanyName,						
					branch.FullAddress AS BranchAddress,
					policeStation.Name AS PoliceStation,
					district.Name AS District,
					branch.PostOffice AS PostOffice,
					branch.PostCode AS PostCode,
					branch.Phone AS Phone,
					branch.Email AS Email,
					branch.Fax AS Fax
					FROM Branch branch
					INNER JOIN Company company ON branch.CompanyId = company.Id
					INNER JOIN District district ON branch.DistrictId = district.Id
					INNER JOIN PoliceStation policeStation ON branch.PoliceStationId = policeStation.Id
					WHERE (branch.IsActive = 1 AND company.IsActive =1)
					AND company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds)
					AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
					AND ((branch.Name LIKE '%' + @BranchName + '%') OR (@BranchName IS NULL))
			--ORDER BY BranchName ASC
	
	SELECT @RowCount = COUNT(*) FROM @ReturnTable

	IF(@MaxRows IS NULL)
	BEGIN
		SET @MaxRows = @RowCount
	END
			
	SELECT * FROM @ReturnTable
	WHERE RowID BETWEEN @StartRowIndex + 1 AND @StartRowIndex + @MaxRows
END


