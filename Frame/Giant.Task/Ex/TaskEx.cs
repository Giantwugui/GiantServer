using System.Threading.Tasks;

namespace Giant.DataTask
{
    public static class TaskEx
    {
        public static void Coroutine(this Task task)
        {
        }

        public static void Coroutine<T>(this Task<T> task)
        {
        }
    }
}
