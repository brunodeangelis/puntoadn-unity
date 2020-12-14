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
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject _closeVideo;
    [SerializeField] private GameObject _lastStationPlayerSpawn;
    [SerializeField] private Task _taskPrefab;
    [SerializeField] private List<Color> _colorsForPaths = new List<Color>();

    private GameObject[] _paths;
    public GameObject _crosshair;
    public TextMeshProUGUI _actionLabel;
    //private GameObject _canvasLoadingVideo;
    private GameObject _canvasPlayingVideo;
    //private RenderTexture _loadingVideoTexture;
    private RenderTexture _youtubeVideoTexture;
    private int _coroutineCount = 0;
    private bool _takeScreenshotOnNextFrame;

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
    public int _choosePathsColorCycles = 6;
    public SoundAudioClip[] _soundAudioClipArray;
    public List<Sprite> _hanoiSprites = new List<Sprite>();

    [HideInInspector] public Color _chosenPathColor;
    [HideInInspector] public Vector3 _lastCheckpointPosition;
    [HideInInspector] public bool _isVideoPlaying;
    [HideInInspector] public bool _isVideoPaused;
    [HideInInspector] public GameObject _playingVideo;
    [HideInInspector] public PathDirection _chosenDirection;
    [HideInInspector] public bool _isCutscenePlaying;
    [HideInInspector] public int _currentStation;
    [HideInInspector] public int _videosPlayed = 0;

    static System.Random rnd = new System.Random();

    #region Singleton
    private static GameManager _instance;
    public static GameManager _i { get { return _instance; } }
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

        _crosshair = GameObject.Find("Crosshair");
        //_canvasLoadingVideo = GameObject.Find("Loading Video");
        _canvasPlayingVideo = GameObject.Find("Playing Video");

        //_loadingVideoTexture = Resources.Load<RenderTexture>("RenderTextures/Loading Video");
        _youtubeVideoTexture = Resources.Load<RenderTexture>("RenderTextures/Youtube Video");

        InitializeGame();
    }

    private void InitializeGame() {
        //Player.Instance.transform.Find("Visibility Sphere").gameObject.SetActive(true);

        _paths = GameObject.FindGameObjectsWithTag("Path");
        //foreach (var path in _paths)
        //{
        //    path.transform.localScale = new Vector3(0, 0, 0);
        //}

        RenderSettings.fog = true;
    }

    private void CloseVideo() {
        _isVideoPlaying = false;
        //_youtubeVideoTexture.DiscardContents();

        _playingVideo.GetComponent<VideoPlayer>().enabled = false;
        _canvasPlayingVideo.GetComponent<RawImage>().DOFade(0f, 0.3f);

        //_canvasLoadingVideo.GetComponent<VideoPlayer>().enabled = false;
        //_canvasLoadingVideo.GetComponent<RawImage>().DOFade(0f, 0.3f)
        //    .OnComplete(() => _canvasLoadingVideo.GetComponent<RawImage>().enabled = false);

        _crosshair.SetActive(true);

        //VideoPlayerProgress.Instance.GetComponent<Image>().DOFade(0.0f, 0.3f);
        VideoPlayerProgress.Instance.transform.parent.GetComponent<CanvasGroup>().DOFade(0.0f, 0.3f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _playingVideo = null;
    }

    private void PauseVideo() {
        _isVideoPaused = true;
        _playingVideo.GetComponent<VideoPlayer>().Pause();
    }

    private void ResumeVideo() {
        _isVideoPaused = false;
        _playingVideo.GetComponent<VideoPlayer>().Play();
    }

    private void PressToPlay_OnMainVideoStartLoading(GameObject videoGO) {
        _isVideoPlaying = true;
        //_youtubeVideoTexture.Create();
        _playingVideo = videoGO;
        _playingVideo.GetComponent<VideoPlayer>().enabled = true;
        _playingVideo.GetComponent<VideoPlayer>().loopPointReached += OnMainVideoEnded;

        VideoPlayerProgress.Instance.videoPlayer = _playingVideo.GetComponent<VideoPlayer>();

        #region UI

        Debug.Log($"Videos played: {_videosPlayed}");
        if (_videosPlayed == 1) {
            GameObject.Find("How to/First Station/Text 2").GetComponent<TextMeshProUGUI>().DOFade(0f, 0.2f);
            PauseVideo();
            PlayTimeline("First Video Timeline");
        } else if (_videosPlayed == 3) {
            CreateTask("Si ya terminaste, continuá por el camino que no está iluminado");
        }

        _canvasPlayingVideo.GetComponent<RawImage>().DOFade(1f, 0.3f);

        //_canvasLoadingVideo.GetComponent<VideoPlayer>().enabled = true;
        //_canvasLoadingVideo.GetComponent<RawImage>().enabled = true;
        //_canvasLoadingVideo.GetComponent<RawImage>().DOFade(1f, 0.3f);

        _crosshair.SetActive(false);

        _actionLabel.DOFade(0f, 0.2f);

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
        if (_currentStation >= 5) {
            player.transform.position = _lastStationPlayerSpawn.transform.position;
        } else {
            player.transform.position = _lastCheckpointPosition;
        }
    }

    private void Checkpoint_OnCheckpointHit(Checkpoint checkpoint) {
        Debug.Log("Checkpoint hit.");
        _lastCheckpointPosition = checkpoint.gameObject.transform.position;
        Destroy(checkpoint.gameObject);
    }

    private void OnDestroy() {
        // Remove registered events
        Checkpoint.OnCheckpointHit -= Checkpoint_OnCheckpointHit;
        Player.OnPlayerDeath -= Player_OnPlayerDeath;
        PressToPlay.OnMainVideoStartLoading -= PressToPlay_OnMainVideoStartLoading;
        global::CloseVideo.OnCloseVideo -= CloseVideo;

        //_loadingVideoTexture.Release();
        _youtubeVideoTexture.Release();
    }

    public void GrowPaths() {
        foreach (var path in _paths) {
            path.transform.DOScale(path.GetComponent<WalkingPath>()._scale, 5f);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            TakeScreenshot();
        }

        if (_isVideoPlaying) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                CloseVideo();
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                if (_isVideoPaused)
                    ResumeVideo();
                else
                    PauseVideo();
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
        _actionLabel.DOFade(0f, 0f);
    }

    public void PlayTimeline(string name) {
        PlayableDirector timeline = GameObject.Find(name).GetComponent<PlayableDirector>();
        timeline?.Play();
    }

    public void StartPathsColorCoroutine() {
        StartCoroutine("SetPathsColor");
    }

    IEnumerator SetPathsColor() {
        float emissionIntensity = 2.3f;

        while (true) {
            if (_coroutineCount >= _choosePathsColorCycles) {
                _coroutineCount = 0;
                yield break;
            }

            List<Color> clonedColors = new List<Color>(_colorsForPaths);

            foreach (var path in _paths) {
                MaterialPropertyBlock _materialPropertyBlock = new MaterialPropertyBlock();

                //Color color = Color.HSVToRGB(30f * (index + 1) / 360, 1, 1, true);
                //_materialPropertyBlock.SetColor("_EmissionColor", UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1) * 2);
                int randomColorIndex = UnityEngine.Random.Range(0, clonedColors.Count);
                _materialPropertyBlock.SetColor("_EmissionColor", clonedColors[randomColorIndex] * emissionIntensity);
                clonedColors.RemoveAt(randomColorIndex);

                var led1 = path.transform.GetChild(0);
                var led2 = path.transform.GetChild(1);

                led1.gameObject.SetActive(true);
                led2.gameObject.SetActive(true);

                if (_coroutineCount == 0) {
                    led1.transform.localScale = new Vector3(0f, led1.transform.localScale.y, led1.transform.localScale.z);
                    led2.transform.localScale = new Vector3(0f, led1.transform.localScale.y, led1.transform.localScale.z);

                    led1.transform.DOScaleX(1f, 5f);
                    led2.transform.DOScaleX(1f, 5f);
                }

                led1.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
                led2.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
            }

            _coroutineCount++;

            yield return new WaitForSeconds(0.3f);
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

    public void ActivateDisableAtStartObjects() {
        var objs = FindObjectsOfTypeAll<DisableAtStart>();
        Debug.Log($"Found {objs.Count} objects");
        int i = 0;
        foreach (var obj in objs) {
            i++;
            obj.gameObject.SetActive(true);
        }
        Debug.Log($"Ran loop {i} times.");
    }

    public void DeactivateDisableAtStartObjects() {
        var objs = FindObjectsOfTypeAll<DisableAtStart>();
        foreach (var obj in objs) {
            obj.gameObject.SetActive(false);
        }
    }

    public void CreateTask(string name) {
        var tasks = GameObject.Find("[UI]/Tasks");
        var newTask = Instantiate(_taskPrefab, tasks.transform);

        newTask.name = name;
        newTask._text = $"• {name}.";

        if (tasks.transform.childCount > 0) {
            Vector3 newTaskPos = newTask.transform.localPosition;
            newTask.transform.localPosition = new Vector3(newTaskPos.x, newTaskPos.y - 40 * tasks.transform.childCount, newTaskPos.z);
        }

        newTask.Enter();
    }

    public void DeleteTask(string name) {
        var go = GameObject.Find($"[UI]/Tasks/{name}");
        if (go == null) return;

        var tasks = GameObject.Find("[UI]/Tasks");
        var taskText = go.GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < tasks.transform.childCount; i++) {
            Transform childTask = tasks.transform.GetChild(i);
            if (tasks.transform.childCount > 1) {
                if (i > 0) {
                    childTask.DOLocalMoveY(childTask.localPosition.y + 40, 0.3f);
                }
            }
        }

        taskText.DOFade(0f, 0.2f).OnComplete(() => Destroy(go));
    }

    public void EndFirstVideoTimeline() {
        GameObject.Find("How to/First Video").GetComponent<CanvasGroup>().DOFade(0, 0.2f);
        ResumeVideo();
    }

    [Serializable]
    public class SoundAudioClip {
        public SoundManager.Sound _sound;
        public AudioClip _audioClip;
    }

    public void TakeScreenshot() {
        ScreenCapture.CaptureScreenshot("../CaminoRecorrido.png");
        Debug.Log("Screenshot saved.");
    }

    //private void OnPostRender() {
    //    if (_takeScreenshotOnNextFrame) {
    //        _takeScreenshotOnNextFrame = false;

    //        RenderTexture renderTexture = Camera.main.targetTexture;
    //        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
    //        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
    //        renderResult.ReadPixels(rect, 0, 0);

    //        byte[] byteArray = renderResult.EncodeToPNG();
    //        Debug.Log(byteArray);
    //        System.IO.File.WriteAllBytes(Application.dataPath + "/CaminoRecorrido.png", byteArray);
    //        Debug.Log("Screenshot saved.");

    //        RenderTexture.ReleaseTemporary(renderTexture);
    //        Camera.main.targetTexture = null;
    //    }
    //}

    //public void TakeScreenshot(int width, int height) {
    //    Camera.main.targetTexture = RenderTexture.GetTemporary(width, height, 16);
    //    _takeScreenshotOnNextFrame = true;
    //}

    public static List<T> FindObjectsOfTypeAll<T>() {
        List<T> results = new List<T>();
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            var s = SceneManager.GetSceneAt(i);
            if (s.isLoaded) {
                var allGameObjects = s.GetRootGameObjects();
                for (int j = 0; j < allGameObjects.Length; j++) {
                    var go = allGameObjects[j];
                    results.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }
        }
        return results;
    }
}
