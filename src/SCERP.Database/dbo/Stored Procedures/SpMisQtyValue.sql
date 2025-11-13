CREATE procedure [dbo].[SpMisQtyValue]
@CompId varchar(3)
as 
 truncate table MIS_MonthValue

 insert into MIS_MonthValue (BuyerRefId,Q1,V1,Q2,V2,Q3,V3,Q4,V4,Q5,V5,Q6,V6,Q7,V7,Q8,V8,Q9,V9,Q10,V10,Q11,V11,Q12,V12)
 select distinct  B.BuyerRefId,0 as Q1,0 as V1,0 as Q2,0 as V2,0 as Q3,0 as V3,0 as Q4,0 as V4,0 as Q5,0 as V5,0 as Q6,
 0 as V6,0 as Q7,0 as V7,0 as Q8,0 as V8,0 as Q9,0 as V9,0 as Q10,0 as V10,0 as Q11,0 as V11,0 as Q12,0 as V12 from OM_BuyerOrder as BO
inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_BuyOrdStyle as BOS on BO.OrderNo=BOS.OrderNo and BOS.CompId=BOS.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1

 update MIS_MonthValue set Q1=(
select Convert(int,ISNULL(SUM(ISNULL(SH.Quantity,0))-SUM(ISNULL(SH.DespatchQty,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='1'
)

update MIS_MonthValue set V1=(
select  Convert(int, ISNULL(SUM(ISNULL(SH.Quantity,0)*ISNULL(BOS.Rate,0))-SUM(ISNULL(SH.DespatchQty,0)*ISNULL(BOS.Rate,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)=''
)

 update MIS_MonthValue set Q2=(
select Convert(int,ISNULL(SUM(ISNULL(SH.Quantity,0))-SUM(ISNULL(SH.DespatchQty,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='2'
)

update MIS_MonthValue set V2=(
select  Convert(int, ISNULL(SUM(ISNULL(SH.Quantity,0)*ISNULL(BOS.Rate,0))-SUM(ISNULL(SH.DespatchQty,0)*ISNULL(BOS.Rate,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='2'
)


 update MIS_MonthValue set Q3=(
select Convert(int,ISNULL(SUM(ISNULL(SH.Quantity,0))-SUM(ISNULL(SH.DespatchQty,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='3'
)

update MIS_MonthValue set V3=(
select  Convert(int, ISNULL(SUM(ISNULL(SH.Quantity,0)*ISNULL(BOS.Rate,0))-SUM(ISNULL(SH.DespatchQty,0)*ISNULL(BOS.Rate,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='3'
)


 update MIS_MonthValue set Q4=(
select Convert(int,ISNULL(SUM(ISNULL(SH.Quantity,0))-SUM(ISNULL(SH.DespatchQty,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='4'
)

update MIS_MonthValue set V4=(
select  Convert(int, ISNULL(SUM(ISNULL(SH.Quantity,0)*ISNULL(BOS.Rate,0))-SUM(ISNULL(SH.DespatchQty,0)*ISNULL(BOS.Rate,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='4'
)

 update MIS_MonthValue set Q5=(
select Convert(int,ISNULL(SUM(ISNULL(SH.Quantity,0))-SUM(ISNULL(SH.DespatchQty,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='5'
)

update MIS_MonthValue set V5=(
select  Convert(int, ISNULL(SUM(ISNULL(SH.Quantity,0)*ISNULL(BOS.Rate,0))-SUM(ISNULL(SH.DespatchQty,0)*ISNULL(BOS.Rate,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='5'
)


 update MIS_MonthValue set Q6=(
select Convert(int,ISNULL(SUM(ISNULL(SH.Quantity,0))-SUM(ISNULL(SH.DespatchQty,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='6'
)

update MIS_MonthValue set V6=(
select  Convert(int, ISNULL(SUM(ISNULL(SH.Quantity,0)*ISNULL(BOS.Rate,0))-SUM(ISNULL(SH.DespatchQty,0)*ISNULL(BOS.Rate,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2019' and Month(SH.ShipDate)='6'
)

 update MIS_MonthValue set Q7=(
select Convert(int,ISNULL(SUM(ISNULL(SH.Quantity,0))-SUM(ISNULL(SH.DespatchQty,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='7'
)

update MIS_MonthValue set V7=(
select  Convert(int, ISNULL(SUM(ISNULL(SH.Quantity,0)*ISNULL(BOS.Rate,0))-SUM(ISNULL(SH.DespatchQty,0)*ISNULL(BOS.Rate,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='7'
)

 update MIS_MonthValue set Q8=(
select Convert(int,ISNULL(SUM(ISNULL(SH.Quantity,0))-SUM(ISNULL(SH.DespatchQty,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='8'
)

update MIS_MonthValue set V8=(
select  Convert(int, ISNULL(SUM(ISNULL(SH.Quantity,0)*ISNULL(BOS.Rate,0))-SUM(ISNULL(SH.DespatchQty,0)*ISNULL(BOS.Rate,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='8'
)

 update MIS_MonthValue set Q9=(
select Convert(int,ISNULL(SUM(ISNULL(SH.Quantity,0))-SUM(ISNULL(SH.DespatchQty,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='9'
)

update MIS_MonthValue set V9=(
select  Convert(int, ISNULL(SUM(ISNULL(SH.Quantity,0)*ISNULL(BOS.Rate,0))-SUM(ISNULL(SH.DespatchQty,0)*ISNULL(BOS.Rate,0)),0)) from OM_BuyOrdShip as SH
inner join OM_BuyOrdStyle as BOS on SH.OrderNo=BOS.OrderNo and SH.OrderStyleRefId=BOS.OrderStyleRefId and BOS.CompId=SH.CompId
inner join OM_BuyerOrder as BO on BOS.OrderNo=BO.OrderNo and BOS.CompId=BO.CompId
 where  BO.CompId=@CompId and BO.SCont='N' and BOS.ActiveStatus=1 and BO.BuyerRefId=MIS_MonthValue.BuyerRefId and year(SH.ShipDate)='2020' and Month(SH.ShipDate)='9'
)
select B.BuyerName,MV.Q1,MV.V1,MV.Q2,MV.V2,MV.Q3,MV.V3,MV.Q4,MV.V4,MV.Q5,MV.V5,MV.Q6,MV.V6,MV.Q7,MV.V7,MV.Q8,MV.V8,MV.Q9,MV.V9,MV.Q10,MV.V10,MV.Q11,MV.V11,MV.Q12,MV.V12 from MIS_MonthValue as MV 
inner join OM_Buyer as B on MV.BuyerRefId=B.BuyerRefId
order by B.BuyerName


--exec SpMisQtyValue '001'