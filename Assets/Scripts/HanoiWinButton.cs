﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanoiWinButton : MonoBehaviour {
    private bool _isPlayerLooking;
    private List<Sprite> _sprites = new List<Sprite>();

    private void Start() {
        _sprites = GameManager._i._hanoiSprites;
    }

    private void Update() {
        if (_isPlayerLooking) {
            if (Input.GetMouseButtonDown(0)) {
                var symbols = GameObject.FindGameObjectsWithTag("HanoiSymbol");

                if (symbols[0].GetComponent<SpriteRenderer>().sprite == _sprites[0] &&
                    symbols[1].GetComponent<SpriteRenderer>().sprite == _sprites[1] &&
                    symbols[2].GetComponent<SpriteRenderer>().sprite == _sprites[2]) {
                    GameManager._i.OpenNearbyWall();
                    SoundManager.PlaySound(SoundManager.Sound.Success);
                    GameManager._i.CreateTask("Continuá por el camino para llegar a la próxima estación");
                } else {
                    SoundManager.PlaySound(SoundManager.Sound.Error);
                }
            }
        }
    }

    private void OnRaycastEnter(GameObject sender) {
        if (sender.CompareTag("Player")) _isPlayerLooking = true;
    }

    private void OnRaycastExit(GameObject sender) {
        if (sender.CompareTag("Player")) _isPlayerLooking = false;
    }
}