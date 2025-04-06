using System.Collections;
using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class TMPTypewriter : MonoBehaviour
{
    public static TMPTypewriter Instance = null;

    // Audio settings
    [Header("Audio Settings")]
    [SerializeField] private AudioClip typeSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minPitch = 0.1f;
    [SerializeField] private float maxPitch = 1.1f;
    [SerializeField] private bool playSound = true;

    // Dictionary to track active typewriter coroutines
    private Dictionary<TMP_Text, Coroutine> activeTypewriters = new Dictionary<TMP_Text, Coroutine>();

    private void Awake()
    {
        Instance = this;
        // Create audio source if not assigned
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.5f;
        }
    }

    public void Type(TMP_Text textComponent, string textToType, float duration, Action onComplete = null)
    {
        Stop(textComponent);
        
        Coroutine typewriterCoroutine = StartCoroutine(TypeTextCoroutine(textComponent, textToType, duration, onComplete));
        activeTypewriters[textComponent] = typewriterCoroutine;
    }
    
    public void TypeBySpeed(TMP_Text textComponent, string textToType, float charsPerSecond, Action onComplete = null)
    {
        if (!textComponent.enabled)
        {
            textComponent.enabled = true;
        }
        float duration = textToType.Length / Mathf.Max(0.1f, charsPerSecond);
        Type(textComponent, textToType, duration, onComplete);
    }
    
    public void Stop(TMP_Text textComponent)
    {
        if (activeTypewriters.TryGetValue(textComponent, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            activeTypewriters.Remove(textComponent);
        }
    }
    
    public void StopAll()
    {
        foreach (var coroutine in activeTypewriters.Values)
        {
            StopCoroutine(coroutine);
        }
        activeTypewriters.Clear();
    }
    
    public void Complete(TMP_Text textComponent, string fullText)
    {
        Stop(textComponent);
        textComponent.text = fullText;
    }

    public void SetTypeSound(AudioClip sound)
    {
        typeSound = sound;
    }

    public void EnableSound(bool enable)
    {
        playSound = enable;
    }

    public void SetPitchRange(float min, float max)
    {
        minPitch = min;
        maxPitch = max;
    }
    
    private IEnumerator TypeTextCoroutine(TMP_Text textComponent, string textToType, float duration, Action onComplete)
    {
        if (textComponent == null) yield break;
        
        textComponent.text = "";
        
        float timePerChar = duration / textToType.Length;
        float timer = 0f;
        int lastCharIndex = 0;
        int charIndex = 0;
        
        while (charIndex < textToType.Length)
        {
            timer += Time.deltaTime;
            charIndex = Mathf.FloorToInt(timer / timePerChar);
            charIndex = Mathf.Min(charIndex, textToType.Length);
            
            // If we added a new character, play sound
            if (charIndex > lastCharIndex)
            {
                if (playSound && typeSound != null)
                {
                    // Randomize pitch for each character
                    audioSource.volume = SoundManager.Instance.Voulume * 0.05f;
                    audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
                    audioSource.PlayOneShot(typeSound);
                }
                lastCharIndex = charIndex;
            }
            
            textComponent.text = textToType.Substring(0, charIndex);
            
            yield return null;
        }
        
        textComponent.text = textToType;
        
        if (activeTypewriters.ContainsKey(textComponent))
        {
            activeTypewriters.Remove(textComponent);
        }
        
        onComplete?.Invoke();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Instance = null;
    }
}