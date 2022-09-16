using UnityEngine;
using System.IO;

namespace SavingSystem
{
    public class SavingSystemManager : MonoBehaviour
    {
        private string _persistentPath;
        private Health _playerHealth;

        private void Awake()
        {
            _persistentPath = Application.persistentDataPath + "/save.json";
            Load();
            _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        public void Save()
        {
            if (!_playerHealth.IsAlive()) return;
            var saveData = new SaveData();
            var saveableEntities = FindObjectsOfType<SaveableEntity>();
            foreach (var component in saveableEntities) component.PopulateSaveData(saveData);
            File.WriteAllText(_persistentPath, JsonUtility.ToJson(saveData, true));
            print($"Saved at {_persistentPath}");
        }

        private void Load()
        {
            if (!File.Exists(_persistentPath)) return;

            var saveData = new SaveData();
            var jsonData = File.ReadAllText(_persistentPath);
            JsonUtility.FromJsonOverwrite(jsonData, saveData);

            var saveableEntities = FindObjectsOfType<SaveableEntity>();
            
            foreach (var entity in saveableEntities) entity.LoadFromSaveData(saveData);
            print("savegame loaded");
        }

        // public void ClearSavedData()
        // {
        //     if (File.Exists(_persistentPath)) File.Delete(_persistentPath);
        // }
        
    }
}