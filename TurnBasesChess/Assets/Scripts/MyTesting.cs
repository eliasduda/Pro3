﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using Unity.Mathematics;

//Tests the player and pathfinding per mouseclick
public class MyTesting : MonoBehaviour
{
    [SerializeField] private MyPlayer player;
    [SerializeField] private Pathfinding pathfinding;
    


    private void Start()
    {
        pathfinding = new Pathfinding(5, 5);
        pathfinding.setObstacles(); //look for obstacles

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseP = UtilsClass.GetMouseWorldPosition();
            if (!player.haspath) //gibt dem player inen pfad wenn dieser gerade keinen hat
            {
                player.setTarget(mouseP);
            }

            //DEBUG  zeichnet player pfad
            List<Vector3> path = pathfinding.FindPathV3(IsoMatrix.InvIso(player.transform.position), IsoMatrix.InvIso(mouseP));

            if (path != null)
            {
                for (int i = 0; i<path.Count -1; i++)
                {
                    Debug.DrawLine(path[i],  path[i+1], Color.red, 500f);

                }
            }
            
        }
    }
}