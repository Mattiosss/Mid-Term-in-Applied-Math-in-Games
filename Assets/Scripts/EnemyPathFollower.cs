using UnityEngine;

public class EnemyPathFollower : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform[] waypoints;
    
    public float speed = 2f;

    private int currentWaypointIndex = 0;

    void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            GameObject waypointParent = GameObject.Find("WayPoint");
            if (waypointParent != null)
            {
                int count = waypointParent.transform.childCount;
                waypoints = new Transform[count];
                for (int i = 0; i < count; i++)
                {
                    waypoints[i] = waypointParent.transform.GetChild(i);
                }
            }
            else
            {
                Debug.LogError("No WayPoint parent found in the scene!");
            }
        }

        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (currentWaypointIndex >= waypoints.Length) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            waypoints[currentWaypointIndex].position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex++;
        }

        if (currentWaypointIndex >= waypoints.Length)
        {
            Destroy(gameObject);
        }
    }
}
