using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class PressToPlay : MonoBehaviour {
    [SerializeField] private bool _fadeBackgroundMusic = true;
    [SerializeField] [Range(0f, 1f)] private float _volume = 1.0f;
    private AudioSource _musicFromVideo;

    private VideoPlayer _videoPlayer;
    private GameObject _ui;
    private bool _isPlayerLooking = false;

    public GameObject _mainPlayer;
    public static event Action<GameObject> OnMainVideoStartLoading;
    [HideInInspector] public bool _alreadySeen = false;

    private void Awake() {
        CloseVideo.OnCloseVideo += CloseVideo_OnCloseVideo;
        _ui = GameManager._i._interactText;
        _musicFromVideo = GameManager._i._musicFromVideo;
        _videoPlayer = _mainPlayer.GetComponent<VideoPlayer>();
    }

    private void OnRaycastEnter(GameObject sender) {
        if (sender.transform.tag == "Player") _isPlayerLooking = true;
    }

    private void OnRaycastExit(GameObject sender) {
        if (sender.transform.tag == "Player") _isPlayerLooking = false;
    }

    private void Update() {
        if (_isPlayerLooking && !GameManager._i._isVideoPlaying) {
            //_ui.SetActive(true);

            if (Input.GetMouseButtonDown(0))
                PlayVideo();
        } else {
            //_ui.SetActive(false);
        }
    }

    private void PlayVideo() {
        _videoPlayer.clip = GetComponent<VideoPlayer>().clip;
        _videoPlayer.SetTargetAudioSource(0, _musicFromVideo);
        _videoPlayer.enabled = true;
        _musicFromVideo.volume = _volume;

        if (!_alreadySeen) GameManager._i._videosPlayed++;
        _alreadySeen = true;

        if (_fadeBackgroundMusic) {
            GameManager._i._backgroundMusic.volume = 0;
        }

        OnMainVideoStartLoading?.Invoke(_mainPlayer);
    }

    private void CloseVideo_OnCloseVideo() {
        _videoPlayer.enabled = false;
    }
}