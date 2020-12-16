using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class InputNumbersWinButton : MonoBehaviour {
    private bool _isPlayerLooking;
    private bool _hasWon = false;

    // Update is called once per frame
    void Update() {
        if (_isPlayerLooking) {
            if (Input.GetMouseButtonDown(0)) {
                NumberInput[] inputs = FindObjectsOfType<NumberInput>();
                string[] values = new string[inputs.Length];

                foreach (NumberInput input in inputs) {
                    values[input._index] = input.GetComponent<TextMeshProUGUI>().text;
                }

                string concatenatedValues = string.Join("", values);

                if (concatenatedValues == GameManager._i._inputStationWinnerNumber) {
                    if (!_hasWon) {
                        _hasWon = true;
                        GameManager._i.OpenNearbyWall();
                        SoundManager.PlaySound(SoundManager.Sound.Success);

                        foreach (NumberInput input in inputs) {
                            input.GetComponent<TextMeshProUGUI>().color = Color.green;
                        }

                        GameManager._i.CreateTask("Continuá por el camino para llegar a la próxima estación");
                    }
                } else {
                    foreach (NumberInput input in inputs) {
                        input.GetComponent<TextMeshProUGUI>().color = Color.red;
                        input.GetComponent<TextMeshProUGUI>().DOColor(Color.white, 1.5f);
                    }

                    SoundManager.PlaySound(SoundManager.Sound.Error);
                }
            }
        }

    }

    private void OnRaycastEnter(GameObject sender) {
        if (sender.transform.tag == "Player") _isPlayerLooking = true;
    }

    private void OnRaycastExit(GameObject sender) {
        if (sender.transform.tag == "Player") _isPlayerLooking = false;
    }
}
