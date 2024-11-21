using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectNested : ScrollRect
{
    //đối tượng ScrollRectNested chính
    private ScrollRectNested m_Parent;

    public enum Direction
    {
        Horizontal,
        Vertical
    }
    //hướng vuốt
    private Direction m_Direction = Direction.Horizontal;
    //hướng hoạt động hiện tại
    private Direction m_BeginDragDirection = Direction.Horizontal;

    protected override void Awake()
    {
        base.Awake();
        //tìm đối tượng chính
        Transform parent = transform.parent;
        if (parent)
        {
            m_Parent = parent.GetComponentInParent<ScrollRectNested>();
        }
        m_Direction = this.horizontal ? Direction.Horizontal : Direction.Vertical;
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (m_Parent)
        {
            m_BeginDragDirection = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) ? Direction.Horizontal : Direction.Vertical;
            if (m_BeginDragDirection != m_Direction)
            {
                //Hướng hoạt động hiện tại không bằng hướng trượt và sự kiện được chuyển cho đối tượng mẹ
                ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
                return;
            }
        }

        base.OnBeginDrag(eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (m_Parent)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //Hướng hoạt động hiện tại không bằng hướng trượt và sự kiện được chuyển cho đối tượng mẹ
                ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.dragHandler);
                return;
            }
        }
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (m_Parent)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //Hướng hoạt động hiện tại không bằng hướng trượt và sự kiện được chuyển cho đối tượng mẹ
                ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.endDragHandler);
                return;
            }
        }
        base.OnEndDrag(eventData);
    }

    public override void OnScroll(PointerEventData data)
    {
        if (m_Parent)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //Hướng hoạt động hiện tại không bằng hướng trượt và sự kiện được chuyển cho đối tượng mẹ
                ExecuteEvents.Execute(m_Parent.gameObject, data, ExecuteEvents.scrollHandler);
                return;
            }
        }
        base.OnScroll(data);
    }
}