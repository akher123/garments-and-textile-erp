Create View VwStanderdMinValDetail
as
select SMV.*,SP.SubProcessName from PROD_StanderdMinValDetail as SMV
inner join PROD_SubProcess as SP on SMV.SubProcessRefId=SP.SubProcessRefId and SMV.CompId=SP.CompId