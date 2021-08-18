using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
  public static ScreenManager instance;
  private GameObject currentSceen;
  public GameObject endScreen;
  public GameObject gameScreen;
  public GameObject mainScreen;
  public GameObject returnScreen;

  public Button lengthButton;
  public Button strengthButton;
  public Button offlineButton;
  
  public Text lengthCostText;
  public Text lengthValueText;
  
  public Text strengthCostText;
  public Text strengthValueText;

  public Text offlineCostText;
  public Text offlineValueText;

  public Text gameScreenMoney;
  public Text endScreenMoney;
  public Text returnScreenMoney;

  private int gameCount;

  void Awake()
  {
    if (ScreenManager.instance)
    {
      Destroy(base.gameObject);
    }
    else
    {
      ScreenManager.instance = this;
    }

    currentSceen = mainScreen;
  }

  void Start()
  {
    CheckIdles();
    UpdateTexts();
  }

  public void ChangeScreen(Screens screen)
  {
    currentSceen.SetActive(false);
    switch (screen)
    {
      case Screens.MAIN:
        currentSceen = mainScreen;
        UpdateTexts();
        CheckIdles();
        break;
      case Screens.GAME:
        currentSceen = gameScreen;
        gameCount++;
        break;
      case Screens.END:
        currentSceen = endScreen;
        SetEndScreenMoney();
        break;
      case Screens.RETURN:
        currentSceen = returnScreen;
        SetReturnScreenMoney();
        break;
    }
    currentSceen.SetActive(true);
  }

  public void SetEndScreenMoney()
  {
    endScreenMoney.text = "$" + IdleManager.instance.totalGain;
  }
  public void SetReturnScreenMoney()
  {
    returnScreenMoney.text = "$" + IdleManager.instance.totalGain + " gained while idle!";
  }
  public void UpdateTexts()
  {
    gameScreenMoney.text = "$" + IdleManager.instance.wallet;
    lengthCostText.text = "$" + IdleManager.instance.length;
    lengthValueText.text = -IdleManager.instance.length + "m";
    strengthCostText.text = "$" + IdleManager.instance.strengthCost;
    strengthValueText.text = IdleManager.instance.strength + "fishes";
    offlineCostText.text = "$" + IdleManager.instance.offlineEarningCost;
    offlineValueText.text = "$" + IdleManager.instance.offlineEarning + "/min";
  }

  public void CheckIdles()
  {
    int lengthCost = IdleManager.instance.lengthCost;
    int strengthCost = IdleManager.instance.strengthCost;
    int offlineEarningsCost = IdleManager.instance.offlineEarningCost;
    int wallet = IdleManager.instance.wallet;

    if (wallet < lengthCost)
      lengthButton.interactable = false;
    
    else
      lengthButton.interactable = true;
    

    if (wallet < strengthCost)
      strengthButton.interactable = false;
    
    else
      strengthButton.interactable = true;
    
    if (wallet < offlineEarningsCost)
      offlineButton.interactable = false;
    
    else
      offlineButton.interactable = true;
    
    
  }

}
