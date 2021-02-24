using static ClinicalTools.SimEncounters.ReaderGeneralSectionHandler;

namespace ClinicalTools.SimEncounters
{
    public interface ISwipableSection
    {
        void StartMove();
        void Move(Direction dir, float dist);
        void EndMove();
    }
}
