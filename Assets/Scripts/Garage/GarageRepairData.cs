using UnityEngine;

namespace Garage
{
    [CreateAssetMenu(fileName = "Repair_", menuName = "Garage Tycoon/Repair Data")]
    public class GarageRepairData : ScriptableObject
    {
        [Header("Repair Information")]
        public string repairName;

        [Header("Economy")]
        public float cost;

        [Header("Condition")]
        [Range(0f, 1f)]
        public float conditionIncrease;
        [Tooltip("1 game day - 60 seconds")]
        public int repairingTime = 2;
    }
}