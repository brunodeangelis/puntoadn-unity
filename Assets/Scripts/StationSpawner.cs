using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace YoutubePlayer
{
    public class StationSpawner : MonoBehaviour
    {
        public PathDirection _pathDirection;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (GameManager.Instance._lastStationSpawned != null)
                    Destroy(GameManager.Instance._lastStationSpawned);

                SpawnRandomStation();
            }
        }
        private void SpawnRandomStation()
        {
            List<GameObject> stations = GameManager.Instance._stations;
            List<Station> stationBlueprints = GameManager.Instance._stationBlueprints;

            int r = UnityEngine.Random.Range(0, stations.Count);
            int r2 = UnityEngine.Random.Range(0, stationBlueprints.Count);

            Station stationBlueprint = Instantiate(stationBlueprints[r2]);
            GameObject instancedStation = Instantiate(stations[r]);

            FillStation(instancedStation, stationBlueprint);

            // Ubico estación en la posición del "Station Position"
            instancedStation.transform.position = transform.GetChild(0).transform.position;
            instancedStation.transform.LookAt(Player.Instance.transform);

            stations.RemoveAt(r);
            stationBlueprints.RemoveAt(r2);

            GameManager.Instance._stations = stations;
            GameManager.Instance._stationBlueprints = stationBlueprints;
            GameManager.Instance._lastStationSpawned = instancedStation;

            Destroy(gameObject);
        }

        private void FillStation(GameObject station, Station stationBlueprint)
        {
            // Preparo para agarrar un stationItem aleatorio
            int r = UnityEngine.Random.Range(0, GameManager.Instance._stationItems.Count);

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();

            foreach (Transform child in station.transform)
            {
                if (child.CompareTag("StationItemSpawner") && stationBlueprint._stationData._projects.Count > 0)
                {
                    // Agarro proyecto random de la lista
                    int r2 = UnityEngine.Random.Range(0, stationBlueprint._stationData._projects.Count);

                    Project project = stationBlueprint._stationData._projects[r2];

                    Debug.Log(stationBlueprint._stationData._projects);
                    Debug.Log(r2);

                    GameObject stationItem = Instantiate(GameManager.Instance._stationItems[r], child.parent);
                    stationItem.transform.position = child.transform.position;

                    Transform screen = stationItem.transform.Find("Screen");

                    screen.GetComponent<VideoPlayer>().clip = project._videoClip;
                    screen.GetComponent<VideoPlayer>().enabled = true;

                    // Saco el proyecto de la lista, ya lo usé
                    stationBlueprint._stationData._projects.Remove(project);
                }
            }

            // Saco stationItem de la lista, ya lo instancié
            GameManager.Instance._stationItems.RemoveAt(r);
        }
    }
}

public enum PathDirection
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}
