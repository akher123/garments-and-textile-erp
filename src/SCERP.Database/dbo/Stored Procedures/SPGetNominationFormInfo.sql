-- ====================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <28/07/2018>
-- Description:	<> EXEC SPGetNominationFormInfo '9BC5D1C6-0AD1-4D9E-A7F3-03CA3B155BC8', 'superadmin','2018-04-10' 
-- ====================================================================================================================

CREATE PROCEDURE [dbo].[SPGetNominationFormInfo]
			

							 @employeeId UNIQUEIDENTIFIER
							,@userName NVARCHAR(100)
							,@prepareDate DATETIME

AS
BEGIN
	
			SET NOCOUNT ON;

			DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);			
			DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);

			INSERT INTO @ListOfBranchUnitDepartmentIds
			SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @userName;

			INSERT INTO @ListOfEmployeeTypeIds
			SELECT DISTINCT EmployeeTypeId FROM UserPermissionForEmployeeLevel
			WHERE UserName = @UserName;

			DECLARE @FromDate DATETIME;
			SELECT @FromDate=JoiningDate FROM Employee employee
			WHERE employee.EmployeeId = @employeeId AND employee.IsActive =1 

			SET @FromDate = @prepareDate
			
			DECLARE @WeekEndCount INT = 0;
			DECLARE @OverTimeRate DECIMAL(18,2) = 0.00

			SELECT @WeekEndCount = COUNT(*) FROM Weekends WHERE Weekends.IsActive = 1
			SET @OverTimeRate = dbo.fnGetOverTimeRate(CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)


			SELECT		  Employee.EmployeeId
						 ,Employee.EmployeeCardId
						 ,Employee.NameInBengali 
						 ,Company.NameInBengali AS CompanyName
						 ,Department.NameInBengali AS Department
						 ,Employee.FathersNameInBengali
						 ,Employee.MothersNameInBengali
						 ,Employee.SpousesNameInBengali
						 ,EmployeeDesignation.TitleInBengali AS Designation						
						 ,EmployeeGrade.NameInBengali AS Grade

						 ,employeePresentAddress.MailingAddressInBengali AS PreMailingAddress
						 ,employeePresentAddress.PostOfficeInBengali AS PrePostOffice
						 ,PST.NameInBengali AS PrePolice
						 ,DIS.NameInBengali AS PreDist

						 ,EmployeePermanentAddress.MailingAddressInBengali AS PerMailingAddress
						 ,EmployeePermanentAddress.PostOfficeInBengali AS PerPostOffice
						 ,PSTE.NameInBengali AS PerPolice
						 ,DIST.NameInBengali AS PerDist

						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) JoiningDate
						 ,Employee.JoiningDate AS JoinDateCalculation
						 ,Employee.ConfirmationDate AS ConfirmationDate
						 ,Unit.NameInBengali AS Unit				
						 ,Section.NameInBengali AS Section						 																																	
						 ,EmployeeSalary.BasicSalary
						 ,EmployeeSalary.HouseRent
						 ,EmployeeSalary.MedicalAllowance 
						 ,EmployeeSalary.Conveyance
						 ,EmployeeSalary.FoodAllowance	
						 ,EmployeeSalary.EntertainmentAllowance					
						 ,EmployeeSalary.GrossSalary	
						 ,@WeekEndCount AS Weekend			
						 ,CONVERT(VARCHAR(10),@prepareDate, 103) AS prepareDate
						 ,@OverTimeRate AS OverTimeRate
						 ,CAST(((EmployeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@FromDate, @FromDate))) AS DECIMAL(18,2)) AS EmployeeOTRate
						 ,dbo.fnIntegerToBengaliWords(CONVERT(BIGINT,EmployeeSalary.GrossSalary)) AS AmountInWords 
						 ,SkillSet.Title AS SkillType
						 ,Employee.PhotographPath
						 ,CONVERT(VARCHAR(10),DATEADD(DAY,-7,Employee.JoiningDate), 103) ApplicationDate	
						 ,EmployeeType.Title AS EmployeeType
						 ,CONVERT(VARCHAR(10),Employee.DateOfBirth, 103) DateOfBirth
						 ,MaritalState.TitleInBengali AS MaritalState		
						 ,CAST(DATEDIFF(YEAR, employee.DateOfBirth, employee.JoiningDate) AS NVARCHAR(10)) AS Age
						 ,Gender.TitleInBengali AS Gender
						 																												
FROM			    	Employee AS  employee

						LEFT JOIN (SELECT EmployeeId, FromDate, JobTypeId, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId 

						LEFT JOIN EmployeePresentAddress AS employeePresentAddress  ON employee.EmployeeId = employeePresentAddress.EmployeeId AND employeePresentAddress.[Status] = 1 AND employeePresentAddress.IsActive = 1
						LEFT JOIN District DIS ON employeePresentAddress.DistrictId = DIS.Id 
						LEFT OUTER JOIN PoliceStation PST ON employeePresentAddress.PoliceStationId = PST.Id 
						LEFT JOIN EmployeePermanentAddress AS EmployeePermanentAddress ON employee.EmployeeId = EmployeePermanentAddress.EmployeeId						
						LEFT JOIN District DIST ON EmployeePermanentAddress.DistrictId = DIST.Id 
						LEFT OUTER JOIN PoliceStation PSTE ON EmployeePermanentAddress.PoliceStationId = PSTE.Id 
						INNER JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
						INNER JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
						INNER JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
						INNER JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
						INNER JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
						INNER JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
						INNER JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
						INNER JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
						INNER JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
						INNER JOIN Company  AS company ON branch.CompanyId=company.Id
						LEFT JOIN  SkillSet ON SkillSet.Id = employeeCompanyInfo.JobTypeId AND SkillSet.IsActive = 1

						LEFT JOIN (SELECT EmployeeId, FromDate, BasicSalary, HouseRent, MedicalAllowance, Conveyance, FoodAllowance, EntertainmentAllowance, GrossSalary,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSalary
						FROM EmployeeSalary AS EmployeeSalary 
						WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND EmployeeSalary.IsActive=1) EmployeeSalary 
						ON employee.EmployeeId = EmployeeSalary.EmployeeId AND EmployeeSalary.rowNumSalary = 1

						LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
						LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
						LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
						LEFT JOIN Line line on departmentLine.LineId = line.LineId
						LEFT JOIN MaritalState ON Employee.MaritalStateId = MaritalState.MaritalStateId
						LEFT JOIN Gender ON Employee.GenderId = Gender.GenderId
						
						WHERE employee.IsActive = 1 					
						AND employeeCompanyInfo.rowNum = 1 
						AND ((employee.EmployeeId = @employeeId) OR (@employeeId IS NULL))						
						AND (employeePresentAddress.Status = 1 AND employeePresentAddress.IsActive = 1) 
						AND branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
						AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)					
						ORDER BY EmployeeCardId ASC		
							
END