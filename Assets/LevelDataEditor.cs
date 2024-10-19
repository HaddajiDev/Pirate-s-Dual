using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Level_Data))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Level_Data levelData = (Level_Data)target;

        if (GUILayout.Button("Generate Levels"))
        {
            GenerateLevels(levelData);
            EditorUtility.SetDirty(levelData); // Marks as dirty so changes are saved
        }
    }

    private void GenerateLevels(Level_Data levelData)
    {
        int numLevels = 100; // Generate 100 levels as specified
        levelData.levels = new Level[numLevels];

        // Define starting values
        int startHealth = 1;
        int startMaxShotForce = 45;
        int startMinShotForce = 30;        
        int startMinAngle = 35;
        int startMaxAngle = 40;
        int startDamageMin = 1;
        int startDamageMax = 5;
        int startDistance = 0;

        for (int i = 0; i < numLevels; i++)
        {
            Level newLevel = new Level();

            newLevel.level = i + 1;
            // Health progression
            newLevel.Health = Mathf.Min(startHealth + i, 70);

            // Force progression (gradual increase between 30 and 55)
            newLevel.MaxshootForce = Mathf.Min(startMaxShotForce + i / 2, 55);
            newLevel.MinshootForce = Mathf.Min(startMinShotForce + i / 2, 45);

            // Angle progression (gradually widen angle, max 35-70)
            newLevel.Min_Angle = Mathf.Min(startMinAngle + i / 2, 70);
            newLevel.Max_Angle = Mathf.Min(startMaxAngle + i / 2, 70);

            // Fire shots (first 5 levels true)
            newLevel.Fire = i > 5;

            // Burst shots (first few levels 1 or 3)
            newLevel.Burst_Shoots = (i > 10) ? 3 : 1;

            // Power Ups (after level 20)
            newLevel.usePowerUps = i >= 20;

            // Damage progression (gradually increase, max 50)
            newLevel.DamageMin = Mathf.Min(startDamageMin + i / 2, 50);
            newLevel.DamageMax = Mathf.Min(startDamageMax + i / 2, 50);

            // Skins progression (random values within provided ranges)
            newLevel.ship = Random.Range(0, 5);
            newLevel.anchor = Random.Range(0, 8);
            newLevel.flag = Random.Range(0, 8);
            newLevel.sail = Random.Range(0, 7);
            newLevel.helm = Random.Range(0, 7);
            newLevel.cannon = Random.Range(0, 4);

            // Distance progression (start adjusting after level 20, max 80)
            newLevel.distance = (i >= 20) ? Mathf.Min(startDistance + (i - 20), 80) : 0;

            // Assign the level to the array
            levelData.levels[i] = newLevel;
        }

        Debug.Log("Levels generated successfully!");
    }
}

#endif
