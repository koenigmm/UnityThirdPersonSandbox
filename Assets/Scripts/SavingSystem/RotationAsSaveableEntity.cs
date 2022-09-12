namespace SavingSystem
{
    public class RotationAsSaveableEntity : SaveableEntity
    {
#if UNITY_EDITOR
        private void Update()
        {
            SetGuid();
        }
#endif
        public override void PopulateSaveData(SaveData saveData)
        {
            var rotationWithID = new QuaternionWithID()
            {
                uuid = uuid,
                savedQuaternion = transform.rotation
            };

            saveData.rotationComponents.Add(rotationWithID);
        }

        public override void LoadFromSaveData(SaveData saveData)
        {
            foreach (var rotation in saveData.rotationComponents)
            {
                if (rotation.uuid != uuid) continue;
                transform.rotation = rotation.savedQuaternion;
            }
        }
    }
}