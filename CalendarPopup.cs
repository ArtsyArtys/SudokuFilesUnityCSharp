using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Globalization;
using TMPro;

public class CalendarPopup : MonoBehaviour
{

  public GameObject[] daySquares;  //Holds 37 for full grid
  public GameObject trophy; // day has been completed
  public string[] months = new string[12];
  public TextMeshProUGUI headerText;
  public TextMeshProUGUI subHeaderText;

  [SerializeField]
  int monthCounter = DateTime.Now.Month - 1;
  [SerializeField]
  int yearCounter = 0;

  [SerializeField]
  DateTime iMonth;
  [SerializeField]
  DateTime curDisplay;
  public int iDay;
  int dayOffset;

  void Start()
  {
    // Debug.Log("Monthcounter = " + monthCounter);
    CreateMonths();
    CreateCalendar();
    iDay = (DateTime.Now.Day);
  }

  /*Adds all the months to the months Array and sets the current month
  in the header label*/
  void CreateMonths()
  {
    iMonth = new DateTime(2000, 1, 1);

    for (int i = 0; i < 12; i++)
    {
        iMonth = new DateTime(DateTime.Now.Year, i + 1, 1);
        months[i] = iMonth.ToString("MMMM");
    }
    iMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // The magical Line :)
    headerText.text = months[DateTime.Now.Month - 1]; //+ " " + DateTime.Now.Year;
    subHeaderText.text = "(" + DateTime.Now.Year + ")";
  }

  /*Sets the days to their correct labels*/
  void CreateCalendar()
  {
    curDisplay = iMonth;
    Debug.Log(iMonth);
    int startDay = (int)curDisplay.DayOfWeek;
    ClearDaySquares();
    for (int i = startDay; i < DateTime.DaysInMonth(curDisplay.Year, curDisplay.Month) + startDay; i++)
    {
      Debug.Log((i - startDay + 1).ToString());
      daySquares[i].GetComponentInChildren<TextMeshProUGUI>().SetText((i - startDay + 1).ToString());
    }
    if (daySquares[35].GetComponentInChildren<TextMeshProUGUI>().text == null)
    {
      daySquares[35].SetActive(false);
    }
    if (daySquares[36].GetComponentInChildren<TextMeshProUGUI>().text == null)
    {
      daySquares[36].SetActive(false);
    }
  }




  /*when arrow clicked / swiped go to next/previous month */
  public void MoveMonth(bool increment)
  {
    if (increment)
    {
      monthCounter++;
      if (monthCounter > 11)
      {
          monthCounter = 0;
          yearCounter++;
      }

      headerText.SetText(months[monthCounter]);
      subHeaderText.SetText("(" + (DateTime.Now.Year + yearCounter) + ")");
      ClearDaySquares();
      iMonth = iMonth.AddMonths(1);
      CreateCalendar();
    }
    else
    {
    monthCounter--;
    if (monthCounter < 0)
    {
      monthCounter = 11;
      yearCounter--;
    }
    headerText.SetText(months[monthCounter]);
    subHeaderText.SetText("(" + (DateTime.Now.Year + yearCounter) + ")");
    ClearDaySquares();
    iMonth = iMonth.AddMonths(-1);
    CreateCalendar();
    }
  }

  void ClearDaySquares()
  {
    for (int i = 0; i < daySquares.Length; i++)
    {
        daySquares[i].SetActive(true);
        daySquares[i].GetComponentInChildren<TextMeshProUGUI>().text = null;
    }
  }
}
