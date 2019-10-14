using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CarGenerator : MonoBehaviour
{
    public TouchField touchField;
    public GameObject mesh;
    public CarPhysics carPhysics;
    public Transform drawingContent;
    public Tubular.Demo pipeMeshGenerator;

    private void Awake()
    {
        touchField.onStartDrawing += StartDraw;
        touchField.onDrawNewPoint += AddNewPoint;
        touchField.onFinishDrawing += FinishDraw;
    }

    public void StartDraw()
    {
        points.Clear();

        for (int i = 0; i < drawingContent.childCount; i++)
        {
            Destroy(drawingContent.GetChild(i).gameObject);
        }
    }

    public void AddNewPoint(Vector2 point)
    {
        GameObject pObject = new GameObject();
        pObject.transform.parent = drawingContent;
        pObject.transform.localPosition = point;
        points.Add(pObject.transform);

        if (points.Count >= 2)
        {
            SetMesh();
        }
    }

    public void FinishDraw()
    {
        mesh.GetComponent<MeshFilter>().mesh = null;
        SpawnCar();
        for (int i = 0; i < drawingContent.transform.childCount; i++)
        {
            Destroy(drawingContent.transform.GetChild(i).gameObject);
        }
    }

    public void SpawnCar()
    {
        carPhysics.UpdateCar(
            points,
            carHeight,
            generatedMesh);
    }

    public List<Transform> points = new List<Transform>();

    private Mesh generatedMesh;

    private float carHeight
    {
        get
        {
            float minY= 2f, maxY=-2f;
            for (int i = 0; i < points.Count; i++)
            {
                if (minY >= points[i].transform.localPosition.y)
                {
                    minY = points[i].transform.localPosition.y;
                }
                if (maxY <= points[i].transform.localPosition.y)
                {
                    maxY = points[i].transform.localPosition.y;
                }
            }
            return Mathf.Abs(minY - maxY);
        }
    }

    public void SetMesh()
    {
        generatedMesh =  pipeMeshGenerator.GenerateMesh(points);

        mesh.GetComponent<MeshFilter>().mesh = generatedMesh;
    }
}
