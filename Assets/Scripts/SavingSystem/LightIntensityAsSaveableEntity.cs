using UnityEngine;
using SavingSystem;

public class LightIntensityAsSaveableEntity : SaveableEntity
{
    // SerializeField prevents racing conditions with the saving system
    [SerializeField] private Light lightToSave;

#if UNITY_EDITOR
    private void Update()
    {
        SetGuid();
    }

#endif

    public override void LoadFromSaveData(SaveData saveData)
    {
        foreach (var lightComponent in saveData.lightComponents)
            {
                if (lightComponent.uuid != uuid) continue;
                lightToSave.intensity = lightComponent.savedFloat;
            }
    }

    public override void PopulateSaveData(SaveData saveData)
    {
        var positionWithID = new FloatWithID
            {
                uuid = uuid,
                savedFloat = lightToSave.intensity
            };

            saveData.lightComponents.Add(positionWithID);
    }
}
