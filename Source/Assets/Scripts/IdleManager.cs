
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using  System;

public class IdleManager : MonoBehaviour
{
    [HideInInspector] public int length;
    [HideInInspector] public int lengthCost;

    [HideInInspector] public int strength;
    [HideInInspector] public int strengthCost;

    [HideInInspector] public int offlineEarning;
    [HideInInspector] public int offlineEarningCost;

    [HideInInspector] public int wallet;
    [HideInInspector] public int totalGain;

    public int[] costs = new int[]
    {
        120,
        151,
        197,
        250,
        324,
        414,
        537,
        687,
        892,
        1145,
        1484,
        1911,
        2479,
        3196,
        4148,
        5359,
        6954,
        9000,
        11687
    };

    public static IdleManager instance;

    private void Awake()
    {
        if (IdleManager.instance)
            UnityEngine.Object.Destroy(gameObject);
        else
            IdleManager.instance = this;
        

        length = -PlayerPrefs.GetInt("Length", 30);
        strength = PlayerPrefs.GetInt("Strength", 3);
        offlineEarning = PlayerPrefs.GetInt("Offline", 3);
        wallet = PlayerPrefs.GetInt("Wallet", 0);
        
        lengthCost = costs[-length / 10 - 3];
        strengthCost = costs[strength  - 3];
        offlineEarningCost = costs[offlineEarning  - 3];
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        //code to calculate idle time
        if (pauseStatus)
        {
            DateTime now = DateTime.Now;
            PlayerPrefs.SetString("Date",now.ToString());
            MonoBehaviour.print(now.ToString());
        }
        else
        {
            string _string = PlayerPrefs.GetString("Date",string.Empty);
            if (_string != string.Empty)
            {
                DateTime d = DateTime.Parse(_string);
                totalGain = (int)((DateTime.Now - d).TotalMinutes * offlineEarning + 1.0);
                ScreenManager.instance.ChangeScreen(Screens.RETURN);
            }
        }
    }

    private void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }

    public void BuyLength()
    {
        length -= 10;
        wallet -= lengthCost;
        lengthCost = costs[-length / 10 - 3];
        PlayerPrefs.SetInt("Length",-length);
        PlayerPrefs.SetInt("Wallet",wallet);
        ScreenManager.instance.ChangeScreen(Screens.MAIN);
    }
    
    public void BuyStrength()
    {
        strength++;
        wallet -= strengthCost;
        strengthCost = costs[strength - 3];
        PlayerPrefs.SetInt("Strength",strength);
        PlayerPrefs.SetInt("Wallet",wallet);
        ScreenManager.instance.ChangeScreen(Screens.MAIN);
    }
    
    public void BuyOfflineEarnings()
    {
        offlineEarning++;
        wallet -= offlineEarningCost;
        offlineEarningCost = costs[offlineEarning - 3];
        PlayerPrefs.SetInt("Offline",offlineEarning);
        PlayerPrefs.SetInt("Wallet",wallet);
        ScreenManager.instance.ChangeScreen(Screens.MAIN);
    }
    
    public void CollectMoney()
    {
        wallet = wallet + totalGain;
        PlayerPrefs.SetInt("Wallet",wallet);
        ScreenManager.instance.ChangeScreen(Screens.MAIN);
    }
    
    public void CollectDoubleMoney()
    {
        wallet = wallet + (totalGain * 2);
        PlayerPrefs.SetInt("Wallet",wallet);
        ScreenManager.instance.ChangeScreen(Screens.MAIN);
    }

}
