using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Video;
using YoutubePlayer;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject _closeVideo;

    private List<GameObject> _spawns;
    //private List<GameObject> _stations;
    //private List<GameObject> _stationItems;
    private WalkingPath[] _paths;
    private MaterialPropertyBlock _materialPropertyBlock;
    private int[] _hueValues = new int[] { 0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };
    private List<int> _hueValuesList = new List<int>();
    private GameObject _backdrop;
    public GameObject _crosshair;
    private GameObject _canvasLoadingVideo;
    private GameObject _canvasPlayingVideo;
    private RenderTexture _loadingVideoTexture;
    private RenderTexture _youtubeVideoTexture;
    private int _coroutineCount = 0;


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
    public List<Station> _stationBlueprints = new List<Station>();
    public List<GameObject> _stations = new List<GameObject>();
    public List<GameObject> _stationItems = new List<GameObject>();
    public List<GameObject> _minigames = new List<GameObject>();
    public GameObject _lastStationSpawned;
    public GameObject _interactText;
    public string _inputStationWinnerNumber;
    public float _wallCheckRadius = 15f;
    public bool _isCutscenePlaying;
    public int _choosePathsColorCycles = 6;

    [HideInInspector] public Vector3 _lastCheckpointPosition;
    [HideInInspector] public bool _isVideoPlaying;
    [HideInInspector] public GameObject _playingVideo;
    [HideInInspector] public PathDirection _chosenDirection;

    static System.Random rnd = new System.Random();

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

        //GameObject.Find("Start Station Pillar").transform.DOScale(new Vector3(0, 0, 0), 0f);

        InitializeGame();
    }

    private void InitializeGame() {
        //Player.Instance.transform.Find("Visibility Sphere").gameObject.SetActive(true);

        _paths = FindObjectsOfType<WalkingPath>();
        //foreach (var path in _paths)
        //{
        //    path.transform.localScale = new Vector3(0, 0, 0);
        //}

        _hueValuesList = _hueValues.ToList();

        RenderSettings.fog = true;
    }

    private void CloseVideo() {
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

    private void PressToPlay_OnMainVideoStartLoading(GameObject videoGO) {
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

    private void OnMainVideoEnded(VideoPlayer source) {
        CloseVideo();
    }

    private void Player_OnPlayerDeath(Player player) {
        player.transform.position = _lastCheckpointPosition;
    }

    private void Checkpoint_OnCheckpointHit(Checkpoint checkpoint) {
        _lastCheckpointPosition = checkpoint.gameObject.transform.position;
        Destroy(checkpoint.gameObject);
    }

    private void OnDestroy() {
        // Remove registered events
        Checkpoint.OnCheckpointHit -= Checkpoint_OnCheckpointHit;
        Player.OnPlayerDeath -= Player_OnPlayerDeath;
        PressToPlay.OnMainVideoStartLoading -= PressToPlay_OnMainVideoStartLoading;
        global::CloseVideo.OnCloseVideo -= CloseVideo;

        _loadingVideoTexture.Release();
        _youtubeVideoTexture.Release();
    }

    public void GrowPaths() {
        foreach (var path in _paths) {
            path.transform.DOScale(path._scale, 5f);
        }
    }

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.Q)) {

        //}

        if (_isVideoPlaying) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                CloseVideo();
            }
        }
    }

    public void OpenNearbyWall() {
        int wallsLayerMask = 1 << LayerMask.NameToLayer("Walls");
        Collider[] hitColliders = Physics.OverlapSphere(Player.Instance.transform.position, _wallCheckRadius, wallsLayerMask);

        foreach (var hitCollider in hitColliders) {
            Wall wall = hitCollider.GetComponent<Wall>();
            if (wall != null) wall.Down();
        }
    }

    public void EndCutscene() {
        _isCutscenePlaying = false;
        _crosshair.GetComponent<Image>().DOFade(1f, 0.3f);
    }

    public void StartCutscene() {
        _isCutscenePlaying = true;
        _crosshair.GetComponent<Image>().DOFade(0f, 0f);
    }

    public void PlayTimeline(string name) {
        PlayableDirector timeline = GameObject.Find(name).GetComponent<PlayableDirector>();
        timeline?.Play();
    }

    public void StartPathsColorCoroutine() {
        StartCoroutine("SetPathsColor");
    }

    IEnumerator SetPathsColor() {
        while (true) {
            if (_coroutineCount >= _choosePathsColorCycles) {
                _coroutineCount = 0;
                yield break;
            }

            foreach (var path in _paths) {
                MaterialPropertyBlock _materialPropertyBlock = new MaterialPropertyBlock();

                //Color color = Color.HSVToRGB(30f * (index + 1) / 360, 1, 1, true);
                _materialPropertyBlock.SetColor("_EmissionColor", UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1) * 2);

                var led1 = path.transform.GetChild(0);
                var led2 = path.transform.GetChild(1);

                led1.gameObject.SetActive(true);
                led2.gameObject.SetActive(true);

                if (_coroutineCount == 0) {
                    led1.transform.localScale = new Vector3(0f, led1.transform.localScale.y, led1.transform.localScale.z);
                    led2.transform.localScale = new Vector3(0f, led1.transform.localScale.y, led1.transform.localScale.z);

                    led1.transform.DOScaleX(2f, 3f);
                    led2.transform.DOScaleX(2f, 3f);
                }

                led1.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
                led2.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
            }

            _coroutineCount++;

            yield return new WaitForSeconds(0.4f);
        }
    }

    public void SetFog(float amount) {
        DOTween.To(
            () => RenderSettings.fogEndDistance,
            x => RenderSettings.fogEndDistance = x,
            amount,
            2f
        );
    }
}
