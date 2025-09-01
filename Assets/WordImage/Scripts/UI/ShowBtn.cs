using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using YG.Example;

public class ShowBtn : MonoBehaviour
{
    public TimerADSR timerADSR;
    public GameObject btn;
    public GameObject panel;
    public GameObject panel1;
    private void Start()
    {
        ReceivingPurchaseExample.ActionSuccessPurchased += ActionSuccessPurchased;
        if (YG2.saves.NOTads)
        {
            btn.SetActive(true);
            panel1.SetActive(false);
            panel.SetActive(true);
            return;
        }
        timerADSR.StartTimer();
        
    }

    private void ActionSuccessPurchased()
    {
        btn.SetActive(true);
        panel1.SetActive(false);
        panel.SetActive(true);
    }

    private void Update()
    {
        if (YG2.saves.NOTads)
        {
            return; 
        }
        if (!timerADSR.flagStartTimer )
        {
            btn.SetActive(true);
        }
    }
}
