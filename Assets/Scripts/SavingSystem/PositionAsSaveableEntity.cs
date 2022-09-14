using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SavingSystem
{
    public class PositionAsSaveableEntity : SaveableEntity
    {
        [SerializeField] private bool isAIControlled;
        
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
                print(transform.name);
                
                if (isAIControlled)
                    HandleAIMovement(position.savedVector3);
                else
                    HandlePlayerMovement(position.savedVector3);

            }
        }

        private void HandleAIMovement(Vector3 position)
        {
            TryGetComponent(out NavMeshAgent navMeshAgent);
            if (navMeshAgent == null) return;
            navMeshAgent.Warp(position);
        }

        private void HandlePlayerMovement(Vector3 position)
        {
            TryGetComponent(out CharacterController characterController);
            if (characterController == null) return;
            characterController.enabled = false;
            transform.position = position;
            characterController.enabled = true;
        }
        
    }
}