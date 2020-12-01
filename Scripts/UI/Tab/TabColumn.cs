
namespace ClinicalTools.UI
{
    public class TabColumn : TabGroup
    {
        protected override int Compare(TabGroup child1, TabGroup child2)
        {
            var yComparision = -child1.transform.position.y.CompareTo(child2.transform.position.y);
            if (yComparision != 0)
                return yComparision;
            return child1.transform.position.x.CompareTo(child2.transform.position.x);
        }
    }
}