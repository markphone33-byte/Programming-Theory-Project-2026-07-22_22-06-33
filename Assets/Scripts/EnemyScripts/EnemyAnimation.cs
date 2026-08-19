using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    private EnemyAttack enemyAttackScript;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyAttackScript = GetComponent<EnemyAttack>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void AnimateMovement(float speed, bool inChase)
    {
        animator.SetFloat("Speed", speed / 5);
        // If idle and just started moving then set IsIdle to false
        if (speed > 0 && animator.GetBool("IsIdle") && !enemyAttackScript.isStunned)
        {
            animator.SetBool("IsIdle", false);
        }
        // If moving and just stopped then set IsIdle to true
        else if (speed == 0 && !animator.GetBool("IsIdle"))
        {
            animator.SetBool("IsIdle", true);
        }

        // Enters and exits chase
        if (inChase != animator.GetBool("InChase"))
        {
            animator.SetBool("InChase", inChase);
        }
    }

    public void AnimateAttack()
    {
        animator.SetTrigger("Attack");
        animator.SetBool("IsIdle", true);
    }
}
