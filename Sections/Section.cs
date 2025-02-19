using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Exerussus._1Extensions.Sections
{
    [Serializable]
    public class Section
    {
        public Section() { }

        public Section(Vector2 leftBottom, Vector2 rightTop)
        {
            this.leftBottom = leftBottom;
            this.rightTop = rightTop;
        }

        [FormerlySerializedAs("minPosition")] [SerializeField, ReadOnly] private Vector2 leftBottom;
        [FormerlySerializedAs("maxPosition")] [SerializeField, ReadOnly] private Vector2 rightTop;
        [SerializeField, ReadOnly] private Vector2 centerPosition;
        
        public Vector2 BottomCenter { get; private set; }
        public Vector2 TopCenter { get; private set; }
        public Vector2 LeftCenter { get; private set; }
        public Vector2 RightCenter { get; private set; }
        
        public Vector2 LeftBottom
        {
            get => leftBottom;
            set
            {
                leftBottom = value;
                Recalculate();
            }
        }

        public Vector2 RightTop
        {
            get => rightTop;
            set
            {
                rightTop = value;
                if (rightTop.x < leftBottom.x) rightTop.x = leftBottom.x;
                if (rightTop.y < leftBottom.y) rightTop.y = leftBottom.y;
                Recalculate();
            }
        }

        public Vector2 CenterPosition => centerPosition;
        
#if UNITY_EDITOR
        [Button]
#endif
        public void SetPoints(Vector2 leftBottomPoint, Vector2 rightTopPint)
        {
            leftBottom = leftBottomPoint;
            rightTop = rightTopPint;
            Recalculate();
        }
        
        private void Recalculate()
        {
            centerPosition = leftBottom + (rightTop - leftBottom) / 2f;
            BottomCenter = new Vector2(centerPosition.x, leftBottom.y);
            TopCenter = new Vector2(centerPosition.x, rightTop.y);
            LeftCenter = new Vector2(leftBottom.x, centerPosition.y);
            RightCenter = new Vector2(rightTop.x, centerPosition.y);
        }
    }
}