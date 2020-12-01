using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.UI
{
    /// <summary>
    /// Allows groups of tabbable fields to be sorted by a group to allow finding the next/last field.
    /// </summary>
    public abstract class TabGroup : MonoBehaviour, IComparable<TabGroup>
    {
        private const string TOP_TAG = "FieldGroup";
        private TabGroup group;
        private CanvasGroup canvasGroup;
        /// <summary>
        /// True if neither this group nor any of its parent groups have a canvasGroup with an alpha less than .5 
        /// </summary>
        protected bool Visible {
            get {
                if (canvasGroup != null && canvasGroup.alpha < .5f)
                    return false;
                else if (group == null)
                    return true;
                else
                    return group.Visible;
            }
        }

        /// <summary>
        /// Finds all tabbable fields contained under the top most parent group.
        /// </summary>
        /// <returns>A list of all tabbable fields contained under the top most parent group.</returns>
        /// <remarks>This should be reworked so their can be "top groups" that are under this group's "top group," without all their tabbable fields being included.</remarks>
        protected List<TabField> AllFields()
        {
            if (group != null)
                return group.AllFields();

            var fields = new List<TabField>(GetComponentsInChildren<TabField>());
            fields.Sort();

            return fields;
        }

        protected virtual void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            // The top tab group should have a "FieldGroup" tag to indicate it's at the top
            // This could hypothetically be changed to an editor bool if wanted, but I had already started to use tags for organization, so I just continued with that
            if (!CompareTag(TOP_TAG))
                group = transform.parent.GetComponentInParent<TabGroup>();
        }

        /// <summary>
        /// Compares the elements based on their lowest shared group.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(TabGroup other)
        {
            if (group == null)
                return 0;

            var otherGrp = other;
            while (otherGrp.group != null) {
                if (group == otherGrp.group)
                    return group.Compare(this, otherGrp);

                otherGrp = otherGrp.group;
            }

            return group.CompareTo(other);
        }

        protected virtual int Compare(TabGroup child1, TabGroup child2) => 0;
    }
}
