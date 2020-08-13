using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YoutubePlayer;

namespace YoutubePlayer { 
    public class PressToPlay : MonoBehaviour
    {
        private GameObject _ui;
        private GameObject _player;

        [SerializeField]
        private string _videoUrl;

        private bool _isVideoPlaying;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _ui = GameObject.Find("Key Press Text");
        }

        private void Update()
        {
            float distance = Vector3.Distance(_player.transform.position, transform.position);

            if (distance < 8)
            {
                if (!_isVideoPlaying) _ui.SetActive(true);
                else _ui.SetActive(false);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    _isVideoPlaying = true;
                    Debug.Log("hi!");

                    GetComponent<YoutubePlayer>().enabled = true;
                }
            } else
            {
                _ui.SetActive(false);
            }
        }
    }
}