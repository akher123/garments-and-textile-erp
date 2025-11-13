CREATE procedure SpGetBankPaymentCheque(
@VoucherMasterId bigint
)
as
select VoucherDate,
dbo.fnNumberToWords(SUM(Acc_VoucherDetail.Debit))
 as  TotalAmountInWord,
SUM(Acc_VoucherDetail.Debit) as Amount,
Acc_GLAccounts.AccountName
from Acc_VoucherMaster
inner join Acc_VoucherDetail on Acc_VoucherMaster.Id=Acc_VoucherDetail.RefId
inner join Acc_GLAccounts on Acc_VoucherDetail.GLID=Acc_GLAccounts.Id
where  LEFT(Acc_GLAccounts.ControlCode,5)!='10207' and Acc_VoucherMaster.VoucherType='BP' and Acc_VoucherMaster.Id= @VoucherMasterId and Acc_VoucherDetail.Debit>0
group by Acc_GLAccounts.AccountName, VoucherDate,TotalAmountInWord 









