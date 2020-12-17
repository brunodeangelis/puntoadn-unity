using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class StationNameSetter : MonoBehaviour
{
    private TextMeshProUGUI _textElement;
    private CanvasGroup _group;
    //private bool _isStationNameShowing;

    [SerializeField] private float _checkDistance = 11f;

    public string _nameForUI;
    private float _dist;

    private void Start() {
        //_isStationNameShowing = GameManager._i._isStationNameShowing;
        _textElement = GameObject.Find("Current Station UI/Current Station Text").GetComponent<TextMeshProUGUI>();
        _group = _textElement.transform.parent.GetComponent<CanvasGroup>();
    }

    private void Update() {
        _dist = Vector3.Distance(transform.position, Player.Instance.transform.position);
        if (_dist > 20f) return;
        if (_dist < _checkDistance) {
            if (!GameManager._i._isStationNameShowing) {
                _textElement.text = _nameForUI;
                _group.DOFade(1f, 0.2f)
                    .OnComplete(() => {
                        GameManager._i._isStationNameShowing = true;
                    });
            }
        } else {
            _group.DOFade(0f, 0.2f)
                .OnComplete(() => {
                    GameManager._i._isStationNameShowing = false;
                });
        }
    }
}
