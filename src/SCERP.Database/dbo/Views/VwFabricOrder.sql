CREATE view VwFabricOrder
as
select FO.[FabricOrderId]
      , FO.[FabricOrderRefId]
      , FO.[SupplierId]
      , FO.[OrderDate]
      , FO.[ExpDate]
      , FO.[PreparedBy]
      , FO.[Remarks]
      , FO.[CompId]
      , FO.[BuyerRefId]
      , FO.[OrderNo]
      , FO.[MerchandiserRefId]
      , FO.[Status]
     
	  ,S.CompanyName as Supplier
	  ,B.BuyerName
	   ,BO.RefNo as OrderName
	   ,M.EmpName as Merchandiser
	   ,
	   StyleName= substring((SELECT ( ', ' + ST.StyleName)
                       FROM OM_FabricOrderDetail FOD
					   inner join VOM_BuyOrdStyle as ST on FOD.OrderStyleRefId=ST.OrderStyleRefId and FOD.CompId=ST.CompId
					   where FOD.FabricOrderId=FO.FabricOrderId
                       FOR XML PATH( '' )
                      ), 3, 1000 )
	  from OM_FabricOrder as FO
	  inner join Mrc_SupplierCompany as S on FO.SupplierId=S.SupplierCompanyId
inner join OM_Buyer as B on FO.BuyerRefId=B.BuyerRefId and FO.CompId=B.CompId
inner join OM_Merchandiser as M on FO.MerchandiserRefId=M.EmpId and FO.CompId=M.CompId
inner join OM_BuyerOrder as BO on FO.OrderNo=BO.OrderNo and FO.CompId=BO.CompId



