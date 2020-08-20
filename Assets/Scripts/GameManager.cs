using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using YoutubePlayer;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject _closeVideo;

    private List<GameObject> _spawns;
    private List<GameObject> _stations;
    private List<GameObject> _stationItems;

    private GameObject[] _paths;
    private MaterialPropertyBlock _materialPropertyBlock;

    private int[] _hueValues = new int[] { 0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };
    private List<int> _hueValuesList = new List<int>();

    public static Transform RecursiveFindChild(Transform parent, string childName) {
        Transform child = null;
        for (int i = 0; i < parent.childCount; i++) {
            child = parent.GetChild(i);
            if (child.name == childName) {
                break;
            } else {
                child = RecursiveFindChild(child, childName);
                if (child != null) {
                    break;
                }
            }
        }

        return child;
    }

    [HideInInspector] public Checkpoint _lastCheckpoint;

    private GameObject _backdrop;
    private GameObject _canvasLoadingVideo;
    private GameObject _canvasPlayingVideo;

    private RenderTexture _loadingVideoTexture;
    private RenderTexture _youtubeVideoTexture;

    [HideInInspector] public bool _isVideoPlaying;
    [HideInInspector] public bool _isInsideScreenRange;
    [HideInInspector] public GameObject playingVideo;

    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    #endregion

    private void Start() {
        // Register events
        Checkpoint.OnCheckpointHit += Checkpoint_OnCheckpointHit;
        Player.OnPlayerDeath += Player_OnPlayerDeath;
        PressToPlay.OnMainVideoStartLoading += PressToPlay_OnMainVideoStartLoading;
        CloseVideo.OnCloseVideo += CloseVideo_OnCloseVideo;

        _spawns = GameObject.FindGameObjectsWithTag("StationSpawner").ToList();
        _stations = GameObject.FindGameObjectsWithTag("Station").ToList();
        _stationItems = GameObject.FindGameObjectsWithTag("StationItem").ToList();

        _backdrop = GameObject.Find("Backdrop");
        _canvasLoadingVideo = GameObject.Find("Loading Video");
        _canvasPlayingVideo = GameObject.Find("Playing Video");

        _loadingVideoTexture = Resources.Load<RenderTexture>("RenderTextures/Loading Video");
        _youtubeVideoTexture = Resources.Load<RenderTexture>("RenderTextures/Youtube Video");

        _paths = GameObject.FindGameObjectsWithTag("Path");

        _hueValuesList = _hueValues.ToList();

        SpawnStations();
    }

    private void CloseVideo_OnCloseVideo() {
        _isVideoPlaying = false;
        _youtubeVideoTexture.Release();

        playingVideo.GetComponent<VideoPlayer>().enabled = false;
        _canvasPlayingVideo.GetComponent<RawImage>().DOFade(0f, 0.3f);

        _canvasLoadingVideo.GetComponent<VideoPlayer>().enabled = false;
        _canvasLoadingVideo.GetComponent<RawImage>().DOFade(0f, 0.3f)
            .OnComplete(() => _canvasLoadingVideo.GetComponent<RawImage>().enabled = false);

        _backdrop.GetComponent<Image>().DOFade(0f, 0.3f);

        VideoPlayerProgress.Instance.GetComponent<Image>().DOFade(0.0f, 0.3f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void PressToPlay_OnMainVideoStartLoading(GameObject videoGO) {
        _isVideoPlaying = true;
        playingVideo = videoGO;
        VideoPlayerProgress.Instance.videoPlayer = playingVideo.GetComponent<VideoPlayer>();

        #region Show UI
        _canvasPlayingVideo.GetComponent<RawImage>().DOFade(1f, 0.3f);

        _canvasLoadingVideo.GetComponent<VideoPlayer>().enabled = true;
        _canvasLoadingVideo.GetComponent<RawImage>().enabled = true;
        _canvasLoadingVideo.GetComponent<RawImage>().DOFade(1f, 0.3f);

        _backdrop.GetComponent<Image>().DOFade(0.7f, 0.3f);
        _closeVideo.GetComponent<Text>().DOFade(1f, 0.3f);

        VideoPlayerProgress.Instance.GetComponent<Image>().DOFade(1.0f, 0.3f);
        VideoPlayerProgress.Instance.GetComponent<Image>().fillAmount = 0.0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        #endregion
    }

    private void Player_OnPlayerDeath(Player player) {
        player.transform.position = _lastCheckpoint.transform.position;
    }

    private void Checkpoint_OnCheckpointHit(Checkpoint checkpoint) {
        _lastCheckpoint = checkpoint;
        Debug.Log(_lastCheckpoint);
    }

    private void OnDestroy() {
        // Remove registered events
        Checkpoint.OnCheckpointHit -= Checkpoint_OnCheckpointHit;
        Player.OnPlayerDeath -= Player_OnPlayerDeath;
        PressToPlay.OnMainVideoStartLoading -= PressToPlay_OnMainVideoStartLoading;
        CloseVideo.OnCloseVideo -= CloseVideo_OnCloseVideo;

        _loadingVideoTexture.Release();
        _youtubeVideoTexture.Release();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            int index = 0;
            foreach (GameObject path in _paths) {
                path.GetComponent<Animator>().Play("path_scale");
                index++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            foreach (GameObject path in _paths) {
                MaterialPropertyBlock _materialPropertyBlock = new MaterialPropertyBlock();

                //Color color = Color.HSVToRGB(30f * (index + 1) / 360, 1, 1, true);
                _materialPropertyBlock.SetColor("_EmissionColor", Random.ColorHSV(0, 1, 1, 1, 1, 1) * 3);

                path.transform.GetChild(0).GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
                path.transform.GetChild(1).GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
            }
        }
    }

    private void SpawnStations() {
        for (int i = 0; i < _spawns.Count; i++) {
            for (int j = _stations.Count - 1; j >= 0; j--) {
                MaterialPropertyBlock _materialPropertyBlock = new MaterialPropertyBlock();
                _materialPropertyBlock.SetColor("_FresnelColor", Random.ColorHSV(0, 1, 1, 1, 1, 1) * 25);

                FillStation(_stations[j]);
                Transform _stationMesh = RecursiveFindChild(_stations[j].transform, "mesh");
                _stationMesh.gameObject.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);

                _stations[j].transform.position = _spawns[i].transform.position;
                _stations.RemoveAt(j);
                break;
            }
        }
    }

    private void FillStation(GameObject station) {
        foreach (Transform _child in station.transform) {
            if (_child.CompareTag("StationItemSpawner")) {
                MaterialPropertyBlock _materialPropertyBlock = new MaterialPropertyBlock();
                _materialPropertyBlock.SetColor("_BaseColor", Random.ColorHSV(0, 1, 1, 1));

                int _randomIdx = Random.Range(0, _stationItems.Count);

                GameObject _itemInstance = Instantiate(_stationItems[_randomIdx], _child.parent);
                Transform _instanceChild = _itemInstance.transform.GetChild(0);
                _instanceChild.gameObject.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
                _itemInstance.transform.position = _child.transform.position;

                //itemInstance.transform.SetParent(child.parent);
                //stationItems.RemoveAt(randomIdx);
            }
        }
    }
}
