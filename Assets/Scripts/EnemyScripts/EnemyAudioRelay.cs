using UnityEngine;

public class EnemyAudioRelay : MonoBehaviour
{
    private EnemyAudio mainScript;

    void Awake()
    {
        mainScript = GetComponentInParent<EnemyAudio>();
    }

    public void PlayFootstepSound()
    {
        mainScript.PlayFootstepSound();
    }
}
