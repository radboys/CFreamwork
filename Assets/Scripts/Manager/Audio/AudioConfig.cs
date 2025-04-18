using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct AudioInfo
{
    public string key;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Config/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    [Header("背景音乐")]
    public List<AudioInfo> musicInfos;
    [Header("音效")]
    public List<AudioInfo> sfxInfos;
}
