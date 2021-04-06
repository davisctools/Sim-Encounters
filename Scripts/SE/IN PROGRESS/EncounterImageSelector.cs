using ClinicalTools.Collections;
using ClinicalTools.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterImageSelector : MonoBehaviour, IEncounterImageSelector, ICloseHandler
    {
        public RectTransform OptionParent { get => optionParent; set => optionParent = value; }
        [SerializeField] private RectTransform optionParent;
        public ToggleGroup OptionGroup { get => optionGroup; set => optionGroup = value; }
        [SerializeField] private ToggleGroup optionGroup;
        public Image PreviewImage { get => previewImage; set => previewImage = value; }
        [SerializeField] private Image previewImage;
        public GameObject PreviewObject { get => previewObject; set => previewObject = value; }
        [SerializeField] private GameObject previewObject;
        public Button AddButton { get => addButton; set => addButton = value; }
        [SerializeField] private Button addButton;
        public Button DeleteButton { get => deleteButton; set => deleteButton = value; }
        [SerializeField] private Button deleteButton;
        public Button UpdateButton { get => updateButton; set => updateButton = value; }
        [SerializeField] private Button updateButton;
        public Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;
        public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }
        [SerializeField] private CanvasGroup canvasGroup;
        public GameObject MustSaveFirstObject { get => mustSaveFirstObject; set => mustSaveFirstObject = value; }
        [SerializeField] private GameObject mustSaveFirstObject;

        protected BaseEncounterImageOption.Pool OptionFactory { get; set; }
        protected IEncounterImageUploader ImageUploader { get; set; }
        protected IEncounterImageUpdater ImageUpdater { get; set; }
        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        protected BaseMessageHandler MessageHandler { get; set; }
        [Inject]
        public virtual void Inject(
            BaseEncounterImageOption.Pool optionFactory,
            IEncounterImageUploader imageUploader,
            IEncounterImageUpdater imageUpdater,
            BaseConfirmationPopup confirmationPopup,
            BaseMessageHandler messageHandler)
        {

            OptionFactory = optionFactory;
            ImageUploader = imageUploader;
            ImageUpdater = imageUpdater;
            ConfirmationPopup = confirmationPopup;
            MessageHandler = messageHandler;
        }


        protected virtual void Start()
        {
            AddButton.onClick.AddListener(AddImage);
            DeleteButton.onClick.AddListener(DeleteImage);
            UpdateButton.onClick.AddListener(ReplaceImage);
            ApplyButton.onClick.AddListener(Apply);
        }


        protected User User { get; set; }
        protected Encounter Encounter { get; set; }
        protected KeyedCollection<EncounterImage> Images { get; set; }
        protected EncounterImage SelectedImage { get; set; }
        protected WaitableTask<EncounterImage> CurrentTask { get; set; }
        protected Dictionary<string, BaseEncounterImageOption> ImageOptions { get; } = new Dictionary<string, BaseEncounterImageOption>();

        public virtual WaitableTask<EncounterImage> SelectImage(User user, Encounter encounter, string key)
        {
            if (encounter.Metadata.RecordNumber <= 0) {
                MessageHandler.ShowMessage("Save encounter before adding image.");
                return new WaitableTask<EncounterImage>(new Exception("Encounter must be saved before selecting images."));
            } 

            gameObject.SetActive(true);
            if (CurrentTask?.IsCompleted() == false)
                CurrentTask.SetError(new Exception());

            User = user;
            Encounter = encounter;
            if (Images != encounter.Content.Images)
                ResetImages(encounter.Content.Images);

            CurrentTask = new WaitableTask<EncounterImage>();

            if (key != null && Images.ContainsKey(key)) {
                SelectedImage = Images[key];
                ImageOptions[key].Select();
            } else {
                if (SelectedImage != null && ImageOptions.ContainsKey(SelectedImage.Key))
                    ImageOptions[SelectedImage.Key].Deselect();
                SelectedImage = null;
            }

            return CurrentTask;
        }

        protected virtual void ResetImages(KeyedCollection<EncounterImage> images)
        {
            Images = images;
            foreach (var option in ImageOptions.Values)
                DespawnOption(option);
            ImageOptions.Clear();
            foreach (var image in Images.Values)
                AddImageOption(image);
        }

        protected virtual BaseEncounterImageOption AddImageOption(EncounterImage image)
        {
            var option = OptionFactory.Spawn();

            option.transform.SetParent(optionParent);
            option.SetGroup(optionGroup);
            optionParent.transform.localScale = Vector3.one;

            option.ImageSelected += OnImageSelected;
            option.ImageDeselected += OnImageDeselected;
            option.Initialize(image);
            ImageOptions.Add(image.Key, option);
            UpdateButton.interactable = DeleteButton.interactable = true;

            return option;
        }

        protected virtual void RemoveEncounterImage(string key)
        {
            var option = ImageOptions[key];
            ImageOptions.Remove(key);
            DespawnOption(option);
            UpdateButton.interactable = DeleteButton.interactable = false;
        }
        protected virtual void DespawnOption(BaseEncounterImageOption option)
        {
            option.ImageSelected -= OnImageSelected;
            option.ImageDeselected -= OnImageDeselected;
            OptionFactory.Despawn(option);
        }


        protected virtual void Cancel()
        {
            if (CurrentTask?.IsCompleted() == true)
                CurrentTask.SetError(new Exception());
            gameObject.SetActive(false);
        }
        protected virtual void Apply()
        {
            if (CurrentTask?.IsCompleted() == false)
                CurrentTask.SetResult(SelectedImage);
            gameObject.SetActive(false);
        }

        protected virtual void OnImageSelected(EncounterImage image)
        {
            SelectedImage = image;
            PreviewImage.sprite = image.Sprite;
            PreviewObject.SetActive(true);
        }
        protected virtual void OnImageDeselected(EncounterImage image)
        {
            if (SelectedImage != image)
                return;

            SelectedImage = null;
            PreviewImage.sprite = null;
            PreviewObject.SetActive(false);
        }

        protected virtual void AddImage()
        {
            var task = ImageUploader.UploadImage(User, Encounter);
            task.AddOnCompletedListener(OnImageAdded);
        }
        protected virtual void OnImageAdded(TaskResult<EncounterImage> image)
        {
            if (!image.HasValue())
                return;

            var value = image.Value;
            AddImageOption(value).Select();
        }

        protected virtual void ReplaceImage()
        {
            var task = ImageUpdater.UpdateImage(User, Encounter, SelectedImage);
            task.AddOnCompletedListener(OnImageReplaced);
        }
        protected virtual void OnImageReplaced(TaskResult<EncounterImage> image)
        {
            if (!image.HasValue())
                return;

            var value = image.Value;
            RemoveEncounterImage(value.Key);
            AddImageOption(value).Select();
        }

        protected virtual void DeleteImage()
            => ConfirmationPopup.ShowConfirmation(OnDeleteImage, "Delete Image", "Are you sure you want to delete the image?");
        protected virtual void OnDeleteImage()
        {
            Images.Remove(SelectedImage.Key);
            RemoveEncounterImage(SelectedImage.Key);
        }

        public virtual void Close(object sender) => Cancel();
    }
}