using System.Collections.Generic;

namespace SavingSystem
{
    [System.Serializable]
    public class SaveData
    {
        // player
        public Level playerLevel;
        public int levelUpTokensInInventory;
        public int healingPotionsInInventory;
        public AmmunitionInventoryData ammunitionInventoryData;
        public List<IntWithID> ammunitionInWeapons = new();

        public List<FloatWithID> healthComponents = new();
        public List<FloatWithID> lightComponents = new();
        public List<Vector3WithID> currentPositionComponents = new();
        public List<Vector3WithID> defaultPositions = new();
        public List<QuaternionWithID> rotationComponents = new();
        public List<BoolWithID> pickUpComponents = new();
    }
    
    
}