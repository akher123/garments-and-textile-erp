CREATE VIEW [dbo].[VwSkillMatrix]
AS
Select D.SkillMatrixDetailId,D.SkillMatrixId,D.ProcessPercentage,D.SkillMatrixGradeId,D.CompId,D.IsActive,SM.EmployeeId,SM.Name AS EmployeeName,SM.Designation,SG.GradeName,SP.SkillMatrixProcessId,SP.ProcessName from HrmSkillMatrixDetail AS D
INNER JOIN HrmSkillMatrix SM
ON D.SkillMatrixId=SM.SkillMatrixId AND D.CompId=SM.CompId
INNER JOIN HrmSkillMatrixGrade SG
ON D.SkillMatrixGradeId=SG.SkillMatrixGradeId AND D.CompId=SG.CompId
INNER JOIN HrmSkillMatrixProcess AS SP
ON D.SkillMatrixProcessId=SP.SkillMatrixProcessId AND D.CompId=SP.CompId


