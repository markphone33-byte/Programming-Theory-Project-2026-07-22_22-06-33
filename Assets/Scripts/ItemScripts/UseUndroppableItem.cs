using Unity.VisualScripting;
using UnityEngine;

public class UseUndroppableItem : UseItem
{
    [SerializeField] private ParticleSystem slashParticle;
    private PlayerAttack playerAttackScript;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerAttackScript = player.GetComponent<PlayerAttack>();
    }

    public override void Use()
    {
        string selectedItemName = PlayerInventory.Instance.GetSelectedItem().name;

        if (selectedItemName == "Fists")
        {
            FistsAttack();
        }
    }

    private void FistsAttack()
    {
        Vector3 attackHalfExtents = new Vector3(2f, 1.5f, 0.7f);
        float damage = 10f;
        float particleSpeed = 8f;
        playerAttackScript.BasicMeleeAttack(damage, attackHalfExtents, particleSpeed);
    }
}
