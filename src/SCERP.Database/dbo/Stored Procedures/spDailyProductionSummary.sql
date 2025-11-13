CREATE procedure [dbo].[spDailyProductionSummary]
@ReportDate dateTime

as
DECLARE @YID  INT;
DECLARE @MID  INT;

set @YID=YEAR(@ReportDate);
set @MID=MONTH(@ReportDate)-1;

IF @MID<=0
set @MID=12
set @YID=@YID-1


select 'Knitting' as Process ,
SUM(Quantity) as TDProd,
(select SUM(Quantity) from PROD_KnittingRoll where YEAR(PROD_KnittingRoll.RollDate)=YEAR(@ReportDate) and MONTH(PROD_KnittingRoll.RollDate)=MONTH(@ReportDate)) as UpToDate ,
(
select SUM(Quantity) from PROD_KnittingRoll where YEAR(PROD_KnittingRoll.RollDate)=@YID and MONTH(PROD_KnittingRoll.RollDate)=@MID) as LMP,
'KG' as MUnit from PROD_KnittingRoll where Convert(date,PROD_KnittingRoll.RollDate)=Convert(date,@ReportDate)

 union

select 
'Dyeing' as Process,
ISNULL(SUM(Bt.BatchQty),0)as TDProd,
(select SUM(ISNULL(BatchQty,0))  from Pro_Batch   where YEAR(UnLoadingDateTime)=YEAR(@ReportDate) and MONTH(UnLoadingDateTime)=MONTH(@ReportDate)) as UpToDate,
(select SUM(ISNULL(BatchQty,0))  from Pro_Batch  where YEAR(UnLoadingDateTime)=@YID and MONTH(UnLoadingDateTime)=@MID ) as  LMP  ,
'KG' as MUnit
  from Pro_Batch as Bt  where CONVERT(date,UnLoadingDateTime)=CONVERT(date,@ReportDate)


 union


select 'Cutting' as Process , SUM(BC.Quantity) as TDProd, 
(select SUM(BC.Quantity) from PROD_CuttingBatch  as CB
inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId
where CB.ComponentRefId='001' and   YEAR(CB.CuttingDate)=YEAR(@ReportDate) and MONTH(CB.CuttingDate)=MONTH(@ReportDate) 
) as UpToDate,
(select SUM(BC.Quantity) from PROD_CuttingBatch  as CB
inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId
where CB.ComponentRefId='001' and  YEAR(CB.CuttingDate)=@YID and MONTH(CB.CuttingDate)=@MID
) as LMP ,'PCS' as MUnit from PROD_CuttingBatch  as CB
inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId
where CB.ComponentRefId='001' and Convert(date, CB.CuttingDate)=Convert(date,@ReportDate)

 union

select 'Sewing' as Process , SUM(SPD.Quantity) as TDProd,(
select  SUM(FPD.Quantity) as TDProd from PROD_SewingOutPutProcess as FP 
inner join PROD_SewingOutPutProcessDetail as FPD on FP.SewingOutPutProcessId=FPD.SewingOutPutProcessId
where YEAR(FP.OutputDate)=YEAR(@ReportDate) and MONTH(FP.OutputDate)=MONTH(@ReportDate)
) as  UpToDate,
(select  SUM(FPD.Quantity) as TDProd from PROD_SewingOutPutProcess as FP 
inner join PROD_SewingOutPutProcessDetail as FPD on FP.SewingOutPutProcessId=FPD.SewingOutPutProcessId
where  YEAR(FP.OutputDate)=@YID and MONTH(FP.OutputDate)=@MID) as LMP
,'PCS' as MUnit from PROD_SewingOutPutProcess as SP 
inner join PROD_SewingOutPutProcessDetail as SPD on SP.SewingOutPutProcessId=SPD.SewingOutPutProcessId
where Convert(date,SP.OutputDate)=Convert(date,@ReportDate)
 
 union
select 'Finishing' as Process , SUM(FPD.InputQuantity) as TDProd,(
select  SUM(FPD.InputQuantity) as TDProd from PROD_FinishingProcess as FP 
inner join PROD_FinishingProcessDetail as FPD on FP.FinishingProcessId=FPD.FinishingProcessId
where YEAR(FP.InputDate)=YEAR(@ReportDate) and MONTH(FP.InputDate)=MONTH(@ReportDate)  and FType=2
) as  UpToDate,
(select  SUM(FPD.InputQuantity) as TDProd from PROD_FinishingProcess as FP 
inner join PROD_FinishingProcessDetail as FPD on FP.FinishingProcessId=FPD.FinishingProcessId
where  YEAR(FP.InputDate)=@YID and MONTH(FP.InputDate)=@MID  and FType=2) as LMP
,'PCS' as MUnit from PROD_FinishingProcess as FP 
inner join PROD_FinishingProcessDetail as FPD on FP.FinishingProcessId=FPD.FinishingProcessId
where Convert(date,FP.InputDate)=Convert(date,@ReportDate) and FType=2


--exec spDailyProductionSummary '2018-01-09'





