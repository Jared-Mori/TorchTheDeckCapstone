using UnityEngine;

[System.Serializable]
public class Weapon : Card
{
    public int damage;

    public virtual void Attack(Entity target, Entity attacker)
    {
        Debug.Log("Attacking with weapon");
    }
}

[System.Serializable]
public class Stone : Weapon
{
    public Stone()
    {
        cardName = "Stone";
        description = "A hefty stone. You could probably throw it.";
        artwork = levelManager.spriteManager.itemSprites[265];
        damage = 1;
        uses = 1;
        rarity = 0;
        isStackable = true;
    }

    public override void Attack(Entity target, Entity attacker)
    {
        Debug.Log("Attacking with stone");
        target.Defend(damage);
        Use();
    }
}

[System.Serializable]
public class IronSword : Weapon
{
    public IronSword()
    {
        cardName = "Iron Sword";
        description = "A rusted sword. It's still sharp, but probably won't hold out for long.";
        damage = 2;
        uses = 4;
        rarity = 1;
    }

    public override void Attack(Entity target, Entity attacker)
    {
        Debug.Log("Attacking with sword");
        target.Defend(damage);
        Use();
    }
}