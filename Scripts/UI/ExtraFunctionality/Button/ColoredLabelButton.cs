using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Button))]
    public class ColoredLabelButton : MonoBehaviour
    {
        public TextMeshProUGUI Label { get => label; set => label = value; }
        [SerializeField] private TextMeshProUGUI label;

        protected Button Button { get; set; }
        protected virtual void Awake() => Button = GetComponent<Button>();
        protected virtual void Update() => Label.color = Button.image.color;
    }
}