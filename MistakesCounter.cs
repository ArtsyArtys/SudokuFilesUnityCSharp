using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MistakesCounter : MonoBehaviour
{

    int mistakes;
    int gridNumber;

    public TextMeshProUGUI text;

    void Start()
    {
      CheckSaveData();
    }

    public void SaveData()
    {
      mistakes = GameManager.mistakes;
      switch (gridNumber)
      {
        case 0:
          PlayerPrefs.SetInt("mistakes0", mistakes);
          break;
        case 1:
          PlayerPrefs.SetInt("mistakes1", mistakes);
          break;
        case 2:
          PlayerPrefs.SetInt("mistakes2", mistakes);
          break;
        case 3:
          PlayerPrefs.SetInt("mistakes3", mistakes);
          break;
        default:
          return;
      }

    }

    void CheckSaveData()
    {
      gridNumber = GameManager.ReturnLvlByScene();

      switch (gridNumber)
      {
        case 0:
          if (PlayerPrefs.HasKey("mistakes0"))
          {
            int m = PlayerPrefs.GetInt("mistakes0");
            GameManager.mistakes = m;
            GameManager.mistakes0 = m;
          }
          else
          {
            PlayerPrefs.SetInt("mistakes0", 0);
            GameManager.mistakes = 0;
            GameManager.mistakes0 = 0;
          }
          break;

        case 1:
          if (PlayerPrefs.HasKey("mistakes1"))
          {
            int m = PlayerPrefs.GetInt("mistakes1");
            GameManager.mistakes = m;
            GameManager.mistakes1 = m;
          }
          else
          {
            PlayerPrefs.SetInt("mistakes1", 0);
            GameManager.mistakes = 0;
            GameManager.mistakes1 = 0;
          }
          break;

        case 2:
          if (PlayerPrefs.HasKey("mistakes2"))
          {
            int m = PlayerPrefs.GetInt("mistakes2");
            GameManager.mistakes = m;
            GameManager.mistakes2 = m;
          }
          else
          {
            PlayerPrefs.SetInt("mistakes2", 0);
            GameManager.mistakes = 0;
            GameManager.mistakes2 = 0;
          }
          break;

        case 3:
          if (PlayerPrefs.HasKey("mistakes3"))
          {
            int m = PlayerPrefs.GetInt("mistakes3");
            GameManager.mistakes = m;
            GameManager.mistakes3 = m;
          }
          else
          {
            PlayerPrefs.SetInt("mistakes3", 0);
            GameManager.mistakes = 0;
            GameManager.mistakes3 = 0;
          }
          break;

        default:
          GameManager.mistakes = 0;
          return;
      }
    }

    void Update()
    {
      mistakes = GameManager.mistakes;
      text.SetText("Mistakes: " + mistakes.ToString() + "/3");
      if (mistakes >= 3)
      {
        GameManager.mistakes = 0;
        // play advert
        SaveData();
      }
    }

}
