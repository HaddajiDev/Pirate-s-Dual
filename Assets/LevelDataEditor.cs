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
            EditorUtility.SetDirty(levelData);
        }
    }

    private void GenerateLevels(Level_Data levelData)
    {
        int numLevels = 100;
        levelData.levels = new Level[numLevels];
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
            newLevel.Health = Mathf.Min(startHealth + i, 70);

            newLevel.MaxshootForce = Mathf.Min(startMaxShotForce + i / 2, 55);
            newLevel.MinshootForce = Mathf.Min(startMinShotForce + i / 2, 45);
            newLevel.Min_Angle = Mathf.Min(startMinAngle + i / 2, 70);
            newLevel.Max_Angle = Mathf.Min(startMaxAngle + i / 2, 70);
            newLevel.Fire = i > 5;
            newLevel.Burst_Shoots = (i > 10) ? 3 : 1;
            newLevel.usePowerUps = i >= 20;
            newLevel.DamageMin = Mathf.Min(startDamageMin + i / 2, 50);
            newLevel.DamageMax = Mathf.Min(startDamageMax + i / 2, 50);
            newLevel.ship = Random.Range(0, 5);
            newLevel.anchor = Random.Range(0, 8);
            newLevel.flag = Random.Range(0, 8);
            newLevel.sail = Random.Range(0, 7);
            newLevel.helm = Random.Range(0, 7);
            newLevel.cannon = Random.Range(0, 4);
            newLevel.distance = (i >= 20) ? Mathf.Min(startDistance + (i - 20), 80) : 0;
            levelData.levels[i] = newLevel;
        }
    }
}

#endif
