using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class ADS_REWARDED : MonoBehaviour
{
    public string rewardID;  
  
    //MyRewardAdvShow(rewardID);

    // ����� ������ � ������ ������� ������ ��������, ����� OnReward ���������� �� ������� ��������������
    private void OnEnable()
    {
        YG2.onRewardAdv += OnReward;
        
    }

    // ���������� ���������� ������ �� ������� ��� ����������� �������
    private void OnDisable()
    {
        YG2.onRewardAdv -= OnReward;
        
    }

    // ����� ������� �� ��������������
    public void MyRewardAdvShow()
    {

        YG2.RewardedAdvShow(rewardID);



    }


    // ����� �������� �� ������� OnReward (����� ��������������)
    private void OnReward(string id)
    {
        if (rewardID == id)
        {
            GameManager.Instance.data.Coins += 25;
        }


    }
}
