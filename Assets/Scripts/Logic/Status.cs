using UnityEngine;

public class Status
{
    public string statusName;
    public int duration;
    public int turnsLeft;

    public void ApplyStatus(CombatDetails target, CombatDetails user)
    {
        target.statusEffects.Add(this);
    }

    public void UpdateStatus(CombatDetails target, CombatDetails user)
    {
        if (turnsLeft > 0)
        {
            turnsLeft--;
            StatusEffect(target, user);
        }
        else
        {
            target.statusEffects.Remove(this);
        }
    }

    public virtual void StatusEffect(CombatDetails target, CombatDetails user)
    {
        // Implement specific status effect logic here
        Debug.Log("Status effect logic executed.");
    }
}

public class Poison : Status
{
    public int damagePerTurn;

    public Poison()
    {
        statusName = "Poison";
        duration = 3;
        turnsLeft = duration;
    }

    public override void StatusEffect(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Poison effect applied.");
        CombatMechanics.TakeDamage(target, user, damagePerTurn);
    }
}

public class Burn : Status
{
    public int damagePerTurn;

    public Burn()
    {
        statusName = "Burn";
        duration = 4;
        turnsLeft = duration;
    }

    public override void StatusEffect(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Burn effect applied.");
        CombatMechanics.TakeDamage(target, user, damagePerTurn);
    }
}

public class Paralysis : Status
{
    public Paralysis()
    {
        statusName = "Paralysis";
        duration = 1;
        turnsLeft = duration;
    }

    public override void StatusEffect(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Paralysis effect applied.");
        target.energy = 0; // Prevents the target from using energy
    }
}

public class Exhausted : Status
{
    public Exhausted()
    {
        statusName = "Exhausted";
        duration = 2;
        turnsLeft = duration;
    }

    public override void StatusEffect(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Exhausted effect applied.");
        target.energy = Mathf.Max(0, target.energy - 1); // Reduces energy by 1, but not below 0
    }
}

public class Haste : Status
{
    public Haste()
    {
        statusName = "Haste";
        duration = 3;
        turnsLeft = duration;
    }

    public override void StatusEffect(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Haste effect applied.");
        target.energy += 1; // Increases energy by 1
        if (turnsLeft == 0)
        {
            Paralysis paralysis = new Paralysis();
            paralysis.ApplyStatus(target, user); // Apply paralysis when haste expires
        }
    }
}

/// <summary>
/// Interface for status effects on cards
/// </summary>
public interface IStatusEffect
{
    Status status { get; set; }
}

