using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio
{
    [Header("AudioSource")]
    private AudioSource sourceMusic; // ссылка на воспроизведение музыки
    private AudioSource sourceSFX; // ссылка на воспроизведение звука
    private AudioSource sourceRandomPitchSFX; // ссылка на воспроизведение звука с рандомной частотой

    [Header("Volume")]
    private float volumeSFX = 1; // Громкость звука
    private float volumeMusic = 1; // Громкость музыки

    [Header("AudioClip")]
    [SerializeField] private AudioClip[] sounds; // Массив звуков
    [SerializeField] private AudioClip DefaultSound; // Звук по умолчанию, если из массива не найден
    [SerializeField] private AudioClip MenuMusic; // Музыка для меню
    [SerializeField] private AudioClip GameMusic; // Музыка для игры

    public float VolumeSFX 
    {
        get { return volumeSFX; }
        set { volumeSFX = value; sourceSFX.volume = volumeSFX; sourceRandomPitchSFX.volume = volumeSFX; }
    }
    public float VolumeMusic 
    {
        get { return volumeMusic; }
        set { volumeMusic = value; sourceMusic.volume = volumeMusic; }
    }

    public AudioSource SourceMusic 
    {
        get { return sourceMusic; }
        set { sourceMusic = value; }
    }
    public AudioSource SourceSFX 
    {
        get { return sourceSFX; }
        set { sourceSFX = value;}
    }
    public AudioSource SourceRandomPitchSFX 
    {
        get { return sourceRandomPitchSFX; }
        set { sourceRandomPitchSFX = value; }
    }


    private AudioClip GetSound(string NameClip) 
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == NameClip) 
            {
                return sounds[i];
            }
        }

        return DefaultSound;

    }
    public void PlaySound(string NameClip) 
    {
        SourceSFX.PlayOneShot(GetSound(NameClip));
    }
    public void PlaySoundRandomPitch(string NameClip) 
    {
        SourceSFX.pitch = Random.Range(0.6f,1f);
        sourceSFX.PlayOneShot(GetSound(NameClip));
    }
    public void PlayMusic(bool menu) 
    {
        if (menu)
        {
            SourceMusic.clip = MenuMusic;
        }
        else 
        {
            sourceMusic.clip = GameMusic;
        }
        sourceMusic.loop = true;
        sourceMusic.volume = volumeMusic;
        sourceMusic.Play();
    }
}
