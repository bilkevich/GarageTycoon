using Garage;
using TMPro;
using UnityEngine;

namespace UI
{
    public class OwnedGaragesUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text ownedGaragesText;

        private void Update()
        {
            if (GarageManager.Instance == null)
                return;

            int ownedCount = 0;

            foreach (GarageData garage in GarageManager.Instance.Garages)
            {
                GarageRuntimeData runtimeData =
                    GarageManager.Instance.GetRuntimeData(garage.id);

                if (runtimeData != null && runtimeData.IsOwned)
                {
                    ownedCount++;
                }
            }

            ownedGaragesText.text = $"Owned Garages: {ownedCount}";
        }
    }
}