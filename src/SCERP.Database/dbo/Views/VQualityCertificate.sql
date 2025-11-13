



CREATE view [dbo].[VQualityCertificate]
AS
SELECT VINIEMSTOR.*, INQC.QCReferenceNo,INQC.SendingDate,INQC.QualityCertificateId,INQC.IsGrnConverted FROM Inventory_QualityCertificate AS INQC

INNER JOIN VInventoryItemStore AS VINIEMSTOR ON INQC.ItemStoreId=VINIEMSTOR.ItemStoreId
where INQC.IsActive=1






