using Unity.VisualScripting;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioClip[] gruntClips;
    [SerializeField] private AudioSource footstepsAudioSource;
    [SerializeField] private AudioSource gruntAudioSource;
    [SerializeField] private float averagePitch = 1f;
    [SerializeField] private float pitchRange = 0.2f;
    [SerializeField] private float volume = 1;

    void Awake()
    {
        
    }

    public void PlayFootstepSound()
    {
        float averagePitch = 1f;
        float pitchRange = 0.2f;
        float volume = 0.4f;

        AudioClip randomClip = footstepClips[Random.Range(0, footstepClips.Length)];
        footstepsAudioSource.pitch = Random.Range(averagePitch - pitchRange, averagePitch + pitchRange);
        footstepsAudioSource.PlayOneShot(randomClip, volume);
    }

    public void PlayGruntSoud()
    {
        float averagePitch = 0.8f;
        float pitchRange = 0.1f;
        float volume = 0.2f;

        AudioClip randomClip = gruntClips[Random.Range(0, gruntClips.Length)];
        gruntAudioSource.pitch = Random.Range(averagePitch - pitchRange, averagePitch + pitchRange);
        gruntAudioSource.PlayOneShot(randomClip, volume);
    }
}
