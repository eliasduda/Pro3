using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

//verwaltet das movement des players auf einem isometrischen grid (benutzt das pathfinding)
public class MyPlayer : MonoBehaviour
{
    private List<Vector3> path; //liste an paths (von kästchen zu kästchen = ein path)
    private int pathIndex;
    public int moveSpeed;
    public bool haspath = false; //gibt an ob der player gerade wohin moved

    //sucht einen Pfad von der aktuellen position zu target
    public void setTarget(Vector3 target)
    {
        pathIndex = 0;
        path = Pathfinding.Instance.FindPathV3(IsoMatrix.InvIso(this.GetPosition()), IsoMatrix.InvIso(target));

        if(path != null && path.Count > 1)
        {
            path.RemoveAt(0); //entfernt den ersten path da dieser seine eigene position ist
            haspath = true;
            Debug.Log("Path set to length: " + path.Count);
        }
        else
        {
            Debug.Log("Player has no valid path");
        }

        
    }

    //moved den player indem alle paths abgearbeitet werden
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
        } else
        {
            haspath = false;
        }
    }

    //stoppt den player (der pfad wird gelöscht)
    private void StopMoving()
    {
        path = null;
    }
    private Vector3 GetPosition()
    {
        return transform.position;
    }

    
}
