using UnityEngine;

[System.Serializable]
public class Rock : Entity
{
    public override void SetDefaults()
    {
        entityType = EntityType.Rock;
        facing = Direction.Down;
    }

    public override void Interact()
    {
        Debug.Log("Interacting with rock");
        Stone stone = new Stone();
        stone.AddToDeck();
        this.Die();
    }
}
