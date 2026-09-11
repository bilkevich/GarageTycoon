using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Cooperative
{
    public class CooperativeMapObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private CooperativeData cooperativeData;
        [SerializeField] private Button button;
        [SerializeField] private GarageListUI garageListUI;
        //[SerializeField] private MapUI mapUI;

        private void Awake()
        {
            button.onClick.AddListener(OnClicked);
            FillInfo();
        }

        private void FillInfo()
        {
            nameText.text = cooperativeData.cooperativeName;
            infoText.text = $" attractiveness {cooperativeData.attractivenessMultiplier}";
        }

        private void OnClicked()
        {
            //Debug.LogError($"on clicked {cooperativeData.cooperativeName}");
            //mapUI.OpenCooperative(cooperativeData);
            garageListUI.EnableObject(true);
            garageListUI.ShowAvailableGarages(cooperativeData);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClicked);
        }
    }
}