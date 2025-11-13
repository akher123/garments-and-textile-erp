CREATE procedure [dbo].[spGetRollSticker]
@KnittingRollId bigint,
@CompId varchar(3)
as

select KR.GSM,KR.CharllRollNo,KR.KnittingRollId, KR.RollDate,KR.RollRefNo,KR.RollLength,KR.Quantity,
PR.ProgramRefId,
  (select  top(1) BuyerName from OM_Buyer where BuyerRefId=PR.BuyerRefId and CompId=PR.CompId) as Buyer,
(select top(1)StyleName from VOM_BuyOrdStyle where OrderStyleRefId=PR.OrderStyleRefId  and CompId=PR.CompId ) as StyleName,
(select top(1)RefNo from VOM_BuyOrdStyle where OrderStyleRefId=PR.OrderStyleRefId  and CompId=PR.CompId ) as OrderNo,
   (select top(1)SizeName from OM_Size where SizeRefId=KR.SizeRefId  and CompId=PR.CompId ) as SizeName,
(select top(1)Name from Party where PartyId=PR.PartyId   ) as PartyName,
ltrim(rtrim((select top(1)ColorName from OM_Color where ColorRefId=KR.ColorRefId  and CompId=PR.CompId)))as ColorName,
  (select top(1)SizeName from OM_Size where SizeRefId=KR.FinishSizeRefId  and CompId=PR.CompId ) as FinishSizeName ,
(select ItemName from Inventory_Item where ItemCode=KR.ItemCode and CompId=KR.CompId) as ItemName ,

'' as FDia,
STUFF((SELECT distinct',' + ItemName
            from VProgramDetail where ProgramId=KR.ProgramId and MType='I'
            FOR XML PATH('')) ,1,1,'') as YarnName,
STUFF((SELECT  distinct ',' + LotName
            from VProgramDetail where ProgramId=KR.ProgramId and MType='I'
            FOR XML PATH('')) ,1,1,'')  as Lot,
STUFF((SELECT distinct ',' +SizeName 
            from VProgramDetail where ProgramId=KR.ProgramId and MType='I'
            FOR XML PATH('')) ,1,1,'')   as CountName,

ISNULL(KR.StLength, (select  top(1) PD.SleeveLength from PLAN_ProgramDetail  as PD
inner join PROD_KnittingRoll as KR on PD.ProgramId=KR.ProgramId
where PD.ProgramId=KR.ProgramId and KR.KnittingRollId=KR.KnittingRollId and  MType='O' and PD.SizeRefId=KR.SizeRefId and PD.ColorRefId=KR.ColorRefId and PD.ItemCode=KR.ItemCode and PD.FinishSizeRefId=KR.FinishSizeRefId)) as StLength
 from PROD_KnittingRoll as KR
inner join PLAN_Program as PR on KR.ProgramId=PR.ProgramId
where KR.KnittingRollId=@KnittingRollId

