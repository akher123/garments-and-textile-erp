



--select * from [dbo].[VwCommLcInfo]

CREATE view [dbo].[VwCommLcInfo]
as

select LC.*,ISNULL((select SUM(CommPackingCredit.Amount) from CommPackingCredit where LcId=LC.LcId and IsAcive=1),0) as PcAmount

,
(select SUM(Quantity) from CommSalseContact  where LcId=LC.LcId) as ScQty 
,(select SUM(Amount) from CommSalseContact  where LcId=LC.LcId) as ScAmnt,

ISNULL((select SUM(BbLcAmount) 
 from CommBbLcInfo where LcRefId=LC.LcId),0) as BbLcAmount,
ISNULL((select SUM(BbLcQuantity)  from CommBbLcInfo where LcRefId=LC.LcId),0) as BbLcQuantity 
,(select BuyerName from OM_Buyer where BuyerId=LC.BuyerId) as BuyerName,

(select BankName from CommBank where BankId=LC.ReceivingBankId) as RcvBank
from COMMLcInfo  as LC where IsActive=1







