using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicWandering : KinematicBase
{
    [SerializeField]
    float _changeDirTime;
    float changeDirTime;

    [Range(5, 10)]
    public int rd;

    [Range(10, 20)]
    public int forwardVec;

    Vector3 direction;
    Vector3 center;

    [SerializeField]
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        direction = Random.insideUnitSphere.normalized;
        changeDirTime = _changeDirTime;
    }

    // Update is called once per frame
    void Update()
    {
        changeDirTime -= Time.deltaTime;
        center = transform.position + (transform.up * forwardVec);

        if (changeDirTime <= 0)
        {
            direction = Random.insideUnitSphere.normalized;
            changeDirTime = _changeDirTime;
        }
        Vector3 newPosOfTarget = center + (direction * rd);
        target.position = new Vector3(newPosOfTarget.x, 0, newPosOfTarget.z);

        GetKinematicOutput(target);
        ApplyMovement();
    }
}
