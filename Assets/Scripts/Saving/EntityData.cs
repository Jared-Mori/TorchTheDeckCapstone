using UnityEngine;
using System.Collections.Generic; 

[System.Serializable]
public class EntityData
{
    public EntityType entityType;
    public int xPos, yPos;
    public Direction facing;
    public int health, maxHealth;
    public int energy, maxEnergy;
    public string entityName;
    public bool isAttacker;
    public bool isOpenedChest = false;
    public bool isOpenedDoor = false;
    public Equipment[] gear = new Equipment[5]; // 0: Helmet, 1: Chestpiece, 2: Boots, 3: Shield, 4: Accessory
    public List<Card> deck = new List<Card>();
}

[System.Serializable]
public class SaveData
{
    public int level;
    public EntityData[] entityDataArray;
}