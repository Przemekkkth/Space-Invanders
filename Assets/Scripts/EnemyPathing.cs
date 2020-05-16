using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig = null;
    List<Transform> waypoints;
    int waypointIndex = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    private void Move()
    {
        if (!waveConfig) return;
        if( waypointIndex < waypoints.Count)
        {
            if( transform.position == waypoints[waypointIndex].transform.position )
            {
                waypointIndex++;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, waveConfig.GetMoveSpeed() * Time.deltaTime);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
