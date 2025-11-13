
-- =====================================================================================================================================================
-- Author : Yasir
-- Create date: 2016-12-03
-- Description:	EXEC SPGetOverTimeAttendanceByTime   1, 1, 1, -1, -1, -1, -1, -1, -1,  'superadmin', '2016-11-30','22:10:00.000','22:30:00.000'
-- =====================================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetOverTimeAttendanceByTime]


								  @CompanyID INT = NULL
								 ,@BranchID INT = NULL
								 ,@BranchUnitID INT = NULL
								 ,@BranchUnitDepartmentID INT = NULL
								 ,@DepartmentSectionId INT = NULL
								 ,@DepartmentLineId INT = NULL
								 ,@EmployeeTypeID INT = NULL
								 ,@EmployeeGradeID INT = NULL
								 ,@EmployeeDesignationID INT = NULL															
								 ,@UserName NVARCHAR(100)
								 ,@FromDate DATETIME = NULL
								 ,@FromTime TIME = NULL
								 ,@ToTime TIME = NULL

AS
BEGIN	
								SET NOCOUNT ON;

												
				SELECT 			 EmployeeInOut.EmployeeId				AS EmployeeId
								,EmployeeInOut.EmployeeCardId			AS EmployeeCardId
								,EmployeeInOut.EmployeeName				AS EmployeeName
								,EmployeeInOut.CompanyName				AS CompanyName
								,EmployeeInOut.BranchName				AS BranchName
								,EmployeeInOut.UnitName					AS UnitName
								,EmployeeInOut.DepartmentName			AS DepartmentName
								,EmployeeInOut.SectionName				AS Section
								,EmployeeInOut.LineName					AS Line
								,EmployeeInOut.EmployeeDesignation		AS Designation
								,CONVERT(VARCHAR(15), EmployeeInOut.TransactionDate, 106) AS Date
								,EmployeeInOut.InTime					AS InTime
								,EmployeeInOut.OutTime					AS OutTime

								FROM EmployeeInOut
																								
								WHERE (EmployeeInOut.IsActive = 1) 															
								AND ((EmployeeInOut.CompanyId = @CompanyID) OR (@CompanyID = -1))
								AND ((EmployeeInOut.BranchId = @BranchID) OR  (@BranchID = -1))
								AND ((EmployeeInOut.BranchUnitId = @BranchUnitID) OR (@BranchUnitID = -1))

								AND ((EmployeeInOut.BranchUnitDepartmentId = @BranchUnitDepartmentID) OR (@BranchUnitDepartmentID = -1))
								AND ((EmployeeInOut.DepartmentSectionId = @DepartmentSectionId) OR (@DepartmentSectionId = -1))
								AND ((EmployeeInOut.DepartmentLineId = @DepartmentLineId) OR (@DepartmentLineId = -1))
								AND ((EmployeeInOut.EmployeeTypeId = @EmployeeTypeID) OR (@EmployeeTypeID = -1))

								AND ((EmployeeInOut.Id = @EmployeeDesignationID) OR (@EmployeeDesignationID = -1))
								AND ((EmployeeInOut.Id = @EmployeeGradeID) OR (@EmployeeGradeID = -1))
							
								AND CAST(EmployeeInOut.TransactionDate AS DATE) = @FromDate											
								AND EmployeeInOut.OutTime BETWEEN @FromTime AND @ToTime

								ORDER BY EmployeeInOut.EmployeeCardId
	
END