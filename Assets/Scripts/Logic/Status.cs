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

    public virtual void AmplifyStatus(CombatDetails target, CombatDetails user)
    {
        // Implement specific amplification logic here
        Debug.Log("Amplified status effect logic executed.");
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
        damagePerTurn = 2;
    }

    public override void StatusEffect(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Poison effect applied.");
        CombatMechanics.TakeDamage(target, user, damagePerTurn);
    }

    public override void AmplifyStatus(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Amplified poison effect applied.");
        turnsLeft += 1; // Increase duration

        if (damagePerTurn < 5)
        {
            damagePerTurn += 1; // Increase damage per turn
        }
        else
        {
            Debug.Log("Poison damage is already at maximum.");
        }
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
        damagePerTurn = 1;
    }

    public override void StatusEffect(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Burn effect applied.");
        CombatMechanics.TakeDamage(target, user, damagePerTurn);
    }

    public override void AmplifyStatus(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Amplified Burn effect applied.");
        turnsLeft += 1; // Increase duration

        if (damagePerTurn < 5)
        {
            damagePerTurn += 1; // Increase damage per turn
        }
        else
        {
            Debug.Log("Burn damage is already at maximum.");
        }
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

    public override void AmplifyStatus(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Amplified Paralysis effect applied.");
        turnsLeft += 1; // Increase duration
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

    public override void AmplifyStatus(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Amplified Exhausted effect applied.");
        turnsLeft += 1; // Increase duration
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

    public override void AmplifyStatus(CombatDetails target, CombatDetails user)
    {
        Debug.Log("Amplified Haste effect applied.");
        turnsLeft += 1; // Increase duration
    }
}

/// <summary>
/// Interface for status effects on cards
/// </summary>
public interface IStatusEffect
{
    Status status { get; set; }
}

