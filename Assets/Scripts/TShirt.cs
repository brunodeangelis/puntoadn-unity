using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TShirt : MonoBehaviour {
    [SerializeField] private bool _isCorrectTShirt;
    private bool _isPlayerLooking;
    [SerializeField] private VisualEffect _particlesVFX;

    //public GameObject _TShirtBase;
    public Wall _wall;

    private void Update() {
        if (Input.GetMouseButtonUp(0)) {
            if (_isPlayerLooking) {
                if (_isCorrectTShirt) {
                    transform.DOMoveY(2f, 2f);
                    _particlesVFX.SetVector4("Color", new Vector4(7f, 36f, 7f, 5f)); // Green
                    _wall.Down();
                    SoundManager.PlaySound(SoundManager.Sound.Success);
                } else {
                    _particlesVFX.SetVector4("Color", new Vector4(32f, 0f, 0f, 5f)); // Red
                    SoundManager.PlaySound(SoundManager.Sound.Error);
                }
            }
        }
    }

    void OnRaycastEnter(GameObject sender) {
        _isPlayerLooking = true;
        //transform.DOScale(transform.localScale + new Vector3(0.1f, 0.1f, 0.1f), 0.3f);
    }

    void OnRaycastExit(GameObject sender) {
        _isPlayerLooking = false;
        //transform.DOScale(transform.localScale - new Vector3(0.1f, 0.1f, 0.1f), 0.3f);
    }
}
