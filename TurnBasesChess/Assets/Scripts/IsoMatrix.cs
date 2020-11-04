using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IsoMatrix
{

    public static Vector3[] isomatrix = { new Vector3(1, 0.5f, 0), new Vector3(-1, 0.5f, 0), new Vector3(0, 0, 0) };
    public static Vector3[] invisomatrix = { new Vector3(0.5f, -0.5f, 0), new Vector3(1, 1, 0), new Vector3(0, 0, 0) };

    public static Vector3 Iso(Vector3 vec)
    {
        float x = vec.x * isomatrix[0].x + vec.y * isomatrix[1].x + vec.z * isomatrix[2].x;
        float y = vec.x * isomatrix[0].y + vec.y * isomatrix[1].y + vec.z * isomatrix[2].y;
        float z = vec.x * isomatrix[0].z + vec.y * isomatrix[1].z + vec.z * isomatrix[2].z;
        Vector3 newVec = new Vector3(x, y, z);
        return newVec;
    }

    public static Vector3 InvIso(Vector3 vec)
    {
        float x = vec.x * invisomatrix[0].x + vec.y * invisomatrix[1].x + vec.z * invisomatrix[2].x;
        float y = vec.x * invisomatrix[0].y + vec.y * invisomatrix[1].y + vec.z * invisomatrix[2].y;
        float z = vec.x * invisomatrix[0].z + vec.y * invisomatrix[1].z + vec.z * invisomatrix[2].z;
        Vector3 newVec = new Vector3(x, y, z);
        return newVec;
    }
}
