using UnityEngine;
using System.Linq;

public class DataSynchronizerFor_GameUpdates : MonoBehaviour
{
    private DataManager DataManager;

    private void Awake()
    {
        DataManager = DataManager.Instance;
    }

    public void Update_DatasStructures()
    {
        Update_MaxAvailableLevel();
        Update_LevelAchivmentsStarsStructure();
        // todo  (ОБНОВА) подумать перед обновой нужно ли это для других элементов 
    }

    // Прогрузка следующего уровня (если до обновления игрок прошел все уровни то после обновы у него должен быть доступен след уровень из обновы)
    private void Update_MaxAvailableLevel()
    {
        int lastAvailableLevel = DataManager.GetLastAvailableLevelNumber();
        int[] array = DataManager.GetStarsCountsAchivmentOnLevels();
        // Если  после обновления количество уровней изменилось
        if (array.Length < DataManager.COUNT_LEVELS)
        {
            // Если игрок достиг последнего доступного уровня в прошлом обновлении
            if (lastAvailableLevel == array.Length - 1)
            {
                // То уровень увеличиваеться на +1 чтобы стал доступен новый уровень из обновления
                lastAvailableLevel++;
                DataManager.SetLastAvailableLevelNumber(lastAvailableLevel);
            }
        }
    }

    // Обновление списка уровней если в обновлении появлись новые уровни 
    private void Update_LevelAchivmentsStarsStructure()
    {
        int[] array = DataManager.GetStarsCountsAchivmentOnLevels();
        // Если  после обновления количество уровней изменилось
        if (array.Length < DataManager.COUNT_LEVELS)
        {
            var list = array.ToList<int>();
            int countAddedLevels = DataManager.COUNT_LEVELS - array.Length;
            // Добавление новых значений в массив
            for (int i = 0; i < countAddedLevels; i++) list.Add(0);
            DataManager.SetStarsCountsAchivmentOnLevels(list.ToArray());
        }
    }
}