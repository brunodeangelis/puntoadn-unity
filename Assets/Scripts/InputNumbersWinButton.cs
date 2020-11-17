﻿using System;
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
                string concatenatedValues = "";

                foreach (NumberInput input in inputs)
                {
                    string inputText = input.GetComponent<TextMeshProUGUI>().text;
                    concatenatedValues += inputText;
                }

                char[] charArray = concatenatedValues.ToCharArray();
                Array.Reverse(charArray);
                string reversedValues = new string(charArray);

                if (reversedValues == GameManager.Instance._inputStationWinnerNumber)
                {
                    GameManager.Instance.OpenNearbyWall();
                    Debug.Log("ganaste!");
                } else
                {
                    //foreach (NumberInput input in inputs)
                    //{
                    //    Color.HSVToRGB()
                    //    Color newColor = Color.white;
                    //    input.GetComponent<TextMeshProUGUI>().material.color = newColor;
                    //    DOTween.To(() => newColor, x => newColor = x, Color.white, 0.2f);
                    //}
                    Debug.Log("perdiste!");
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