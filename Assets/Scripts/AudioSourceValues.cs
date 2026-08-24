using UnityEngine;

public class AudioSourceValues : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float averagePitch = 1f;
    [SerializeField] private float pitchRange = 0.2f;
    [SerializeField] private float volume = 0.2f;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetRandomClip()
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }

    private float GetRandomPitch()
    {
        return Random.Range(averagePitch - pitchRange, averagePitch + pitchRange);
    }

    public void PlayRandomClip()
    {
        AudioClip randomClip = GetRandomClip();
        audioSource.pitch = GetRandomPitch();
        audioSource.PlayOneShot(randomClip, volume);
    }

    public float GetMaxDistance()
    {
        return audioSource.maxDistance;
    }
}
