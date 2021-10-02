using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowThePath : MonoBehaviour
{
    public Transform[] waypoints;

    [SerializeField]
    private float moveSpeed = 1f;

    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAllowed)
            Move();
    }

    private void Move()
    {
        //Debug.Log("round: " + round + " waypoint: " + waypointIndex);
        //waypointIndex = waypointIndex % waypoints.Length;

        /*if (waypointIndex <= waypoints.Length - 1)
        {*/
            transform.position = Vector2.MoveTowards(
                transform.position,
                waypoints[waypointIndex % waypoints.Length].transform.position,
                moveSpeed * Time.deltaTime
            );

            if(transform.position == waypoints[waypointIndex % waypoints.Length].transform.position)
            {
                waypointIndex += 1;
            }
        //}
    }
}
