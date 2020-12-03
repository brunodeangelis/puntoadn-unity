using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneTrigger : MonoBehaviour
{
    private PlayableDirector _timeline;

    private void Awake()
    {
        _timeline = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _timeline != null)
        {
            _timeline.Play();
            GameManager._i._isCutscenePlaying = true;

            foreach (var collider in GetComponents<BoxCollider>())
            {
                if (collider.isTrigger) Destroy(collider);
            }
        }
    }
}
