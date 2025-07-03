using UnityEngine;
using System;

public class Gem : MonoBehaviour, ICollectible
{
    public static event Action OnGemCollected;

    [Header("Audio")]
    public AudioClip collectedSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Collect()
    {
        Debug.Log("Gem collected!");

        if (audioSource != null && collectedSound != null)
        {
            audioSource.PlayOneShot(collectedSound);
        }
        else if (collectedSound != null)
        {
            GameObject tempAudio = new GameObject("TempGemSound");
            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            tempSource.clip = collectedSound;
            tempSource.Play();
            Destroy(tempAudio, collectedSound.length);
        }

        OnGemCollected?.Invoke();

        Destroy(gameObject, 0.1f); 
    }
}
