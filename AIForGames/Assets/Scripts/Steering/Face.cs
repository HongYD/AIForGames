using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        GetSteeringOutput(base.target);
        ApplyMovement();
    }

    protected override void GetSteeringOutput(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        if(direction.magnitude <= 0)
        {
            return;
        }
        else
        {
            Align instance = new Align();
            base.target.rotation = Quaternion.Euler( 0,Mathf.Atan2(-direction.z, direction.z),0);
            base.GetSteeringOutput(target);
        }
    }
}
