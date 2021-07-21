

using System.Linq;

namespace ClinicalTools.SimEncounters
{
    public class UserEncounterReader : IUserEncounterReader
    {
        private readonly IEncounterReader dataReader;
        private readonly IDetailedStatusReader detailedStatusReader;
        public UserEncounterReader(IEncounterReader dataReader, IDetailedStatusReader detailedStatusReader)
        {
            this.dataReader = dataReader;
            this.detailedStatusReader = detailedStatusReader;
        }

        public WaitableTask<UserEncounter> GetUserEncounter(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus, SaveType saveType)
        {
            var encounterData = dataReader.GetEncounter(user, metadata, saveType);
            var detailedStatus = detailedStatusReader.GetDetailedStatus(user, metadata, basicStatus);

            var encounter = new WaitableTask<UserEncounter>();
            void processResults() => ProcessResults(user, encounter, encounterData, detailedStatus);
            encounterData.AddOnCompletedListener((result) => processResults());
            detailedStatus.AddOnCompletedListener((result) => processResults());

            return encounter;
        }

        protected void ProcessResults(User user,
            WaitableTask<UserEncounter> result,
            WaitableTask<Encounter> encounterData,
            WaitableTask<EncounterStatus> detailedStatus)
        {
            if (result.IsCompleted() || !encounterData.IsCompleted() || !detailedStatus.IsCompleted())
                return;

            var encounter = new UserEncounter(user, encounterData.Result.Value, detailedStatus.Result.Value);
            SetEncounterStart(encounter);

            result.SetResult(encounter);
        }

        protected virtual void SetEncounterStart(UserEncounter encounter)
        {
            var startPoint = GetStartPoint(encounter);
            if (startPoint == null)
                return;

            encounter.Data.Content.NonImageContent.SetCurrentSection(startPoint.Section.Data);
            startPoint.Section.Data.SetCurrentTab(startPoint.Tab.Data);
        }

        protected SectionTabPair GetStartPoint(UserEncounter encounter)
        {
            UserSection lastSection = null;
            foreach (var section in encounter.Sections.Values) {
                if (section.IsRead()) {
                    lastSection = section;
                    continue;
                }

                return GetStartPoint(lastSection, section);
            }

            return null;
        }

        protected SectionTabPair GetStartPoint(UserSection lastReadSection, UserSection firstUnreadSection)
        {
            UserTab tab = GetLastSequentialTabRead(firstUnreadSection);
            if (tab != null)
                return new SectionTabPair(firstUnreadSection, tab);
            if (lastReadSection == null || lastReadSection.Tabs.ValueArr.Length == 0)
                return null;

            tab = lastReadSection.Tabs.ValueArr[lastReadSection.Tabs.ValueArr.Length - 1];
            return new SectionTabPair(lastReadSection, tab);
        }

        protected UserTab GetLastSequentialTabRead(UserSection section) {
            UserTab lastTab = null;
            foreach (var tab in section.Tabs.Values) {
                if (!tab.IsRead())
                    return lastTab;
                lastTab = tab;
            }
            return lastTab;
        }

        protected class SectionTabPair
        {
            public UserSection Section { get; set; }
            public UserTab Tab { get; set; }

            public SectionTabPair(UserSection section, UserTab tab)
            {
                Section = section;
                Tab = tab;
            }
        }
    }
}