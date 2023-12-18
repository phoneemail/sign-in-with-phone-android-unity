﻿namespace Gpm.Ui
{
    using UnityEngine;

    using MoveToType = InfiniteScroll.MoveToType;

    public interface IMoveScroll
    {
        Vector2 GetScrollPosition();

        void SetScrollPosition(Vector2 position);

        Vector2 GetMovePosition(int dataIndex, MoveToType moveToType);

        Vector2 GetMovePosition(float scrollRate);
    }

    [DisallowMultipleComponent]
    public class ScrollMoveTo : MonoBehaviour
    {
        public enum ScrollType
        {
            INDEX,
            RATE
        }

        private IMoveScroll scroll;

        private void OnEnable()
        {
            if (scroll == null)
            {
                scroll = GetComponent<IMoveScroll>();
            }
        }

        public ScrollType scrollType;
        
        public int dataIndex;
        public MoveToType moveToType;

        public float scrollRate = 0;

        public float time = 0.3f;

        public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public bool autoDestory = false;

        private float rate = 0;
        
        private Vector2 start;
        
        public void Set(int dataIndex, MoveToType moveToType, float time)
        {
            this.scrollType = ScrollType.INDEX;
            this.dataIndex = dataIndex;
            this.moveToType = moveToType;

            this.time = time;
        }

        public void Set(float scrollRate, float time)
        {
            this.scrollType = ScrollType.RATE;
            this.scrollRate = scrollRate;

            this.time = time;
        }

        public void Play()
        {
            rate = 0;
            enabled = true;
        }

        public void Update()
        {
            if (scroll == null)
            {
                return;
            }

            if (rate == 0)
            {
                start = scroll.GetScrollPosition();
            }

            if (time > 0)
            {
                rate += Time.deltaTime / time;
            }
            else
            {
                rate = 1;
            }

            Vector2 end;
            if (scrollType == ScrollType.INDEX)
            {
                end = scroll.GetMovePosition(dataIndex, moveToType);
            }
            else if (scrollType == ScrollType.RATE)
            {
                end = scroll.GetMovePosition(scrollRate);
            }
            else
            {
                end = start;
                rate = 1;

            }

            if (rate < 1)
            {
                scroll.SetScrollPosition(Vector2.Lerp(start, end, curve.Evaluate(rate)));                
            }
            else
            {
                scroll.SetScrollPosition(end);
                rate = 0;

                if (autoDestory == true)
                {
                    Destroy(this);
                }
                enabled = false;
            }
        }
    }
}