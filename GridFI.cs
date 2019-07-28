using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridFI {

	public static bool[] numbersRevealed = new bool[81];
	static bool[] numbersFilled = new bool[81];
	public static bool[] boxIsFilled = new bool[9];
	static int[] boxes = new int[9];
	static int numOfSpotsToFill;
	static int numOfSpotsFilled;
	static int spotsRevealedMin;
	static int spotsRevealedMax;
	static int numOfBoxesFilled;
	static int numOfBoxesToFill;
	public static int easyMin = 36, easyMax = 41;
	public static int medMin = 30, medMax = 35;
	public static int hardMin = 23, hardMax = 29;
	public static int expMin = 20, expMax = 23;
	static int randomBox;
	static int tempSpotInBox;
	static List<int> spotsInBox = new List<int>() { 0, 1, 2, 9, 10, 11, 18, 19, 20 };
	// List<int> tempSpotsInBox = new List<int>();
	// static int saveSlot = 0;

	static void PickRandomBox()
	{
		randomBox = Shuffler.ShuffleIntFromArray(boxes);
	}

 static void CheckBoxAsFilled()
	{
		for (int i = 0; i < 9; i++)
		{
			if (!boxIsFilled[i] && randomBox == i)
			{
				numOfBoxesFilled++;
				boxIsFilled[i] = true;
			}
		}
	}

	static void FillRandomSpotInBox()
	{
		tempSpotInBox = Shuffler.ShuffleIntFromList(spotsInBox);
		if (randomBox < 3)
		{
			tempSpotInBox = tempSpotInBox + (3 * randomBox);
		}
		else if (randomBox < 6)
		{
			tempSpotInBox = tempSpotInBox + 27 + (3 * (randomBox - 3));
		}
		else if (randomBox < 9)
		{
			tempSpotInBox = tempSpotInBox + 54 + (3 * (randomBox - 6));
		}
		if (!numbersFilled[tempSpotInBox])
		{
			CheckBoxAsFilled();
			numbersFilled[tempSpotInBox] = true;
			numOfSpotsFilled++;
		}
		else
		{
			PickRandomBox();
			FillRandomSpotInBox();
		}
	}

	static void FillSpotsInGrid()
	{
		for (int i = 0; i < numOfSpotsToFill; i++)
		{
			if (numOfSpotsFilled < numOfSpotsToFill)
			{
				PickRandomBox();
				FillRandomSpotInBox();
			}
		}
	}


	public static bool[] RevealSpotsFilled(int dif)
	{
		boxes = Shuffler.FillArray(boxes);
		FindDifficulty(dif);
		FillSpotsInGrid();
		for (int i = 0; i < 81; i++)
		{
			if (numbersFilled[i])
			{
				numbersRevealed[i] = true;
			}
		}
		bool[] arr = new bool[81];
		for (int i = 0; i < 81; i++)
		{
			if (numbersRevealed[i])
			{
				arr[i] = true;
			}
		}
		if (AreBoxesFilled())
		{
			return arr;
		}
		else
		{
			Debug.Log("Not enough boxes filled, rerun RevealSpotsFilled");
			return RevealSpotsFilled(dif);
		}
	}

	static bool AreBoxesFilled()
	{
		if (numOfBoxesFilled < numOfBoxesToFill)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	static void FindDifficulty(int dif)
  {
    switch (dif)
    {
      case 0: // easy
        spotsRevealedMin = easyMin;
        spotsRevealedMax = easyMax;
				numOfBoxesToFill = 9;
        break;
      case 1: // medium
        spotsRevealedMin = medMin;
        spotsRevealedMax = medMax;
				numOfBoxesToFill = 8;
        break;
      case 2: // hard
        spotsRevealedMin = hardMin;
        spotsRevealedMax = hardMax;
				numOfBoxesToFill = 8;
        break;
      case 3: // expert
        spotsRevealedMin = expMin;
        spotsRevealedMax = expMax;
				numOfBoxesToFill = 7;
        break;
      default:
        spotsRevealedMin = easyMin;
        spotsRevealedMax = easyMax;
				numOfBoxesToFill = 9;
        break;
    }
    numOfSpotsToFill = Random.Range(spotsRevealedMin, spotsRevealedMax);
    // gridRandomizer = new int[numOfSpotsToFill];
  }
}
