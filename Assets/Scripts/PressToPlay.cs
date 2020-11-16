using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using YoutubePlayer;

namespace YoutubePlayer {

    [RequireComponent(typeof(YoutubePlayer))]
    public class PressToPlay : MonoBehaviour {
        private GameObject _ui;

        private bool _isPlayerLooking = false;

        public GameObject _mainYoutubePlayer;

        public static event Action<GameObject> OnMainVideoStartLoading;

        private void Awake() {
            CloseVideo.OnCloseVideo += CloseVideo_OnCloseVideo;

            _ui = GameManager.Instance._interactText;
        }

        private void OnRaycastEnter(GameObject sender) {
            if (sender.transform.tag == "Player") _isPlayerLooking = true;
        }

        private void OnRaycastExit(GameObject sender) {
            if (sender.transform.tag == "Player") _isPlayerLooking = false;
        }

        private void Update() {
            if (_isPlayerLooking && !GameManager.Instance._isVideoPlaying)
            {
                //_ui.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                    PlayVideo();
            } else
            {
                //_ui.SetActive(false);
            }
        }
        
        private void PlayVideo() {
            _mainYoutubePlayer.GetComponent<VideoPlayer>().clip = GetComponent<VideoPlayer>().clip;
            _mainYoutubePlayer.GetComponent<VideoPlayer>().enabled = true;
            OnMainVideoStartLoading?.Invoke(_mainYoutubePlayer);
        }

        private void CloseVideo_OnCloseVideo() {
            _mainYoutubePlayer.GetComponent<VideoPlayer>().enabled = false;
        }
    }
}