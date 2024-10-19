using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Check_Bought_Bullet : MonoBehaviour
{
    Button _Mybutton;
    public int index;
    public TMPro.TMP_Text Coins_Text;
    public TMPro.TMP_Text Diamond_Text;

    public GameObject CostObject;
    public GameObject OwnedObject;
    public GameObject Ad_Object;

    public TMPro.TMP_Text BulletName;
    public Image BulletSprite;

    public GameObject infos;

    void Start()
    {
        _Mybutton = GetComponent<Button>();
        _Mybutton.onClick.AddListener(() => Shop.Instance.Buy_Bullet(index));
        _Mybutton.onClick.AddListener(() => SetOwned());
        Bullets bullet = GameManager.Instance.player_1.bulletsData.Get_Bullet(index);

        BulletName.text = bullet.Name;
        BulletSprite.sprite = bullet.sr;

        infos.GetComponentInChildren<TMPro.TMP_Text>().text = $"Damage : {bullet.Damage} \n Usage per game : {bullet.Limit}";

        if (GameManager.Instance.shop.bullets.data.Contains(index))
        {
            _Mybutton.interactable = false;
            OwnedObject.SetActive(true);
            CostObject.SetActive(false);            
        }
        else if (bullet.ad == false)
        {
            CostObject.SetActive(true);            
            Coins_Text.text = bullet.cost.Coins.ToString();
            Diamond_Text.text = bullet.cost.Diamond.ToString();
        }
        else if (bullet.ad == true)
        {
            OwnedObject.SetActive(false);
            CostObject.SetActive(false);
            Ad_Object.SetActive(true);
            _Mybutton.onClick.RemoveAllListeners();
            _Mybutton.onClick.AddListener(() => GetFreeSkin());
        }
    }

    private bool isFlipped = false;

    public void SetOwned()
    {
        if (GameManager.Instance.shop.bullets.data.Contains(index))
        {
            _Mybutton.interactable = false;
            OwnedObject.SetActive(true);
            CostObject.SetActive(false);
        }
    }

    public void flip(Transform img)
    {
        float targetRotation = isFlipped ? 0f : 180f;
        img.DORotate(new Vector3(0, 0, targetRotation), 0.3f);
        isFlipped = !isFlipped;
    }

    public void setInfo()
    {
        if (infos.activeInHierarchy)
            infos.SetActive(false);
        else
            infos.SetActive(true);
    }

    public void GetFreeSkin()
    {
        Time.timeScale = 1;
        GameManager.Instance.MusicSource.Play();
        GameManager.Instance.OceanBackGround.Play();
        UI_Controller.instance.FeedBackPopUp("Arrr, matey! Ye’ve unlocked a new cannon shot for yer arsenal!", UI_Controller.FeedbackType.succes);


        Shop.Instance.bullets.Add_Bullet(index);
        GameManager.Instance.SaveData("bullets", Shop.Instance.bullets.data);
        Ad_Object.SetActive(false);
        _Mybutton.interactable = false;
        OwnedObject.SetActive(true);
    }
}
