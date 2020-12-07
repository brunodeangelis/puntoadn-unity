using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    private PlayableDirector _timeline;
    private PlayableDirector _gameObjectTimeline;
    [SerializeField] private string _timelineName;

    private void Awake()
    {
        _timeline = GetComponent<PlayableDirector>();
        //_gameObjectTimeline = GameObject.Find(_timelineName).GetComponent<PlayableDirector>();
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
