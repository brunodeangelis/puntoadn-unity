using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<GameObject> _spawns;
    private List<GameObject> _stations;
    private List<GameObject> _stationItems;

    public static Transform RecursiveFindChild(Transform parent, string childName)
    {
        Transform child = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            child = parent.GetChild(i);
            if (child.name == childName)
            {
                break;
            }
            else
            {
                child = RecursiveFindChild(child, childName);
                if (child != null)
                {
                    break;
                }
            }
        }

        return child;
    }

    void Start()
    {
        _spawns = GameObject.FindGameObjectsWithTag("StationSpawner").ToList();
        _stations = GameObject.FindGameObjectsWithTag("Station").ToList();
        _stationItems = GameObject.FindGameObjectsWithTag("StationItem").ToList();

        SpawnStations();
    }

    void SpawnStations()
    {
        for (int i = 0; i < _spawns.Count; i++)
        {
            for (int j = _stations.Count - 1; j >= 0; j--)
            {
                Color randomColor = Random.ColorHSV(0, 1, 1, 1, 1, 1);
                FillStation(_stations[j]);
                Transform stationMesh = RecursiveFindChild(_stations[j].transform, "mesh");
                stationMesh.gameObject.GetComponent<Renderer>().material.SetColor("_FresnelColor", randomColor * 25);
                _stations[j].transform.position = _spawns[i].transform.position;
                _stations.RemoveAt(j);
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
                int randomIdx = Random.Range(0, _stationItems.Count);
                GameObject itemInstance = Instantiate(_stationItems[randomIdx], child.parent);
                Transform instanceChild = itemInstance.transform.GetChild(0);
                instanceChild.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", Random.ColorHSV(0, 1, 1, 1));
                itemInstance.transform.position = child.transform.position;
                //itemInstance.transform.SetParent(child.parent);
                //stationItems.RemoveAt(randomIdx);
            }
        }
    }
}
