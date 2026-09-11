using System.Collections.Generic;
using Cooperative;
using Garage;
using UnityEngine;

namespace UI
{
    public class GarageListUI : MonoBehaviour
    {
        [SerializeField] private GarageManager garageManager;
        [SerializeField] private GarageCard garageCardPrefab;
        [SerializeField] private Transform contentParent;
        [SerializeField] private GarageDetailsUI detailsUI;
        
        public void EnableObject(bool enable)
        {
            gameObject.SetActive(enable);
        }
        
        public void ClearList()
        {
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(contentParent.GetChild(i).gameObject);
            }
        }
        
        public void CreateGarageCards()
        {
            foreach (GarageData garage in garageManager.Garages)
            {
                GarageCard card = Instantiate(garageCardPrefab, contentParent);
                card.Setup(garage, detailsUI);
            }
        }
        
        public void ShowAvailableGarages(CooperativeData cooperative)
        {
            ClearList();
            foreach (GarageData garage in garageManager.Garages)
            {
                if(garage.cooperative != cooperative)
                    continue;
                
                GarageCard card = Instantiate(garageCardPrefab, contentParent);
                card.Setup(garage, detailsUI);
            }
        }
    }
}