using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CombatMechanics
{
    public static async Task TakeDamage(CombatDetails target, CombatDetails user, int damage)
    {
        if (target.isShielded)
        {
            target.isShielded = false;
            if (target.entityType == EntityType.Player)
            {
                PileController pc = GameObject.Find("Deck")?.GetComponent<PileController>();
                Armor shield = pc.hand.FirstOrDefault(card => card.itemType == ItemType.Shield) as Armor;
                await shield.ArmorEffect(target, user);
                Card card = shield as Card;
                await AnimationController.TriggerPlayerAction(card); // Trigger the player action animation
                if (card.Use())
                {
                    int index = pc.hand.IndexOf(card);
                    await pc.RemoveCard(index);
                    await pc.AddCard(new TempShield());
                }
                return;
            }
            else
            {
                Armor shield = target.gear[CombatDetails.Shield] as Armor;
                await shield.ArmorEffect(target, user);
                Card card = shield as Card;
                await AnimationController.TriggerEnemyAction(card); // Trigger the enemy action animation
                if(card.Use())
                {
                    user.gear[CombatDetails.Shield] = null;
                }
                return;
            }
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

    public static async Task Defend(CombatDetails target, CombatDetails user, int damage)
    {
        Debug.Log("Defending against " + damage + " damage.");
        Debug.Log("Target name: " + target.entityType.ToString());
        PileController pc = GameObject.Find("Deck")?.GetComponent<PileController>();
        bool mitigated = false;

        if (target.isShielded)
        {
            target.isShielded = false;
            if (target.entityType == EntityType.Player && pc.hand.FirstOrDefault(card => card.itemType == ItemType.Shield) is not TempCard)
            {
                Armor shield = pc.hand.FirstOrDefault(card => card.itemType == ItemType.Shield) as Armor;
                await shield.ArmorEffect(target, user);
                Card card = shield as Card;
                await AnimationController.TriggerPlayerAction(card); // Trigger the player action animation
                if (card.Use())
                {
                    int index = pc.hand.IndexOf(card);
                    await pc.RemoveCard(index);
                    await pc.AddCard(new TempShield());
                }
                return;
            }
            else if (target.gear[CombatDetails.Shield] != null)
            {
                Armor shield = target.gear[CombatDetails.Shield] as Armor;
                await shield.ArmorEffect(target, user);
                Card card = shield as Card;
                await AnimationController.TriggerEnemyAction(card); // Trigger the enemy action animation
                if(card.Use())
                {
                    user.gear[CombatDetails.Shield] = null;
                }
                return;
            }
        }

        // Randomly select if the attack hits helmet, chest, or boots
        int randValue = Random.Range(0, 3);
        ItemType itemType = (ItemType)randValue;

        Debug.Log("Randomly selected item type: " + itemType.ToString());
        if (target.entityType == EntityType.Player && pc.hand.FirstOrDefault(card => card.itemType == itemType) is not TempCard)
        {
            Armor armor = pc.hand.FirstOrDefault(card => card.itemType == itemType) as Armor;
            if (armor == null) { Debug.Log("No boots found in hand."); }
            else
            {
                await armor.ArmorEffect(target, user);
                StatTracker.IncrementCardsPlayed(); // Increment the cards played
                Card card = armor as Card;
                await AnimationController.TriggerPlayerAction(card); // Trigger the player action animation
                if (card.Use()){
                    int index = pc.hand.IndexOf(card);
                    await pc.RemoveCard(index);
                    switch (itemType)
                    {
                        case ItemType.Helmet:
                            await pc.AddCard(new TempHelm());
                            break;
                        case ItemType.Chestpiece:
                            await pc.AddCard(new TempChest());
                            break;
                        case ItemType.Boots:
                            await pc.AddCard(new TempBoots());
                            break;
                        default:
                            Debug.Log("Unknown item type: " + itemType.ToString());
                            break;
                    }
                }
                mitigated = true;
            }
        }
        else if (target.gear[randValue] != null)
        {
            await AnimationController.TriggerEnemyAction(target.gear[randValue]); // Trigger the enemy action animation
            await target.gear[randValue].Effect(user, target);
            if (target.gear[randValue].Use()){
                target.gear[randValue] = null;
            }
            mitigated = true;
        }

        int postMitigationDamage = damage;
        
        if (mitigated)
        {
            Debug.Log("Damage mitigated by armor.");
            postMitigationDamage = Mathf.FloorToInt(damage / 2);
        }

        Debug.Log("Triggering animation for damage: " + postMitigationDamage.ToString());
        AnimationController animationController = GameObject.Find("AnimationController")?.GetComponent<AnimationController>();

        if (target.entityType == EntityType.Player)
        { animationController.TriggerAnimation(postMitigationDamage.ToString(), "Damage", true); }
        else
        { animationController.TriggerAnimation(postMitigationDamage.ToString(), "Damage", false); }

        await TakeDamage(target, user, postMitigationDamage);
    }

    public static async Task ApplyStatusEffects(CombatDetails target, CombatDetails user)
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
                await target.statusEffects[i].UpdateStatus(target, user);
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
