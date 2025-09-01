using System;
using System.Linq;
using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static BoosterDataSO;

public class BoosterManager : MonoBehaviour
{
    [SerializeField] private BoosterDataSO boosterData; // ������ �� ScriptableObject
    private PopupOpener popupOpener;
    public Popup popupHint;
    public TextMeshProUGUI text;

    private bool hintUsed = false;
    public BoosterUi boosterUi;

    // ������ �� UI-�������� (������� ���������� ������ � ������)
    [System.Serializable]
    public struct BoosterUI
    {
        public BoosterDataSO.BoosterType type;
        public Toggle button; // ������ �������
        public TextMeshProUGUI costText; // ��������� ���� ��� ���������
    }

    [SerializeField] private BoosterUI[] boosterUIs; // ������ UI-��������� ��� ��������

    private void Start()
    {
        popupOpener = GetComponent<PopupOpener>();
        


    }

   

    // ��������� ������ � ������� �� ScriptableObject
    private BoosterDataSO.BoosterInfo GetBoosterInfo(BoosterDataSO.BoosterType type)
    {
        foreach (var booster in boosterData.boosters)
        {
            if (booster.type == type)
                return booster;
        }
        return new BoosterDataSO.BoosterInfo(); // ���������� ������ ���������, ���� �� �������
    }

    // ���������� UI ��� ���� ��������
    public void UpdateBoosterUI()
    {
        foreach (var boosterUI in boosterUIs)
        {
           
            // ������� ������ � ������� �� ����
            var boosterInfo = GetBoosterInfo(boosterUI.type);

            // ��������� ����� ���������
            boosterUI.costText.text = boosterInfo.cost.ToString();
            // ����������� ������
            //boosterUI.button.interactable = playerCoins >= boosterInfo.cost;
            // ����������� ������� �����
            boosterUI.button.onValueChanged.RemoveAllListeners();
            boosterUI.button.onValueChanged.AddListener((a) => ActivateBooster(boosterUI.type));

            if (boosterUI.type == BoosterType.Hint)
            {
                Data.OnChangeCurrentIndexLvl += OnChangeCurrentIndexLvl;
            }

        }
    }

    

    // ��������� �������
    private void ActivateBooster(BoosterDataSO.BoosterType type)
    {
        var boosterInfo = GetBoosterInfo(type);
        //if (playerCoins < boosterInfo.cost)
        //{
        //    Debug.Log("������������ �����!");
        //    return;
        //}
        Debug.Log("BoosterType.Loopa 0");
        //playerCoins -= boosterInfo.cost;
        switch (type)
        {
            case BoosterDataSO.BoosterType.Loopa:

                if (GameManager.Instance.data.Coins >= boosterInfo.cost)
                {
                    GameManager.Instance.data.Coins -= boosterInfo.cost;
                    Debug.Log("BoosterType.Loopa");
                }
                else
                {
                    Debug.Log("qqqqqqqqqqqqq");
                    popupOpener.OpenPopup();
                    //����� � ������� ������������ �� ��� ������ ������� ������ + ������ ���������� �������!
                    return;
                }
                
                
                RevealLetter();
                break;
            case BoosterDataSO.BoosterType.Hint:
                if (GameManager.Instance.data.Coins >= boosterInfo.cost)
                {
                    GameManager.Instance.data.Coins -= boosterInfo.cost;
                    Debug.Log("BoosterType.Loopa");
                }
                else
                {
                    Debug.Log("qqqqqqqqqqqqq");
                    popupOpener.OpenPopup();
                    //����� � ������� ������������ �� ��� ������ ������� ������ + ������ ���������� �������!
                    return;
                }

                HintLvl();
                break;
                //case BoosterDataSO.BoosterType.RemoveLetters:
                //    //RemoveWrongLetters();
                //    break;
        }

        // ��������� UI ����� ������������� �������
        UpdateBoosterUI();
    }

    private void RevealLetter()
    {
        
        var unswerPrefabs = GameManager.Instance.uiManager.unswerPrefabs;
        string answer = GameManager.Instance.uiManager.answer;
        string[] answers = answer.Select(c => c.ToString()).ToArray();
      
        var index = 0;
        while(unswerPrefabs[index].GetComponent<UnswerUI>().LetterText.text != "" 
            && index < answers.Length - 1)
        {
            index++;
        }
       
        GameManager.Instance.uiManager.currentLetterIndex = index;
        GameManager.Instance.uiManager.currentAnswer[index] = answers[index];
        unswerPrefabs[index].GetComponent<UnswerUI>().LetterText.text = answers[index];

        GameManager.Instance.uiManager.�heckWin();

    }

    private void HintLvl()
    {
        if (!hintUsed)
        {
            Debug.Log(GameManager.Instance.data.CurrentIndexLvl + " GameManager.Instance.data.CurrentIndexLvl");
            text.text = boosterData.hints[GameManager.Instance.data.CurrentIndexLvl];
            popupOpener.ShowPopup(popupHint);
            hintUsed = true;
            boosterUi.blockUsed.SetActive(true);
            boosterUi.togle.interactable = false;
        }

    }
    private void OnChangeCurrentIndexLvl()
    {
        hintUsed = false;
        boosterUi.blockUsed.SetActive(false);
        boosterUi.togle.interactable = true;
        popupOpener.HidePopup(popupHint);
    }
}
