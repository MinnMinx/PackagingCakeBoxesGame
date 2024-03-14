using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PackagingCakeBoxes
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Prefabs"), SerializeField]
        private GameObject ObstaclePrefab;
        [SerializeField]
        private GameObject CakePrefab;
        [SerializeField]
        private GameObject GiftPrefab;

        [Header("Scene Reference"), SerializeField]
        private Transform gameWorld;

        [Header("Preferences"), SerializeField]
        private float moveDuration = 0.8f;

        private GridManager gridManager;
        private List<MoveableObject> moveableObjects = new List<MoveableObject>();
        private bool isObjMoving = false, stopped = false;

        public float TimeLeft { get; private set; }

        private static float DEFAULT_LEVEL_TIME = 45;
        private static float STAR_3_TIME = 35;
        private static float STAR_2_TIME = 20;
        private static float STAR_1_TIME = 10;

        private void Start()
        {
            gridManager = GetComponent<GridManager>();
            if (gridManager == null)
            {
                Debug.LogError("Cannot start level because GridManager not found");
                return;
            }
            TimeLeft = DEFAULT_LEVEL_TIME;

            InitializeLevel();
        }

        void InitializeLevel()
        {
            LevelData levelData = GameInfo.Instance.curLevel?.levelData;
            if (levelData == null)
                return;

            // Spawning objects
            // Spawning obj
            for (int i = 0; i < levelData.ObstaclesPos.Length; i++)
            {
                var gridPos = levelData.ObstaclesPos[i];
                var obstacle = Instantiate(ObstaclePrefab, gameWorld);
                obstacle.transform.position = gridManager.GetWorldPos(gridPos.x, gridPos.y);
                obstacle.transform.localScale = new Vector3(gridManager.GridSize, gridManager.GridSize);
            }
            SpawnMovableObject(levelData);
        }
        void SpawnMovableObject(LevelData levelData)
        {
            // Spawn cakes
            for (int i = 0; i < levelData.CakesPos.Length; i++)
            {
                var gridPos = levelData.CakesPos[i];
                var cake = Instantiate(CakePrefab, gameWorld);
                cake.transform.position = gridManager.GetWorldPos(gridPos.x, gridPos.y);
                cake.transform.localScale = new Vector3(gridManager.GridSize, gridManager.GridSize);
                moveableObjects.Add(new MoveableObject
                {
                    GameObj = cake,
                    GridPos = gridPos,
                    isGiftBox = false,
                    isDead = false,
                });
            }
            // Spawn gift
            var giftObject = Instantiate(GiftPrefab, gameWorld);
            giftObject.transform.position = gridManager.GetWorldPos(levelData.GiftPos.x, levelData.GiftPos.y);
            giftObject.transform.localScale = new Vector3(gridManager.GridSize, gridManager.GridSize);
            moveableObjects.Add(new MoveableObject
            {
                GameObj = giftObject,
                GridPos = levelData.GiftPos,
                isGiftBox = true,
                isDead = false,
            });
        }

        public void ResetLevel()
        {
            gridManager.Awake();
            TimeLeft = DEFAULT_LEVEL_TIME;
            // Destroy old object if any
            moveableObjects.Clear();
            for (int i = 0; i < gameWorld.childCount; i++)
            {
                var child = gameWorld.GetChild(i);
                if (child != null && child.gameObject != null)
                    Destroy(child.gameObject);
            }

            InitializeLevel();
        }

        public void MoveObject(SwipeDirection dir)
        {
            if (!isObjMoving && GameInfo.Instance.curLevel?.levelData != null)
            {
                isObjMoving = true;
                StartCoroutine(MoveObjectCoroutine(dir));
            }
        }

        IEnumerator MoveObjectCoroutine(SwipeDirection dir)
        {
            var obstacleList = GameInfo.Instance.curLevel.levelData.ObstaclesPos;
            // Sort objects for calculate later
            switch (dir)
            {
                case SwipeDirection.Left:
                    moveableObjects.Sort(new MoveLeftSorter());
                    break;
                case SwipeDirection.Right:
                    moveableObjects.Sort(new MoveRightSorter());
                    break;
                case SwipeDirection.Up:
                    moveableObjects.Sort(new MoveUpSorter());
                    break;
                case SwipeDirection.Down:
                    moveableObjects.Sort(new MoveDownSorter());
                    break;
            }

            // Save old grid pos
            Dictionary<MoveableObject, Vector2Int> oldGridPos = new Dictionary<MoveableObject, Vector2Int>(moveableObjects.Count);
            foreach (var obj in moveableObjects)
                oldGridPos.Add(obj, obj.GridPos);
            // Calculate new grid position for each moveable object
            CalculateNewGridPos(dir, obstacleList);
            // Move coroutine
            float timeLeft = 0;
            while (timeLeft <= moveDuration)
            {
                // Update timer
                timeLeft += Time.deltaTime;
                yield return null;
                for (int i = 0; i < moveableObjects.Count; i++)
                {
                    var obj = moveableObjects[i];
                    // Old pos equal new pos
                    if (!oldGridPos.ContainsKey(obj) || oldGridPos[obj] == obj.GridPos)
                        continue;
                    var targetWorldPos = gridManager.GetWorldPos(obj.GridPos.x, obj.GridPos.y);
                    var oldWorldPos = gridManager.GetWorldPos(oldGridPos[obj].x, oldGridPos[obj].y);
                    obj.GameObj.transform.position = Vector3.Lerp(oldWorldPos, targetWorldPos, timeLeft / moveDuration);
                }
            }
            // Remove all ded object
            for (int i = moveableObjects.Count - 1; i >= 0 ; i--)
            {
                MoveableObject obj = moveableObjects[i];
                if (obj.isDead)
                {
                    Destroy(obj.GameObj);
                    moveableObjects.RemoveAt(i);
                }
            }
            isObjMoving = false;
            yield break;
        }

        private void CalculateNewGridPos(SwipeDirection dir, Vector2Int[] obstacleList)
        {
            int giftIndex = moveableObjects.FindIndex(obj => obj.isGiftBox);
            Vector2Int step = Vector2Int.zero;
            int maxGridPos = 0;
            switch (dir)
            {
                case SwipeDirection.Left:
                    step = Vector2Int.left;
                    maxGridPos = 0;
                    break;
                case SwipeDirection.Right:
                    step = Vector2Int.right;
                    maxGridPos = gridManager.GridCount.x - 1;
                    break;
                case SwipeDirection.Up:
                    step = Vector2Int.down;
                    maxGridPos = 0;
                    break;
                case SwipeDirection.Down:
                    step = Vector2Int.up;
                    maxGridPos = gridManager.GridCount.y - 1;
                    break;
            }

            for (int i = moveableObjects.Count - 1; i >= 0; i--)
            {
                var obj = moveableObjects[i];
                var newGridPos = obj.GridPos;
                // Keep moving 1 step until cannot move anymore
                while (true)
                {
                    newGridPos += step;
                    // Check if object can have this new grid position
                    // Check obstacle
                    if (obstacleList.Any(obs => obs == newGridPos))
                    {
                        // There's already an obstacle here
                        newGridPos -= step;
                        break;
                    }
                    // Check moved objects
                    if (i < moveableObjects.Count - 1)
                    {
                        var nextObj = moveableObjects.Skip(i + 1).FirstOrDefault(nextObj => nextObj.GridPos == newGridPos);
                        if (obj.isGiftBox && nextObj != null)
                        {
                            // Eat the cake and remove them
                            nextObj.isDead = true;
                            break;
                        }
                        else if (!obj.isGiftBox && nextObj != null)
                        {
                            // There's already a gift there
                            if (nextObj.isGiftBox)
                            {
                                obj.isDead = true;
                            }
                            // There's already a cake here
                            else
                            {
                                newGridPos -= step;
                            }
                            break;
                        }
                    }
                    // Check if object is already at board's side
                    if (dir == SwipeDirection.Left && newGridPos.x <= maxGridPos)
                    {
                        newGridPos.x = Mathf.Max(newGridPos.x, maxGridPos);
                        break;
                    }
                    else if (dir == SwipeDirection.Right && newGridPos.x >= maxGridPos)
                    {
                        newGridPos.x = Mathf.Min(newGridPos.x, maxGridPos);
                        break;
                    }
                    else if (dir == SwipeDirection.Up && newGridPos.y <= maxGridPos)
                    {
                        newGridPos.y = Mathf.Max(newGridPos.y, maxGridPos);
                        break;
                    }
                    else if (dir == SwipeDirection.Down && newGridPos.y >= maxGridPos)
                    {
                        newGridPos.y = Mathf.Min(newGridPos.y, maxGridPos);
                        break;
                    }
                }
                obj.GridPos = newGridPos;
            }
        }

        private void Update()
        {
            if (TimeLeft <= 0 || stopped)
                return;

            TimeLeft -= Mathf.Min(TimeLeft, Time.deltaTime);
        }

        public bool IsLevelEnded(out LevelResult result)
        {
            stopped = true;
            // Either ran out of time or no cake left
            int cakeLeft = moveableObjects.Count(obj => !obj.isGiftBox);
            result = LevelResult.FAILED;
            if (TimeLeft <= 0 && cakeLeft != 0)
                return true;
            else if (cakeLeft == 0)
            {
                if (TimeLeft >= STAR_3_TIME)
                    result = LevelResult.STAR_3;
                else if (TimeLeft >= STAR_2_TIME)
                    result = LevelResult.STAR_2;
                else if (TimeLeft >= STAR_1_TIME)
                    result = LevelResult.STAR_1;
                else
                    result = LevelResult.STAR_0;
                return true;
            }
            stopped = false;
            return false;
        }
    }
}