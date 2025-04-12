using UnityEngine;

[System.Serializable]
public class Rock : Entity
{
    public override void SetDefaults()
    {
        entityType = EntityType.Rock;
        facing = Direction.Down;
        artwork = Resources.Load<Sprite>("Sprites/Entities/Rock");
    }

    public override void Interact()
    {
        Debug.Log("Interacting with rock");
        Stone stone = new Stone();
        stone.AddToDeck();
        this.Die();
    }
}
