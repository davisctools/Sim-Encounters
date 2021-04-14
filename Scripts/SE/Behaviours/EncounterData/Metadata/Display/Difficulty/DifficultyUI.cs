using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class DifficultyUI : MonoBehaviour
    {
        public virtual TextMeshProUGUI Label { get => label; set => label = value; }
        [SerializeField] private TextMeshProUGUI label;
        public virtual Image Image { get => image; set => image = value; }
        [SerializeField] private Image image;
        public virtual Sprite BeginnerSprite { get => beginnerSprite; set => beginnerSprite = value; }
        [SerializeField] private Sprite beginnerSprite;
        public virtual Sprite IntermediateSprite { get => intermediateSprite; set => intermediateSprite = value; }
        [SerializeField] private Sprite intermediateSprite;
        public virtual Sprite AdvancedSprite { get => advancedSprite; set => advancedSprite = value; }
        [SerializeField] private Sprite advancedSprite;

        public void Display(EncounterDifficulty difficulty)
        {
            Label.text = difficulty.ToString();
            Sprite sprite;
            if (difficulty == EncounterDifficulty.Beginner)
                sprite = BeginnerSprite;
            else if (difficulty == EncounterDifficulty.Intermediate)
                sprite = IntermediateSprite;
            else if (difficulty == EncounterDifficulty.Advanced)
                sprite = AdvancedSprite;
            else
                sprite = null;
            Image.sprite = sprite;
        }
    }
}