using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{   
    public Sound[] sounds;

    bool played;

    void Awake()
    {   
        played = false;
       foreach(Sound s in sounds)
       {
           s.source = gameObject.AddComponent<AudioSource>();
           s.source.clip = s.audioClip;

           s.source.volume = s.volume;
           s.source.pitch = s.pitch;
           s.source.playOnAwake = s.playOnAwake;
           s.source.loop = s.loop;
       } 
    }

    public void Play(string name,float FadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s.source.volume < s.volume)
        {
          s.volume += s.source.volume * Time.deltaTime / FadeTime;  
        }
        else
        {
            s.source.volume = s.volume;
        }
        s.source.Play();
    }

    public void Stop(string name,float FadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s.source.volume < s.volume)
        {
          s.volume -= s.source.volume * Time.deltaTime / FadeTime;  
        }
        else
        {
            s.source.volume = s.volume;
        }
        s.source.Stop();
    }

    public void StopWithVolume()
    {
        foreach(Sound s in sounds)
        {
            s.source.volume = 0;
        }
    }

    public void StopAndPlayWithVolume(string name,float FadeTime,float volume)
    {   
       Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s.name == name)
        {
            foreach(Sound b in sounds)
            {
            b.source.volume = 0;
            s.volume = volume;
            s.source.volume = s.volume;
            s.source.Play();
            }
        
        }
    }

}
