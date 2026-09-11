using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garage
{
    public class GarageRuntimeData
    {
        public GarageData Data { get; private set; }

        public bool IsOwned { get; private set; }
        public bool IsRented { get; private set; }

        public float CurrentCondition { get; private set; }
        private List<GarageRepairData> completedRepairs;
        public float RepairedPrice { get; private set; }

        public bool IsRepairing;
        public GarageRepairData CurrentRepair;
        public long RepairEndTime;
        
        public event Action OnRepairCompleted;

        public GarageRuntimeData(GarageData data)
        {
            Data = data;
            IsOwned = false;
            IsRented = false;
            CurrentCondition = data.condition;
            completedRepairs = new List<GarageRepairData>();
            RepairedPrice = 0f;
        }

        public void SetOwned()
        {
            IsOwned = true;
        }

        public void SetRented()
        {
            if (!IsOwned)
                return;

            IsRented = true;
        }

        public void SetSold()
        {
            IsOwned = false;
            IsRented = false;
            RepairedPrice = 0f;
        }

        public bool IsRepairCompleted(GarageRepairData repair)
        {
            return completedRepairs.Contains(repair);
        }

        public float GetMonthlyIncome()
        {
            if (!IsRented)
                return 0f;

            return Data.monthlyRent;
        }

        public bool TryStartRepair(GarageRepairData repair)
        {
            if (repair == null)
                return false;

            if (IsRepairCompleted(repair))
                return false;

            if (IsRepairing)
                return false;

            if (!GameManager.Instance.TrySpendMoney(repair.cost))
                return false;

            IsRepairing = true;
            CurrentRepair = repair;

            RepairEndTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                            + (long)repair.repairingTime;

            return true;
        }

        public void UpdateRepair()
        {
            if (!IsRepairing)
                return;
            
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (currentTime < RepairEndTime)
                return;

            CompleteRepair();
        }
        
        public int GetRepairTimeRemaining()
        {
            if (!IsRepairing)
                return 0;

            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return Mathf.Max(0, (int)(RepairEndTime - currentTime));
        }

        private void CompleteRepair()
        {
            completedRepairs.Add(CurrentRepair);

            RepairedPrice += CurrentRepair.cost;
            
            CurrentCondition += CurrentRepair.conditionIncrease;
            CurrentCondition = Mathf.Clamp01(CurrentCondition);

            CurrentRepair = null;
            IsRepairing = false;
            RepairEndTime = 0;
            OnRepairCompleted?.Invoke();
        }
    }
}