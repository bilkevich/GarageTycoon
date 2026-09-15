using Garage;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player")]
    [SerializeField] private float startingMoney = 20000f;

    [SerializeField] private GameObject transactionPanel;
    [SerializeField] private UserInput userInput;
    
    public UserInput UserInput => userInput;
    public float Money { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Money = startingMoney;
        //UserInput.
    }

    public bool TrySpendMoney(float amount)
    {
        if (amount <= 0)
            return false;

        if (Money < amount)
            return false;

        Money -= amount;
        return true;
    }

    public void AddMoney(float amount)
    {
        if (amount <= 0)
            return;

        Money += amount;
    }

    private void OnShowTransactionHistory(InputAction.CallbackContext context)
    {
        Debug.LogError("OnShowTransactionHistory!!!!!!!!");
        transactionPanel.SetActive(transactionPanel.activeInHierarchy);
    }
}
