using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class MenuEncountersInfoReader : IMenuEncountersInfoReader
    {
        private readonly IMenuEncountersReader menuEncountersReader;
        public MenuEncountersInfoReader(IMenuEncountersReader menuEncountersReader)
        {
            this.menuEncountersReader = menuEncountersReader;
        }

        public virtual WaitableTask<IMenuEncountersInfo> GetMenuEncountersInfo(User user)
        {
            var categories = new WaitableTask<IMenuEncountersInfo>();

            var menuEncounters = menuEncountersReader.GetMenuEncounters(user);
            menuEncounters.AddOnCompletedListener((result) => ProcessResults(user, categories, result));

            return categories;
        }
        protected virtual void ProcessResults(User user, WaitableTask<IMenuEncountersInfo> result, TaskResult<List<MenuEncounter>> menuEncounters)
        {
            if (menuEncounters.Value == null) {
                result.SetError(null);
                return;
            }

            var menuEncountersInfo = new MenuEncountersInfo(user);
            foreach (var menuEncounter in menuEncounters.Value)
                menuEncountersInfo.AddEncounter(menuEncounter);

            result.SetResult(menuEncountersInfo);
        }
    }
}