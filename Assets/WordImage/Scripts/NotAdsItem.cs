using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using YG.Example;

public class NotAdsItem : MonoBehaviour
{
    
    void Start()
    {
        ReceivingPurchaseExample.ActionSuccessPurchased += ActionSuccessPurchased;
        if (YG2.saves.NOTads)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        } 
    }

    private void ActionSuccessPurchased()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
