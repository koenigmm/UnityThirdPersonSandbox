using UnityEditor;
using UnityEngine;

namespace SavingSystem
{
    [ExecuteAlways]
    public abstract class SaveableEntity : MonoBehaviour
    {
        [Header("Saveable Entity")] [SerializeField]
        protected string uuid = "";

        [SerializeField] protected bool isSaveable = true;

        public abstract void PopulateSaveData(SaveData saveData);

        public abstract void LoadFromSaveData(SaveData saveData);

#if UNITY_EDITOR
        protected void SetGuid(bool shouldCheckIfIsInPlayOrPrefabMode = true)
        {
            if (shouldCheckIfIsInPlayOrPrefabMode && IsInPlayOrPrefabMode()) return;

            var serializedObject = new SerializedObject(this);
            var serializedProperty = serializedObject.FindProperty("uuid");

            if (!string.IsNullOrEmpty(serializedProperty.stringValue)) return;

            serializedProperty.stringValue = GUID.Generate().ToString();
            serializedObject.ApplyModifiedProperties();
        }


        private bool IsInPlayOrPrefabMode()
        {
            return string.IsNullOrEmpty(gameObject.scene.path) || Application.isPlaying;
        }
#endif
    }
}