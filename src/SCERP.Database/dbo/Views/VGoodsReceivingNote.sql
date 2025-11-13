CREATE view [dbo].[VGoodsReceivingNote]
AS

SELECT GRN.GRNNumber,
GRN.DeductionAmt,
GRN.GRNDate,
GRN.GoodsReceivingNotesId,
GRN.Remarks,GRN.IsSendToStoreLedger,
(select Name from Employee where EmployeeId=GRN.AppBy ) as AppByName,GRN.IsApproved,
VQC.*  FROM  Inventory_GoodsReceivingNote AS GRN

INNER JOIN VQualityCertificate AS  VQC ON GRN.QualityCertificateId=VQC.QualityCertificateId

where GRN.IsActive=1

