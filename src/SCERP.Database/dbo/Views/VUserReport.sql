

CREATE view [dbo].[VUserReport]
as 
select ur.UserReportId,cq.CustomSqlQuaryId, cq.Name as ReportName, u.UserName,ur.IsEnable from UserReport as ur

inner join [User] as u on ur.UserName=u.UserName

inner join CustomSqlQuary as cq on ur.SqlQuaryRefId=cq.SqlQuaryRefId

