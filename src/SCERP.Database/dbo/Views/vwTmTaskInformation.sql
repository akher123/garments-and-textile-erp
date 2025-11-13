

CREATE VIEW [dbo].[vwTmTaskInformation]
AS
SELECT M.ModuleId,M.ModuleName,S.SubjectId,S.SubjectName,T.TaskId,T.TaskName,T.AssignDate,T.EndDate,A.Assignee,TS.TaskStatus,A.AssigneeId,TS.TaskStatusId,TT.TaskTypeId,TT.TaskType FROM TmTask  AS T
INNER JOIN TmSubject AS S ON T.SubjectId=S.SubjectId
INNER JOIN TmModule AS M ON T.ModuleId=M.ModuleId
INNER JOIN TmAssignee AS A ON T.AssigneeId=A.AssigneeId
INNER JOIN TmTaskStatus AS TS ON T.TaskStatusId=TS.TaskStatusId
INNER JOIN TmTaskType AS TT ON T.TaskTypeId=TT.TaskTypeId


