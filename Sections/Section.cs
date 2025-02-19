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
        
        [SerializeField, ReadOnly] private Vector2 bottomCenter;
        [SerializeField, ReadOnly] private Vector2 topCenter;
        [SerializeField, ReadOnly] private Vector2 leftCenter;
        [SerializeField, ReadOnly] private Vector2 rightCenter;
        
        public Vector2 CenterPosition => centerPosition;
        public Vector2 BottomCenter => bottomCenter;
        public Vector2 TopCenter => topCenter;
        public Vector2 LeftCenter => leftCenter;
        public Vector2 RightCenter => rightCenter;
        
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
        
#if UNITY_EDITOR
        [Button]
#endif
        public void SetPoints(Vector2 leftBottomPoint, Vector2 rightTopPint)
        {
            leftBottom = leftBottomPoint;
            rightTop = rightTopPint;
            Recalculate();
        }
             
#if UNITY_EDITOR
        [Button]
#endif
        private void Recalculate()
        {
            centerPosition = leftBottom + (rightTop - leftBottom) / 2f;
            bottomCenter = new Vector2(centerPosition.x, leftBottom.y);
            topCenter = new Vector2(centerPosition.x, rightTop.y);
            leftCenter = new Vector2(leftBottom.x, centerPosition.y);
            rightCenter = new Vector2(rightTop.x, centerPosition.y);
        }
    }
}