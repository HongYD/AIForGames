using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendVector3
{
    public static Vector2 ToVector2(this UnityEngine.Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
}
                      