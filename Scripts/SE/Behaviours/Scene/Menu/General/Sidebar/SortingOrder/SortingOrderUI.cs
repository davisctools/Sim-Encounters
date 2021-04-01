using UnityEngine;
using UnityEngine.UI;
using System;
using ClinicalTools.UI.Extensions;


namespace ClinicalTools.SimEncounters
{
    public class SortingOrderUI : MonoBehaviour
    {
        public Toggle PatientNameAscending { get => patientNameAscending; set => patientNameAscending = value; }
        [SerializeField] private Toggle patientNameAscending;
        public Toggle PatientNameDescending { get => patientNameDescending; set => patientNameDescending = value; }
        [SerializeField] private Toggle patientNameDescending;
        public Toggle DatePublishedAscending { get => datePublishedAscending; set => datePublishedAscending = value; }
        [SerializeField] private Toggle datePublishedAscending;
        public Toggle DatePublishedDescending { get => datePublishedDescending; set => datePublishedDescending = value; }
        [SerializeField] private Toggle datePublishedDescending;
        public Toggle AuthorAscending { get => authorAscending; set => authorAscending = value; }
        [SerializeField] private Toggle authorAscending;
        public Toggle AuthorDescending { get => authorDescending; set => authorDescending = value; }
        [SerializeField] private Toggle authorDescending;
        public Toggle DifficultyAscending { get => difficultyAscending; set => difficultyAscending = value; }
        [SerializeField] private Toggle difficultyAscending;
        public Toggle DifficultyDescending { get => difficultyDescending; set => difficultyDescending = value; }
        [SerializeField] private Toggle difficultyDescending;

        public Comparison<MenuEncounter> Comparison { get; protected set; } = ComparePatientNameAscending;
        public event Action<Comparison<MenuEncounter>> SortingOrderChanged;

        public void Awake()
        {
            PatientNameAscending.isOn = true;

            PatientNameAscending.AddOnSelectListener(() => SetComparer(ComparePatientNameAscending));
            PatientNameDescending.AddOnSelectListener(() => SetComparer(ComparePatientNameDescending));
            DatePublishedAscending.AddOnSelectListener(() => SetComparer(CompareDatePublishedAscending));
            DatePublishedDescending.AddOnSelectListener(() => SetComparer(CompareDatePublishedDescending));
            AuthorAscending.AddOnSelectListener(() => SetComparer(CompareAuthorAscending));
            AuthorDescending.AddOnSelectListener(() => SetComparer(CompareAuthorDescending));
            DifficultyAscending.AddOnSelectListener(() => SetComparer(CompareDifficultyAscending));
            DifficultyDescending.AddOnSelectListener(() => SetComparer(CompareDifficultyDescending));
        }

        protected void SetComparer(Comparison<MenuEncounter> comparison)
        {
            Comparison = comparison;
            SortingOrderChanged?.Invoke(comparison);
        }

        protected static int ComparePatientNameAscending(MenuEncounter x, MenuEncounter y)
            => x.GetLatestMetadata().Title.CompareTo(y.GetLatestMetadata().Title);
        protected static int ComparePatientNameDescending(MenuEncounter x, MenuEncounter y)
            => y.GetLatestMetadata().Title.CompareTo(x.GetLatestMetadata().Title);
        protected static int CompareDatePublishedAscending(MenuEncounter x, MenuEncounter y)
            => x.GetLatestMetadata().DateModified.CompareTo(y.GetLatestMetadata().DateModified);
        protected static int CompareDatePublishedDescending(MenuEncounter x, MenuEncounter y)
            => y.GetLatestMetadata().DateModified.CompareTo(x.GetLatestMetadata().DateModified);
        protected static int CompareAuthorAscending(MenuEncounter x, MenuEncounter y)
            => x.GetLatestMetadata().Author.Name.CompareTo(y.GetLatestMetadata().Author.Name);
        protected static int CompareAuthorDescending(MenuEncounter x, MenuEncounter y)
            => y.GetLatestMetadata().Author.Name.CompareTo(x.GetLatestMetadata().Author.Name);
        protected static int CompareDifficultyAscending(MenuEncounter x, MenuEncounter y)
            => x.GetLatestMetadata().Difficulty.CompareTo(y.GetLatestMetadata().Difficulty);
        protected static int CompareDifficultyDescending(MenuEncounter x, MenuEncounter y)
            => y.GetLatestMetadata().Difficulty.CompareTo(x.GetLatestMetadata().Difficulty);
    }
}