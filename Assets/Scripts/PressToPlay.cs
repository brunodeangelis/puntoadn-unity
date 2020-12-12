using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class PressToPlay : MonoBehaviour {
    private GameObject _ui;
    private bool _isPlayerLooking = false;

    public GameObject _mainPlayer;
    public static event Action<GameObject> OnMainVideoStartLoading;
    [HideInInspector] public bool _alreadySeen = false;

    private void Awake() {
        CloseVideo.OnCloseVideo += CloseVideo_OnCloseVideo;

        _ui = GameManager._i._interactText;
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
        _mainPlayer.GetComponent<VideoPlayer>().clip = GetComponent<VideoPlayer>().clip;
        _mainPlayer.GetComponent<VideoPlayer>().enabled = true;

        if (!_alreadySeen) GameManager._i._videosPlayed++;
        _alreadySeen = true;

        OnMainVideoStartLoading?.Invoke(_mainPlayer);
    }

    private void CloseVideo_OnCloseVideo() {
        _mainPlayer.GetComponent<VideoPlayer>().enabled = false;
    }
}