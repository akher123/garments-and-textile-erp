
CREATE view [dbo].[VwTnaCTE]
as

WITH CTE AS(
select

( 
case
 when (OM_TNA.ASDate is null ) or (OM_TNA.ASDate='') or (OM_TNA.ASDate='0') then

	( 
	case 
	 when (OM_TNA.PSDate is null ) or (OM_TNA.PSDate='') or (OM_TNA.PSDate='0')  or (OM_TNA.PSDate='-') then 'Black '
	 when DATEDIFF(DAY,GETDATE(),dbo.fnDateConvert(OM_TNA.PSDate)) > 5 then 'Grey '
	 when DATEDIFF(DAY,GETDATE(),dbo.fnDateConvert(OM_TNA.PSDate)) in (4, 5 ) then 'Yellow '
	 when DATEDIFF(DAY,GETDATE(),dbo.fnDateConvert(OM_TNA.PSDate)) in (3, 2, 1, 0 ) then 'DeepPink '
	 when DATEDIFF(DAY,GETDATE(),dbo.fnDateConvert(OM_TNA.PSDate)) in (-2, -1 ) then 'Red' 
	 else 'DarkRed ' end) 
 
 else
  (
	 case

	 when DATEDIFF(DAY,  dbo.fnDateConvert(OM_TNA.ASDate) ,dbo.fnDateConvert(OM_TNA.PSDate)) >=0 then 'Green '
	
	 else 'Blue ' end
	 )

 end
 )
 
 as AlertColor ,



  OM_TNA.OrderStyleRefId,
  OM_TNA.ShortName,
  ' Plan Date :'+Convert(varchar,OM_TNA.PSDate,103) +' Actual Date :'+ Convert(varchar,OM_TNA.ASDate,103)+'Remartks :'+OM_TNA.UpdateRemarks as UpdateRemarks,

  Convert(varchar,OM_TNA.ASDate,103)  as ASDate,
  Convert(varchar,OM_TNA.AEDate,103)  as AEDate,
  OM_TNA.CompId
 from OM_TNA 
 

 ) 


 select * from CTE


