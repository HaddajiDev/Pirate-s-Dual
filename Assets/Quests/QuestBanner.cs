using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class QuestBanner : MonoBehaviour
{
    public int index;
    public QuestData questData;

    public TMP_Text Descrption;
    public TMP_Text diammonds;
    public TMP_Text coins;
    public TMP_Text Counter;

    public Button ClaimButton;

    public Slider slider;
    public Button infoButton;
    

    void Start()
    {
        UpdateStats();        
    }

    public void UpdateStats()
    {
        Quest quest = questData.Get_Quest(index);
        slider.maxValue = quest.Goal_Value;
        QuestSpawner.instance.QuestCompleted = 0;
        if (quest.type == Quest.Type.wins)
        {
            slider.value = GameManager.Instance.WinCount;
            Counter.text = $"{GameManager.Instance.WinCount} / {quest.Goal_Value}";
            if (GameManager.Instance.WinCount >= quest.Goal_Value)
            {
                ClaimButton.onClick.RemoveAllListeners();
                ClaimButton.onClick.AddListener(() => ClaimReward(quest.cost));
                ClaimButton.interactable = true;
                ClaimButton.transform.DOScale(new Vector3(1.08f, 1.08f, 1), 0.7f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
                QuestSpawner.instance.QuestCompleted += 1;
            }                
        }
        else if (quest.type == Quest.Type.fireShots)
        {
            slider.value = GameManager.Instance.FireShots;
            Counter.text = $"{GameManager.Instance.FireShots} / {quest.Goal_Value}";
            if (GameManager.Instance.FireShots >= quest.Goal_Value)
            {
                ClaimButton.onClick.RemoveAllListeners();
                ClaimButton.onClick.AddListener(() => ClaimReward(quest.cost));
                ClaimButton.interactable = true;
                ClaimButton.transform.DOScale(new Vector3(1.08f, 1.08f, 1), 0.7f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
                QuestSpawner.instance.QuestCompleted += 1;
            }
            infoButton.gameObject.SetActive(true);
            infoButton.onClick.RemoveAllListeners();
            infoButton.onClick.AddListener(() => UI_Controller.instance.Show_SelectPowerUps_Controller(1));
        }
        else if (quest.type == Quest.Type.noMissShots)
        {
            slider.value = GameManager.Instance.noMissShots;
            Counter.text = $"{GameManager.Instance.noMissShots} / {quest.Goal_Value}";
            if (GameManager.Instance.noMissShots >= quest.Goal_Value)
            {
                ClaimButton.onClick.RemoveAllListeners();
                ClaimButton.onClick.AddListener(() => ClaimReward(quest.cost));
                ClaimButton.interactable = true;
                ClaimButton.transform.DOScale(new Vector3(1.08f, 1.08f, 1), 0.7f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
                QuestSpawner.instance.QuestCompleted += 1;
            }
        }
        Descrption.text = quest.Descrption;
        diammonds.text = quest.cost.Diamond.ToString();
        coins.text = quest.cost.Coins.ToString();

        
        if(QuestSpawner.instance.QuestCompleted > 0)
        {
            UI_Controller.instance.QuestNotification.SetActive(true);
            TMP_Text count = UI_Controller.instance.QuestNotification.GetComponentInChildren<TMP_Text>();
            count.text = $"{QuestSpawner.instance.QuestCompleted}";
            Image circle = UI_Controller.instance.QuestNotification.GetComponentInChildren<Image>();
            circle.color = Color.green;
            count.color = Color.black;            
        }
    }


    private void ClaimReward(Cost cost)
    {
        ClaimButton.interactable = false;
        GameManager.Instance.Coins += cost.Coins;
        GameManager.Instance.Diamond += cost.Diamond;
        UI_Controller.instance.SetCurrencyUI();
        GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
        GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);

        GameManager.Instance.currentQuests.Remove(index);
        GameManager.Instance.SetList("current_Quests", GameManager.Instance.currentQuests);
        
        TMPro.TMP_Text count = UI_Controller.instance.QuestNotification.GetComponentInChildren<TMPro.TMP_Text>();
        count.text = $"{GameManager.Instance.currentQuests.Count}";

        gameObject.transform.DOScale(1.1f, 0.2f).OnComplete(() =>
        {
            gameObject.transform.DOScale(0, 0.3f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
        if (GameManager.Instance.currentQuests.Count == 0)
        {
            QuestSpawner.instance.NoQuests.SetActive(true);
            UI_Controller.instance.QuestNotification.SetActive(false);
        }
    }
}
