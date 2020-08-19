using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using YoutubePlayer;

namespace YoutubePlayer {

    [RequireComponent(typeof(YoutubePlayer))]
    public class PressToPlay : MonoBehaviour
    {
        private GameObject _ui;
        private Player _player;

        //public static event Action<PressToPlay> OnEnterScreenRange;
        //public static event Action<PressToPlay> OnLeaveScreenRange;
        public static event Action OnMainVideoStartLoading;

        private void Start()
        {
            CloseVideo.OnVideoEnd += CloseVideo_OnVideoEnd;
            //OnEnterScreenRange += PressToPlay_OnEnterScreenRange;
            //OnLeaveScreenRange += PressToPlay_OnLeaveScreenRange;

            _player = Player.Instance;
            _ui = GameObject.Find("Interact Text");
        }

        private void CloseVideo_OnVideoEnd()
        {
            Debug.Log("video ended!!");
        }

        //private void PressToPlay_OnLeaveScreenRange(PressToPlay obj)
        //{
        //    _ui.SetActive(false);
        //}

        //private void PressToPlay_OnEnterScreenRange(PressToPlay screen)
        //{
        //    _ui.SetActive(true);


        //}

        //private void OnDestroy()
        //{
        //    OnEnterScreenRange -= PressToPlay_OnEnterScreenRange;
        //    OnLeaveScreenRange -= PressToPlay_OnLeaveScreenRange;
        //}

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "Player") _player.isInsideScreenRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.tag == "Player") _player.isInsideScreenRange = false;
        }

        private void Update()
        {
            if (_player.isInsideScreenRange && !GameManager.Instance.isVideoPlaying)
            {
                _ui.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    OnMainVideoStartLoading?.Invoke();

                    GetComponent<YoutubePlayer>().enabled = true;

                    GameManager.Instance.isVideoPlaying = true;

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
            else
            {
                _ui.SetActive(false);
            }
        }
    }
}