using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class TextHandler : MonoBehaviour
{

    public TextMeshProUGUI propertyInfo;
    private string propertyName;
    [CanBeNull] private string propertyValue;
    
    public TextMeshProUGUI Roll;
    private string roll;
    
    public TextMeshProUGUI Money;
    private string money;

    public void updateTile (string propertyName)
    {
        this.propertyName = propertyName;
        propertyInfo.text = $"Current tile: {this.propertyName}";
    }
    
    public void updateProperty (string propertyName, int? propertyValue)
    {
        this.propertyName = propertyName;
        this.propertyValue = propertyValue.ToString();
        propertyInfo.text = $"Current property: {this.propertyName}\nValue: {this.propertyValue}";
    }

    public void updateRoll(int roll)
    {
        this.roll = roll.ToString();
        Roll.text = $"Rolled: {this.roll}";
    }

    public void updateMoney(int money)
    {
        this.money = money.ToString();
        Money.text = $"Money: {this.money}";
    }
    

    // Update is called once per frame
    void Update()
    {

    }
}
