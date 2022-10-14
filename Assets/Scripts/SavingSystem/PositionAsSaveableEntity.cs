using UnityEngine;
using UnityEngine.AI;

namespace SavingSystem
{
    public class PositionAsSaveableEntity : SaveableEntity
    {
        [SerializeField] private bool isAIControlled;
        private const string UUID_DEFAULT_POSITION_EXTENSTION = "defaultPosition";

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
            saveData.currentPositionComponents.Add(positionWithID);

            if (!TryGetComponent(out EnemyStateMachine enemyStateMachine)) return;

             var defaultPositionWithID = new Vector3WithID
            {
                uuid = uuid + UUID_DEFAULT_POSITION_EXTENSTION,
                savedVector3 = enemyStateMachine.DefaultPosition
            };
            saveData.defaultPositions.Add(defaultPositionWithID);

        }

        public override void LoadFromSaveData(SaveData saveData)
        {
            foreach (var savedPosition in saveData.currentPositionComponents)
            {
                if (savedPosition.uuid != uuid) continue;

                if (isAIControlled)
                    HandleAIMovement(savedPosition.savedVector3);
                else
                    HandlePlayerMovement(savedPosition.savedVector3);
            }

             foreach (var savedDefaultPosition in saveData.defaultPositions)
            {
                if (savedDefaultPosition.uuid != uuid + UUID_DEFAULT_POSITION_EXTENSTION) continue;
                if (!TryGetComponent(out EnemyStateMachine enemyStateMachine)) continue;
                enemyStateMachine.DefaultPosition = savedDefaultPosition.savedVector3;
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