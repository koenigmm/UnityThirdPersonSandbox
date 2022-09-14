﻿using UnityEngine;
using System.IO;

namespace SavingSystem
{
    public class SavingSystemManager : MonoBehaviour
    {
        private string _persistentPath;

        private void Awake()
        {
            _persistentPath = Application.persistentDataPath + "/save.json";
            Load();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public void Save()
        {
            var saveData = new SaveData();
            var saveableEntities = FindObjectsOfType<SaveableEntity>();
            foreach (var component in saveableEntities) component.PopulateSaveData(saveData);
            File.WriteAllText(_persistentPath, JsonUtility.ToJson(saveData, true));
        }

        private void Load()
        {
            if (!File.Exists(_persistentPath)) return;

            var saveData = new SaveData();
            var jsonData = File.ReadAllText(_persistentPath);
            JsonUtility.FromJsonOverwrite(jsonData, saveData);

            var saveableEntities = FindObjectsOfType<SaveableEntity>();
            
            foreach (var entity in saveableEntities) entity.LoadFromSaveData(saveData);
        }

        // public void ClearSavedData()
        // {
        //     if (File.Exists(_persistentPath)) File.Delete(_persistentPath);
        // }
        
    }
}