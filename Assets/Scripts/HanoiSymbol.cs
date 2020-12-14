using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanoiSymbol : MonoBehaviour
{
    private List<Sprite> _sprites = new List<Sprite>();
    private SpriteRenderer _spriteRenderer;
    private int _currentIndex = 0;

    private void Start() {
        _sprites = GameManager._i._hanoiSprites;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void NextSprite() {
        _currentIndex++;
        if (_currentIndex >= _sprites.Count) {
            _currentIndex = 0;
        }

        _spriteRenderer.sprite = _sprites[_currentIndex];
    }

    public void PreviousSprite() {
        _currentIndex--;
        if (_currentIndex < 0) {
            _currentIndex = _sprites.Count - 1;
        }

        _spriteRenderer.sprite = _sprites[_currentIndex];
    }
}
