using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Button))]
    public class UrlButton : MonoBehaviour
    {
        public string Url { get => url; set => url = value; }
        [SerializeField] private string url;

        protected virtual void Awake()
        {
            if (string.IsNullOrWhiteSpace(Url))
                return;

            var button = GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(OpenUrl);
        }

        protected virtual void OpenUrl() => Application.OpenURL(Url);
    }
}