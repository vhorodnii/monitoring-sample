namespace Sample.Web.ProcessingTask;

class DuplicateTaskException : Exception
{
    public DuplicateTaskException(string id)
    {
        Id = id;
    }

    public string Id { get; }
}
