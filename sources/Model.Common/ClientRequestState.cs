namespace Queue.Model.Common
{
    public enum ClientRequestState
    {
        Waiting,
        Calling,
        Absence,
        Rendering,
        Postponed,
        Rendered,
        Canceled
    }
}