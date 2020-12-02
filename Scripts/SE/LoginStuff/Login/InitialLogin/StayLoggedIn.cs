using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class StayLoggedIn
    {
        private const string PLAYER_PREF_KEY = "StayLoggedIn";

        private const int FALSE_VALUE = 0;
        private const int TRUE_VALUE = 1;

        public bool Value => PlayerPrefs.GetInt(PLAYER_PREF_KEY) == TRUE_VALUE;
        public void SetValue(bool value) => PlayerPrefs.SetInt(PLAYER_PREF_KEY, value ? TRUE_VALUE : FALSE_VALUE);
    }
}