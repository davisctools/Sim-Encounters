using ClinicalTools.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EmbeddedReaderSideController : MonoBehaviour, IReaderSceneStarter, IMenuSceneStarter
    {
        [SerializeField] private Button button;
        [SerializeField] private AnchorsToMatchAspectRatio aspectRatioHandler;

        protected AnimationMonitor AnimationMonitor { get; set; }
        protected ISelector<LoadingReaderSceneInfoSelectedEventArgs> LoadingReaderSceneInfoSelector { get; set; }
        protected ISelectedListener<WriterSceneInfoSelectedEventArgs> WriterSceneInfoSelector { get; set; }
        protected ICurve Curve { get; set; }
        [Inject]
        public virtual void Inject(
            AnimationMonitor animationMonitor,
            ISelector<LoadingReaderSceneInfoSelectedEventArgs> loadingReaderSceneInfoSelector,
            ISelectedListener<WriterSceneInfoSelectedEventArgs> writerSceneInfoSelector,
            ICurve curve)
        {
            AnimationMonitor = animationMonitor;
            LoadingReaderSceneInfoSelector = loadingReaderSceneInfoSelector;
            WriterSceneInfoSelector = writerSceneInfoSelector;
            Curve = curve;
        }

        protected virtual void Start()
        {
            button.onClick.AddListener(OnButtonClicked);

            WriterSceneInfoSelector.Selected += Initialize;
            if (WriterSceneInfoSelector.CurrentValue != null)
                Initialize(this, WriterSceneInfoSelector.CurrentValue);

            aspectRatioHandler.Offset = aspectRatioHandler.Width;
        }

        protected float LastScreenWidth { get; set; }
        protected float LastScreenHeight { get; set; }
        protected virtual void Update()
        {
            if (Screen.width == LastScreenWidth && Screen.height == LastScreenHeight)
                return;

            LastScreenWidth = Screen.width;
            LastScreenHeight = Screen.height;
            if (opened)
                return;

            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            aspectRatioHandler.Offset = aspectRatioHandler.Width;
        }

        protected virtual void Initialize(object sender, WriterSceneInfoSelectedEventArgs e)
        {
            var sceneInfo = e.SceneInfo;
            var encounterStatus = new EncounterStatus(new EncounterBasicStatus(), new EncounterContentStatus());
            var encounter = new UserEncounter(sceneInfo.User, sceneInfo.Encounter, encounterStatus);
            var encounterResult = new WaitableTask<UserEncounter>(encounter);
            var loadingInfo = new LoadingReaderSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounterResult);
            LoadingReaderSceneInfoSelector.Select(this, new LoadingReaderSceneInfoSelectedEventArgs(loadingInfo));
        }

        public virtual void StartScene(LoadingReaderSceneInfo encounterSceneInfo)
        {
            LoadingReaderSceneInfoSelector.Select(this, new LoadingReaderSceneInfoSelectedEventArgs(encounterSceneInfo));

            if (!opened)
                OnButtonClicked();
        }

        private bool opened = false;
        private Coroutine currentCoroutine;
        protected virtual void OnButtonClicked()
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            if (opened)
                currentCoroutine = StartCoroutine(Close());
            else
                currentCoroutine = StartCoroutine(Open());

            opened = !opened;
        }

        protected virtual float AnimationTime { get; } = .5f;
        protected virtual IEnumerator Close()
        {
            AnimationMonitor.AnimationStarting(this);
            var curveX = Curve.GetCurveX(aspectRatioHandler.Offset / aspectRatioHandler.Width);
            while (aspectRatioHandler.Offset < aspectRatioHandler.Width) {
                yield return null;
                curveX += Time.deltaTime / AnimationTime;
                aspectRatioHandler.Offset = Curve.GetCurveY(curveX) * aspectRatioHandler.Width;
            }
            aspectRatioHandler.Offset = aspectRatioHandler.Width;
            AnimationMonitor.AnimationStopping(this);
        }
        protected virtual IEnumerator Open()
        {
            AnimationMonitor.AnimationStarting(this);
            var curveX = Curve.GetCurveX(1 - aspectRatioHandler.Offset / aspectRatioHandler.Width);
            while (aspectRatioHandler.Offset > 0) {
                yield return null;
                curveX += Time.deltaTime / AnimationTime;
                aspectRatioHandler.Offset = aspectRatioHandler.Width - Curve.GetCurveY(curveX) * aspectRatioHandler.Width;
            }
            aspectRatioHandler.Offset = 0;
            AnimationMonitor.AnimationStopping(this);
        }

        public virtual void StartScene(LoadingMenuSceneInfo loadingSceneInfo)
        {
            if (opened)
                OnButtonClicked();
        }
    }
}