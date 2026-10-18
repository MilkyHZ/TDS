using UnityEngine;

[CreateAssetMenu(fileName = "Round_", menuName = "ScriptableObjects/Create Round Data", order = 1)]
public class RoundData: ScriptableObject
{
    public SpawnedCreature[] Creatures;

    public int GetTotalCreatureCount() 
    {
        int totalCount = 0;
        foreach (SpawnedCreature creature in Creatures) 
        {
            totalCount += creature.SpawnCount;
        }
        return totalCount;
    }
}

[System.Serializable]
public struct SpawnedCreature
{
    public CreatureBehaviour Type;
    public Sprite PreviewIcon;
    public int SpawnCount;
}
