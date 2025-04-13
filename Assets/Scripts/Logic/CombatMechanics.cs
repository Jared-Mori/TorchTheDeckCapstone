using UnityEngine;

public class CombatMechanics
{
    public static void TakeDamage(CombatDetails target, CombatDetails user, int damage)
    {
        if (target.isShielded)
        {
            target.isShielded = false;
            Shield shield = target.gear[CombatDetails.Shield] as Shield;
            shield.ArmorEffect(target, user);
            return;
        }

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

    public static void Defend(CombatDetails target, CombatDetails user, int damage)
    {
        Debug.Log("Defending against " + damage + " damage.");
        Debug.Log("Target name: " + target.entityType.ToString());

        if (target.isShielded)
        {
            TakeDamage(target, user, damage);
        }
        else
        {
            int randomValue = Random.Range(0, 3);
            if (target.gear[randomValue] != null)
            {
                target.gear[randomValue].Use();
                Armor armor = target.gear[randomValue] as Armor;
                armor.ArmorEffect(target, user);
                return;
            }
            else
            {
                TakeDamage(target, user, damage);
            }
        }
    }

    public static void ApplyStatusEffects(CombatDetails target, CombatDetails user)
    {
        for (int i = 0; i < target.statusEffects.Count; i++)
        {
            if(target.statusEffects[i] != null)
            {
                target.statusEffects[i].UpdateStatus(target, user);
            }
        }
    }
}
