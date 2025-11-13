CREATE PROCEDURE SpProdPartyWiseDailyKnittingRoll
@RollDate DateTime=null,
@CompId varchar(03)
AS
BEGIN 
select KD.PartyName,KD.ProgramRefId,KD.MachineName,KD.ItemName,KD.GSM,KD.SizeName,KD.FinishSizeName,Sum(KD.Quantity) as Qty,0 AS CollarCuffQty from VwKnittingRoll as KD
Where KD.RollDate=@RollDate AND KD.CompId=@CompId
group by KD.PartyName,KD.ProgramRefId,KD.MachineName,KD.ItemName,KD.GSM,KD.SizeName,KD.FinishSizeName
END
