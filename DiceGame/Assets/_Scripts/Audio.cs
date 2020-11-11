using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Private_Variables
[System.Serializable]
public class Audio
{
    private AudioSource sourceFX;
    public AudioSource SourceFX
    {
        get { return sourceFX; }
        set { sourceFX = value; }
    }

    private AudioSource sourceMusic;
    public AudioSource SourceMusic
    {
        get { return sourceMusic; }
        set { sourceMusic = value; }
    }

    private AudioSource sourceRandomPitchSFX;
    public AudioSource SourceRandomPitchSFX
    {
        get { return sourceRandomPitchSFX; }
        set 
        { 
            sourceRandomPitchSFX = value; 
            DataStore.SaveOptions();
        }
    }

    private float musicVolume = 1f;
    public float MusicVolume 
    {
        get { return musicVolume; }
        set 
        { 
            musicVolume = value; 
            SourceMusic.volume = musicVolume;
            DataStore.SaveOptions();
        }
    }
    private float sfxVolume = 1f;
    public float SfxVolume 
    {
        get { return sfxVolume;}
        set 
        { 
            sfxVolume = value; 
            SourceFX.volume = sfxVolume;
            SourceRandomPitchSFX.volume = sfxVolume;
            DataStore.SaveOptions();
        }
    }
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private AudioClip defaultClip;
    [SerializeField] private AudioClip mainMenu;
    [SerializeField] private AudioClip gameMusic;

    private AudioClip GetSound(string clipName)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].name == clipName)
            {
                return sounds[i];
            }
        }

        Debug.Log("Cannot find a clip " + clipName);
        return defaultClip;
    }

    public void PlaySound(string clipName)
    {
        SourceFX.PlayOneShot(GetSound(clipName), sfxVolume);
    }

    public void PlaySoundRandomPitch(string clipName)
    {
        SourceRandomPitchSFX.pitch = Random.Range(.7f, 1.3f);
        SourceRandomPitchSFX.PlayOneShot(GetSound(clipName), sfxVolume);
    }

    public void PlayMusic(bool menu)
    {
        if(menu)
        {
            SourceMusic.clip = mainMenu;
        }
        else
        {
            SourceMusic.clip = gameMusic;
        }

        SourceMusic.volume = musicVolume;
        SourceMusic.loop = true;
        SourceMusic.Play();
    }
}
#endregion