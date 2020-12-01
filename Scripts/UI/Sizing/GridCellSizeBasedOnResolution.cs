using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class GridCellSizeBasedOnResolution : MonoBehaviour
    {
        public float LandscapeCellsPerRow { get => landscapeCellsPerRow; set => landscapeCellsPerRow = value; }
        [Tooltip("Cells per row in a 1920x1080 resolution")]
        [SerializeField] private float landscapeCellsPerRow;
        public float PortraitCellsPerRow { get => portraitCellsPerRow; set => portraitCellsPerRow = value; }
        [Tooltip("Cells per row in a 1080x1920 resolution")]
        [SerializeField] private float portraitCellsPerRow;
        public float CellHeightPerWidth { get => cellHeightPerWidth; set => cellHeightPerWidth = value; }
        [SerializeField] private float cellHeightPerWidth;

        private readonly Vector2 LandscapeScreenDimensions = new Vector2(1920, 1080);
        private readonly Vector2 PortraitScreenDimensions = new Vector2(1080, 1920);

        protected GridLayoutGroup Grid { get; set; }

        protected virtual void Awake()
        {
            Grid = GetComponent<GridLayoutGroup>();
            UpdateSize();
        }
        protected virtual void Update() => UpdateSize();

        private Vector2 canvasSize = new Vector2();
        private Rect rect = new Rect();
        private Rect padding;
        protected virtual void UpdateSize()
        {
            var currentCanvasSize = new Vector2(Screen.width, Screen.height);
            var rectTrans = (RectTransform)transform;
            var currentRect = rectTrans.rect;
            var currentPadding = new Rect(Grid.padding.top, Grid.padding.bottom, Grid.padding.left, Grid.padding.right);

            if (currentCanvasSize == canvasSize && currentRect == rect && currentPadding == padding)
                return;

            canvasSize = currentCanvasSize;
            rect = currentRect;
            padding = currentPadding;

            var cellSize = Grid.cellSize;
            cellSize.x = GetCellWidth(canvasSize);
            if (ShouldSizeHeight())
                cellSize.y = cellSize.x * CellHeightPerWidth;

            Grid.cellSize = cellSize;
        }

        protected virtual float GetCellWidth(Vector2 canvasSize)
        {
            var width = rect.width;
            var spacing = Grid.spacing.x;
            var cellsPerRow = GetCellsPerRow(canvasSize);
            cellsPerRow = Mathf.Max(1, cellsPerRow);
            var totalSpacing = (cellsPerRow - 1) * spacing;
            var totalPadding = Grid.padding.left + Grid.padding.right;
            return (width - totalSpacing - totalPadding) / cellsPerRow - .001f;
        }
        protected virtual int GetCellsPerRow(Vector2 dimensions)
        {
            var slope = GetPerRowSlope();
            var y1 = PortraitCellsPerRow;
            var x1 = GetAspectRatioProportion(PortraitScreenDimensions);
            var x = GetAspectRatioProportion(dimensions);

            var proportion = slope * (x - x1) + y1;
            return Mathf.RoundToInt(proportion);
        }
        protected virtual float GetPerRowSlope()
        {
            var y1 = PortraitCellsPerRow;
            var y2 = LandscapeCellsPerRow;
            var x1 = GetAspectRatioProportion(PortraitScreenDimensions);
            var x2 = GetAspectRatioProportion(LandscapeScreenDimensions);
            return (y1 - y2) / (x1 - x2);
        }

        protected virtual float GetAspectRatioProportion(Vector2 dimensions)
            => dimensions.x / dimensions.y;

        private const float Tolerance = .0001f;
        protected virtual bool ShouldSizeHeight() => CellHeightPerWidth > Tolerance;
    }
}