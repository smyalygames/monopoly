using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHandler : MonoBehaviour
{

    public GameObject GameUI;
    public Button rollDice;
    public GameObject buyButton;
    public Button buyButtonButton;
    public Button nextTurn;
    public GameObject nextTurnObj;

    //Inventory
    public GameObject PropertyUI;
    public Button inventory;
    public TextMeshProUGUI inventoryText;
    private bool inventoryOpen = false;
    private Inventory inventoryClass;

    public GameObject card;
    public Button cardButton;
    
    //Trade
    public GameObject TradeUI;
    public Button tradeButton;
    public Button tradeCompletedButton;
    private bool tradeOpen = false;
    private Trade trade;

    public void DisableRollDice()
    {
        rollDice.interactable = false;
    }

    public void EnableRollDice()
    {
        rollDice.interactable = true;
    }

    public void DisableBuying()
    {
        buyButton.SetActive(false);
    }

    public void EnableBuying()
    {
        buyButton.SetActive(true);
    }

    public void EnableNextTurn()
    {
        nextTurnObj.SetActive(true);
    }

    void NextTurn()
    {
        main.board.NextPlayer();
        nextTurnObj.SetActive(false);
    }

    void OpenInventory()
    {
        inventoryClass.UpdateInventory();
        GameUI.SetActive(false);
        PropertyUI.SetActive(true);
        inventoryText.text = "Close Inventory";
    }

    void CloseInventory()
    {
        GameUI.SetActive(true);
        PropertyUI.SetActive(false);
        inventoryText.text = "Open Inventory";
    }

    void ToggleInventory()
    {
        if (!inventoryOpen)
        {
            OpenInventory();
            inventoryOpen = true;
        }
        else
        {
            CloseInventory();
            inventoryOpen = false;
        }
    }

    void CloseCard()
    {
        card.SetActive(false);
    }
    
    //Trade

    void OpenTrade()
    {
        trade.OpenTrade();
        GameUI.SetActive(false);
        TradeUI.SetActive(true);
    }

    void CloseTrade()
    {
        TradeUI.SetActive(false);
        GameUI.SetActive(true);
    }

    void ToggleTrade()
    {
        if (!tradeOpen)
        {
            OpenTrade();
            tradeOpen = true;
        }
        else
        {
            CloseTrade();
            tradeOpen = false;
        }
    }

    private Main main;

    void Awake()
    { 
        main = FindObjectOfType<Main>();
        inventoryClass = FindObjectOfType<Inventory>();
        trade = FindObjectOfType<Trade>();
    }

    void Start()
    {
        buyButtonButton.onClick.AddListener(BuyPropertyClick);
        nextTurn.onClick.AddListener(NextTurn);
        inventory.onClick.AddListener(ToggleInventory);
        cardButton.onClick.AddListener(CloseCard);
        tradeButton.onClick.AddListener(ToggleTrade);
        tradeCompletedButton.onClick.AddListener(ToggleTrade);
    }

    void BuyPropertyClick() {
        main.board.BuyProperty();
    }
}