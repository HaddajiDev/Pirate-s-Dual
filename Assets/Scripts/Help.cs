using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    public string tut;
    public Sprite tutSprite;

    public Image helpImg;
    public TMPro.TMP_Text helpTxt;
    
    public void HelpMe()
    {
        UI_Controller.instance.Show_Help_Controller(1);
        helpTxt.text = tut;
        helpImg.sprite = tutSprite;
    }
}
