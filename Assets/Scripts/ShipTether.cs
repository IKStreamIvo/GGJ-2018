using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTether : MonoBehaviour {

    public float distanceThreshold = 2f;

    public Ship ship1;
    public Ship ship2;

    public LineRenderer line;

	void Start ()
    {
        line = GetComponent<LineRenderer>();
    }
	
	void Update ()
    {
        //Check distance
        Vector2 pos1 = ship1.transform.position;
        Vector2 pos2 = ship2.transform.position;

        if(Vector2.Distance(pos1, pos2) <= distanceThreshold)
        {
            //Update line positions
            line.SetPosition(0, pos1);
            line.SetPosition(1, pos2);
            line.startWidth = 1f - (Vector2.Distance(pos1, pos2) / distanceThreshold);
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }

    }
}
