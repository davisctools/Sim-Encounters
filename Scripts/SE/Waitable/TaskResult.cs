using System;

namespace ClinicalTools.SimEncounters
{
    public class TaskResult
    {
        public Exception Exception { get; }
        public bool IsError() => Exception != null;
        public TaskResult() { }
        public TaskResult(Exception exception) => Exception = exception;
    }
    public class TaskResult<T>
    {
        private readonly T value = default;
        public T Value {
            get {
                if (Exception != null)
                    throw Exception;
                return value;
            }
        }
        public Exception Exception { get; }

        public TaskResult(T value) => this.value = value;
        public TaskResult(Exception exception) => Exception = exception;

        public bool IsError() => Exception != null;
        public bool HasValue() => Exception == null && Value != null;
    }
}