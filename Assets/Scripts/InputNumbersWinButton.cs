using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class InputNumbersWinButton : MonoBehaviour
{
    private bool _isPlayerLooking;

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerLooking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NumberInput[] inputs = FindObjectsOfType<NumberInput>();
                string[] values = new string[inputs.Length];

                foreach (NumberInput input in inputs)
                {
                    values[input._index] = input.GetComponent<TextMeshProUGUI>().text;
                }

                string concatenatedValues = string.Join("", values);

                if (concatenatedValues == GameManager.Instance._inputStationWinnerNumber)
                {
                    GameManager.Instance.OpenNearbyWall();
                    SoundManager.PlaySound(SoundManager.Sound.Success);
                } else
                {
                    //foreach (NumberInput input in inputs)
                    //{
                    //    Color.HSVToRGB()
                    //    Color newColor = Color.white;
                    //    input.GetComponent<TextMeshProUGUI>().material.color = newColor;
                    //    DOTween.To(() => newColor, x => newColor = x, Color.white, 0.2f);
                    //}
                    SoundManager.PlaySound(SoundManager.Sound.Error);
                }
            }
        }

    }

    private void OnRaycastEnter(GameObject sender)
    {
        if (sender.transform.tag == "Player") _isPlayerLooking = true;
    }

    private void OnRaycastExit(GameObject sender)
    {
        if (sender.transform.tag == "Player") _isPlayerLooking = false;
    }
}
