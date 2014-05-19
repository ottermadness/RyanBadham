using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

public class Path
{
    public List<Vector3> Waypoints = new List<Vector3>();
    public int next = 0;
    public bool draw;

    public bool Looped;

    public void Draw()
    {
        if (draw)
        {
            for (int i = 1; i < Waypoints.Count; i++)
            {
                Debug.DrawLine(Waypoints[i - 1], Waypoints[i], Color.cyan);
            }
            if (Looped && (Waypoints.Count > 0))
            {
                Debug.DrawLine(Waypoints[0], Waypoints[Waypoints.Count - 1], Color.cyan);
            }
        }
    }

    public Vector3 NextWaypoint()
    {
        return Waypoints[next];
    }

    public bool IsLast()
    {
        return (next == Waypoints.Count - 1);
    }

    public void AdvanceToNext()
    {
        if (Looped)
        {
            next = (next + 1) % Waypoints.Count;
        }
        else
        {
            if (next != Waypoints.Count - 1)
            {
                next = next + 1;
            }
        }
    }
}

