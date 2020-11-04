using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    private List<Vector3> path;
    private int pathIndex;
    public int moveSpeed;

    public void setTarget(Vector3 target)
    {
        pathIndex = 0;
        path = Pathfinding.Instance.FindPathV3(IsoMatrix.InvIso(this.GetPosition()), IsoMatrix.InvIso(target));

        if(path != null && path.Count > 1)
        {
            path.RemoveAt(0);
        }

        Debug.Log("Path set to length: " + path.Count);
    }

    public void handleMovement()
    {
        if(path != null)
        {
            Vector3 targetPosition = path[pathIndex];
            if(Vector3.Distance(transform.position, targetPosition) > 1)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;
                transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;
            } else
            {
                pathIndex++;
                if ( pathIndex >= path.Count)
                {
                    this.StopMoving();
                    Debug.Log("stopped Moving");
                }
            }
        } 
    }

    private void StopMoving()
    {
        path = null;
    }
    private Vector3 GetPosition()
    {
        return transform.position;
    }

    
}
