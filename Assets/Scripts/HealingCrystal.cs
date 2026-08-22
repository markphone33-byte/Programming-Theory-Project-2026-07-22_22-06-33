using System.Collections.Generic;
using UnityEngine;

public class HealingCrystal : MonoBehaviour
{
    private float nextHealTime;
    [SerializeField] private float healInterval = 2;
    [SerializeField] private float healPower = 5;
    private List<Collider> playersToHeal = new List<Collider>();

    void Awake()
    {
        nextHealTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextHealTime <= Time.time)
        {
            foreach (Collider player in playersToHeal)
            {
                player.GetComponent<PlayerHealth>()?.Heal(healPower);
            }
            nextHealTime = Time.time + healInterval;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playersToHeal.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        playersToHeal.Remove(other);
    }
}
