using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class UserStatus
    {
        public HashSet<string> StartedCategories { get; } = new HashSet<string>();
    }
}