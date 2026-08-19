using System.Collections;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip fistsAttackSound;
    [SerializeField] private AudioClip fistsHitSound;
    [SerializeField] private float fistsSoundVolume = 0.5f;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClips;
    private bool footstepSoundOnCooldown = false;
    [SerializeField] private float baseFootstepSoundCooldown = 1.5f;
    [SerializeField] float footstepAveragePitch = 1f;
    [SerializeField] float footstepPitchRange = 0.2f;
    [SerializeField] float footstepBaseVolume = 0.15f;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFistsSound(bool attackDidHit)
    {
        if (attackDidHit)
        {
            audioSource.PlayOneShot(fistsHitSound, fistsSoundVolume);
        }
        else if (!attackDidHit)
        {
            audioSource.PlayOneShot(fistsAttackSound, fistsSoundVolume);
        }
    }

    public void PlayFootstepSound(float bonusSpeed)
    {
        if (!footstepSoundOnCooldown)
        {
            float footstepVolume = Mathf.Clamp01(footstepBaseVolume + (bonusSpeed * 0.5f));
            AudioClip randomClip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.pitch = Random.Range(footstepAveragePitch - footstepPitchRange, footstepAveragePitch + footstepPitchRange);
            audioSource.PlayOneShot(randomClip, footstepVolume);

            StartCoroutine(StartFootstepSoundCooldown(baseFootstepSoundCooldown / (bonusSpeed + 1)));
        }
    }

    IEnumerator StartFootstepSoundCooldown(float seconds)
    {
        footstepSoundOnCooldown = true;
        yield return new WaitForSeconds(seconds);
        footstepSoundOnCooldown = false;
    }
}
