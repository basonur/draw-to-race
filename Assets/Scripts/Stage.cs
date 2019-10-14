using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject startPoint, finishPoint;
    public float camXOffset;
    public int direction;
    public Vector3 rotation;
    public PositiveAxis positiveAxis;
}
public enum PositiveAxis
{
    X,
    Z
}
