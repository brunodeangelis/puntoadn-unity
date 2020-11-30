using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using YoutubePlayer;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _closeVideo;

    private List<GameObject> _spawns;
    //private List<GameObject> _stations;
    //private List<GameObject> _stationItems;
    private WalkingPath[] _paths;
    private MaterialPropertyBlock _materialPropertyBlock;
    private int[] _hueValues = new int[] { 0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };
    private List<int> _hueValuesList = new List<int>();
    private GameObject _backdrop;
    private GameObject _crosshair;
    private GameObject _canvasLoadingVideo;
    private GameObject _canvasPlayingVideo;
    private RenderTexture _loadingVideoTexture;
    private RenderTexture _youtubeVideoTexture;

    public static Transform RecursiveFindChild(Transform parent, string childName)
    {
        Transform child = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            child = parent.GetChild(i);
            if (child.name == childName)
            {
                break;
            } else
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
    public List<Station> _stationBlueprints = new List<Station>();
    public List<GameObject> _stations = new List<GameObject>();
    public List<GameObject> _stationItems = new List<GameObject>();
    public List<GameObject> _minigames = new List<GameObject>();
    public GameObject _lastStationSpawned;
    public GameObject _interactText;
    public string _inputStationWinnerNumber;
    public float _wallCheckRadius = 15f;
    public bool _isCutscenePlaying;

    [HideInInspector] public Vector3 _lastCheckpointPosition;
    [HideInInspector] public bool _isVideoPlaying;
    [HideInInspector] public GameObject _playingVideo;
    [HideInInspector] public PathDirection _chosenDirection;

    static System.Random rnd = new System.Random();

    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
    }
    #endregion

    private void Start()
    {
        Checkpoint.OnCheckpointHit += Checkpoint_OnCheckpointHit;
        Player.OnPlayerDeath += Player_OnPlayerDeath;
        PressToPlay.OnMainVideoStartLoading += PressToPlay_OnMainVideoStartLoading;
        global::CloseVideo.OnCloseVideo += CloseVideo;

        _spawns = GameObject.FindGameObjectsWithTag("StationSpawner").ToList();
        //_stations = GameObject.FindGameObjectsWithTag("Station").ToList();
        //_stationItems = GameObject.FindGameObjectsWithTag("StationItem").ToList();

        _backdrop = GameObject.Find("Backdrop");
        _crosshair = GameObject.Find("Crosshair");
        _canvasLoadingVideo = GameObject.Find("Loading Video");
        _canvasPlayingVideo = GameObject.Find("Playing Video");

        _loadingVideoTexture = Resources.Load<RenderTexture>("RenderTextures/Loading Video");
        _youtubeVideoTexture = Resources.Load<RenderTexture>("RenderTextures/Youtube Video");

        InitializeGame();
    }

    private void InitializeGame()
    {
        Player.Instance.transform.Find("Visibility Sphere").gameObject.SetActive(true);

        //_paths = FindObjectsOfType<WalkingPath>();
        //foreach (var path in _paths)
        //{
        //    path.transform.localScale = new Vector3(0, 0, 0);
        //}

        _hueValuesList = _hueValues.ToList();

        RenderSettings.fog = true;
    }

    private void CloseVideo()
    {
        _isVideoPlaying = false;
        //_youtubeVideoTexture.DiscardContents();

        _playingVideo.GetComponent<VideoPlayer>().enabled = false;
        _canvasPlayingVideo.GetComponent<RawImage>().DOFade(0f, 0.3f);

        _canvasLoadingVideo.GetComponent<VideoPlayer>().enabled = false;
        _canvasLoadingVideo.GetComponent<RawImage>().DOFade(0f, 0.3f)
            .OnComplete(() => _canvasLoadingVideo.GetComponent<RawImage>().enabled = false);

        //_backdrop.GetComponent<Image>().DOFade(0f, 0.3f);
        _crosshair.SetActive(true);

        //VideoPlayerProgress.Instance.GetComponent<Image>().DOFade(0.0f, 0.3f);
        VideoPlayerProgress.Instance.transform.parent.GetComponent<CanvasGroup>().DOFade(0.0f, 0.3f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _playingVideo = null;
    }

    private void PressToPlay_OnMainVideoStartLoading(GameObject videoGO)
    {
        _isVideoPlaying = true;
        //_youtubeVideoTexture.Create();
        _playingVideo = videoGO;
        _playingVideo.GetComponent<VideoPlayer>().enabled = true;
        _playingVideo.GetComponent<VideoPlayer>().loopPointReached += OnMainVideoEnded;

        VideoPlayerProgress.Instance.videoPlayer = _playingVideo.GetComponent<VideoPlayer>();

        #region Show UI
        _canvasPlayingVideo.GetComponent<RawImage>().DOFade(1f, 0.3f);

        _canvasLoadingVideo.GetComponent<VideoPlayer>().enabled = true;
        _canvasLoadingVideo.GetComponent<RawImage>().enabled = true;
        _canvasLoadingVideo.GetComponent<RawImage>().DOFade(1f, 0.3f);

        //_backdrop.GetComponent<Image>().DOFade(0.7f, 0.3f);
        _crosshair.SetActive(false);

        //_closeVideo.GetComponent<Text>().DOFade(1f, 0.3f);

        //VideoPlayerProgress.Instance.GetComponent<Image>().DOFade(1.0f, 0.3f);
        VideoPlayerProgress.Instance.transform.parent.GetComponent<CanvasGroup>().DOFade(1.0f, 0.3f);
        VideoPlayerProgress.Instance.GetComponent<Image>().fillAmount = 0.0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        #endregion
    }

    private void OnMainVideoEnded(VideoPlayer source)
    {
        CloseVideo();
    }

    private void Player_OnPlayerDeath(Player player)
    {
        player.transform.position = _lastCheckpointPosition;
    }

    private void Checkpoint_OnCheckpointHit(Checkpoint checkpoint)
    {
        _lastCheckpointPosition = checkpoint.gameObject.transform.position;
        Destroy(checkpoint.gameObject);
    }

    private void OnDestroy()
    {
        // Remove registered events
        Checkpoint.OnCheckpointHit -= Checkpoint_OnCheckpointHit;
        Player.OnPlayerDeath -= Player_OnPlayerDeath;
        PressToPlay.OnMainVideoStartLoading -= PressToPlay_OnMainVideoStartLoading;
        global::CloseVideo.OnCloseVideo -= CloseVideo;

        _loadingVideoTexture.Release();
        _youtubeVideoTexture.Release();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T)) {
        //    foreach (WalkingPath path in _paths) {
        //        path.transform.DOScale(path._scale, 1f);
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (WalkingPath path in _paths)
            {
                MaterialPropertyBlock _materialPropertyBlock = new MaterialPropertyBlock();

                //Color color = Color.HSVToRGB(30f * (index + 1) / 360, 1, 1, true);
                _materialPropertyBlock.SetColor("_EmissionColor", UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1) * 3);

                path.transform.GetChild(0).GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
                path.transform.GetChild(1).GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
            }
        }

        if (_isVideoPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseVideo();
            }
        }
    }

    public void OpenNearbyWall()
    {
        int wallsLayerMask = 1 << LayerMask.NameToLayer("Walls");
        Collider[] hitColliders = Physics.OverlapSphere(Player.Instance.transform.position, _wallCheckRadius, wallsLayerMask);

        foreach (var hitCollider in hitColliders)
        {
            Wall wall = hitCollider.GetComponent<Wall>();
            if (wall != null) wall.Down();
        }
    }

    public void EndCutscene()
    {
        _isCutscenePlaying = false;
    }
}
