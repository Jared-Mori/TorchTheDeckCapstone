using UnityEngine;

public interface IBoss
{
    
}

public class Necromancer : Enemy , IBoss
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        entityType = EntityType.Necromancer;
        viewDistance = 3;
        maxHealth = 60;
        health = maxHealth;
        maxEnergy = 5;
        energy = maxEnergy;
    }

    public void IncrementIdleCounter()
    {
        Animator animator = GetComponent<Animator>();
        int currentCounter = animator.GetInteger("IdleCounter");
        animator.SetInteger("IdleCounter", currentCounter + 1);
    }

    public void ResetIdleCounter()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetInteger("IdleCounter", 0);
        animator.SetTrigger("PlayIdle2");
    }

    public override void Walk()
    {
        facing = Direction.Down;
    }
}
