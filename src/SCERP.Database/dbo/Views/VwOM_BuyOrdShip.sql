create view VwOM_BuyOrdShip

as

select SH.* ,
(select CountryName from Country where Id=SH.CountryId ) as CountryName,
(select top(1) PortOfLoadingName from OM_PortOfLoading where PortOfLoadingRefId=SH.PortOfLoadingRefId) as PortOfLoadingName,
(select IModeName from OM_ItemMode where IModeRefId=SH.IModeRefId) as ModeName
from OM_BuyOrdShip  as SH 