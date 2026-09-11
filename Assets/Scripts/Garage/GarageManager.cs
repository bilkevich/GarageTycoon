using System.Collections.Generic;
using Garage;
using UnityEngine;

public class GarageManager : MonoBehaviour
{
    public static GarageManager Instance { get; private set; }
    
    [SerializeField] private GarageData[] garages;

    private Dictionary<int, GarageRuntimeData> runtimeGarages;

    public GarageData[] Garages => garages;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        InitializeRuntimeData();
    }
    
    
    private float repairCheckTimer;

    private void Update()
    {
        repairCheckTimer += Time.deltaTime;

        if (repairCheckTimer < 1f)
            return;

        repairCheckTimer = 0f;

        foreach (var garage in runtimeGarages.Values)
        {
            garage.UpdateRepair();
        }
    }

    private void InitializeRuntimeData()
    {
        runtimeGarages = new Dictionary<int, GarageRuntimeData>();

        foreach (GarageData garage in garages)
        {
            GarageRuntimeData runtimeData = new GarageRuntimeData(garage);
            runtimeGarages.Add(garage.id, runtimeData);
        }
    }

    public GarageRuntimeData GetRuntimeData(int garageId)
    {
        if (runtimeGarages.TryGetValue(garageId, out GarageRuntimeData runtimeData))
        {
            return runtimeData;
        }

        return null;
    }

    public bool TryBuyGarage(int garageId)
    {
        GarageRuntimeData garage = GetRuntimeData(garageId);

        if (garage == null)
        {
            Debug.LogError("Garage not found.");
            return false;
        }

        if (garage.IsOwned)
        {
            Debug.Log("Garage is already owned.");
            return false;
        }

        float price = GarageValuation.GetPurchasePrice(garage.Data);

        if (!GameManager.Instance.TrySpendMoney(price))
        {
            Debug.Log("Not enough money to buy this garage.");
            return false;
        }

        garage.SetOwned();

        Debug.Log($"Garage purchased: {garage.Data.garageName}");

        return true;
    }
    
    public bool TryRentGarage(int garageId)
    {
        GarageRuntimeData garage = GetRuntimeData(garageId);

        if (garage == null)
        {
            Debug.LogError("Garage not found.");
            return false;
        }

        if (!garage.IsOwned)
        {
            Debug.Log("Garage must be owned before renting.");
            return false;
        }

        if (garage.IsRented)
        {
            Debug.Log("Garage is already rented.");
            return false;
        }

        garage.SetRented();

        Debug.Log($"Garage rented: {garage.Data.garageName}");

        return true;
    }
    
    public void ProcessMonthlyIncome()
    {
        float totalIncome = 0f;

        foreach (GarageData garage in garages)
        {
            GarageRuntimeData runtimeData = GetRuntimeData(garage.id);

            if (runtimeData != null && runtimeData.IsRented)
            {
                float income = runtimeData.GetMonthlyIncome();
                totalIncome += income;

                Debug.Log(
                    $"Rent income: {garage.garageName} +${income:N0}"
                );
            }
        }

        if (totalIncome > 0f)
        {
            GameManager.Instance.AddMoney(totalIncome);

            Debug.Log(
                $"Total monthly rent income: +${totalIncome:N0}"
            );
        }
    }
    
    public bool TrySellGarage(int garageId)
    {
        GarageRuntimeData garage = GetRuntimeData(garageId);

        if (garage == null)
        {
            Debug.LogError("Garage not found.");
            return false;
        }

        if (!garage.IsOwned)
        {
            Debug.Log("Garage is not owned.");
            return false;
        }

        float salePrice = GarageValuation.GetSellPrice(garage, garage.Data);

        GameManager.Instance.AddMoney(salePrice);

        garage.SetSold();

        Debug.Log($"Garage sold: {garage.Data.garageName} for ${salePrice:N0}");

        return true;
    }
}
