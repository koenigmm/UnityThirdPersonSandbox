using UnityEngine;

namespace SavingSystem
{
    [System.Serializable]
    public struct IntWithID
    {
        public string uuid;
        public int savedInt;
    }

    [System.Serializable]
    public struct StringWithID
    {
        public string uuid;
        public string savedString;
    }
    
    [System.Serializable]
    public struct Vector3WithID
    {
        public string uuid;
        public Vector3 savedVector3;
    }
    
    [System.Serializable]
    public struct QuaternionWithID
    {
        public string uuid;
        public Quaternion savedQuaternion;
    }

    [System.Serializable]
    public struct FloatWithID
    {
        public string uuid;
        public float savedFloat;
    }
    
    [System.Serializable]
    public struct BoolWithID
    {
        public string uuid;
        public bool savedBool;
    }
    
    [System.Serializable]
    public struct Level
    {
        public int healthLevel;
        public int staminaLevel;
    }
    
    [System.Serializable]
    public struct AmmunitionInventoryData
    {
        public int bullets;
        public int heavyBullets;
    }
}