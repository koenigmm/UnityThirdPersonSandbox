namespace SavingSystem
{
    public class PositionAsSaveableEntity : SaveableEntity
    {
        
#if UNITY_EDITOR
        private void Update()
        {
            SetGuid();
        }

#endif

        public override void PopulateSaveData(SaveData saveData)
        {
            var positionWithID = new Vector3WithID
            {
                uuid = uuid,
                savedVector3 = transform.position
            };

            saveData.positionComponents.Add(positionWithID);
        }

        public override void LoadFromSaveData(SaveData saveData)
        {
            foreach (var position in saveData.positionComponents)
            {
                if (position.uuid != uuid) continue;
                transform.position = position.savedVector3;
            }
        }
    }
}