using System;

namespace ClinicalTools.SimEncounters
{
    public interface ILogoutHandler
    {
        event Action Logout;
    }
}