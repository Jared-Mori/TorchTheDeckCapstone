using System.Linq;
using UnityEngine;

public class CombatMechanics
{
    public static void TakeDamage(CombatDetails target, CombatDetails user, int damage)
    {
        if (target.isShielded)
        {
            target.isShielded = false;
            if (target.entityType == EntityType.Player)
            {
                PileController pc = GameObject.Find("Deck")?.GetComponent<PileController>();
                Shield shield = pc.hand.OfType<Shield>().FirstOrDefault();
                shield.ArmorEffect(target, user);
                if (shield.Use())
                {
                    int index = pc.hand.IndexOf(shield);
                    pc.RemoveCard(index);
                    pc.AddCard(new TempShield());
                }
                return;
            }
            else
            {
                Armor shield = target.gear[CombatDetails.Shield] as Armor;
                shield.ArmorEffect(target, user);
                Card card = shield as Card;
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

    public static void Defend(CombatDetails target, CombatDetails user, int damage)
    {
        Debug.Log("Defending against " + damage + " damage.");
        Debug.Log("Target name: " + target.entityType.ToString());
        PileController pc = GameObject.Find("Deck")?.GetComponent<PileController>();

        if (target.isShielded)
        {
            target.isShielded = false;
            if (target.entityType == EntityType.Player)
            {
                Shield shield = pc.hand.OfType<Shield>().FirstOrDefault();
                shield.ArmorEffect(target, user);
                if (shield.Use())
                {
                    int index = pc.hand.IndexOf(shield);
                    pc.RemoveCard(index);
                    pc.AddCard(new TempShield());
                }
                return;
            }
            else
            {
                Armor shield = target.gear[CombatDetails.Shield] as Armor;
                shield.ArmorEffect(target, user);
                Card card = shield as Card;
                if(card.Use())
                {
                    user.gear[CombatDetails.Shield] = null;
                }
                return;
            }
        }

        int randValue = Random.Range(0, 3);
        switch (randValue)
        {
            case CombatDetails.Helmet:
                if (target.entityType == EntityType.Player)
                {
                    foreach (Card card in pc.hand){
                        if (card.itemType == ItemType.Helmet){
                            Armor armor = card as Armor;
                            armor.ArmorEffect(user, target);
                            if (card.Use()){
                                int index = pc.hand.IndexOf(card);
                                pc.RemoveCard(index);
                                pc.AddCard(new TempHelm());
                            }
                            break;
                        }
                    }
                }
                else
                {
                    target.gear[CombatDetails.Helmet].Effect(user, target);
                    if (target.gear[CombatDetails.Helmet].Use()){
                        target.gear[CombatDetails.Helmet] = null;
                    }
                }
                break;
            case CombatDetails.Chestpiece:
                if (target.entityType == EntityType.Player)
                {
                    foreach (Card card in pc.hand){
                        if (card.itemType == ItemType.Chestpiece){
                            Armor armor = card as Armor;
                            armor.ArmorEffect(user, target);
                            if (card.Use()){
                                int index = pc.hand.IndexOf(card);
                                pc.RemoveCard(index);
                                pc.AddCard(new TempChest());
                            }
                            break;
                        }
                    }
                }
                else
                {
                    target.gear[CombatDetails.Chestpiece].Effect(user, target);
                    if (target.gear[CombatDetails.Chestpiece].Use()){
                        target.gear[CombatDetails.Chestpiece] = null;
                    }
                }
                break;
            case CombatDetails.Boots:
                if (target.entityType == EntityType.Player)
                {
                    foreach (Card card in pc.hand){
                        if (card.itemType == ItemType.Boots){
                            Armor armor = card as Armor;
                            armor.ArmorEffect(user, target);
                            if (card.Use()){
                                int index = pc.hand.IndexOf(card);
                                pc.RemoveCard(index);
                                pc.AddCard(new TempBoots());
                            }
                            break;
                        }
                    }
                }
                {
                    target.gear[CombatDetails.Boots].Effect(user, target);
                    if (target.gear[CombatDetails.Boots].Use()){
                        target.gear[CombatDetails.Boots] = null;
                    }
                }
                break;
        }


        Debug.Log("Triggering animation for damage: " + damage.ToString());
        AnimationController animationController = GameObject.Find("AnimationController")?.GetComponent<AnimationController>();

        if (target.entityType == EntityType.Player)
        { animationController.TriggerAnimation(damage.ToString(), "Damage", true); }
        else
        { animationController.TriggerAnimation(damage.ToString(), "Damage", false); }

        TakeDamage(target, user, damage);
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
