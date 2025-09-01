using System;
using UnityEngine;
using YG;


public class Data : MonoBehaviour
{
    public int currentIndexLvl = 0;
    public int coins = 50;
    public string hint = "";
    public static Action<int> OnMoneyUpdate;
    public static Action OnChangeCurrentIndexLvl;


    private void Start()
    {
        currentIndexLvl = YG2.saves.currentIndexLvl;
        coins = YG2.saves.coins;
        
    }
    public int Coins
    {
        get { return YG2.saves.coins; }
        set
        {

            if (coins < 0) coins = 0;
            coins = value;
            YG2.saves.coins = coins;
            YG2.SaveProgress();
            OnMoneyUpdate?.Invoke(coins);

            
        }
    }

    public int CurrentIndexLvl
    {
        get { return currentIndexLvl; }
        set
        {

            if (currentIndexLvl <= 0) currentIndexLvl = -1;
            if (currentIndexLvl >= GameManager.Instance.levelManager.levels.Count) return;
            currentIndexLvl = value;
            YG2.saves.currentIndexLvl = currentIndexLvl;
            YG2.SaveProgress();
            OnChangeCurrentIndexLvl?.Invoke();
        }
    }
}
