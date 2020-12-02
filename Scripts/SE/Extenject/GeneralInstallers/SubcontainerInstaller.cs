using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class SubcontainerInstaller : MonoInstaller
    {
        /// <summary>
        /// Should not be called directly, but allows the class to be used as a MonoInstaller.
        /// </summary>
        public override void InstallBindings() => Install(Container);

        public abstract void Install(DiContainer container);
    }
}