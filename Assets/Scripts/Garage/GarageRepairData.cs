using UnityEngine;
using UnityEngine.Serialization;

namespace Garage
{
    [CreateAssetMenu(fileName = "Repair_", menuName = "Garage Tycoon/Repair Data")]
    public class GarageRepairData : ScriptableObject
    {
        [Header("Repair Information")]
        public string repairName;

        [FormerlySerializedAs("cost")] [Header("Economy")]
        public float baseCost;
        public float costPerSquareMeter;
        
        [Header("Condition")]
        [Range(0f, 1f)]
        public float conditionIncrease;
        [Tooltip("1 game day - 60 seconds")]
        public int repairingTime = 2;
    }
}