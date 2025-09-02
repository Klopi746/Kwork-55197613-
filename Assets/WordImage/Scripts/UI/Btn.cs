using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Btn : MonoBehaviour
{
    public Button btn;
    private void Start()
    {
        btn.onClick.AddListener(clickBtn);
        if (YG2.saves.NOTads)
        {

        }
    }

    public void clickBtn()
    {
        GameManager.Instance.levelManager.NextLvl();
    }

    public void AdvShowNextLvl()
    {
      
        if(YG2.saves.NOTads) return;
        YG2.InterstitialAdvShow();
    }
}
