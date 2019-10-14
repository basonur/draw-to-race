using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarPhysics : MonoBehaviour
{
    public GameObject body;
    public WheelCollider frontWheelLeft,frontWhellRight,backWheelLeft,backWheelRight;
    public MeshFilter meshFilter;
    public BoxCollider boxCollider;
    public Stage currentStage;

    public void UpdateCar(List<Transform> points,float carHeight,Mesh mesh)
    {
        body.transform.position = body.transform.position += new Vector3(0,1f,0);
        Destroy(boxCollider);
        meshFilter.mesh = mesh;

        transform.rotation = new Quaternion(0,transform.rotation.y,0,transform.rotation.w);

        while (colliders.Count > 0)
        {
            Destroy(colliders[colliders.Count-1]);
            colliders.RemoveAt(colliders.Count-1);
        }

        colliders.Clear();

        for (int i = 0; i < points.Count; i++)
        {
            AddCollider(points[i].localPosition);
        }

        frontWheelLeft.transform.localPosition = new Vector3(0, points[points.Count-1].position.y, points[points.Count-1].position.x) - new Vector3(-.1f,.1f,0);
        frontWhellRight.transform.localPosition = new Vector3(0, points[points.Count-1].position.y, points[points.Count-1].position.x) - new Vector3(.1f,.1f,0);

        backWheelLeft.transform.localPosition = new Vector3(0, points[0].position.y, points[0].position.x) - new Vector3(-.1f, .1f, 0);
        backWheelRight.transform.localPosition = new Vector3(0, points[0].position.y, points[0].position.x) - new Vector3(.1f, .1f, 0);
    }

    private List<SphereCollider> colliders = new List<SphereCollider>();

    private void AddCollider(Vector3 pos)
    {
        SphereCollider sphereCollider = this.gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = .1f;
        sphereCollider.center = new Vector3(0,pos.y,pos.x);

        colliders.Add(sphereCollider);
    }

    public void StartCar()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        transform.DOMove(currentStage.startPoint.transform.position,2f).onComplete += 
        ()=> { transform.DORotate(currentStage.rotation, 2f, RotateMode.Fast).onComplete += 
            SetTorque;};
    }

    private void SetTorque()
    {
        if (this.GetComponent<Rigidbody>().isKinematic == true)
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
        }

        /*switch (currentStage.positiveAxis)
        {
            case PositiveAxis.X:
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                break;
            case PositiveAxis.Z:
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
                break;
            default:
                break;
        }*/

        frontWheelLeft.motorTorque = 50f * currentStage.direction;
        frontWhellRight.motorTorque = 50f * currentStage.direction;
        backWheelLeft.motorTorque = 50f * currentStage.direction;
        backWheelRight.motorTorque = 50f * currentStage.direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<StepTrigger>())
        {
            other.GetComponent<StepTrigger>().Explode();
        }
    }

    public void StopCar()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
    }
}
