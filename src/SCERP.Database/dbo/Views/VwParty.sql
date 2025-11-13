
CREATE view [dbo].[VwParty]
as
select P.* ,
(select top(1)AccountName from Acc_GLAccounts where Id=P.KglId) AS KnittingAcName,
(select top(1)AccountName from Acc_GLAccounts where Id=P.KRglId) AS KnittingRcvAcName,
(select top(1)AccountName from Acc_GLAccounts where Id=P.DglId) AS DyeingAcName,
(select top(1)AccountName from Acc_GLAccounts where Id=P.PGlId) AS PrintPayAcName,
(select top(1)AccountName from Acc_GLAccounts where Id=P.EmGlId) AS EmPayAcName
from Party  as P
