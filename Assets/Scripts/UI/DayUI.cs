using Core;
using TMPro;
using UnityEngine;

public class DayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;

    private void Update()
    {
        if (TimeManager.Instance == null)
            return;

        dayText.text =
            $"Month: {TimeManager.Instance.CurrentMonth}\n" +
            $"Day: {TimeManager.Instance.CurrentDay}";
    }
}