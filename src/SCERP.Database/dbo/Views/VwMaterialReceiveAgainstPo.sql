
CREATE view [dbo].[VwMaterialReceiveAgainstPo]
as 
select MR.*,Comp.Name as CompanyName,Comp.FullAddress,Sup.CompanyName as Supplier from Inventory_MaterialReceiveAgainstPo as MR

inner join Mrc_SupplierCompany as Sup on MR.SupplierId=Sup.SupplierCompanyId

inner join Company as Comp on MR.CompId=Comp.CompanyRefId




