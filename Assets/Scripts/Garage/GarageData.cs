using Cooperative;
using UnityEngine;
using UnityEngine.Serialization;

namespace Garage
{
    [CreateAssetMenu(fileName = "Garage_", menuName = "Garage Tycoon/Garage Data")]
    public class GarageData : ScriptableObject
    {
        [Header("Basic Information")]
        public int id;
        public string garageName;

        [Header("Characteristics")]
        public float area;
        [Range(0f, 1f)]
        public float condition;

        [Header("Economy")]
        public float BasePrice;
        public float monthlyRent;
        
        [Header("Repairs")]
        public GarageRepairData[] availableRepairs;
        
        public CooperativeData cooperative;
    }
}