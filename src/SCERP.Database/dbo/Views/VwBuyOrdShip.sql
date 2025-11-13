 CREATE VIEW VwBuyOrdShip
 AS
 SELECT
  SH.*,
( SELECT top(1) CountryName FROM Country WHERE Id=SH.CountryId) as CountryName,
( SELECT TOP(1) PortOfLoadingName FROM OM_PortOfLoading WHERE PortOfLoadingRefId=SH.PortOfLoadingRefId AND CompId=SH.CompId) AS PortOfLoadingName
 FROM OM_BuyOrdShip AS SH

