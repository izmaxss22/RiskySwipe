using UnityEngine;

[CreateAssetMenu(fileName = "VisualDesignDataManager", menuName = "VisualDesignDataManager")]
public class VisualDesignDataManager : ScriptableObject
{
    //public GameObject[] playerSkinPrefabs { get; }
    //public GameObject[] playerParticlesSkinPrefabs { get; }

    public int GetPlayerParticlesSkin()
    {
        // TODO  (ОБНОВА) получить это значение из сохранненых
        return 0;
    }

    public int GetPlayerSkin()
    {
        // TODO  (ОБНОВА) получить это значение из сохранненых
        return 0;
    }
}