﻿namespace ClinicalTools.SimEncounters
{
    public interface IEncounterDataTextRetriever
    {
        WaitableTask<string> GetDataText(User user, OldEncounterMetadata metadata);
    }
}