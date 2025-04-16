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

        Debug.Log("Triggering animation for healing: " + healAmount.ToString());
        AnimationController animationController = GameObject.Find("AnimationController")?.GetComponent<AnimationController>();
        
        if (target.entityType == EntityType.Player)
        { animationController.TriggerAnimation(healAmount.ToString(), "Healing", true); }
        else
        { animationController.TriggerAnimation(healAmount.ToString(), "Healing", false); }
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
            target.isShielded = false;
            PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
            var card = pc.GetEquippedCard(ItemType.Shield);
            if (card != null && card.card is Armor armor)
            {
                armor.ArmorEffect(target, user);
            }
            if (card.card.uses == 0)
            {
                target.gear[CombatDetails.Shield] = new TempShield();
            }
            return;
        }
        else
        {
            int randomValue = Random.Range(0, 3);
            if (target.gear[randomValue] != null)
            {
                Armor armor = target.gear[randomValue] as Armor;
                armor.ArmorEffect(target, user);

                if (target.gear[randomValue].Use())
                {
                    switch (randomValue)
                    {
                        case CombatDetails.Helmet:
                            target.gear[CombatDetails.Helmet] = new TempHelm();
                            break;
                        case CombatDetails.Chestpiece:
                            target.gear[CombatDetails.Chestpiece] = new TempChest();
                            break;
                        case CombatDetails.Boots:
                            target.gear[CombatDetails.Boots] = new TempBoots();
                            break;
                    }
                }
                return;
            }
            else
            {
                Debug.Log("Triggering animation for damage: " + damage.ToString());
                AnimationController animationController = GameObject.Find("AnimationController")?.GetComponent<AnimationController>();

                if (target.entityType == EntityType.Player)
                { animationController.TriggerAnimation(damage.ToString(), "Damage", true); }
                else
                { animationController.TriggerAnimation(damage.ToString(), "Damage", false); }

                TakeDamage(target, user, damage);
            }
        }
    }

    public static void ApplyStatusEffects(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Applying status effects to " + target.entityType.ToString() + ".");
        for (int i = 0; i < target.statusEffects.Count; i++)
        {
            if(target.statusEffects[i] != null)
            {
                string statusValue = "";
                switch(target.statusEffects[i].statusName)
                {
                    case "Burn":
                        Burn burn = target.statusEffects[i] as Burn;
                        statusValue = burn.damagePerTurn.ToString();
                        break;
                    
                    case "Poison":
                        Poison poison = target.statusEffects[i] as Poison;
                        statusValue = poison.damagePerTurn.ToString();
                        break;
                    default:
                        break;
                }


                Debug.Log("Updating status effect: " + target.statusEffects[i].statusName);
                target.statusEffects[i].UpdateStatus(target, user);
                AnimationController animationController = GameObject.Find("AnimationController")?.GetComponent<AnimationController>();
                if (target.entityType == EntityType.Player)
                {
                    animationController.TriggerAnimation(statusValue, target.statusEffects[i].statusName, true);
                }
                else
                {
                    Debug.Log("Triggering animation for enemy status effect: " + target.statusEffects[i].statusName);
                    animationController.TriggerAnimation(statusValue, target.statusEffects[i].statusName, false);
                }
            }
        }
    }
}
