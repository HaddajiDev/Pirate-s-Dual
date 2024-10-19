using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftsSpawner : MonoBehaviour
{
    public static GiftsSpawner instance;  

    public GameObject GiftObject;
    public GameObject noGifts;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < GameManager.Instance.currentGifts.Count; i++)
        {
            GameObject gift = Instantiate(GiftObject, transform);
            GiftBanner banner = gift.GetComponent<GiftBanner>();
            banner.index = GameManager.Instance.currentGifts[i];
        }
        if (GameManager.Instance.currentGifts.Count == 0)
        {
            noGifts.SetActive(true);
            UI_Controller.instance.GiftsNotification.SetActive(false);
        }
        else
        {
            UI_Controller.instance.GiftsNotification.SetActive(true);
            TMPro.TMP_Text count = UI_Controller.instance.GiftsNotification.GetComponentInChildren<TMPro.TMP_Text>();
            count.text = $"{GameManager.Instance.currentGifts.Count}";
        }
    }
}
