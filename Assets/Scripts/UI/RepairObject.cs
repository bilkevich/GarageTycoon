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
            repairText.text = repairData.repairName;
            priceText.text = $"Price: {repairData.cost}";
            this.repairData = repairData;

            //if repair is already completed disable button
            if (runtimeData.IsRepairCompleted(repairData))
            {
                repairButton.gameObject.SetActive(false);
                timerText.gameObject.SetActive(false);
                return;
            }
            InitRepairTimer(repairData);
            
            repairButton.onClick.RemoveAllListeners();
            repairButton.onClick.AddListener(()=> OnRepairClicked(runtimeData));
        }

        //unsubscribe from event when object is disable
        private void OnDisable()
        {
            currRuntimeData.OnRepairCompleted -= HandleRepairCompleted;
        }

        private void OnRepairClicked(GarageRuntimeData runtimeData)
        {
            if (!runtimeData.TryStartRepair(repairData))
                return;
                
            repairButton.gameObject.SetActive(false);
            currRuntimeData = runtimeData;
            runtimeData.OnRepairCompleted += HandleRepairCompleted;
        }

        private void HandleRepairCompleted()
        {
            currRuntimeData.OnRepairCompleted -= HandleRepairCompleted;
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

            int minutes = secondsRemaining / 60;
            int seconds = secondsRemaining % 60;

            timerText.text = $"{minutes:00}:{seconds:00}";
        }
        
        public void InitRepairTimer(GarageRepairData garage)
        {
            int secondsRemaining = garage.repairingTime;

            int minutes = secondsRemaining / 60;
            int seconds = secondsRemaining % 60;

            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}