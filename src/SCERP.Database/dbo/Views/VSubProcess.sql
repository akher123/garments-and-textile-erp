
create View [dbo].[VSubProcess]
As
select s.SubProcessId,s.CompId,s.SubProcessRefId,s.ProcessRefId,s.SubProcessName,p.ProcessId,p.ProcessName from PROD_SubProcess As s
INNER JOIN PLAN_Process As p
ON s.ProcessRefId=p.ProcessRefId And s.CompId=p.CompId