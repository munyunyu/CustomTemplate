namespace Template.Library.Models.Queue
{
    public record EmailQueueMessage(Guid EmailQueueId);

    public record NotificationQueueMessage(Guid NotificationId, string Type);
}
