using System.Collections.Generic;

namespace SavingSystem
{
    [System.Serializable]
    public class SaveData
    {
        public List<IntWithID> healthComponents = new();
        public List<IntWithID> tokenComponents = new();
        public List<Vector3WithID> positionComponents = new();
        public List<QuaternionWithID> rotationComponents = new();
    }
    
    
}