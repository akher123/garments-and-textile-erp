CREATE  procedure spGetLcInfoReport
@LcId int 
as 

select 
(select BuyerName from OM_Buyer where BuyerId=COMMLcInfo.BuyerId) as Buyer,
LcNo ,
LcDate,
LcQuantity,
LcAmount,
( select Sum(CommSalseContact.Quantity) from CommSalseContact where LcId=COMMLcInfo.LcId) as TTLREPSCLCQTY,
( select Sum(CommSalseContact.Amount) from CommSalseContact where LcId=COMMLcInfo.LcId) as TTLREPSCLCVALUE,
( select Sum(InvoiceValue) from CommExport where LcId=COMMLcInfo.LcId) as TTLREPSCLCSHIPMENTVALUE,
(select SUM(RealizedValue) from CommExport where  LcId=COMMLcInfo.LcId) as TTLREPSCLCREALIZEDVALUE,
(select SUM(BbLcAmount) from CommBbLcInfo where  LcRefId=COMMLcInfo.LcId) as BBLCAMOUNT,
(select SUM(CommPackingCredit.Amount) from CommPackingCredit where LcId=COMMLcInfo.LcId) as PCAMOUNTINBDT,
(select SUM(CommPackingCredit.Amount) from CommPackingCredit where LcId=COMMLcInfo.LcId)/83.5 as PCAMOUNTINUSD ,
(select SUM(InvoiceQuantity) from CommExport where LcId=COMMLcInfo.LcId) as FINALSHIPMENTQTY,
(select SUM(InvoiceQuantity) from CommExport where LcId=COMMLcInfo.LcId)-LcQuantity as FINALSHIPMENTQTYSTATUSBETWEENMAINSCLC,
( select Sum(InvoiceValue) from CommExport where LcId=COMMLcInfo.LcId)-LcAmount as FINALSCLCVALUESTATUSFILEWISE,
0 as FINALSCLCVALUESTATUSBANKWISE
 
 from COMMLcInfo where LcId=@LcId


 --execute spGetLcInfoReport 192


--select SUM(RealizedValue) from CommExport where LcId=192



