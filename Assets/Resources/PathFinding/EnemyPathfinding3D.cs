using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/*Extraido del tutorial sobre Pathfinding de https://www.youtube.com/watch?v=4T7KHysRw84 */

[RequireComponent (typeof (Rigidbody))]
[RequireComponent(typeof(Seeker))]
public class EnemyPathfinding3D : MonoBehaviour {
    //What to chase?
    public Transform target;
    //How many times each second we will update our path
    public float updateRate = 2f;
    //Caching
    private Seeker seeker;
    private Rigidbody rb;

    //The calculated path
    public Path path;

    //The AI's speed per second
    public float speed = 300f;
    public ForceMode fMode;

    [HideInInspector]
    public bool pathIsEnded = false;
    //Max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
    //The Waypoint we are currently moving towards
    private int currentWaypoint = 0;



	// Use this for initialization
	void Start () {
        Debug.Log("Starting pathfinding!");
        target = GameObject.Find("Arwin").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>();
        
        if(target == null)
        {
            Debug.LogError("No target found!");
            return;
        }
        //Start a new path to the target position, return the result to the OnPathComplete method
        seeker.StartPath(transform.position, target.position,OnPathComplete);

        StartCoroutine(UpdatePath());
	}
	
    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            //TODO:Insert a player search here.
            Debug.Log("EnemyPathfinding: Player not found!");
            yield return false;
        }
        //Start a new path to the target position, return the result to the OnPathComplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path. Did it have an error?" + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            //TODO: Insert a player search here.
            return;
        }
        //TODO: always look at player? Tal como está no cambia de dirección

        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count) //Final waypoint reached
        {
            if (pathIsEnded)
                return;
            Debug.Log("End of path reached.");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //Move the AI in the direction of the waypoint
        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    public void setTarget(string target)
    {
        this.target = GameObject.Find(target).transform;
    }
}
