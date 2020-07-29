using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<GameObject> spawns;
    private List<GameObject> stations;

    // Start is called before the first frame update
    void Start()
    {
        spawns = GameObject.FindGameObjectsWithTag("StationSpawner").ToList();
        stations = GameObject.FindGameObjectsWithTag("Station").ToList();
        SpawnStations();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnStations()
    {
        Debug.Log(stations.Count);

        for (int i = 0; i < spawns.Count; i++)
        {
            for (int j = stations.Count - 1; j >= 0; j--)
            {
                stations[j].transform.position = spawns[i].transform.position;
                stations.RemoveAt(j);
                break;
            }
        }

        Debug.Log(stations.Count);
    }
}
