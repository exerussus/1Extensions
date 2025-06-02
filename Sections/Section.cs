using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Exerussus._1Extensions.Sections
{
    [Serializable]
    public abstract class Section
    {
        protected abstract Vector2 LeftBottomProtected { get ; set; }
        protected abstract Vector2 RightTopProtected { get ; set; }
        
        public abstract Vector2 CenterPosition { get ; protected set; }
        public abstract Vector2 BottomCenter { get ; protected set; }
        public abstract Vector2 TopCenter { get ; protected set; }
        public abstract Vector2 LeftCenter { get ; protected set; }
        public abstract Vector2 RightCenter { get ; protected set; }
        
        public Vector2 LeftBottom
        {
            get => LeftBottomProtected;
            set
            {
                LeftBottomProtected = value;
                Recalculate();
            }
        }

        public Vector2 RightTop
        {
            get => RightTopProtected;
            set
            {
                RightTopProtected = value;
                if (RightTopProtected.x < LeftBottomProtected.x)
                {
                    var newValue = RightTopProtected;
                    newValue.x = LeftBottomProtected.x;
                    RightTopProtected = newValue;
                }
                if (RightTopProtected.y < LeftBottomProtected.y)
                {
                    var newValue = RightTopProtected;
                    newValue.x = LeftBottomProtected.y;
                    RightTopProtected = newValue;
                }
                Recalculate();
            }
        }

        public virtual void SetPoints(Vector2 leftBottomPoint, Vector2 rightTopPint)
        {
            LeftBottomProtected = leftBottomPoint;
            RightTopProtected = rightTopPint;
            Recalculate();
        }
        
        public virtual void Recalculate()
        {
            CenterPosition = LeftBottomProtected + (RightTopProtected - LeftBottomProtected) / 2f;
            BottomCenter = new Vector2(CenterPosition.x, LeftBottomProtected.y);
            TopCenter = new Vector2(CenterPosition.x, RightTopProtected.y);
            LeftCenter = new Vector2(LeftBottomProtected.x, CenterPosition.y);
            RightCenter = new Vector2(RightTopProtected.x, CenterPosition.y);
        }
    }
}