using UnityEngine;

public static class DialogueJsonLoader
{
    public static LevelDialogueData LoadLevelFromResources(string resourcePath)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);
        if (jsonFile == null)
        {
            Debug.LogError($"Level dialogue JSON not found at Resources/{resourcePath}.json");
            return null;
        }

        try
        {
            return JsonUtility.FromJson<LevelDialogueData>(jsonFile.text);
        }
        catch
        {
            Debug.LogError($"Failed to parse level dialogue JSON at Resources/{resourcePath}.json");
            return null;
        }
    }
}
