CREATE procedure [dbo].[spAccCashBook]
@FromDate datetime,
@ToDate Datetime,
@GLID int

AS 
BEGIN

	DECLARE @AccountCodeTable TABLE(AccountCode NUMERIC(18,0));

	INSERT INTO @AccountCodeTable
	SELECT AccountCode FROM Acc_GLAccounts_Hidden WHERE IsActive = 1

(select '' as VoucherNo ,'' as VoucherRefNo, '' as VoucherDate,0 as  AccountCode,'Opening Balance' as  AccountName, Convert(varchar(200),gl.AccountCode)+' '+gl.AccountName as Particulars,0 as Debit ,0 as Credit ,SUM(vd.Debit)-SUM(vd.Credit) as OpBalance 
from Acc_VoucherMaster as vm 
inner join Acc_VoucherDetail as vd on vm.Id=vd.RefId
inner join Acc_GLAccounts as gl on vd.GLID = gl.Id AND gl.AccountCode NOT IN (SELECT AccountCode FROM @AccountCodeTable)
where vm.VoucherDate < @FromDate  and gl.Id=@GLID
group by gl.AccountCode,gl.AccountName)
 
union 

(
select  VM.VoucherNo,vm.VoucherRefNo, VM.VoucherDate,gl.AccountCode,gl.AccountName,vd.Particulars,vd.Credit,vd.Debit,0 as OpBalance from Acc_VoucherMaster as vm 
inner join Acc_VoucherDetail as vd on vm.Id=vd.RefId
inner join Acc_GLAccounts as gl on vd.GLID=gl.Id AND gl.AccountCode NOT IN (SELECT AccountCode FROM @AccountCodeTable)
where gl.id <>@GLID and  vm.Id in (select  VM.Id from Acc_VoucherMaster as vm 
inner join Acc_VoucherDetail as vd on vm.Id=vd.RefId
inner join Acc_GLAccounts as gl on vd.GLID=gl.Id
where vm.VoucherDate between @FromDate and @ToDate and gl.Id=@GLID))

END





--exec spAccCashBook '2017-08-02' ,'2017-08-02',156


--select * from Acc_GLAccounts where Id='156'











