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

        private bool _isPlayerInsideScreen = false;

        public GameObject _mainYoutubePlayer;

        public static event Action<GameObject> OnMainVideoStartLoading;

        private void Awake() {
            CloseVideo.OnCloseVideo += CloseVideo_OnCloseVideo;

            _ui = GameManager.Instance._interactText;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.transform.tag == "Player") _isPlayerInsideScreen = true;
        }

        private void OnTriggerExit(Collider other) {
            if (other.transform.tag == "Player") _isPlayerInsideScreen = false;
        }

        private void Update() {
            if (_isPlayerInsideScreen && !GameManager.Instance._isVideoPlaying)
            {
                _ui.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                    PlayVideo();
            } else
            {
                _ui.SetActive(false);
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