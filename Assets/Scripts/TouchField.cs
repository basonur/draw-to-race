using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchVector;
    [HideInInspector]
    public Vector2 PointerOld;
    [HideInInspector]
    protected int PointerId;
    [HideInInspector]
    public bool Pressed;

    public Vector2 drawContentCenter;
    public Vector2 drawContentSize;

    public float touchDragHandlingDistance = 1f;
    public List<Vector2> points = new List<Vector2>();

    public Action onStartDrawing;
    public Action onFinishDrawing;
    public Action<Vector2> onDrawNewPoint;

    private void Start()
    {
        drawContentCenter = GetComponent<RectTransform>().position;
        drawContentSize = GetComponent<RectTransform>().sizeDelta;
    }

    void Update()
    {
        if (Pressed)
        {
            TouchVector = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;

            if (Vector3.Distance(new Vector2(Input.mousePosition.x, Input.mousePosition.y), PointerOld) >= touchDragHandlingDistance)
            {
                if (normalizePoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)) != Vector2.zero)
                {
                    onDrawNewPoint?.Invoke(normalizePoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
                }
            }

            PointerOld = Input.mousePosition;
        }
        else
        {
            TouchVector = new Vector2();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        points.Clear();
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
        onStartDrawing?.Invoke();

        if (normalizePoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)) != Vector2.zero)
        {
            onDrawNewPoint?.Invoke(normalizePoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
        }
        else
        {
            onDrawNewPoint?.Invoke(normalizePoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
            points.Clear();
            Pressed = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onFinishDrawing?.Invoke();
        Pressed = false;
    }
    
    private Vector2 normalizePoint(Vector2 v)
    {
        Vector2 newVector = new Vector2((v.x - drawContentCenter.x) / (drawContentSize.x /2), (v.y - drawContentCenter.y) / (drawContentSize.y / 2));

        if (Mathf.Abs( newVector.x )>= 1f || Mathf.Abs(newVector.y) >= 1f)
        {
            return Vector2.zero;
        }
        return newVector;
    }
}