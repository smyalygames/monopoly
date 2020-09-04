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

    private Main main;

    void Awake()
    { 
        main = FindObjectOfType<Main>();
        inventoryClass = FindObjectOfType<Inventory>();
    }

    void Start()
    {
        buyButtonButton.onClick.AddListener(BuyPropertyClick);
        nextTurn.onClick.AddListener(NextTurn);
        inventory.onClick.AddListener(ToggleInventory);
        cardButton.onClick.AddListener(CloseCard);
    }

    void BuyPropertyClick() {
        main.board.BuyProperty();
    }
}