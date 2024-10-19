using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UI_Animator : MonoBehaviour
{
    Image m_Image;

    public Sprite[] sprites;
    public float m_Speed = 0.12f;

    private int m_IndexSprite;
    Coroutine m_CorotineAnim;
    bool IsDone;

    private void Start()
    {
        m_Image = GetComponent<Image>();
        Func_PlayUIAnim();
    }

    public void Func_PlayUIAnim()
    {
        IsDone = false;
        StartCoroutine(Func_PlayAnimUI());
    }

    public void Func_StopUIAnim()
    {
        IsDone = true;
        StopCoroutine(Func_PlayAnimUI());
    }
    IEnumerator Func_PlayAnimUI()
    {
        yield return new WaitForSeconds(m_Speed);
        if (m_IndexSprite >= sprites.Length)
        {
            m_IndexSprite = 0;
        }
        m_Image.sprite = sprites[m_IndexSprite];
        m_IndexSprite += 1;
        if (IsDone == false)
            m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
    }

}

