﻿using ClinicalTools.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace ClinicalTools.SimEncounters
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ReaderPanelLabel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string valueName = null;
        protected virtual string Name => !string.IsNullOrWhiteSpace(valueName) ? valueName : name;

        public List<GameObject> ControlledObjects { get => controlledObjects; set => controlledObjects = value; }
        [SerializeField] private List<GameObject> controlledObjects;

        public string DefaultValue { get => defaultValue; set => defaultValue = value; }
        [SerializeField] private string defaultValue;
        public string Prefix { get => prefix; set => prefix = value; }
        [Multiline] [SerializeField] private string prefix;
        public string IgnoreValue { get => ignoreValue; set => ignoreValue = value; }
        [SerializeField] private string ignoreValue;
        public bool Trim { get => trim; set => trim = value; }
        [SerializeField] private bool trim;

        protected TextMeshProUGUI Label => (label == null) ? label = GetComponent<TextMeshProUGUI>() : label;
        private TextMeshProUGUI label;

        protected virtual VisitedLinksManager VisitedLinksManager { get; set; }
        protected virtual TagsFormatter TagsFormatter { get; set; }
        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            VisitedLinksManager visitedLinksManager,
            TagsFormatter tagsFormatter,
            ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            VisitedLinksManager = visitedLinksManager;
            TagsFormatter = tagsFormatter;

            PanelSelectedListener = panelSelectedListener;
            PanelSelectedListener.Selected += OnPanelSelected;
            if (PanelSelectedListener.CurrentValue != null)
                OnPanelSelected(PanelSelectedListener, PanelSelectedListener.CurrentValue);
        }

        protected Panel Panel { get; set; }
        protected virtual void OnDestroy()
        {
            if (PanelSelectedListener != null)
                PanelSelectedListener.Selected -= OnPanelSelected;
        }

        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs eventArgs)
        {
            if (Panel == eventArgs.Panel)
                return;

            Panel = eventArgs.Panel;

            if (!eventArgs.Panel.Values.ContainsKey(Name)) {
                HideControlledObjects();
                return;
            }

            var value = eventArgs.Panel.Values[Name];
            SetText(value);

            if (string.IsNullOrWhiteSpace(value) || value.Trim().Equals(IgnoreValue, StringComparison.InvariantCultureIgnoreCase))
                HideControlledObjects();
        }

        protected ParsedDocument ParsedDocument { get; set; }
        protected virtual void SetText(string value)
        {
            var text = "";
            if (Prefix != null)
                text += Prefix;
            if (value != null) {
                if (Trim)
                    value = value.Trim();

                text += value;
            }

            ParsedDocument = new ParsedDocument(text);
            Label.text = TagsFormatter.ProcessDocument(ParsedDocument);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            var linkCount = Label.textInfo.linkInfo.Length;
            if (linkCount == 0)
                return;

            var linkIndex = TMP_TextUtilities.FindIntersectingLink(Label, eventData.position, Camera.current);
            if (linkIndex < 0 || linkIndex >= linkCount)
                return;

            var link = Label.textInfo.linkInfo[linkIndex];
            var linkURL = link.GetLinkID();
            if (!linkURL.StartsWith("URL:"))
                return;

            var url = linkURL.Substring(4);
            VisitedLinksManager.VisitLink(url);
            Application.OpenURL(url);
            Label.text = TagsFormatter.ProcessDocument(ParsedDocument);
        }

        protected virtual void HideControlledObjects()
        {
            Label.text = DefaultValue;
            foreach (var controlledObject in ControlledObjects) {
                if (controlledObject == null)
                    Debug.LogError(gameObject.name);
                else
                    controlledObject.SetActive(false);
            }
        }
    }
}