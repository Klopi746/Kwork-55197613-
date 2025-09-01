using Ricimi;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using YG;

public class LevelManager: MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    public List<LevelDataSO> levels; // Список всех уровней
    public PopupOpener popupOpener;
    private int currentIndexLvl = 0;

    private void Start()
    {
        popupOpener.GetComponent<PopupOpener>();
        currentIndexLvl = YG2.saves.currentIndexLvl;
    }
    public void LoadLevel(int index)
    {
        if (index >= 0 && index < levels.Count)
        {
            LevelDataSO level = levels[index];
            uiManager.InitUiLvl(level);
           
        }  else
        {
            Debug.Log("TUTAzzzz");
            GameManager.Instance.data.CurrentIndexLvl = 0;
            LevelDataSO level = levels[GameManager.Instance.data.CurrentIndexLvl];
            uiManager.InitUiLvl(level);
        }
    }

    public void NextLvl()
    {

        
        if (GameManager.Instance.data.CurrentIndexLvl >= 0 && 
            GameManager.Instance.data.CurrentIndexLvl < levels.Count - 1)
        {
            GameManager.Instance.data.CurrentIndexLvl++;
            LevelDataSO level = levels[GameManager.Instance.data.CurrentIndexLvl];
            uiManager.InitUiLvl(level);
        }
        //else if (GameManager.Instance.data.CurrentIndexLvl == levels.Count - 1)
        //{
        //    LevelDataSO level = levels[GameManager.Instance.data.CurrentIndexLvl];
        //    uiManager.InitUiLvl(level);
        //}
        else
        {

            GameManager.Instance.data.CurrentIndexLvl = 0;
            LoadLevel(GameManager.Instance.data.CurrentIndexLvl);
            return;
        }

        if (GameManager.Instance.data.CurrentIndexLvl % 10 == 0 
            &&  YG2.reviewCanShow)
        {
            popupOpener.ShowPopup();
        }

        //GameManager.Instance.data.CurrentIndexLvl++;
    }
}
