using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerCam : MonoBehaviour
{
    public GameObject target;
    public Vector3 offSet;
    public TouchField touchField;

    private void LateUpdate()
    {
        offSet.z = Mathf.Clamp(offSet.z, -15f, 15f);
        offSet.z += touchField.TouchVector.normalized.x/5f;

        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position,target.transform.position+offSet,Time.deltaTime*40f);
            transform.LookAt(target.transform);
        }
    }
}
