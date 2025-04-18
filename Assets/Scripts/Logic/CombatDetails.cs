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
    public const int Helmet = 0, Chestpiece = 1, Boots = 2, Shield = 3, Accessory = 4, Weapon = 5, Bow = 6;
    public Card[] gear; // 0: Helmet, 1: Chestpiece, 2: Boots, 3: Shield, 4: Accessory, 5: Weapon, 6: Bow

    // Combat states
    public bool isShielded;
    public List<Status> statusEffects; // 0: Poison, 1: Burn, 2: Freeze

    // Other Data

    public CombatDetails(EntityType entityType, int health, int healthMax, int energy, int energyMax) {
        this.entityType = entityType;
        this.health = health;
        this.healthMax = healthMax;
        this.energy = energy;
        this.energyMax = energyMax;
        isShielded = false;
        deck = new List<Card>();
        gear = new Card[7] {null, null, null, null, null, null, null}; // Initialize gear array with 7 slots
        statusEffects = new List<Status>();
    }
}