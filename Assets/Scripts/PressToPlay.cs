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
        private Player _player;

        private GameObject _mainYoutubePlayer;

        public static event Action<GameObject> OnMainVideoStartLoading;

        private void Awake() {
            CloseVideo.OnCloseVideo += CloseVideo_OnCloseVideo;
        }

        private void Start() {
            _player = Player.Instance;
            _ui = GameObject.Find("Interact Text");
            _mainYoutubePlayer = transform.Find("Youtube Player").gameObject;

            Debug.Log(_mainYoutubePlayer.GetComponent<YoutubePlayer>().youtubeUrl);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.transform.tag == "Player") _player.isInsideScreenRange = true;
            Debug.Log(GetComponent<YoutubePlayer>().youtubeUrl);
        }

        private void OnTriggerExit(Collider other) {
            if (other.transform.tag == "Player") _player.isInsideScreenRange = false;
            Debug.Log(GetComponent<YoutubePlayer>().youtubeUrl);
        }

        private void Update() {
            if (_player.isInsideScreenRange && !GameManager.Instance._isVideoPlaying) {
                _ui.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                    PlayVideo();
            } else {
                _ui.SetActive(false);
            }
        }
        
        private void PlayVideo() {
            _mainYoutubePlayer.GetComponent<VideoPlayer>().enabled = true;

            Debug.Log(GetComponent<YoutubePlayer>().youtubeUrl);
            _mainYoutubePlayer.GetComponent<YoutubePlayer>().youtubeUrl = GetComponent<YoutubePlayer>().youtubeUrl;
            Debug.Log(_mainYoutubePlayer.GetComponent<YoutubePlayer>().youtubeUrl);
            _mainYoutubePlayer.GetComponent<YoutubePlayer>().enabled = true;

            OnMainVideoStartLoading?.Invoke(_mainYoutubePlayer);
        }

        private void CloseVideo_OnCloseVideo() {
            _mainYoutubePlayer.GetComponent<YoutubePlayer>().enabled = false;
        }
    }
}