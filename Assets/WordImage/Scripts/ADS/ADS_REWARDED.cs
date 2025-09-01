using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class ADS_REWARDED : MonoBehaviour
{
    public string rewardID;  
  
    //MyRewardAdvShow(rewardID);

    // Когда объект с данным классом станет активным, метод OnReward подпишится на событие вознаграждения
    private void OnEnable()
    {
        YG2.onRewardAdv += OnReward;
        
    }

    // Необходимо отписывать методы от событий при деактивации объекта
    private void OnDisable()
    {
        YG2.onRewardAdv -= OnReward;
        
    }

    // Вызов рекламы за вознаграждение
    public void MyRewardAdvShow()
    {

        YG2.RewardedAdvShow(rewardID);



    }


    // Метод подписан на событие OnReward (ивент вознаграждения)
    private void OnReward(string id)
    {
        if (rewardID == id)
        {
            GameManager.Instance.data.Coins += 25;
        }


    }
}
