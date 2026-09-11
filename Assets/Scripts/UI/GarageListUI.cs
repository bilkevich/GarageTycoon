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

        private void Start()
        {
            CreateGarageCards();
        }

        private void CreateGarageCards()
        {
            foreach (GarageData garage in garageManager.Garages)
            {
                GarageCard card = Instantiate(garageCardPrefab, contentParent);
                card.Setup(garage, detailsUI);
            }
        }
    }
}