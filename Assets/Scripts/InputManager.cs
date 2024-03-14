using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PackagingCakeBoxes
{
    public class InputManager : MonoBehaviour
    {
        private Vector2 touchStartPos;
        [SerializeField]
        private float minSwipeDistance = 50f;
        [SerializeField]
        private LevelManager levelMng;

        private void Update()
        {
            if (levelMng == null || levelMng.IsLevelEnded(out _))
            {
                return;
            }

            if (Input.touchCount > 0)
            {
                // Get the last touch
                Touch touch = Input.GetTouch(Input.touchCount - 1);

                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    Vector2 swipeDist = touch.position - touchStartPos;

                    if (swipeDist.magnitude >= minSwipeDistance)
                    {
                        var swipeDir = SwipeDirection.Left;
                        // Check for left/right swipe
                        if (Mathf.Abs(swipeDist.x) > Mathf.Abs(swipeDist.y))
                        {
                            if (swipeDist.x > 0)
                            {
                                swipeDir = SwipeDirection.Right;
                            }
                            else
                            {
                                swipeDir = SwipeDirection.Left;
                            }
                        }
                        // Check for up/down swipe
                        else
                        {
                            if (swipeDist.y > 0)
                            {
                                swipeDir = SwipeDirection.Up;
                            }
                            else
                            {
                                swipeDir = SwipeDirection.Down;
                            }
                        }
                        levelMng.MoveObject(swipeDir);
                    }
                }
            }

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                levelMng.MoveObject(SwipeDirection.Down);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                levelMng.MoveObject(SwipeDirection.Up);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                levelMng.MoveObject(SwipeDirection.Left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                levelMng.MoveObject(SwipeDirection.Right);
            }
#endif
        }
    }

    public enum SwipeDirection
    {
        Left, Right, Up, Down
    }
}