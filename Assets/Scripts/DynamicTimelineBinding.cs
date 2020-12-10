using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DynamicTimelineBinding : MonoBehaviour {
    [SerializeField] private List<GameObject> _trackList = new List<GameObject>();
    [SerializeField] private bool _autoBindTracks = true;
    private PlayableDirector _timeline;
    private TimelineAsset _timelineAsset;

    private void Start() {
        _timeline = GetComponent<PlayableDirector>();
        if (_autoBindTracks) BindTimelineTracks();
    }

    public void BindTimelineTracks() {
        _timelineAsset = (TimelineAsset)_timeline.playableAsset;

        for (var i = 0; i < _trackList.Count; i++) {
            if (_trackList[i] != null) {
                var track = (_timelineAsset.markerTrack == null) ? _timelineAsset.GetOutputTrack(i) : _timelineAsset.GetOutputTrack(i + 1);
                _timeline.SetGenericBinding(track, _trackList[i]);
            }
        }
    }
}
