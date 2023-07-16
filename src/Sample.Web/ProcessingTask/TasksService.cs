using Sample.Shared.ProcessingTask;
using System.Collections.Concurrent;

namespace Sample.Web.ProcessingTask;

public class TasksService : ITasksService
{
    private readonly ConcurrentDictionary<string, ProcessingTask> _tasks = new();

    public void StartNewTask(NewTaskRequest task)
    {
        if (_tasks.TryGetValue(task.Id, out _))
        {
            _tasks.Remove(task.Id, out _);
        }

        var newTask = new ProcessingTask
        {
            Id = task.Id,
            CleanDocument = task.CleanDocument,
            ConvertToPdf = task.ConvertToPdf,
            Name = task.Id,
            State = TaskState.Processing
        };

        _tasks.TryAdd(task.Id, newTask);
    }

    public bool NeedConverting(string id)
    {
        return _tasks[id].ConvertToPdf;
    }

    public void CompleteTask(string id)
    {
        if (!_tasks.TryGetValue(id, out var task))
        {
            throw new TaskNotFoundException(id);
        }

        task.State = TaskState.Completed;
    }
}

public enum TaskState
{
    Processing,
    Completed
}

public class ProcessingTask
{
    public string Id { get; set; } = string.Empty;
    public TaskState State { get; set; } = TaskState.Processing;
    public string Name { get; set; } = string.Empty;
    public bool ConvertToPdf { get; set; }
    public bool CleanDocument { get; set; }
}
