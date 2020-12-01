using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseLoginHandler : MonoBehaviour, ILoginHandler
    {
        public abstract WaitableTask<User> Login();
    }
}