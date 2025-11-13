
-- =========================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <20/10/2015>
-- Description:	<> exec SPSpecSheetDetailReport 1,'STL-123','PLUMMY_STL-123_1','2015-01-01','2016-01-01'
-- =========================================================================

CREATE PROCEDURE [dbo].[SPSpecSheetDetailReport]


						 @SpecSheetId		INT
					
						 
AS

BEGIN
	
						 SET NOCOUNT ON;

						 SELECT  

						 Mrc_SpecificationSheet.SpecificationSheetId
						,Mrc_Buyer.BuyerName
						,Mrc_BuyerContactPerson.Name
						,Mrc_SpecificationSheet.StyleNo
						,Mrc_SpecificationSheet.JobNo
						,Mrc_SpecificationSheet.ReferenceNo
						,Mrc_SpecificationSheet.Department
						,Mrc_SpecificationSheet.Season
						,Mrc_SpecificationSheet.Brand
						,Mrc_SpecificationSheet.StyleDescription
						,Mrc_SpecificationSheet.FabricationDescription
						,Mrc_SpecificationSheet.Material
						,Mrc_SpecificationSheet.Finishing
						,Mrc_SpecificationSheet.ItemGroup
						,Mrc_SpecificationSheet.SizeRange
						,Mrc_SpecificationSheet.ApproximateQuantity
						,Mrc_SpecificationSheet.LeadTimeInDays
						,CONVERT(VARCHAR(10), Mrc_SpecificationSheet.EntryDate, 103) AS EntryDate
						,CONVERT(VARCHAR(10), Mrc_SpecificationSheet.ShipmentDate, 103) AS ShipmentDate
						,Mrc_SpecificationSheet.Remarks
					
						 FROM        Mrc_SpecificationSheet 
						 LEFT JOIN   Mrc_Buyer ON Mrc_SpecificationSheet.BuyerId = Mrc_Buyer.Id 
						 LEFT JOIN   Mrc_BuyerContactPerson ON Mrc_SpecificationSheet.ContactPersonId = Mrc_BuyerContactPerson.Id 

						 WHERE Mrc_SpecificationSheet.SpecificationSheetId = @SpecSheetId
																																						 													
END








