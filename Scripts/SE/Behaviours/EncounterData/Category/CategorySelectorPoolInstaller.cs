using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class CategorySelectorPoolInstaller : MonoInstaller
    {
        public CategorySelector CategorySelectorPrefab { get => categorySelectorPrefab; set => categorySelectorPrefab = value; }
        [SerializeField] private CategorySelector categorySelectorPrefab;
        public Transform PoolParent { get => poolParent; set => poolParent = value; }
        [SerializeField] private Transform poolParent;
        public int InitialSize { get => initialSize; set => initialSize = value; }
        [SerializeField] private int initialSize;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<CategorySelector, CategorySelector.Pool>()
                .WithInitialSize(InitialSize)
                .FromComponentInNewPrefab(CategorySelectorPrefab)
                .UnderTransform(PoolParent);
        }
    }
}