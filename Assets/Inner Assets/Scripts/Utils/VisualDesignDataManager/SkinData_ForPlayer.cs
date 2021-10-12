using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Inner_Assets.Scripts.DataManagers.VisualDesignDataManager
{
    [CreateAssetMenu(fileName = "SkinsData_ForPlayer", menuName = "SkinsData_ForPlayer")]
    public class SkinData_ForPlayer : ScriptableObject
    {
        [Header("Префабы со скинами персонажей")]
        public List<GameObject> playersSkins;
    }
}