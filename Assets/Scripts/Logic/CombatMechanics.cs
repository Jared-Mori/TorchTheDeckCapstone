using UnityEngine;

public class CombatMechanics
{
    public static void TakeDamage(CombatDetails target, int damage)
    {
        target.health -= damage;
        if (target.health < 0)
        {
            target.health = 0;
        }
    }

    public static void Heal(CombatDetails target, int healAmount)
    {
        target.health += healAmount;
        if (target.health > target.healthMax)
        {
            target.health = target.healthMax;
        }
    }

    public static void UseEnergy(CombatDetails target, int energyCost)
    {
        target.energy -= energyCost;
    }

    public static void Defend(CombatDetails target, int damage)
    {
        Debug.Log("Defending against " + damage + " damage.");
        Debug.Log("Target name: " + target.entityType.ToString());

        if (target.isShielded)
        {
            target.isShielded = false;
            target.gear[CombatDetails.Shield].Use();
            return;
        }
        else
        {
            int randomValue = Random.Range(0, 3);
            if (target.gear[randomValue] != null)
            {
                target.gear[randomValue].Use();
                return;
            }
            else
            {
                TakeDamage(target, damage);
            }
        }
    }
}
