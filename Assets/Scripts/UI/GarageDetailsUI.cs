using System.Collections.Generic;
using Garage;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GarageDetailsUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text infoText;

        [Header("Buttons")]
        [SerializeField] private GameObject buyButton;
        [SerializeField] private TMP_Text buyText;
        [SerializeField] private GameObject rentButton;
        [SerializeField] private GameObject ownedText;
        [SerializeField] private GameObject rentedText;
        [SerializeField] private GameObject sellButton;
        [SerializeField] private TMP_Text sellText;
        [SerializeField] private RepairObject repairPrefab;
        [SerializeField] private Transform contentParent;
        [SerializeField] private List<RepairObject> repairObjectsList = new List<RepairObject>();
        private GarageData currentGarage;
        private GarageRuntimeData currRuntimeData;

        public void ShowGarage(GarageData garage)
        {
            currentGarage = garage;

            currRuntimeData =
                GarageManager.Instance.GetRuntimeData(garage.id);
            
            float marketValue =
                GarageValuation.GetPossibleValue(currRuntimeData.Data);

            var purchasePrice = GarageValuation.GetPurchasePrice(garage);
            
            float priceDifference =
                (marketValue - purchasePrice) / marketValue;

            GetDealRate(priceDifference);
            titleText.text = garage.garageName;
            
            infoText.text =
                $"Area: {garage.area:0} m²\n" +
                $"Condition: {currRuntimeData.CurrentCondition * 100:0}%\n" +
                $"Purchase Price: ${purchasePrice:N0}\n" +
                $"Market Value: ${marketValue:N0}\n" +
                $"Rent: ${garage.monthlyRent*garage.condition:N0}/month\n" +
                $"{GetDealRate(priceDifference)}";

            UpdateOwnershipUI(currRuntimeData);

            Debug.Log(
                $"{garage.garageName} | Purchase: ${purchasePrice:N0} | " +
                $"Market: ${marketValue:N0} | Difference: {priceDifference:P0}"
            );
            
            gameObject.SetActive(true);
        }

        private string GetDealRate(float priceDifference)
        {
            string dealRating;

            if (priceDifference >= 0.15f)
            {
                dealRating = "Deal: Good";
            }
            else if (priceDifference <= -0.15f)
            {
                dealRating = "Deal: Bad";
            }
            else
            {
                dealRating = "Deal: Fair";
            }
            return dealRating;
        }

        //update info after we complete repair
        public void UpdateSellText()
        {
            sellText.text = $"SELL : {GarageValuation.GetSellPrice(currRuntimeData, currRuntimeData.Data)}";
            var purchasePrice = GarageValuation.GetPurchasePrice(currRuntimeData.Data);
            buyText.text = $"BUY : {purchasePrice}";

            float marketValue =
                GarageValuation.GetPossibleValue(currRuntimeData.Data);
            float priceDifference =
                (marketValue - purchasePrice) / marketValue;
            infoText.text =
                $"Area: {currRuntimeData.Data.area:0} m²\n" +
                $"Condition: {currRuntimeData.CurrentCondition * 100:0}%\n" +
                /*$"Purchase Price: ${purchasePrice:N0}\n" +*/
                $"Market Value: ${marketValue:N0}\n" +
                $"Rent: ${currRuntimeData.Data.monthlyRent * currRuntimeData.CurrentCondition:N0}/month\n" +
                $"{GetDealRate(priceDifference)}";
        }

        public void BuyGarage()
        {
            if (currentGarage == null)
                return;

            bool purchased =
                GarageManager.Instance.TryBuyGarage(currentGarage.id);

            if (!purchased)
                return;

            GarageRuntimeData runtimeData =
                GarageManager.Instance.GetRuntimeData(currentGarage.id);

            UpdateOwnershipUI(runtimeData);
        }

        public void RentGarage()
        {
            if (currentGarage == null)
                return;

            bool rented =
                GarageManager.Instance.TryRentGarage(currentGarage.id);

            if (!rented)
                return;

            GarageRuntimeData runtimeData =
                GarageManager.Instance.GetRuntimeData(currentGarage.id);

            UpdateOwnershipUI(runtimeData);
        }

        private void UpdateOwnershipUI(GarageRuntimeData runtimeData)
        {
            bool isOwned = runtimeData != null && runtimeData.IsOwned;
            bool isRented = runtimeData != null && runtimeData.IsRented;

            buyButton.SetActive(!isOwned);
            rentButton.SetActive(isOwned && !isRented);
            sellButton.SetActive(isOwned);
            sellText.text = $"Sell {GarageValuation.GetSellPrice(runtimeData, runtimeData.Data)}";
            var purchasePrice = GarageValuation.GetPurchasePrice(currRuntimeData.Data);
            buyText.text = $"BUY : {purchasePrice}";
            ownedText.SetActive(isOwned);
            rentedText.SetActive(isRented);
            ShowRepairList(runtimeData, isOwned);
        }

        private void ShowRepairList(GarageRuntimeData runtimeData, bool isOwned)
        {
            foreach (RepairObject repairObject in repairObjectsList)
            {
                Destroy(repairObject.gameObject);
            }
            repairObjectsList.Clear();
            
            if(!isOwned)
                return;
            
            foreach (GarageRepairData repair in runtimeData.Data.availableRepairs)
            {
                RepairObject repairObject = Instantiate(repairPrefab, contentParent);
                repairObject.Setup(runtimeData, repair, this);
                repairObjectsList.Add(repairObject);
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
        
        public void SellGarage()
        {
            if (currentGarage == null)
                return;

            bool sold =
                GarageManager.Instance.TrySellGarage(currentGarage.id);

            if (!sold)
                return;

            GarageRuntimeData runtimeData =
                GarageManager.Instance.GetRuntimeData(currentGarage.id);

            UpdateOwnershipUI(runtimeData);
        }
    }
}