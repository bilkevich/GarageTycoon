using Garage;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GarageCard : MonoBehaviour
    {
        [Header("UI")] [SerializeField] private TMP_Text garageNameText;
        [SerializeField] private TMP_Text garageInfoText;

        private GarageData garageData;
        private GarageDetailsUI detailsUI;

        public void Setup(GarageData garage, GarageDetailsUI detailsUI)
        {
            garageData = garage;
            this.detailsUI = detailsUI;
            GarageRuntimeData runtimeData =
                GarageManager.Instance.GetRuntimeData(garage.id);
            
            garageNameText.text = garage.garageName;
            var purchasePrice = GarageValuation.GetPurchasePrice(garage);
            garageInfoText.text =
                $"Area: {garage.area:0} m²\n" +
                $"Condition: {runtimeData.CurrentCondition * 100:0}%\n" +
                $"Price: ${purchasePrice:N0}\n" +
                $"Rent: ${garage.monthlyRent:N0}/month";
        }

        public void OnViewClicked()
        {
            detailsUI.ShowGarage(garageData);
        }
    }
}