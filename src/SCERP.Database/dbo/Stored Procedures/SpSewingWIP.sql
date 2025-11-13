


CREATE PROCEDURE [dbo].[SpSewingWIP]
	@ViewDate datetime,
	@HourId int ,
	@CompId varchar(3)
	
AS
BEGIN
	set @ViewDate= convert(varchar(10),@ViewDate,120)
    --set @ViewDate='2016-09-07'
	truncate table PROD_SewingWIP
    INSERT INTO PROD_SewingWIP (CompId, LineId,LineName,OpeningQty,InputQty,OutputQty,WIP,[Hour],RBuyerName,ROrderName,RStyleName,RColorName,UCBuyerName,UCOrderName,UCStyleName,UCColorName) 
	select CompId, MachineId,Name,0,0,0,0,0,'','','','','','','','' from Production_Machine where ProcessorRefId in ('003','004','007') and CompId=@CompId AND IsActive='True'

	-- Update InputQty Section
	update PROD_SewingWIP set InputQty=ISNULL((select SUM(PROD_SewingInputProcessDetail.InputQuantity) from PROD_SewingInputProcess
    inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId
    where  (CONVERT(varchar(10), InputDate, 120) = @ViewDate)  and PROD_SewingInputProcess.LineId=PROD_SewingWIP.LineId and PROD_SewingInputProcess.CompId=PROD_SewingWIP.CompId),0)

	----Update Oppenning Qty

	update PROD_SewingWIP set OpeningQty = ISNULL((select SUM(PROD_SewingInputProcessDetail.InputQuantity) from PROD_SewingInputProcess
    inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId
    where  (CONVERT(varchar(10), InputDate, 120) < @ViewDate)  and PROD_SewingInputProcess.LineId=PROD_SewingWIP.LineId and PROD_SewingInputProcess.CompId=PROD_SewingWIP.CompId),0)


      update PROD_SewingWIP set OpeningQty=OpeningQty -ISNULL((select SUM(PROD_SewingOutPutProcessDetail.Quantity) from PROD_SewingOutPutProcess
      inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
      where (CONVERT(varchar(10), OutPutDate, 120) < @ViewDate) and PROD_SewingOutPutProcess.LineId=PROD_SewingWIP.LineId and PROD_SewingOutPutProcess.CompId=PROD_SewingWIP.CompId),0)
	
	--Update outputQty Section
      update PROD_SewingWIP set OutputQty=ISNULL((select SUM(PROD_SewingOutPutProcessDetail.Quantity) from PROD_SewingOutPutProcess
      inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
      where (CONVERT(varchar(10), OutPutDate, 120) = @ViewDate) and PROD_SewingOutPutProcess.LineId=PROD_SewingWIP.LineId and PROD_SewingOutPutProcess.CompId=PROD_SewingWIP.CompId),0)
	
	  -- upate WIP Section
	  update PROD_SewingWIP set WIP=(OpeningQty+InputQty)-OutputQty 

	  -- upate HOUR Section
	      
           update PROD_SewingWIP set Hour=  CASE WHEN OutputQty>0 THEN   WIP/(OutputQty) ELSE 0 END 
	  

	  -- Update OrderName
	   update  PROD_SewingWIP set ROrderName= VOM_BuyOrdStyle.RefNo,RBuyerName=VOM_BuyOrdStyle.BuyerName,RStyleName =VOM_BuyOrdStyle.StyleName, RColorName= OM_Color.ColorName   from PROD_SewingInputProcess
       inner join VOM_BuyOrdStyle on PROD_SewingInputProcess.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and PROD_SewingInputProcess.CompId=VOM_BuyOrdStyle.CompId
	    inner join OM_Color on PROD_SewingInputProcess.ColorRefId=OM_Color.ColorRefId and PROD_SewingInputProcess.CompId=OM_Color.CompId
       where  PROD_SewingInputProcess.LineId=PROD_SewingWIP.LineId and PROD_SewingInputProcess.CompId=PROD_SewingWIP.CompId

	  -- --Update Running Style and Color
	   update PROD_SewingWIP set UCOrderName= VOM_BuyOrdStyle.RefNo,UCBuyerName=VOM_BuyOrdStyle.BuyerName,UCStyleName =VOM_BuyOrdStyle.StyleName, UCColorName= OM_Color.ColorName from PROD_SewingInputProcess
       inner join VOM_BuyOrdStyle on PROD_SewingInputProcess.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and PROD_SewingInputProcess.CompId=VOM_BuyOrdStyle.CompId
	   inner join OM_Color on PROD_SewingInputProcess.ColorRefId=OM_Color.ColorRefId and PROD_SewingInputProcess.CompId=OM_Color.CompId
       where Convert(date,PROD_SewingInputProcess.InputDate)=@ViewDate and PROD_SewingInputProcess.LineId=PROD_SewingWIP.LineId  and PROD_SewingInputProcess.CompId=PROD_SewingWIP.CompId


	   --select S.LineName,S.OpeningQty,S.InputQty,S.OutputQty, cast(S.WIP AS int) WIP,S.[Hour],S.RBuyerName,S.ROrderName,S.RStyleName,S.RColorName,S.UCBuyerName,S.UCOrderName,S.UCStyleName,S.UCColorName from PROD_SewingWIP AS S

		select S.LineName,S.OpeningQty,S.InputQty,S.OutputQty, cast(S.WIP AS int) WIP,S.[Hour],S.RBuyerName,S.ROrderName,S.RStyleName,S.RColorName from PROD_SewingWIP AS S




	
END


--EXEC SpSewingWIP @ViewDate='2016-09-02',@HourId=5, @CompId='001'