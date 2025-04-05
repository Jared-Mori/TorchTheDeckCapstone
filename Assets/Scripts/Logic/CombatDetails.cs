using UnityEngine;
using System.Collections.Generic;

public class CombatDetails {
    // basic stats
    public int health;
    public int healthMax;
    public int energy;
    public int energyMax;
    public EntityType entityType;

    // deck info
    public List<Card> deck;
    public const int Helmet = 0, Chestpiece = 1, Boots = 2, Shield = 3, Accessory = 4;
    public Equipment[] gear; // 0: Helmet, 1: Chestpiece, 2: Boots, 3: Shield, 4: Accessory

    // Combat states
    public bool isShielded;

    // Other Data

    public CombatDetails(EntityType entityType, int health, int healthMax, int energy, int energyMax, List<Card> deck, Equipment[] gear) {
        this.entityType = entityType;
        this.health = health;
        this.healthMax = healthMax;
        this.energy = energy;
        this.energyMax = energyMax;
        this.deck = deck;
        this.gear = gear;
        this.isShielded = false;
    }
}