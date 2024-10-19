using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    //Max : 7
    public int lvl_health = 1; //save
    public Image[] levels_health;
    public GameObject cost_health;
    public GameObject Max_health;
    private int Health_Level
    {
        get { return lvl_health; }
        set { lvl_health = value; }
    }

    //Max : 10
    public int lvl_force = 1; //save
    public Image[] levels_force;
    public GameObject cost_force;
    public GameObject Max_force;
    private int Force_Level
    {
        get { return lvl_force; }
        set { lvl_force = value; }
    }

    private void Start()
    {
        Load_Data();
        UpdateUI_Force();
        UpdateUI_Health();
    }
    public void Load_Data()
    {
        addLevels(levels_force, lvl_force - 1);
        addLevels(levels_health, lvl_health - 1);
    }

    public void Upgrade_Health()
    {
        Cost cost = new Cost(2250 * lvl_health, 25 + (lvl_health * 2));
        if (GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            if(Health_Level <= 7)
            {
                Ship ship = GameManager.Instance.player_1.GetComponentInParent<Ship>();
                ship.Health += 10;
                addLevels(levels_health, Health_Level);
                GameManager.Instance.Coins -= cost.Coins;
                GameManager.Instance.Diamond -= cost.Diamond;
                Health_Level++;                
                UpdateUI_Health();
                UI_Controller.instance.SetCurrencyUI();
                GameManager.Instance.SaveData("levelHealth", Health_Level);
                GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
                GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);
                GameManager.Instance.SaveData("health", ship.Health);

                //audio
                GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.Buy);
            }
        }
        else
        {
            UI_Controller.instance.FeedBackPopUp("Not enough currency", UI_Controller.FeedbackType.failed);
        }
    }

    public void Upgrade_Force()
    {
        Cost cost = new Cost(2500 * lvl_force, 30 + (lvl_force * 2));
        if (GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            if(Force_Level <= 7)
            {
                GameManager.Instance.player_1.maxForce += 3;
                addLevels(levels_force, Force_Level);
                GameManager.Instance.Coins -= cost.Coins;
                GameManager.Instance.Diamond -= cost.Diamond;
                Force_Level++;
                UpdateUI_Force();
                UI_Controller.instance.SetCurrencyUI();
                GameManager.Instance.SaveData("levelForce", Force_Level);
                GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
                GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);
                GameManager.Instance.SaveData("force", GameManager.Instance.player_1.maxForce);

                //audio
                GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.Buy);
            }            
        }
        else
        {
            UI_Controller.instance.FeedBackPopUp("Not enough currency", UI_Controller.FeedbackType.failed);
        }
    }
    
    private void addLevels(Image[] imgs, int index)
    {
        for (int i = 0; i < index; i++)
        {
            var col = imgs[i].color;
            col.a = 255;
            imgs[i].color = col;
            if (i == index - 1)
                return;
        }
    }

    private void UpdateUI_Force()
    {
        Cost cost = new Cost(2500 * lvl_force, 30 + (lvl_force * 2));
        UI_Controller.instance.Force_Cost_Upgrade_Coins.text = cost.Coins.ToString();
        UI_Controller.instance.Force_Cost_Upgrade__Diamond.text = cost.Diamond.ToString();
        if (Force_Level == 8)
        {
            cost_force.SetActive(false);
            Max_force.SetActive(true);
        }
    }
    private void UpdateUI_Health()
    {
        Cost cost = new Cost(2250 * lvl_health, 25 + (lvl_health * 2));
        UI_Controller.instance.Health_Cost_Upgrade_Coins.text = cost.Coins.ToString();
        UI_Controller.instance.Health_Cost_Upgrade_Diamond.text = cost.Diamond.ToString();
        if (Health_Level == 8)
        {
            cost_health.SetActive(false);
            Max_health.SetActive(true);
        }
    }
}


