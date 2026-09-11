using System;
using Garage;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class RepairObject : MonoBehaviour
    {
        [Header("UI")] [SerializeField] private TMP_Text repairText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Button repairButton;
        private GarageDetailsUI detailsUI;
        
        GarageRepairData repairData;
        private GarageRuntimeData currRuntimeData;
        public void Setup(GarageRuntimeData runtimeData, GarageRepairData repairData,GarageDetailsUI detailsUI)
        {
            currRuntimeData = runtimeData;
            this.detailsUI = detailsUI;
            this.repairData = repairData;

            repairText.text = repairData.repairName;
            priceText.text = $"Price: {repairData.cost}";
            
            currRuntimeData.OnRepairCompleted -= HandleRepairCompleted;

            // Repair already completed
            if (runtimeData.IsRepairCompleted(repairData))
            {
                repairButton.gameObject.SetActive(false);
                timerText.gameObject.SetActive(false);
                return;
            }

            timerText.gameObject.SetActive(true);

            // Repair is currently running
            if (runtimeData.IsRepairing &&
                runtimeData.CurrentRepair.repairName == repairData.repairName)
            {
                repairButton.gameObject.SetActive(false);

                SubscribeToRepairCompleted();
            }
            else
            {
                repairButton.gameObject.SetActive(true);

                repairButton.onClick.RemoveAllListeners();
                repairButton.onClick.AddListener(() => OnRepairClicked(runtimeData));

                InitRepairTimer(repairData);
            }
            
        }

        private void OnRepairClicked(GarageRuntimeData runtimeData)
        {
            if (!runtimeData.TryStartRepair(repairData))
                return;
                
            repairButton.gameObject.SetActive(false);
            
            SubscribeToRepairCompleted();
        }

        private void HandleRepairCompleted()
        {
            currRuntimeData.OnRepairCompleted -= HandleRepairCompleted;
            timerText.gameObject.SetActive(false);
            detailsUI.UpdateSellText();
        }

        private void Update()
        {
            if (currRuntimeData == null ||
                !currRuntimeData.IsRepairing || 
                !currRuntimeData.CurrentRepair.repairName.Equals(repairData.repairName))
                
                return;
            
            UpdateRepairTimer(currRuntimeData);
        }
        
        public void UpdateRepairTimer(GarageRuntimeData garage)
        {
            int secondsRemaining = garage.GetRepairTimeRemaining();
            //Debug.LogError($"{secondsRemaining}");
            int days = secondsRemaining / 60;
            int remainingSeconds = secondsRemaining % 60;

            int hours = remainingSeconds * 24 / 60;
            int minutes = remainingSeconds * 24 * 60 / 60 % 60;

            timerText.text = $"{days:00}d {hours:00}h {minutes:00}m";
        }
        
        public void InitRepairTimer(GarageRepairData garage)
        {
            int secondsRemaining = garage.repairingTime;

            int days = secondsRemaining / 60;
            int remainingSeconds = secondsRemaining % 60;

            int hours = remainingSeconds * 24 / 60;
            int minutes = remainingSeconds * 24 * 60 / 60 % 60;

            timerText.text = $"{days:00}d {hours:00}h {minutes:00}m";
        }
        
        private void SubscribeToRepairCompleted()
        {
            if (currRuntimeData == null)
                return;

            currRuntimeData.OnRepairCompleted -= HandleRepairCompleted;
            currRuntimeData.OnRepairCompleted += HandleRepairCompleted;
        }
        
        //unsubscribe from event when object is disable
        private void OnDisable()
        {
            if (currRuntimeData != null)
            {
                currRuntimeData.OnRepairCompleted -= HandleRepairCompleted;
            }
        }
    }
}