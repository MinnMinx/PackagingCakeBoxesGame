using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PackagingCakeBoxes
{
    public class MoveableObject
    {
        public GameObject GameObj;
        public Vector2Int GridPos;
        public bool isGiftBox;
        public bool isDead;
    }

    public class MoveUpSorter : IComparer<MoveableObject>
    {
        public int Compare(MoveableObject x, MoveableObject y)
        {
            // 6 4 2
            // 5 3 1
            if (x.GridPos.x != y.GridPos.x)
                return y.GridPos.x.CompareTo(x.GridPos.x);
            else return y.GridPos.y.CompareTo(x.GridPos.y);
        }
    }

    public class MoveDownSorter : IComparer<MoveableObject>
    {
        public int Compare(MoveableObject x, MoveableObject y)
        {
            // 5 3 1
            // 6 4 2
            if (x.GridPos.x != y.GridPos.x)
                return y.GridPos.x.CompareTo(x.GridPos.x);
            else return x.GridPos.y.CompareTo(y.GridPos.y);
        }
    }

    public class MoveLeftSorter : IComparer<MoveableObject>
    {
        public int Compare(MoveableObject x, MoveableObject y)
        {
            // 6 5 4
            // 3 2 1
            if (x.GridPos.y != y.GridPos.y)
                return y.GridPos.y.CompareTo(x.GridPos.y);
            else return y.GridPos.x.CompareTo(x.GridPos.x);
        }
    }

    public class MoveRightSorter : IComparer<MoveableObject>
    {
        public int Compare(MoveableObject x, MoveableObject y)
        {
            // 4 5 6
            // 1 2 3
            if (x.GridPos.y != y.GridPos.y)
                return y.GridPos.y.CompareTo(x.GridPos.y);
            else return x.GridPos.x.CompareTo(y.GridPos.x);
        }
    }
}
