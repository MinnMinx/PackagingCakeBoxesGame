using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PackagingCakeBoxes
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer boardRenderer;
        private Transform boardTransform;

        public float GridSize { get; private set; }
        public Vector2Int GridCount { get; private set; }

        /// <summary>
        /// Get world position from Grid position
        /// </summary>
        /// <param name="colIndex">Column index of the grid position</param>
        /// <param name="rowIndex">Row index of the grid position</param>
        /// <returns></returns>
        public Vector3 GetWorldPos(int colIndex, int rowIndex)
        {
            // Check invalid cases
            if (colIndex < 0 || rowIndex < 0 ||
                GridCount.x == 0 || GridCount.y == 0 ||
                colIndex >= GridCount.x || rowIndex >= GridCount.y)
            {
                return Vector3.zero;
            }
            Vector3 firstGridPos = new Vector3(boardTransform.position.x - GridSize * (GridCount.x / 2f - 0.5f),
                                               boardTransform.position.y + GridSize * (GridCount.y / 2f - 0.5f));
            return firstGridPos + new Vector3(colIndex * GridSize, -rowIndex * GridSize);
        }

        public void Awake()
        {
            if (boardRenderer == null || boardRenderer.transform == null)
            {
                Debug.LogError("Cannot initialize game board because missing board's MeshRenderer");
                return;
            }

            boardTransform = boardRenderer.transform;
            Vector2 boardSize = boardTransform.localScale;
            LevelData data = GameInfo.Instance.curLevel?.levelData;
            if (data == null)
                return;

            GridCount = new Vector2Int(data.ColumnCount, data.RowCount);
            GridSize = boardSize.x / data.ColumnCount;
            UpdateGridRendererCount();
        }

        private void UpdateGridRendererCount()
        {
            // Update rows
            var boardScale = boardTransform.localScale;
            boardScale.y = GridSize * GridCount.y;
            boardTransform.localScale = boardScale;

            // Update UV
            Material frameMat = boardRenderer.material;
            frameMat.mainTextureScale = GridCount;
        }
    }
}