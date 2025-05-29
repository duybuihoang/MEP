using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Beat Data", menuName = "Beat")]
public class AudioBeatSO : ScriptableObject
{
    public string ID;
    public AudioClip audio;
    public List<Beat> beats;
}

[Serializable]
public class Beat
{
    public float time;
    public List<int> beatLane;
}
