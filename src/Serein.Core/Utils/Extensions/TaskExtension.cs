using System.Threading.Tasks;

namespace Serein.Core.Utils.Extensions;

public static class TaskExtension
{
    public static T WaitForResult<T>(this Task<T> task)
    {
        return task.GetAwaiter().GetResult();
    }

    public static void Await(this Task task)
    {
        task.GetAwaiter().GetResult();
    }
}
