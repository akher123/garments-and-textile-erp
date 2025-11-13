
create view VProcessor 
as 
select p.*,PS.ProcessName from PROD_Processor as P
inner join PLAN_Process as PS on P.ProcessRefId=PS.ProcessRefId and PS.CompId=P.CompId