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
            var button = GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(OpenUrl);
        }

        protected virtual void OpenUrl()
        {
            if (!string.IsNullOrWhiteSpace(Url))
                Application.OpenURL(Url);
        }
    }
}