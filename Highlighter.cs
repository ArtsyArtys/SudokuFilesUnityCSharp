using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlighter : MonoBehaviour
{

  int rowOrigin;
  int colOrigin;
  int boxOrigin;
  bool[] gridSpacePicked = new bool[81];
  public bool[] isHighlighted = new bool[81];
  public bool[] isDoubleHighlighted = new bool[81];
  public bool[] isSelected = new bool[81];
  public bool[] isSameNum = new bool[81];
  public bool[] isIncorrect = new bool[81];
  public bool[] canBeChanged = new bool[81];
  bool[] marginIsHighlighted = new bool[36];
  int[,] gridMarginRowsLeft, gridMarginRowsRight, gridMarginColsTop, gridMarginColsBottom;
  bool marginPressed = false;
  Transform[] gridSquares = new Transform[81];
  public Material picked;
  public Material highlighted;
  public Material doubleHighlighted;
  public Material unpicked;
  public Material incorrectHighlighted;
  public Material sameNumHighlighted;
  // public int spacePicked;

  // GridButton GB;

  void Start()
  {
    for (int i = 0; i < 81; i++)
    {
      isIncorrect[i] = false;
    }
    ClearSquareColors();
    // ShowHighlighted();
  }

  void ClearSquareColors()
  {
    Transform[] tempSquares = GetComponentsInChildren<Transform>();

    for (int i = 1; i < tempSquares.Length; i++)
    {
      gridSquares[i - 1] = tempSquares[i];
      gridSquares[i - 1].GetComponent<Image>().material = unpicked;
    }
    // gridSquares[13].GetComponent<Image>().material.color = picked;
  }


  void Update()
  {
    // for (int i = 0; i < 81; i++)
    // {
    //   if (isIncorrect[i])
    //   {
    //     canBeChanged[i] = false;
    //   }
    //   else
    //   {
    //     canBeChanged[i] = true;
    //   }
    // }
  }

  public void HighlightSameNums(bool[] matchingNums)
  {
    for (int i = 0; i < 81; i++)
    {
      if (matchingNums[i])
      {
        isSameNum[i] = true;
        gridSquares[i].GetComponent<Image>().material = sameNumHighlighted;
      }
    }
  }

  public void ShowHighlighted(int spacePicked)
  {
    for (int i = 0; i < 81; i++)
    {
      gridSpacePicked[i] = false;
    }
    gridSpacePicked[spacePicked] = true;
    HighlightRCB(spacePicked);
    for (int i = 0; i < gridSquares.Length; i++)
    {
      Image thisImage = gridSquares[i].GetComponent<Image>();
      if (!isHighlighted[i] && !isDoubleHighlighted[i] && !isSelected[i])
      {
        thisImage.material = unpicked;
      }
      if (isHighlighted[i])
      {
        thisImage.material = highlighted;
      }
      if (isDoubleHighlighted[i])
      {
        thisImage.material = doubleHighlighted;
      }
      if (isSelected[i])
      {
        thisImage.material = picked;
      }
      if (isIncorrect[i])
      {
        thisImage.material = incorrectHighlighted;
      }
    }
  }

  // public void HighlightSameNums(int input)
  // {
  //   for
  // }


  public void HighlightRCB(int space)
  {
    int rowOrigin = (space / 9) * 9;
    int colOrigin = space % 9;
    int boxOrigin = ((space % 9) / 3) * 3 + (space / 27) * 27;
    for (int i = 0; i < 81; i++)
    {
      isHighlighted[i] = false;
      isDoubleHighlighted[i] = false;
      isSelected[i] = false;
    }
    for (int i = 0; i < 81; i++)
    {
      if (i == space)
      {
        // Highlight Box:
        for (int l = 0; l < 9; l++)
        {
          isHighlighted[boxOrigin + (l / 3) * 9 + (l % 3)] = true;
        }
        // Highlight Row:

        // TODO if playerprefs.highlightRow is true
        for (int j = 0; j < 9; j++)
        {
          isHighlighted[rowOrigin + j] = true;
          if (rowOrigin + j == space)
          {
            if (colOrigin % 3 == 0)
            {
              isHighlighted[space + 1] = false;
              isHighlighted[space + 2] = false;
              isDoubleHighlighted[space + 1] = true;
              isDoubleHighlighted[space + 2] = true;
            }
            if (colOrigin != 0)
            {
              if ((colOrigin - 1) % 3 == 0)
              {
                isHighlighted[space - 1] = false;
                isHighlighted[space + 1] = false;
                isDoubleHighlighted[space - 1] = true;
                isDoubleHighlighted[space + 1] = true;
              }
              if ((colOrigin - 2) % 3 == 0)
              {
                isHighlighted[space - 2] = false;
                isHighlighted[space - 1] = false;
                isDoubleHighlighted[space - 2] = true;
                isDoubleHighlighted[space - 1] = true;
              }
            }
          }
        }
        // Highlight Column:

        // TODO if playerprefs.highlightCols is true
        for (int k = 0; k < 9; k++)
        {
          isHighlighted[colOrigin + (k * 9)] = true;
          if (colOrigin + (k * 9) == space)
          {
            if ((rowOrigin % 27) == 0)
            {
              isHighlighted[space + 9] = false;
              isHighlighted[space + 2 * 9] = false;
              isDoubleHighlighted[space + 9] = true;
              isDoubleHighlighted[space + 2 * 9] = true;
            }
            if (rowOrigin != 0)
            {
              if ((rowOrigin - 9) % 27 == 0)
              {
                isHighlighted[space - 9] = false;
                isHighlighted[space + 9] = false;
                isDoubleHighlighted[space - 9] = true;
                isDoubleHighlighted[space + 9] = true;
              }
              if ((rowOrigin - 18) % 27 == 0)
              {
                isHighlighted[space - 9] = false;
                isHighlighted[space - 2 * 9] = false;
                isDoubleHighlighted[space - 9] = true;
                isDoubleHighlighted[space - 2 * 9] = true;
              }
            }
          }
        }
        // always happens: show/highlight selected
        isHighlighted[i] = false;
        isDoubleHighlighted[i] = false;
        isSelected[i] = true;
      }
    }
    // for (int i = 0; i < 81; i++)
    // {
    //   if
    // }
    // if (MarginPressed(gridSpacePicked) && marginPressed)
    // {
    //
    //   CheckMargins();
    //   if (marginLeft)
    //   {
    //     marginIsHighlighted[rowOrigin] = true;
    //   }
    //   if (!marginLeft)
    //   {
    //     marginIsHighlighted[rowOrigin] = false;
    //   }
    //   if (marginRight)
    //   {
    //     marginIsHighlighted[rowOrigin + 9] = true;
    //   }
    //   if (!marginRight)
    //   {
    //     marginIsHighlighted[rowOrigin + 9] = false;
    //   }
    //   if (marginTop)
    //   {
    //     marginIsHighlighted[colOrigin + 18] = true;
    //   }
    //   if (!marginTop)
    //   {
    //     marginIsHighlighted[rowOrigin + 18] = false;
    //   }
    //   if (marginBottom)
    //   {
    //     marginIsHighlighted[colOrigin + 27] = true;
    //   }
    //   if (!marginBottom)
    //   {
    //     marginIsHighlighted[rowOrigin + 27] = false;
    //   }
    //   for (int i = 0; i < 9; i++)
    //   {
    //     if (marginIsHighlighted[i] || marginIsHighlighted[i + 9])
    //     {
    //       isHighlighted[i] = true;
    //     }
    //     if (marginIsHighlighted[i + 18] || marginIsHighlighted[i + 27])
    //     {
    //       isHighlighted[i * 9] = true;
    //     }
    //   }
    // }
    // else
    // {
    for (int i = 0; i < 36; i++)
    {
      marginIsHighlighted[i] = false;
    }
    // }
  }

  bool MarginPressed(bool[] space)
	{
		for (int i = 0; i < 81; i++)
		{
			if (gridSpacePicked[i])
			{
				return false;
			}
			else if (marginPressed)
			{
				return true;
			}
		}
		return false;
	}
}
