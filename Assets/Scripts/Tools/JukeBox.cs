using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class JukeBox : MonoBehaviour
{

    public static JukeBox Instance;
    public AudioSource TargetSource;
    public AudioClip Shoot;
    public AudioClip Hit;
    public AudioClip Death;
    public AudioClip Jump;

    public AudioClip Hellyeah;
    public AudioClip Noo;
    public AudioClip Grounded;

    public AudioSource Hanging;
    public AudioSource WindAudioSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioClip _clip, float _volumne)
    {
        TargetSource.pitch = Random.Range(0.90f, 1.1f);
        TargetSource.PlayOneShot(_clip, _volumne * SoundManager.Instance.Voulume);
    }

    public void PlaySoundDelayed(AudioClip _clip, float _volumne, float _delay)
    {
        StartCoroutine(WaitAndPlay(_clip, _volumne, _delay));
    }

    IEnumerator WaitAndPlay(AudioClip _clip, float _volumne, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        PlaySound(_clip, _volumne);
    }

    public void PlaySoundAtSource(AudioSource s, AudioClip _clip, float _volumne)
    {
        s.pitch = Random.Range(0.90f, 1.1f);
        s.PlayOneShot(_clip, _volumne * SoundManager.Instance.Voulume);
    }

    public void EnableAudioSource(AudioSource s, float volume)
    {
        s.pitch = Random.Range(0.90f, 1.1f);
        s.volume = SoundManager.Instance.Voulume * volume;
        s.Play();
    }

    public void DisableAudioSource(AudioSource s)
    {
        s.Stop();
    }

    public void SetAudioSourceVolume(AudioSource s, float volume)
    {
        s.volume = volume * SoundManager.Instance.Voulume;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}