using AsyncAwaitBestPractices;

namespace Core;

public static class TaskExtension
{
    public static void Forget(this Task task)
    {
        task.SafeFireAndForget();
    }
    
    public static void Forget(this ValueTask task)
    {
        task.SafeFireAndForget();
    }
}