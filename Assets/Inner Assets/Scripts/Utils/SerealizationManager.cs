using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Networking;
using System.Dynamic;

public static class SerealizationManager
{
    public enum SavingType
    {
        StreamingAssets,
        PerssistentDataPath
    }
    
    private static string streamingAssetsPath = Application.streamingAssetsPath + "/";
    private static string persistentDataPath = Application.persistentDataPath + "/";

    #region МЕТОДЫ ЗАГРУЗКИ ОБЬЕКТА ИЗ СЕРЕАЛИЗОВАНОГО ФАЙЛА

    #region МЕТОДЫ ДЛЯ ЗАГРУЗКИ НЕИЗМЕНЯЕМОГО ОБЬЕКТА (ЗАГРУЖЕНОГО ЗАРАНЕЕ В streaming assets)
    // Загрузка сереализованного обьекта (константный = тот которые не должен меняться и который нельзя сохранить)
    // который был добавлен в сборку заранее (типо данных уровней и тд) из streamingAssetsPath 
    public static ObjectType LoadConstantSerealizedObject<ObjectType>(string pathAffterStreamingAssets)
    {
#if UNITY_ANDROID
        // Если это андройд то происходит загрузка уровней характерная для андройд
        // var loadedObject = LoadingFileOnOtherPlatforms<ObjectType>(pathAffterStreamingAssets);
        //todo
        var loadedObject = LoadingFileOnAndroidPlatform<ObjectType>(pathAffterStreamingAssets);
#else
        var loadedObject = LoadingFileOnOtherPlatforms<ObjectType>(pathAffterStreamingAssets);
#endif
        return loadedObject;
    }

    // Загрузка данных характерная для андройд (сначала получение файла по url и создание его во временных файлах системы, а потом загрузка)
    private static ObjectType LoadingFileOnAndroidPlatform<ObjectType>(string pathAffterStreamingAssets)
    {
        // По пути файла происходит генерация запроса
        var request = UnityWebRequest.Get(Path.Combine(streamingAssetsPath, pathAffterStreamingAssets));
        request.SendWebRequest();
        Debug.Log(1);
        // Ожидание окончания запроса
        while (!request.isDone)
        {
            if (request.isNetworkError || request.isHttpError)
            {
                break;
            }
        }
        // Если запрос завершился без ошибок (получилось получить файл по указаному пути)
        if (!request.isNetworkError && !request.isHttpError)
        {
            // Меняеться путь чтобы он направлял не в несуществующую папку
            var newPath = pathAffterStreamingAssets.Replace("/", "");
            // То во временных файлах создаеться полученный из запроса файл
            string pathForSavedFile = Path.Combine(persistentDataPath, newPath);
            File.WriteAllBytes(pathForSavedFile, request.downloadHandler.data);
            //TODO проверка вроде не нужна
            //if (File.Exists(pathForSavedFile))
            //{
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            //FileStream fileStream = new FileStream(filePath, FileMode.Open);
            FileStream fileStream = File.OpenRead(pathForSavedFile);
            ObjectType levelData = (ObjectType)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return levelData;
        }
        return default;
    }

    // Загрузка данных для всех остальный платформ
    private static ObjectType LoadingFileOnOtherPlatforms<ObjectType>(string pathAffterStreamingAssets)
    {
        string filePath = streamingAssetsPath + pathAffterStreamingAssets;
        if (File.Exists(filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.OpenRead(filePath);

            ObjectType levelData = (ObjectType)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return levelData;
        }
        return default;
    }
    #endregion

    // Внешний метод для вызова загрузки файла (этот файл изменяемый (лежимт в perstistent а не streaming)
    public static ObjectType LoadDynamicSerealizedObject<ObjectType>(string pathAffterPersistentDataPath)
    {
        string filePath = persistentDataPath + pathAffterPersistentDataPath;
        Debug.Log(filePath);
        if (File.Exists(filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.OpenRead(filePath);

            ObjectType levelData = (ObjectType)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return levelData;
        }
        return default;
    }

    #endregion

    #region МЕТОДЫ СОХРАНЕНИЕ ОБЬЕКТА В СЕРЕАЛИЗОВАННЫЙ ФАЙЛ
    // Сохранение данных 
    public static void SaveObjectToSerealizedFile<ObjectType>(ObjectType objectForSaving, SavingType savingType, string pathAffter)
    {
        string filePath = "";

        if (savingType == SavingType.PerssistentDataPath)
        {
            filePath = persistentDataPath + pathAffter;

        }
        else if (savingType == SavingType.StreamingAssets)
        {
            filePath = streamingAssetsPath + pathAffter;
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        binaryFormatter.Serialize(fileStream, objectForSaving);
        fileStream.Close();
    }
    #endregion
}
