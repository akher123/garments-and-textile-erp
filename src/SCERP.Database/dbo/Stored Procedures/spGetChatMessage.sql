CREATE procedure [dbo].[spGetChatMessage]
@ReceiverId uniqueidentifier 
as
select ms.MessageId, ms.MessageText,snd.Name as Sender,rcv.Name as Receiver,ms.IsViewed,snd.PhotographPath from Messaging as ms
inner join Employee as snd on ms.SenderId=snd.EmployeeId
inner join Employee as rcv on ms.ReceiverId=rcv.EmployeeId
where ms.ReceiverId=@ReceiverId and ms.IsViewed=0 and CAST(ms.SendTime as DATE)= CAST(GETDATE() AS DATE)
order by ms.MessageId desc




