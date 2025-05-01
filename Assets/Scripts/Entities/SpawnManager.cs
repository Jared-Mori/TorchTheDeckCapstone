using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    public static List<Entity> SpawnEntities(List<Entity> entities, Level level)
    {
        Debug.Log("Spawning Player");
        if (GameObject.Find("Player(Clone)") == null)
        {
            entities.Add(SpawnEntity(Vector3.zero, EntityType.Player)); // Spawn the player at (0, 0, 0
        }

        List<Vector3Int> chestPositions = GetTilePositions(level.chests);
        List<Vector3Int> enemyPositions = GetTilePositions(level.enemies);
        List<Vector3Int> rockPositions = GetTilePositions(level.rocks);

        Debug.Log("Chest positions count: " + chestPositions.Count);
        Debug.Log("Enemy positions count: " + enemyPositions.Count);
        int chestCount = level.chestCount;
        while (chestCount > 0 && chestPositions.Count > 0)
        {
            // Pick a random position from the available chest positions
            int randomIndex = Random.Range(0, chestPositions.Count);
            Vector3Int tilePosition = chestPositions[randomIndex];
            chestPositions.RemoveAt(randomIndex); // Remove the position to avoid duplicate spawns

            // Convert tile position to world position and spawn the chest
            Vector3 worldPosition = level.chests.CellToWorld(tilePosition) + level.chests.tileAnchor;
            entities.Add(SpawnEntity(worldPosition, EntityType.Chest));

            chestCount--;
        }

        int rockCount = level.rockCount;
        while (rockCount > 0 && rockPositions.Count > 0)
        {
            // Pick a random position from the available rock positions
            int randomIndex = Random.Range(0, rockPositions.Count);
            Vector3Int tilePosition = rockPositions[randomIndex];
            rockPositions.RemoveAt(randomIndex); // Remove the position to avoid duplicate spawns

            // Convert tile position to world position and spawn the rock
            Vector3 worldPosition = level.rocks.CellToWorld(tilePosition) + level.rocks.tileAnchor;
            entities.Add(SpawnEntity(worldPosition, EntityType.Rock)); // Cast to EntityType

            rockCount--;
        }


        if (level.levelNumber == 2)
        {
            Vector3Int slimePosition = new Vector3Int(0, 5, 0); // Replace with actual slime position
            entities.Add(SpawnEntity(slimePosition, EntityType.Slime)); // Cast to EntityType

            return entities; // Exit early for level 2
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
                Vector3 worldPosition = level.enemies.CellToWorld(tilePosition) + level.enemies.tileAnchor;
                entities.Add(SpawnEntity(worldPosition, EnemyRandomizer(i))); // Cast to EntityType

                enemyCount--;
            }
        }

        return entities;
    }

    public static List<Entity> RespawnEntities(List<Entity> entities, EntityData[] entityDataArray)
    {
        Debug.Log("Respawning entities...");
        foreach (EntityData entityData in entityDataArray)
        {
            if (entityData != null){
                entities.Add(RespawnEntity(entityData));
            }
        }
        return entities;
    }

    private static Entity SpawnEntity(Vector3 position, EntityType entityType)
    {
        GameObject prefab = null;
        switch (entityType)
        {
            case EntityType.Player:
                prefab = Resources.Load<GameObject>("Prefabs/Player");
                break;
            case EntityType.Chest:
                prefab = Resources.Load<GameObject>("Prefabs/Chest");
                break;
            case EntityType.Door:
                prefab = Resources.Load<GameObject>("Prefabs/Door");
                break;
            case EntityType.Rock:
                prefab = Resources.Load<GameObject>("Prefabs/Rock");
                break;
            case EntityType.Bonepile:
                prefab = Resources.Load<GameObject>("Prefabs/BonePilePrefab");
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
        LevelManager levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        entityObject.GetComponent<Entity>().SetLevelManager(levelManager);

        if (entityType == EntityType.Player)
        {
            Debug.Log("Respawning Player");
            levelManager.playerInstance = entityObject.GetComponent<Player>();
        }


        return entityObject.GetComponent<Entity>();
    }

    private static Entity RespawnEntity(EntityData entityData)
    {
        Debug.Log("Respawning entity: " + entityData.entityType + " at position: " + entityData.xPos + ", " + entityData.yPos);
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
