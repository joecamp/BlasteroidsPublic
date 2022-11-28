using System;

using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Range(0f, 2f)]
    [SerializeField] private float _volumeModifier = .5f;
    [SerializeField] private AudioPlayer musicLoopAudioPlayer;
    public float VolumeModifier { get; private set; }

    public float Pitch { get; private set; } = 1f;
    public static Action<float> OnGlobalPitchChanged;
    public static Action<float> OnVolumeModifierChanged;

    private void Awake()
    {
        VolumeModifier = _volumeModifier;
    }

    private void Update()
    {
        if (_volumeModifier != VolumeModifier)
        {
            VolumeModifier = _volumeModifier;
            OnVolumeModifierChanged?.Invoke(_volumeModifier);
        }
    }

    public void PlayMenuMusic(bool shouldFade = true)
    {
        if (shouldFade)
        {
            musicLoopAudioPlayer.FadeToClipByIndex(0);
        }
        else
        {
            musicLoopAudioPlayer.PlayClipByIndex(0);
        }
    }

    public void PlayGameMusic(bool shouldFade = true)
    {
        if (shouldFade)
        {
            musicLoopAudioPlayer.FadeToClipByIndex(1);
        }
        else
        {
            musicLoopAudioPlayer.PlayClipByIndex(1);
        }
    }

    public void TogglePauseLoopingMusic(bool value)
    {
        musicLoopAudioPlayer.TogglePause(value);
    }

    public void ToggleMuteMusic(bool value)
    {
        musicLoopAudioPlayer.ToggleMute(value);
    }

    public void SetGlobalPitch(float pitch)
    {
        Pitch = pitch;
        OnGlobalPitchChanged?.Invoke(pitch);
    }
}