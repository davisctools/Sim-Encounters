using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

namespace ClinicalTools.UI
{
    public enum CursorState
    {
        Normal, Draggable
    }

    public class MouseInput : MonoBehaviour
    {
#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void SetCanvasCursor(string str);
#endif

        public static MouseInput Instance { get; private set; }

        public SwipeHandler SwipeHandler { get; set; } = new SwipeHandler();

        [SerializeField] private Image cursorImage;
        public Image CursorImage { get => cursorImage; set => cursorImage = value; }
        [SerializeField] private Sprite normalCursorSprite;
        public Sprite NormalCursorSprite { get => normalCursorSprite; set => normalCursorSprite = value; }
        [SerializeField] private Sprite draggableCursorSprite;
        public Sprite DraggableCursorSprite { get => draggableCursorSprite; set => draggableCursorSprite = value; }
        [SerializeField] private Texture2D normalCursorTexture;
        public Texture2D NormalCursorTexture { get => normalCursorTexture; set => normalCursorTexture = value; }
        [SerializeField] private Texture2D draggableCursorTexture;
        public Texture2D DraggableCursorTexture { get => draggableCursorTexture; set => draggableCursorTexture = value; }

        public virtual bool CanDrag => DraggedObjects.Count == 0;

        public virtual CursorState CurrentCursorState {
            get {
                if (DraggedObjects.Count > 0)
                    return CursorState.Draggable;
                else if (CurrentCursorStates.Count == 0)
                    return CursorState.Normal;
                else
                    return CurrentCursorStates[CurrentCursorStates.Count - 1];
            }
        }
        protected virtual List<CursorState> CurrentCursorStates { get; } = new List<CursorState>();

        protected virtual List<IDraggable> DraggedObjects { get; } = new List<IDraggable>();

        protected virtual void UpdateCursor()
        {
            if (CursorImage == null || NormalCursorSprite == null || DraggableCursorSprite == null)
                return;
            SetCursorImage(CurrentCursorState);
        }

        protected virtual void SetCursorImage(CursorState cursorState)
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (cursorState == CursorState.Normal) {
                WindowsCursorConctroller.ChangeCursor(WindowsCursor.StandardArrow);
            } else if (cursorState == CursorState.Draggable) {
                WindowsCursorConctroller.ChangeCursor(WindowsCursor.FourPointedArrowPointingNorthSouthEastAndWest);
            }
#elif UNITY_WEBGL
            if (cursorState == CursorState.Normal) {
                SetCanvasCursor("default");
            } else if (cursorState == CursorState.Draggable) {
                SetCanvasCursor("move");
            }
#else
            UnityEngine.Debug.Log("aaaaa CURSOR");
            if (cursorState == CursorState.Normal) {
                CursorImage.sprite = NormalCursorSprite;
                Cursor.SetCursor(NormalCursorTexture, new Vector2(32, 32), CursorMode.ForceSoftware);
            } else if (cursorState == CursorState.Draggable) {
                CursorImage.sprite = DraggableCursorSprite;
                Cursor.SetCursor(DraggableCursorTexture, new Vector2(32, 32), CursorMode.ForceSoftware);
            }
#endif
        }

        public virtual void SetCursorState(CursorState cursorState)
        {
            CurrentCursorStates.Add(cursorState);
            UpdateCursor();
        }

        public virtual void RemoveCursorState(CursorState cursorState)
        {
            if (!CurrentCursorStates.Contains(cursorState))
                return;

            CurrentCursorStates.Remove(cursorState);
            UpdateCursor();
        }

        protected virtual void Awake()
        {
            Instance = this;
            UpdateCursor();
        }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        protected virtual void LateUpdate()
        {
            if (CurrentCursorState != CursorState.Normal)
                SetCursorImage(CurrentCursorState);
        }
#endif
        protected virtual void Update()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (CurrentCursorState != CursorState.Normal)
                SetCursorImage(CurrentCursorState);
#endif
            if (DraggedObjects.Count > 0) {
                if (Input.GetMouseButtonUp(0))
                    EndDrag(Input.mousePosition);
                else
                    Drag(Input.mousePosition);

                return;
            }

            if (Input.touches.Length == 1)
                SwipeHandler.TouchPosition(Input.touches[0].position);
            else if (Input.GetMouseButton(0))
                SwipeHandler.TouchPosition(Input.mousePosition);
            else if (SwipeHandler.IsSwiping)
                SwipeHandler.ReleaseTouch();
        }

        public bool RegisterDraggable(IDraggable draggable)
        {
            if (!Input.GetMouseButton(0))
                return false;

            DraggedObjects.Add(draggable);
            draggable.StartDrag(Input.mousePosition);
            UpdateCursor();
            return true;
        }

        protected virtual void Drag(Vector3 mousePosition)
        {
            foreach (var draggedObject in DraggedObjects)
                draggedObject.Drag(mousePosition);
        }

        protected virtual void EndDrag(Vector3 mousePosition)
        {
            foreach (var draggedObject in DraggedObjects)
                draggedObject.EndDrag(mousePosition);
            DraggedObjects.Clear();
            UpdateCursor();
        }
    }
}