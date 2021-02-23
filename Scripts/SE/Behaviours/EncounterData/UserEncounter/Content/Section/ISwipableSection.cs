using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface ISwipableSection
    {
        public enum Direction { NA, Left, Right }
        void SwipeStart();
        void SwipeUpdate(Direction dir, float dist);
        void SwipeEnd(Direction dir, float dist, bool changingSections);

        // void 
    }
    public interface ISwipableSection2
    {
        void StartMove();
        void Move(ISwipableSection.Direction dir, float dist);
        void EndMove();

        // void 
    }
}
