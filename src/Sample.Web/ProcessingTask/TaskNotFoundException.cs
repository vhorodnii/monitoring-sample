namespace Sample.Web.ProcessingTask;

class TaskNotFoundException : Exception
{
    public TaskNotFoundException(string id)
    {
        Id = id;
    }

    public string Id { get; }
}
