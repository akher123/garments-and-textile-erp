CREATE procedure [dbo].[spGetEmployeeList]
@BranchId int ,
@UnitId int ,
@DepartmentId int
as
SELECT		employee.[Status], 
	        employee.EmployeeCardId,
			employee.Name, 
			PrsAddress.MobilePhone,PrsAddress.MailingAddress,PrsAddress.MailingAddressInBengali,
			employee.NameInBengali,
			MothersName,MothersNameInBengali,FathersName,FathersNameInBengali,
			employeeDesignation.TitleInBengali as DegNameInBangla,
			employeeType.TitleInBengali as EmpTypeNameInBangla,
			gender.Title AS Gender, 
			employee.JoiningDate AS JoiningDate, 
			employee.ConfirmationDate AS ConfirmationDate, 
			employee.DateOfBirth AS DateOfBirth, 
			employee.QuitDate, 
			company.Name AS CompanyName,
			company.FullAddress AS CompanyAddress,
			branch.Name AS BranchName,
			branch.NameInBengali AS BranchNameInBengali,
	        unit.NameInBengali as UnitNameInBengali,
			unit.Name AS UnitName,
			ISNULL(department.Name, '-') AS Department, 
			department.NameInBengali as DptNameInBengali,
			section.NameInBengali as SeNameInBengali,
			ISNULL(section.Name, '-') AS Section,
			ISNULL(line.Name, '-') AS Line,
			employeeType.Title AS EmployeeType, 
			employeeGrade.Name AS Grade, 
			religion.Name as Religion,
			employeeGrade.NameInBengali as GradeNameInBengali,
			employeeDesignation.Title AS Designation,
			ISNULL(employeeSalary.GrossSalary, 0) AS GrossSalary, 
			ISNULL(employeeSalary.BasicSalary, 0) AS BasicSalary, 
	        employeeSalary.HouseRent,employeeSalary.MedicalAllowance,employeeSalary.Conveyance,employeeSalary.FoodAllowance,
			employeeCompanyInfo.IsEligibleForOvertime AS OTStatus 				
FROM        dbo.Employee AS employee
			LEFT JOIN
            (SELECT EmployeeId, 
					FromDate, 
				    DesignationId, 
					BranchUnitDepartmentId, 
					DepartmentSectionId, 
					DepartmentLineId, 
					IsEligibleForOvertime, 
					ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
			FROM    EmployeeCompanyInfo 
			WHERE   ((CAST(FromDate AS Date) <= CURRENT_TIMESTAMP) AND (IsActive = 1))) employeeCompanyInfo 
					ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1 
LEFT JOIN BranchUnitDepartment AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId 
LEFT JOIN BranchUnit AS branchUnit ON branchUnitDepartment.BranchUnitId = branchUnit.BranchUnitId 
LEFT JOIN UnitDepartment AS unitDepartment ON branchUnitDepartment.UnitDepartmentId = unitDepartment.UnitDepartmentId 
LEFT JOIN Unit AS unit ON branchUnit.UnitId = unit.UnitId 
LEFT JOIN Department AS department ON unitDepartment.DepartmentId = department.Id 
LEFT JOIN Branch AS branch ON branchUnit.BranchId = branch.Id 
LEFT JOIN Company AS company ON branch.CompanyId = company.Id 
LEFT JOIN DepartmentSection departmentSection ON employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId 
LEFT JOIN Section section ON departmentSection.SectionId = section.SectionId 
LEFT JOIN DepartmentLine departmentLine ON employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId 
LEFT JOIN Line line ON departmentLine.LineId = line.LineId 
LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id 
LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id 
LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id 
left join Religion as religion on employee.ReligionId=religion.ReligionId
left join (select EmployeeId,  MobilePhone,MailingAddress,MailingAddressInBengali from EmployeePresentAddress )  PrsAddress
     on employee.EmployeeId=PrsAddress.EmployeeId
LEFT JOIN (SELECT EmployeeId, 
				  BasicSalary, 
				  GrossSalary, HouseRent,MedicalAllowance,Conveyance,FoodAllowance,
				  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
		  FROM    EmployeeSalary 
		  WHERE   (CAST(FromDate AS Date) <= CURRENT_TIMESTAMP) AND IsActive = 1) employeeSalary 
		  ON      employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1 
INNER JOIN Gender gender ON employee.GenderId = gender.GenderId

where employee.IsActive=1 and (branch.Id=@BranchId or @BranchId=-1)  and (unit.UnitId=@UnitId  or  @UnitId=-1) and (department.Id=@DepartmentId or @DepartmentId=-1)

order by employee.EmployeeCardId

