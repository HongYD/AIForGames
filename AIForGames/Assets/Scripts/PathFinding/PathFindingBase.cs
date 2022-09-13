using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathFindingBase
{
    public virtual IEnumerator FindPath(Vector3 startPosition, Vector3 endPosition, Grid grid)
    {
        yield return null;
    }
}
