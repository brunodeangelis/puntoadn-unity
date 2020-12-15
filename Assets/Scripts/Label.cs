using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Label : MonoBehaviour {

    [SerializeField] private bool _focusCrosshair = true;

    private TextMeshProUGUI _actionLabel;
    private GameObject _crosshair;
    private RectTransform _rectTransform;

    public string _text;
    
    private void Start() {
        _actionLabel = GameManager._i._actionLabel;
        _crosshair = GameManager._i._crosshair;
        _rectTransform = _crosshair.GetComponent<RectTransform>();
    }

    private void OnRaycastEnter() {
        if (GameManager._i._isVideoPlaying) return;

        _actionLabel.text = _text;
        _actionLabel.DOFade(1f, 0.2f);

        if (!_focusCrosshair) return;

        DOTween.To(
            () => _rectTransform.sizeDelta,
            (x) => _rectTransform.sizeDelta = x,
            new Vector2(24f, 24f),
            0.15f
        );

        _crosshair.GetComponent<Image>().DOColor(new Color(0.53f, 0.34f, 0.63f), 0.15f);
        SoundManager.PlaySound(SoundManager.Sound.Hover);
    }

    private void OnRaycastExit() {
        if (GameManager._i._isVideoPlaying) return;

        _actionLabel.DOFade(0f, 0.2f);

        if (!_focusCrosshair) return;

        DOTween.To(
            () => _rectTransform.sizeDelta,
            (x) => _rectTransform.sizeDelta = x,
            new Vector2(12f, 12f),
            0.15f
        );

        _crosshair.GetComponent<Image>().DOColor(Color.white, 0.15f);
    }
}
