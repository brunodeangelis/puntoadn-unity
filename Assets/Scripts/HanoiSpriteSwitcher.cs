using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanoiSpriteSwitcher : MonoBehaviour {
    [SerializeField] private bool _isLeftArrow;
    private bool _isPlayerLooking;
    private List<Sprite> _sprites = new List<Sprite>();
    private int _currentIndex = 0;

    private void Start() {
        _sprites = GameManager._i._hanoiSprites;
    }

    private void Update() {
        if (_isPlayerLooking) {
            if (Input.GetMouseButtonDown(0)) {
                SpriteRenderer spriteRenderer = transform.parent.Find("Sprite").GetComponent<SpriteRenderer>();

                if (_isLeftArrow) {
                    _currentIndex--;
                    if (_currentIndex < 0) {
                        _currentIndex = _sprites.Count - 1;
                    }
                } else {
                    _currentIndex++;
                    if (_currentIndex >= _sprites.Count) {
                        _currentIndex = 0;
                    }
                }

                spriteRenderer.sprite = _sprites[_currentIndex];
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
