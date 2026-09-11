using UnityEngine;
using UnityEngine.Serialization;
namespace Cooperative
{
    [CreateAssetMenu(fileName = "Cooperative_", menuName = "Garage Tycoon/Cooperative Data")]
    public class CooperativeData : ScriptableObject
    {
        public string cooperativeName;
        public float repairPriceMultiplier;
        
        /*0.85 — непривабливий
        1.00 — нормальний
        1.10 — хороший
        1.25 — дуже хороший*/
        public float attractivenessMultiplier = 1.0f;
    }
}