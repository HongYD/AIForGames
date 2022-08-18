using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPoint: MonoBehaviour
{
    public Vector3 jumpLocation;
    public Vector3 landingLocation;
    public Vector3 deltaPosition;
    private void Start()
    {
        jumpLocation = this.transform.GetChild(0).transform.position;
        landingLocation = this.transform.GetChild(1).transform.position;
        deltaPosition = landingLocation - jumpLocation;
    }
}
