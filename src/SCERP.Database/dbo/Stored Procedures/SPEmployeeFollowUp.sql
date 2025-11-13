-- ============================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <10/10/2018>
-- Description:	<> EXEC SPEmployeeFollowUp '0835','2018-09-02'
-- ============================================================

CREATE PROCEDURE [dbo].[SPEmployeeFollowUp]
			
							
							   @EmployeeCardId			NVARCHAR(10)	
							  ,@Date					DATETIME
						 		   
AS

BEGIN
	
			SET NOCOUNT ON;

								 SELECT 							
								 Employee.EmployeeCardId				AS EmployeeCardId						
								,Employee.Name							AS EmployeeName				
								,Company.Name							AS Company
								,Branch.Name							AS Branch
								,Unit.Name								AS Unit
								,Department.Name						AS Department
								,Section.Name							AS Section
								,Line.Name								AS Line
								,EmployeeType.Title						AS EmployeeType
								,EmployeeGrade.Name						AS EmployeeGrade
								,EmployeeDesignation.Title				AS EmployeeDesignation
								,CONVERT(VARCHAR(10),Employee.JoiningDate, 103)			AS JoiningDate						
								,CONVERT(VARCHAR(10),Employee.ConfirmationDate, 103)	AS ConfirmationDate
								,CONVERT(VARCHAR(10),Employee.QuitDate, 103)			AS QuitDate
								,qt.Type												AS QuitType
								,employeeSalaryInfo.BasicSalary							AS BasicSalary	
								,employeeSalaryInfo.GrossSalary							AS GrossSalary						
								,ActiveStatus =
									CASE
										WHEN Employee.Status = 1 THEN 'Active'
										WHEN Employee.Status = 2 THEN 'Inactive'
									END
								,Employee.MothersName			AS MothersName
								,Employee.FathersName			AS FathersName
								,Gender.Title					AS GenderName	
								,CONVERT(VARCHAR(10),Employee.DateOfBirth, 103)	AS BirthDate  	
								,PresentAddress.MobilePhone     AS MobilePhone
								,BloodGroup.GroupName			AS BloodGroup		
								,Religion.Name					AS ReligionName
								,MaritalState.Title				AS MaritalState			 
								,CONVERT(VARCHAR(10),Employee.MariageAnniversary, 103)  AS MarriageAnniversaryDate   
								,Country.CountryName			AS CountryName
								,DIST.Name						AS DistrictName
								,Employee.NationalIdNo			AS NationalIdNo
								,Employee.BirthRegistrationNo	AS BirthRegistrationNo
								,Employee.TaxIdentificationNo	AS TaxIdentificationNo
								,EducationLevel.Title			AS EducationLevel

								,(SELECT TOP (1) CONVERT(VARCHAR(10),FromDate, 103)
									FROM EmployeeSalary
									WHERE (EmployeeId = employee.EmployeeId) AND CAST(FromDate AS DATE) <> CAST(employee.JoiningDate AS DATE) AND (IsActive = 1)
									ORDER BY FromDate DESC) AS LastIncrementDate

								,SkillSet.Title AS SkillType
								,DATEDIFF(YEAR, Employee.DateOfBirth, CURRENT_TIMESTAMP)			AS AgeInYear
								,(DATEDIFF(MONTH, Employee.DateOfBirth, CURRENT_TIMESTAMP)%12 )		AS AgeInMonth
								,PermanentAddress.MailingAddress									AS PermanentAddress			
								,(SELECT [DayName] FROM [dbo].[Weekends] WHERE IsActive = 1)		AS Weekend  
								,WorkShift.Name														AS ShiftName
								,(SELECT COUNT(Id) FROM EmployeeInOut WHERE EmployeeId = Employee.EmployeeId AND Status IN('Present','Late'))/18  AS EarnLeave
								,CAST(ROW_NUMBER() OVER(ORDER BY Employee.EmployeeId DESC) AS NVARCHAR(10)) AS RowId

								FROM Employee LEFT JOIN
								EmployeePresentAddress presentAddress ON Employee.EmployeeId = presentAddress.EmployeeId AND presentAddress.IsActive = 1 AND presentAddress.Status = 1 LEFT JOIN
								EmployeePermanentAddress permanentAddress ON Employee.EmployeeId = permanentAddress.EmployeeId AND permanentAddress.IsActive = 1 AND permanentAddress.Status = 1 LEFT JOIN
								District district ON presentAddress.DistrictId = district.Id AND district.IsActive = 1 LEFT JOIN
								PoliceStation policeStation ON presentAddress.PoliceStationId = policeStation.Id AND policeStation.IsActive = 1 LEFT JOIN 
								District DIST ON permanentAddress.DistrictId = DIST.Id AND DIST.IsActive = 1  LEFT JOIN
								PoliceStation PSTE ON permanentAddress.PoliceStationId = PSTE.Id AND PSTE.IsActive = 1  LEFT JOIN		
			
								(SELECT EmployeeId, PunchCardNo, FromDate, JobTypeId, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
								FROM EmployeeCompanyInfo AS employeeCompanyInfo
								WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @Date) OR (@Date IS NULL))
								AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
								ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1  LEFT JOIN   
			
								SkillSet ON SkillSet.Id = employeeCompanyInfo.JobTypeId AND SkillSet.IsActive = 1 LEFT JOIN
                                            
								EmployeeDesignation ON EmployeeCompanyInfo.DesignationId = EmployeeDesignation.Id AND EmployeeDesignation.IsActive = 1 LEFT JOIN
								EmployeeType ON EmployeeDesignation.EmployeeTypeId = EmployeeType.Id AND EmployeeType.IsActive = 1 LEFT JOIN
								EmployeeGrade ON EmployeeDesignation.GradeId = EmployeeGrade.Id AND EmployeeGrade.IsActive = 1  
								LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId 
								LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
								LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
								LEFT JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
								LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
								LEFT JOIN Branch AS branch ON branchUnit.BranchId=branch.Id
								LEFT JOIN Company AS company ON branch.CompanyId = company.Id
								LEFT JOIN DepartmentSection departmentSection ON employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId 
								LEFT JOIN Section section ON departmentSection.SectionId = section.SectionId
								LEFT JOIN DepartmentLine departmentLine ON employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
								LEFT JOIN Line line ON departmentLine.LineId = line.LineId LEFT JOIN
								BloodGroup ON BloodGroup.Id = Employee.BloodGroupId AND BloodGroup.IsActive = 1 LEFT JOIN
								Gender ON Gender.GenderId = Employee.GenderId AND Gender.IsActive = 1 LEFT JOIN
								Religion ON Religion.ReligionId = Employee.ReligionId AND Religion.IsActive = 1 LEFT JOIN
								MaritalState ON MaritalState.MaritalStateId = Employee.MaritalStateId AND MaritalState.IsActive = 1 LEFT JOIN
								Country ON Country.Id = permanentAddress.CountryId AND Country.IsActive = 1 LEFT JOIN

								(SELECT EmployeeEducation.EducationLevelId, EmployeeEducation.EmployeeId, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY EmployeeEducation.EducationLevelId DESC) AS rowNum					
								FROM EmployeeEducation
								WHERE  EmployeeEducation.IsActive = 1) HighestEducationLevel
								ON Employee.EmployeeId = HighestEducationLevel.EmployeeId AND HighestEducationLevel.rowNum = 1
								LEFT OUTER JOIN EducationLevel ON EducationLevel.Id = HighestEducationLevel.EducationLevelId LEFT JOIN

								(SELECT EmployeeId,BasicSalary,GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						
								FROM EmployeeSalary AS employeeSalary
								WHERE ((CAST(employeeSalary.FromDate AS Date) <= @Date) OR (@Date IS NULL))
								AND employeeSalary.IsActive = 1) employeeSalaryInfo 
								ON employee.EmployeeId = employeeSalaryInfo.EmployeeId AND employeeSalaryInfo.rowNumSal = 1

								LEFT JOIN QuitType QT ON employee.QuitTypeId = QT.QuitTypeId	
								LEFT JOIN EmployeeWorkShift ON EmployeeWorkShift.EmployeeId = Employee.EmployeeId AND CAST(	EmployeeWorkShift.ShiftDate AS DATE) = CAST( @Date AS DATE)
								LEFT JOIN BranchUnitWorkShift ON BranchUnitWorkShift.BranchUnitWorkShiftId = EmployeeWorkShift.BranchUnitWorkShiftId
								LEFT JOIN WorkShift ON WorkShift.WorkShiftId = BranchUnitWorkShift.WorkShiftId
											
						        WHERE employee.IsActive = 1								
								AND presentAddress.IsActive = 1
								AND presentAddress.Status = 1
								AND employeeCompanyInfo.rowNum = 1
								AND employeeSalaryInfo.rowNumSal = 1
								AND Employee.EmployeeCardId = @EmployeeCardId
								 				   		
							    ORDER BY employee.EmployeeCardId
					  					  														  						  											  							
END