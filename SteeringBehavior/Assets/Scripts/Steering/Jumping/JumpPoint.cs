using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct JumpPoint
{
    public Vector3 jumpLocation;
    public Vector3 landingLocation;
    private Vector3 deltaPosition;
    public Vector3 DeltaPosition
    {
        get { return landingLocation - jumpLocation; }
    }
}
