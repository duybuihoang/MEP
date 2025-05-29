using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Music Analysis")]
    public List<Beat> beats;
    private float nextBeatTime;
    private float delayTime;
    public float DelayTime { get => delayTime; set => delayTime = value; }
    int currentBeatIndex = 0;
    int maxBeat;

    private void Start()
    {
        if (beats == null || beats.Count == 0)
        {
            Debug.LogWarning("Beat list is empty!");
            return;
        }
        nextBeatTime = beats[0].time;

    }

    void Update()
    {
        if (currentBeatIndex >= beats.Count) return;

        if (Time.time >= nextBeatTime )
        {
            //TriggerBeat(beats[currentBeatIndex]);
            //currentBeatIndex++;

            //if (currentBeatIndex < beats.Count)
            //{
            //    nextBeatTime = beats[currentBeatIndex].time;
            //}
            TileSpawner.Instance.SpawnTile(Random.Range(0, 4));
            nextBeatTime += .5f;
        }
    }


    void TriggerBeat(Beat beat)
    {
        foreach (var lane in beat.beatLane)
        {
            TileSpawner.Instance.SpawnTile(lane);
        }
    }

    public float GetTimeToBeat()
    {
        return nextBeatTime - Time.time;
    }

    public bool IsOnBeat(float tolerance = 0.1f)
    {
        return Mathf.Abs(GetTimeToBeat()) <= tolerance;
    }
}
