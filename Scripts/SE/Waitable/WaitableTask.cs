using System;

namespace ClinicalTools.SimEncounters
{
    public class WaitableTask
    {
        public static WaitableTask CompletedTask = new WaitableTask(true);

        public TaskResult Result { get; protected set; }
        public bool IsCompleted() => Result != null;
        private event Action<TaskResult> Completed;

        public WaitableTask(bool completed = false)
        {
            if (completed)
                Result = new TaskResult();
        }
        public WaitableTask(Exception exception) => Result = new TaskResult(exception);

        public void SetCompleted()
        {
            if (IsCompleted())
                throw new Exception("Result already completed.");

            Result = new TaskResult();
            Complete();
        }
        public void SetError(Exception exception)
        {
            if (IsCompleted())
                throw new Exception("Result already set.");

            Result = new TaskResult(exception);
            Complete();
        }

        protected void Complete()
        {
            if (Completed == null)
                return;

            Completed(Result);
            foreach (Delegate d in Completed.GetInvocationList())
                Completed -= (Action<TaskResult>)d;
        }

        public void AddOnCompletedListener(Action<TaskResult> action)
        {
            if (IsCompleted())
                action.Invoke(Result);
            else
                Completed += action;
        }
    }

    public class WaitableTask<T>
    {
        public TaskResult<T> Result { get; protected set; }
        public bool IsCompleted() => Result != null;

        private event Action<TaskResult<T>> Completed;

        public WaitableTask() { }
        public WaitableTask(T value) => Result = new TaskResult<T>(value);
        public WaitableTask(Exception exception) => Result = new TaskResult<T>(exception);

        public void SetResult(T value)
        {
            if (IsCompleted())
                throw new Exception("Result already set.");

            Result = new TaskResult<T>(value);
            Complete();
        }
        public void SetError(Exception exception)
        {
            if (IsCompleted())
                throw new Exception("Result already set.");

            Result = new TaskResult<T>(exception);
            Complete();
        }

        protected void Complete()
        {
            if (Completed == null)
                return;

            Completed(Result);
            RemoveListeners();
        }

        public void AddOnCompletedListener(Action<TaskResult<T>> action)
        {
            if (IsCompleted())
                action.Invoke(Result);
            else
                Completed += action;
        }

        public void RemoveListeners()
        {
            if (Completed == null)
                return;

            foreach (Delegate d in Completed.GetInvocationList())
                Completed -= (Action<TaskResult<T>>)d;
        }

        public void CopyValueWhenCompleted(WaitableTask<T> destination)
            => AddOnCompletedListener((source) => CopyValue(source, destination));

        private void CopyValue(TaskResult<T> source, WaitableTask<T> destination)
        {
            if (source.IsError())
                destination.SetError(source.Exception);
            else
                destination.SetResult(source.Value);
        }
    }
}