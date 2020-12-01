
namespace ClinicalTools.UI
{
    public class TabRow : TabGroup
    {
        protected override int Compare(TabGroup child1, TabGroup child2)
        {
            var xComparision = child1.transform.position.x.CompareTo(child2.transform.position.x);
            if (xComparision != 0)
                return xComparision;
            return -child1.transform.position.y.CompareTo(child2.transform.position.y);
        }
    }
}