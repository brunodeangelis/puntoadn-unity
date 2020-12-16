using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HanoiWinButton : MonoBehaviour {
    private bool _isPlayerLooking;
    private List<Sprite> _sprites = new List<Sprite>();
    [SerializeField] private SpriteRenderer[] symbols;

    private void Start() {
        _sprites = GameManager._i._hanoiSprites;
    }

    private void Update() {
        if (_isPlayerLooking) {
            if (Input.GetMouseButtonDown(0)) {
                if (symbols[0].sprite == _sprites[0] &&
                    symbols[1].sprite == _sprites[1] &&
                    symbols[2].sprite == _sprites[2]) {
                    GameManager._i.OpenNearbyWall();
                    SoundManager.PlaySound(SoundManager.Sound.Success);
                    GameManager._i.CreateTask("Continuá por el camino para llegar a la próxima estación");
                    for (int i = 0; i < 3; i++) {
                        symbols[i].color = Color.green;
                    }
                } else {
                    for (int i = 0; i < 3; i++) {
                        if (symbols[i].sprite != _sprites[i]) {
                            symbols[i].color = Color.red;
                            symbols[i].DOColor(Color.white, 1.75f);
                        } else {
                            symbols[i].color = Color.green;
                            symbols[i].DOColor(Color.white, 1.75f);
                        }
                    }
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