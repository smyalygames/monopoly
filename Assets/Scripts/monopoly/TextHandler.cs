using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{

    public TextMeshProUGUI propertyInfo;
    private string propertyName;
    [CanBeNull] private string propertyValue;

    public TextMeshProUGUI Roll;
    private string roll;
    
    public TextMeshProUGUI Money;
    private string money;
    
    public GameObject card;
    public TextMeshProUGUI cardGroup;
    public TextMeshProUGUI cardText;

    public void UpdateTile (string propertyName)
    {
        this.propertyName = propertyName;
        propertyInfo.text = $"Current tile: {this.propertyName}";
    }
    
    public void UpdateProperty (string propertyName, int? propertyValue)
    {
        this.propertyName = propertyName;
        this.propertyValue = propertyValue.ToString();
        propertyInfo.text = $"Current property: {this.propertyName}\nValue: {this.propertyValue}";
    }

    public void UpdateRoll(int roll)
    {
        this.roll = roll.ToString();
        Roll.text = $"Rolled: {this.roll}";
    }

    public void UpdateMoney(int money)
    {
        this.money = money.ToString();
        Money.text = $"Money: {this.money}";
    }

    public void ShowCard(int group, string text)
    {
        switch (group)
        {
            case 0:
                cardGroup.text = "Chance";
                break;
            case 1:
                cardGroup.text = "Community Chest";
                break;
            default:
                cardGroup.text = "";
                break;
        }

        cardText.text = text.Replace("\\n", "\n");; //Changes \n to machine readable new line

        card.SetActive(true);
    }

}
