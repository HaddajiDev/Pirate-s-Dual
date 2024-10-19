using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class QuestPopUp : MonoBehaviour
{
    public int index;
    public QuestData questsData;

    public TMP_Text Descirption;
    public TMP_Text Counter;
    public Slider slider;
    CanvasGroup group;
    public Type type;
    void Start()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
        
        group.DOFade(1, 0.3f);

        Quest quest = questsData.Get_Quest(index);
        Descirption.text = quest.Descrption;
        slider.maxValue = quest.Goal_Value;
        if (type == Type.wins)
        {
            Counter.text = $"{GameManager.Instance.WinCount} / {quest.Goal_Value}";
            slider.value = GameManager.Instance.WinCount;
        }
        else if(type == Type.fireShots)
        {
            Counter.text = $"{GameManager.Instance.FireShots} / {quest.Goal_Value}";
            slider.value = GameManager.Instance.FireShots;
        }
        else if (type == Type.noMissShots)
        {
            Counter.text = $"{GameManager.Instance.noMissShots} / {quest.Goal_Value}";
            slider.value = GameManager.Instance.noMissShots;
        }

        Invoke("Destroy_Pop", 2f);
    }

    void Destroy_Pop()
    {
        group.DOFade(0, 0.3f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
    
    public enum Type
    {
        wins,
        fireShots,
        noMissShots,
    }
}
