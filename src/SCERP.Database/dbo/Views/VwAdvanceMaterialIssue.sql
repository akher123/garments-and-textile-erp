





CREATE view [dbo].[VwAdvanceMaterialIssue]
as
select MI.*,
E.Name as Employee ,
RE.Name as RefEmployee ,
C.Name as CompanyName,
B.BuyerName,
(select Name from Party where PartyId=MI.PartyId) as PartyName,
C.FullAddress  from Inventory_AdvanceMaterialIssue as MI
left join OM_Buyer as B on MI.BuyerRefId=B.BuyerRefId and MI.CompId=B.CompId
inner join Employee as E on MI.IssuedBy=E.EmployeeId
left join Employee as RE on MI.RefPerson=RE.EmployeeId
inner join Company as C on MI.CompId=C.CompanyRefId





