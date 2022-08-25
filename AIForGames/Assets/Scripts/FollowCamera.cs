using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position + offset;
    }
}
