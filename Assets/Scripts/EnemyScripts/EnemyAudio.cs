using Unity.VisualScripting;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepClips;
    private AudioSource audioSource;
    [SerializeField] private float averagePitch = 1f;
    [SerializeField] private float pitchRange = 0.2f;
    [SerializeField] private float volume = 1;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstepSound()
    {
        AudioClip randomClip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.pitch = Random.Range(averagePitch-pitchRange, averagePitch+pitchRange);
        audioSource.PlayOneShot(randomClip, volume);
    }
}
