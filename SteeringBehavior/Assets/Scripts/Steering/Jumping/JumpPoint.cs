using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPoint: MonoBehaviour
{
    public Transform jumpLocation;
    public Transform landingLocation;
    public Vector3 deltaPosition;
    private void Start()
    {
        jumpLocation = this.transform.GetChild(0).Find("JumpingPad").transform;
        landingLocation = this.transform.GetChild(1).Find("LandingPad").transform;
        deltaPosition = landingLocation.position - jumpLocation.position;
    }
}
