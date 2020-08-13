using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseVideo : MonoBehaviour
{
    public void EndVideo()
    {
        if (GameManager.Instance.isVideoPlaying)
        {
            GameManager.Instance.isVideoPlaying = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
