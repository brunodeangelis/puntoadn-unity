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

        public GameObject _mainYoutubePlayer;

        public static event Action<GameObject> OnMainVideoStartLoading;

        private void Awake() {
            CloseVideo.OnCloseVideo += CloseVideo_OnCloseVideo;
        }

        private void Start() {
            _player = Player.Instance;
            _ui = GameObject.Find("Interact Text");
            Debug.Log(_mainYoutubePlayer.name);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.transform.tag == "Player") _player.isInsideScreenRange = true;
        }

        private void OnTriggerExit(Collider other) {
            if (other.transform.tag == "Player") _player.isInsideScreenRange = false;
        }

        private void Update() {
            if (_player.isInsideScreenRange && !GameManager.Instance._isVideoPlaying) {
                //_ui.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                    PlayVideo();
            } else {
                //_ui.SetActive(false);
            }
        }
        
        private void PlayVideo() {
            //_mainYoutubePlayer.GetComponent<YoutubePlayer>().youtubeUrl = GetComponent<YoutubePlayer>().youtubeUrl;
            //_mainYoutubePlayer.GetComponent<YoutubePlayer>().enabled = true;
            Debug.Log(GetComponent<VideoPlayer>().clip);
            _mainYoutubePlayer.GetComponent<VideoPlayer>().clip = gameObject.GetComponent<VideoPlayer>().clip;
            Debug.Log(_mainYoutubePlayer.GetComponent<VideoPlayer>().clip);
            OnMainVideoStartLoading?.Invoke(_mainYoutubePlayer);
        }

        private void CloseVideo_OnCloseVideo() {
            //_mainYoutubePlayer.GetComponent<YoutubePlayer>().enabled = false;
            _mainYoutubePlayer.GetComponent<VideoPlayer>().enabled = false;
        }
    }
}