using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<GameObject> _spawns;
    private List<GameObject> _stations;
    private List<GameObject> _stationItems;

    private GameObject[] _paths;
    private MaterialPropertyBlock _materialPropertyBlock;

    private int[] _hueValues = new int[] { 0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };
    private List<int> _hueValuesList = new List<int>();

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

    [HideInInspector] public Checkpoint lastCheckpoint;
    [HideInInspector] public bool isVideoPlaying;
    [HideInInspector] public bool isInsideScreenRange;

    #region Singleton
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
    #endregion
    
    private void Start()
    {
        // Register events
        Checkpoint.OnCheckpointHit += Checkpoint_OnCheckpointHit;
        Player.OnPlayerDeath += Player_OnPlayerDeath;

        _spawns = GameObject.FindGameObjectsWithTag("StationSpawner").ToList();
        _stations = GameObject.FindGameObjectsWithTag("Station").ToList();
        _stationItems = GameObject.FindGameObjectsWithTag("StationItem").ToList();

        SpawnStations();

        _paths = GameObject.FindGameObjectsWithTag("Path");

        _hueValuesList = _hueValues.ToList();
    }

    private void Player_OnPlayerDeath(Player player)
    {
        player.transform.position = lastCheckpoint.transform.position;
    }

    private void Checkpoint_OnCheckpointHit(Checkpoint checkpoint)
    {
        lastCheckpoint = checkpoint;
        Debug.Log(lastCheckpoint);
    }

    private void OnDestroy()
    {
        // Remove registered events
        Checkpoint.OnCheckpointHit -= Checkpoint_OnCheckpointHit;
        Player.OnPlayerDeath -= Player_OnPlayerDeath;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            int index = 0;
            foreach (GameObject path in _paths)
            {
                path.GetComponent<Animator>().Play("path_scale");
                index++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (GameObject path in _paths)
            {
                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                
                //Color color = Color.HSVToRGB(30f * (index + 1) / 360, 1, 1, true);
                materialPropertyBlock.SetColor("_EmissionColor", Random.ColorHSV(0, 1, 1, 1, 1, 1) * 3);

                path.transform.GetChild(0).GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
                path.transform.GetChild(1).GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
            }
        }
    }

    private void SpawnStations()
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

    private void FillStation(GameObject station)
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
