using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderFeedbackUI : MonoBehaviour, IReaderFeedbackUI
    {
        public TextMeshProUGUI OptionTypeLabel { get => optionTypeLabel; set => optionTypeLabel = value; }
        [SerializeField] private TextMeshProUGUI optionTypeLabel;
        public OptionType OptionType {
            get {
                switch (OptionTypeLabel.text.Trim().ToLowerInvariant()) {
                    case "correct":
                        return OptionType.Correct;
                    case "incorrect":
                        return OptionType.Incorrect;
                    case "neutral":
                        return OptionType.Neutral;
                    default:
                        return OptionType.PartiallyCorrect;
                }
            }
        }

        public TextMeshProUGUI IsCorrectLabel { get => isCorrectLabel; set => isCorrectLabel = value; }
        [SerializeField] private TextMeshProUGUI isCorrectLabel;
        public Image Stripes { get => stripes; set => stripes = value; }
        [SerializeField] private Image stripes;
        public List<Image> ColoredImages { get => coloredImages; set => coloredImages = value; }
        [SerializeField] private List<Image> coloredImages = new List<Image>();
        public List<Image> AnsweredColoredImages { get => answeredColoredImages; set => answeredColoredImages = value; }
        [SerializeField] private List<Image> answeredColoredImages = new List<Image>();
        public Button CloseButton { get => closeButton; set => closeButton = value; }
        [SerializeField] private Button closeButton;
        public List<GameObject> ControlledObjects { get => controlledObjects; set => controlledObjects = value; }
        [SerializeField] private List<GameObject> controlledObjects = new List<GameObject>();
        public List<GameObject> CorrectObjects { get => correctObjects; set => correctObjects = value; }
        [SerializeField] private List<GameObject> correctObjects = new List<GameObject>();
        public List<GameObject> IncorrectObjects { get => incorrectObjects; set => incorrectObjects = value; }
        [SerializeField] private List<GameObject> incorrectObjects = new List<GameObject>();

        protected virtual IFeedbackColorManager ColorManager { get; set; }
        [Inject] public virtual void Inject(IFeedbackColorManager colorManager) => ColorManager = colorManager;

        protected virtual void Awake()
        {
            if (CloseButton != null)
                CloseButton.onClick.AddListener(CloseFeedback);
        }

        public virtual void ShowFeedback(bool isOn)
        {
            gameObject.SetActive(true);

            foreach (var controlledObject in ControlledObjects)
                controlledObject.SetActive(true);
            foreach (var incorrectObject in IncorrectObjects)
                incorrectObject.SetActive(OptionType != OptionType.Correct);
            foreach (var correctObject in CorrectObjects)
                correctObject.SetActive(OptionType == OptionType.Correct);

            Color color = ColorManager.GetColor(OptionType);
            foreach (var image in ColoredImages)
                image.color = color;
            if (!isOn)
                color = ColorManager.GetDefaultColor();
            foreach (var image in AnsweredColoredImages)
                image.color = color;

            if (OptionType == OptionType.Neutral)
                IsCorrectLabel.gameObject.SetActive(false);
            else
                IsCorrectLabel.text = GetOptionTypeText(OptionType, isOn);

            Stripes.gameObject.SetActive(ShowStripes(OptionType, isOn));
        }


        protected virtual string GetOptionTypeText(OptionType optionType, bool isOn)
        {
            if (optionType == OptionType.Incorrect)
                return "Incorrect";
            else if (optionType == OptionType.PartiallyCorrect)
                return "Partially Correct";
            else if (!isOn)
                return "Missed Correct Response";
            else
                return "Correct";
        }

        protected virtual bool ShowStripes(OptionType optionType, bool isOn) => optionType == OptionType.Correct && !isOn;

        public virtual void SetParent(Transform parent)
            => transform.SetParent(parent);

        public virtual void CloseFeedback()
        {
            Color color = new Color(0.9372549f, 0.9372549f, 0.9372549f, 1f);
            foreach (var image in ColoredImages)
                image.color = color;
            gameObject.SetActive(false);
        }
    }
}