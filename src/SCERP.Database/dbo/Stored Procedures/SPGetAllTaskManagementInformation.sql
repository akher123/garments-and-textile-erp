CREATE PROCEDURE [dbo].[SPGetAllTaskManagementInformation]

AS
BEGIN
Select M.ModuleName, S.SubjectName,T.TaskName,T.AssignDate,T.EndDate,A.Name as Assignee,ST.Name as StatusName,A.AssigneeId,ST.StatusId from TmTask  AS T
INNER JOIN TmSubject AS S
ON T.SubjectId=S.SubjectId
INNER JOIN TmModule AS M
ON T.ModuleId=M.ModuleId
inner join TmAssignee as A on T.AssigneeId=A.AssigneeId
inner join TmStatus as ST on T.TaskStatus=ST.StatusId
END

--Exec SPGetAllTaskManagementInformation
--Created by Sayeed