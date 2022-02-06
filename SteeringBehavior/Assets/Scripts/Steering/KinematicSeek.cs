using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicSeek : KinematicBase
{
    [SerializeField]
    Transform target;
    // Update is called once per frame
    void Update()
    {
        GetKinematicOutput(target);
        ApplyMovement();
    }
}
