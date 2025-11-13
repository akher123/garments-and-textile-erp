CREATE VIEW [dbo].[VwSkillMatrixEmployee]
AS
SELECT S.SkillMatrixId,S.IsActive,S.CompId,E.EmployeeName,EmployeeCardId,E.EmployeeId,E.Designation,E.CompanyName,E.BranchName,E.UnitName,E.DepartmentName,E.SectionName,
E.LineName,
E.GrossSalary 
from HrmSkillMatrix AS S
INNER JOIN VEmployeeCompanyInfoDetail AS E
ON S.EmployeeId=E.EmployeeId AND S.IsActive='True'


