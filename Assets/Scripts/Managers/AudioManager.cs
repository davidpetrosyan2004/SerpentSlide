using System;
using UnityEngine;

[System.Serializable]
public class AudioManager : MonoBehaviour
{
    public SoundEffect[] sounds;
    public bool isHaptics = true;
    public static AudioManager Instance;

    private void Awake()
    {
        Vibration.Init();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
            sound.source.name = sound.name;
            sound.source.clip = sound.clip;
        }
    }

    public void PlaySound(string soundName, bool oneShot = false)
    {
        SoundEffect sound = Array.Find(sounds, x => x.name == soundName);
        if (sound != null)
        {
            if (oneShot) sound.source.PlayOneShot(sound.source.clip);
            else sound.source.Play();
        }
    }
    public void StopSound(string soundName)
    {
        SoundEffect sound = Array.Find(sounds, x => x.name == soundName);
        if (sound != null)
        {
            sound.source.Stop();
        }
    }

    public void Vibrate()
    {
        if(isHaptics)
            Vibration.VibratePop();
    }

    public AudioSource GetAudioSource(string soundName)
    {
        SoundEffect sound = Array.Find(sounds, x => x.name == soundName);
        if (sound != null)
        {
            return sound.source;
        }
        return null;
    }
}
