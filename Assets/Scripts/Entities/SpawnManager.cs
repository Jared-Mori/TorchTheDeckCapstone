using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    public static List<Entity> SpawnEntities(List<Entity> entities, Level level)
    {
        Tilemap chestSpawns = GameObject.Find("ChestSpawns").GetComponent<Tilemap>();
        Tilemap enemySpawns = GameObject.Find("EnemySpawns").GetComponent<Tilemap>();

        List<Vector3Int> chestPositions = GetTilePositions(chestSpawns);
        List<Vector3Int> enemyPositions = GetTilePositions(enemySpawns);

        int chestCount = level.chestCount;
        while (chestCount > 0 && chestPositions.Count > 0)
        {
            // Pick a random position from the available chest positions
            int randomIndex = Random.Range(0, chestPositions.Count);
            Vector3Int tilePosition = chestPositions[randomIndex];
            chestPositions.RemoveAt(randomIndex); // Remove the position to avoid duplicate spawns

            // Convert tile position to world position and spawn the chest
            Vector3 worldPosition = chestSpawns.CellToWorld(tilePosition) + chestSpawns.tileAnchor;
            entities.Add(SpawnEntity(worldPosition, EntityType.Chest));

            chestCount--;
        }

        for (int i = 0; i < level.enemyCount.Length; i++)
        {
            int enemyCount = level.enemyCount[i];
            while (enemyCount > 0 && enemyPositions.Count > 0)
            {
                // Pick a random position from the available enemy positions
                int randomIndex = Random.Range(0, enemyPositions.Count);
                Vector3Int tilePosition = enemyPositions[randomIndex];
                enemyPositions.RemoveAt(randomIndex); // Remove the position to avoid duplicate spawns

                // Convert tile position to world position and spawn the enemy
                Vector3 worldPosition = enemySpawns.CellToWorld(tilePosition) + enemySpawns.tileAnchor;
                entities.Add(SpawnEntity(worldPosition, EnemyRandomizer(i))); // Cast to EntityType

                enemyCount--;
            }
        }

        return entities;
    }

    public static List<Entity> RespawnEntities(List<Entity> entities, EntityData[] entityDataArray)
    {
        foreach (EntityData entityData in entityDataArray)
        {
            entities.Add(RespawnEntity(entityData));
        }
        return entities;
    }

    private static Entity SpawnEntity(Vector3 position, EntityType entityType)
    {
        GameObject prefab = null;
        Debug.Log("Spawning entity of type: " + entityType);
        switch (entityType)
        {
            case EntityType.Chest:
                prefab = Resources.Load<GameObject>("Prefabs/Chest");
                break;
            case EntityType.Door:
                prefab = Resources.Load<GameObject>("Prefabs/Door");
                break;
            case EntityType.Slime:
                prefab = Resources.Load<GameObject>("Prefabs/Slime");
                break;
            case EntityType.Goblin:
                prefab = Resources.Load<GameObject>("Prefabs/Goblin");
                break;
            case EntityType.SkeletonArcher:
                prefab = Resources.Load<GameObject>("Prefabs/SkeletonArcher");
                break;
            case EntityType.SkeletonSword:
                prefab = Resources.Load<GameObject>("Prefabs/SkeletonSword");
                break;
            case EntityType.Vampire:
                prefab = Resources.Load<GameObject>("Prefabs/Vampire");
                break;
            case EntityType.Werewolf:
                prefab = Resources.Load<GameObject>("Prefabs/Werewolf");
                break;
            case EntityType.Necromancer:
                prefab = Resources.Load<GameObject>("Prefabs/Necromancer");
                break;
        }

        GameObject entityObject = Instantiate(prefab, position, Quaternion.identity);
        return entityObject.GetComponent<Entity>();
    }

    private static Entity RespawnEntity(EntityData entityData)
    {
        Entity entity = SpawnEntity(new Vector3(entityData.xPos, entityData.yPos, 0), entityData.entityType);
        entity.facing = entityData.facing;

        if (entityData.isOpenedChest)
        {
            Chest chest = entity as Chest;
            chest.isOpen = true;
        }

        return entity;
    }

    public static List<Vector3Int> GetTilePositions(Tilemap tilemap)
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        // Iterate through all positions within the tilemap's bounds
        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            // Check if there is a tile at the current position
            if (tilemap.HasTile(position))
            {
                positions.Add(position);
            }
        }

        return positions;
    }

    public static EntityType EnemyRandomizer(int tier)
    {
        EntityType type = EntityType.Player;

        switch (tier)
        {
            case 0:
                type = (EntityType)Random.Range((int)EntityType.Slime, (int)EntityType.Goblin + 1);
                break;
            case 1:
                type = (EntityType)Random.Range((int)EntityType.SkeletonArcher, (int)EntityType.SkeletonSword + 1);
                break;
            case 2:
                type = (EntityType)Random.Range((int)EntityType.Vampire, (int)EntityType.Werewolf + 1);
                break;
            case 3:
                type = EntityType.Necromancer;
                break;
        }

        return type;
    }
}
