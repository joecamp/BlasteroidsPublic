using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private bool playRandomClipOnStart = false;

    private AudioSource audioSource;

    private float baseVolume;
    private float pitchChangeSpeed = 10f;
    private float pitchGoal = 1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        baseVolume = audioSource.volume;
    }

    private void Start()
    {
        AdjustVolume(AudioManager.Instance.VolumeModifier);
        audioSource.pitch = AudioManager.Instance.Pitch;

        AudioManager.OnGlobalPitchChanged += ChangePitch;
        AudioManager.OnVolumeModifierChanged += AdjustVolume;

        if (playRandomClipOnStart)
        {
            PlayRandomClip();
        }
    }

    private void Update()
    {
        audioSource.pitch = Mathf.Lerp(audioSource.pitch, pitchGoal, Time.deltaTime * pitchChangeSpeed);
    }

    public void PlayClipByIndex(int index)
    {
        if (index < 0 || index >= audioClips.Count)
        {
            Debug.LogError("Clip index out of range.");
            return;
        }

        audioSource.Stop();
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    public void FadeToClipByIndex(int index)
    {
        StartCoroutine(FadeCoroutine(index));
    }

    private IEnumerator FadeCoroutine(int index)
    {
        float initVolume = audioSource.volume;
        float t = 0f;

        while (t < 1f)
        {
            audioSource.volume = Mathf.Lerp(initVolume, 0f, t);
            t += Time.deltaTime * 15f;
            yield return null;
        }

        audioSource.volume = 0;

        // Change to new clip
        audioSource.Stop();
        audioSource.clip = audioClips[index];
        audioSource.Play();

        t = 0f;

        while (t < 1f)
        {
            audioSource.volume = Mathf.Lerp(0f, initVolume, t);
            t += Time.deltaTime * 15f;
            yield return null;
        }

        audioSource.volume = initVolume;
    }

    public void PlayRandomClip()
    {
        if (audioClips.Count == 0)
        {
            Debug.LogError("No audio clips defined");
            return;
        }

        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
    }

    private void ChangePitch(float pitch)
    {
        pitchGoal = pitch;
    }

    private void AdjustVolume(float modifier)
    {
        audioSource.volume = baseVolume * modifier;
    }

    public void TogglePause(bool value)
    {
        if (value)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }

    public void ToggleMute(bool value)
    {
        audioSource.mute = value;
    }

    private void OnDestroy()
    {
        AudioManager.OnGlobalPitchChanged -= ChangePitch;
        AudioManager.OnVolumeModifierChanged -= AdjustVolume;
    }
}