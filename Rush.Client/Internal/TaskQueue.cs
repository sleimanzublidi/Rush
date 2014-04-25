namespace Rush.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class TaskQueue
    {
        //private Task tail;
        //private readonly object mutex = new object();
        //public object Mutex
        //{
        //    get { return this.mutex; }
        //}

        //private Task GetTaskToAwait(CancellationToken cancellationToken)
        //{
        //    Task result;
        //    lock (this.mutex)
        //    {
        //        Task task2 = this.tail ?? Task.FromResult<bool>(true);
        //        result = task2.ContinueWith(delegate(Task task)
        //        {
        //        }, cancellationToken);
        //    }
        //    return result;
        //}

        //public T Enqueue<T>(Func<Task, T> taskStart, CancellationToken cancellationToken) where T : Task
        //{
        //    T t;
        //    lock (this.mutex)
        //    {
        //        Task task = this.tail ?? Task.FromResult<bool>(true);
        //        t = taskStart(this.GetTaskToAwait(cancellationToken));
        //        this.tail = Task.WhenAll(new Task[] { task, t });
        //    }
        //    return t;
        //}
    }
}
