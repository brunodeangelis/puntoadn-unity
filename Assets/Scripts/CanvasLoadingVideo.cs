using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using YoutubePlayer;

public class CanvasLoadingVideo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PressToPlay.OnMainVideoStartLoading += PressToPlay_OnMainVideoStartLoading;
    }

    private void PressToPlay_OnMainVideoStartLoading()
    {
        //GetComponent<VideoPlayer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
