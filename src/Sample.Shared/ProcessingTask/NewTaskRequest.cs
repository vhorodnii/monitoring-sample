namespace Sample.Shared.ProcessingTask
{
    public class NewTaskRequest
    {
        public bool ConvertToPdf { get; set; } = false;
        public bool CleanDocument { get; set; } = false;
        public string Id { get; set; } = default!;
    }
}
