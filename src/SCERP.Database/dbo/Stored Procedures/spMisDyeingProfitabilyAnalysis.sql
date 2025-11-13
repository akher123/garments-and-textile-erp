CREATE procedure [dbo].[spMisDyeingProfitabilyAnalysis]
@YearId int 
as

--select YEAR (B.BatchDate) as NYear, DATENAME (month,B.BatchDate) as NMonth,SUM(BD.Quantity) as GreyWit,SUM(BD.Quantity*ISNULL(BD.Rate,0)) as Income,
--(select ROUND(SUM(SL.Quantity*SL.UnitPrice),2) from Inventory_MaterialIssue as MI 
--inner join Inventory_StoreLedger as SL on MI.MaterialIssueId=SL.MaterialIssueId
--where MI.ToppingType in (1,2) and MI.BtRefNo in (
--select BtRefNo from Pro_Batch where Month(BatchDate)=MONTH (B.BatchDate)  and YEAR(BatchDate)=YEAR (B.BatchDate) )) as DyeingCost ,
--(select ROUND(SUM(SL.Quantity*SL.UnitPrice),2)  from Inventory_MaterialIssue as MI 
--inner join Inventory_StoreLedger as SL on MI.MaterialIssueId=SL.MaterialIssueId
--where MI.MachineId>0 and Month(MI.IssueReceiveDate)=MONTH (B.BatchDate)  and YEAR(MI.IssueReceiveDate)=YEAR (B.BatchDate) ) as ETP
--from  Pro_Batch as B 
--inner join PROD_BatchDetail as BD on B.BatchId=BD.BatchId
--where YEAR (B.BatchDate) =@YearId
--group by DATENAME (month,B.BatchDate) , MONTH (B.BatchDate) ,YEAR (B.BatchDate) 
--order by MONTH (B.BatchDate) 


select YEAR (MI.IssueReceiveDate) as NYear, DATENAME (month,MI.IssueReceiveDate) as NMonth, SUM(BD.Quantity) as GreyWit,SUM(BD.Quantity*ISNULL(BD.Rate,0)) as Income,

((SELECT        ISNULL(SUM(SL.Amount), 0) AS Expr1
                               FROM            Inventory_StoreLedger AS SL INNER JOIN
                                                         Inventory_MaterialIssue AS MIS ON SL.MaterialIssueId = MIS.MaterialIssueId 
														
                                                            WHERE        (MIS.IType = 2 ) and MIS.ToppingType  in(1,2,3) and MONTH(MIS.IssueReceiveDate) = MONTH(MI.IssueReceiveDate) and YEAR(MIS.IssueReceiveDate) = YEAR(MI.IssueReceiveDate)  AND (SL.TransactionType = 2 ) 
							   ))AS DyeingCost,

							   (select ROUND(SUM(SL.Amount),2)  from Inventory_MaterialIssue as MII 
inner join Inventory_StoreLedger as SL on MII.MaterialIssueId=SL.MaterialIssueId
where MII.MachineId>0 and MII.IType=1  and Month(MII.IssueReceiveDate)=MONTH (MI.IssueReceiveDate)  and YEAR(MII.IssueReceiveDate)=YEAR (MI.IssueReceiveDate) ) as ETP

 from Inventory_MaterialIssue as MI
inner join Pro_Batch as B on MI.BtRefNo=B.BtRefNo
inner join PROD_BatchDetail as BD on B.BatchId=BD.BatchId
where YEAR (MI.IssueReceiveDate)=@YearId and MI.ToppingType=1 and  MI.IType=2 
group by DATENAME (month,MI.IssueReceiveDate) , MONTH (MI.IssueReceiveDate) ,YEAR (MI.IssueReceiveDate) 
order by MONTH (MI.IssueReceiveDate)









