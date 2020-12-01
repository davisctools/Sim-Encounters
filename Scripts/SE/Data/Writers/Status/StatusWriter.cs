using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class StatusWriter : IStatusWriter
    {
        protected IStatusWriter ServerStatusWriter { get; }
        protected IStatusWriter LocalStatusWriter { get; }
        public StatusWriter(
            [Inject(Id = SaveType.Server)] IStatusWriter serverStatusWriter,
            [Inject(Id = SaveType.Local)] IStatusWriter localStatusWriter)
        {
            ServerStatusWriter = serverStatusWriter;
            LocalStatusWriter = localStatusWriter;
        }

        public WaitableTask WriteStatus(UserEncounter encounter)
        {
            var task = new WaitableTask(); 
            var serverTask = ServerStatusWriter.WriteStatus(encounter);
            var localTask = LocalStatusWriter.WriteStatus(encounter);

            serverTask.AddOnCompletedListener(ProcessTaskResults);
            localTask.AddOnCompletedListener(ProcessTaskResults);

            return task;

            void ProcessTaskResults(TaskResult taskResult) => ProcessResults(task, serverTask, localTask);
        }

        protected void ProcessResults(WaitableTask task, WaitableTask serverTask, WaitableTask localTask)
        {
            if (task.IsCompleted() || !serverTask.IsCompleted() || !localTask.IsCompleted())
                return;

            task.SetCompleted();
        }
    }
}
