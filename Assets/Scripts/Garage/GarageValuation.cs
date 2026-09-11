using UnityEngine;

namespace Garage
{
    public class GarageValuation
    {
        public static float GetSellPrice(GarageRuntimeData runtimeData, GarageData garage)
        {
            return garage.BasePrice *
                   (0.5f + runtimeData.CurrentCondition * .5f) *
                   garage.cooperative.attractivenessMultiplier *
                   GetSizeMultiplayer(garage)+
                   runtimeData.RepairedPrice * garage.cooperative.repairPriceMultiplier;
        }
        public static float GetPurchasePrice(GarageData garage)
        {
            GarageRuntimeData runtimeData =
                GarageManager.Instance.GetRuntimeData(garage.id);
            
            return garage.BasePrice *
                   (.5f + runtimeData.CurrentCondition * .5f) *
                   garage.cooperative.attractivenessMultiplier *
                   GetSizeMultiplayer(garage);
        }
        //the price what we get when garage has perfect conditions
        public static float GetPossibleValue(GarageData garage)
        {
            return garage.BasePrice *
                   garage.cooperative.attractivenessMultiplier *
                   GetSizeMultiplayer(garage);
        }
    
        /*Small       0.75 <=10m
            Medium      1.00   <=20m
            Large       1.30   <=30m
            Extra Large 1.60   >30m*/
        private static float GetSizeMultiplayer(GarageData garage)
        {
            if(garage.area <= 10) return .75f;
            if(garage.area > 10 && garage.area <= 20) return 1f;
            if(garage.area > 20 && garage.area <= 30) return 1.3f;
            return 1.6f;
        }
    }
}