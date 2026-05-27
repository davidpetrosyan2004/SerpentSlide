using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string name;
    public float volume;
    public bool loop;
    public AudioSource source;
    public AudioClip clip;
}
