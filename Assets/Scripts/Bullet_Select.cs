using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet_Select : MonoBehaviour
{
    public Bullets currentBullet;
    public int index;
    public GameObject Check;

    public void Get_Bullet_Stuff()
    {
        TMPro.TMP_Text Name = GetComponentInChildren<TMPro.TMP_Text>();
        Image img = GetComponentsInChildren<Image>()[1];
        Name.text = currentBullet.Name;
        img.sprite = currentBullet.sr;
        GetComponentsInChildren<Image>()[3].GetComponentInChildren<TMPro.TMP_Text>().text = currentBullet.Limit.ToString();
    }

    public void CheckForSelcted()
    {
        UI_Controller.instance.ResetChecks();
        if (index == GameManager.Instance.player_1._selectedBullet)
            Check.SetActive(true);        
    }
}
