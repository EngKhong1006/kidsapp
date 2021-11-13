using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowThePath : MonoBehaviour
{
    [SerializeField] public Transform[] waypoints;
    [SerializeField] private float moveSpeed = 1f;

    public int waypointIndex = 0;
    public bool moveAllowed = false;
    public int coin = 0;

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
        transform.position = Vector2.MoveTowards(
            transform.position,
            waypoints[waypointIndex % waypoints.Length].transform.position,
            moveSpeed * Time.deltaTime
        );

        if(transform.position == waypoints[waypointIndex % waypoints.Length].transform.position)
        {
            waypointIndex += 1;
        }
    }
}
