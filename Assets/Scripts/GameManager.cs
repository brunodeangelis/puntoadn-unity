using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<GameObject> spawns;
    private List<GameObject> stations;
    private List<GameObject> stationItems;

    void Start()
    {
        spawns = GameObject.FindGameObjectsWithTag("StationSpawner").ToList();
        stations = GameObject.FindGameObjectsWithTag("Station").ToList();
        stationItems = GameObject.FindGameObjectsWithTag("StationItem").ToList();

        SpawnStations();
    }

    void SpawnStations()
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            for (int j = stations.Count - 1; j >= 0; j--)
            {
                FillStation(stations[j]);
                stations[j].transform.position = spawns[i].transform.position;
                stations.RemoveAt(j);
                break;
            }
        }
    }

    void FillStation(GameObject station)
    {
        foreach (Transform child in station.transform)
        {
            if (child.CompareTag("StationItemSpawner"))
            {
                int randomIdx = Random.Range(0, stationItems.Count);
                GameObject itemInstance = Instantiate(stationItems[randomIdx]);
                itemInstance.transform.position = child.transform.position;
                itemInstance.transform.SetParent(child.parent);
                //stationItems.RemoveAt(randomIdx);
            }
        }
    }
}
