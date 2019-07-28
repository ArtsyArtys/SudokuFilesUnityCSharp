using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridRevealedNums : MonoBehaviour {


	public bool[] numsCorrect;
	public int lvl;
	public int[] grid = new int[81];
	int[] gridChecker = new int[81];
	int[] amountOfSameNumsRevealed = new int[9];


	public bool CheckSameNumsRevealed(int input)
	{
		for (int i = 0; i < 9; i++)
		{
			amountOfSameNumsRevealed[i] = 0;
		}
		for (int i = 0; i < grid.Length; i++)
		{
			if (numsCorrect[i])
			{
				int number = grid[i] - 1;
				amountOfSameNumsRevealed[number] += 1;
			}
		}
		for (int i = 0; i < 9; i++)
		{
			if (amountOfSameNumsRevealed[i] >= 9) // && playerPrefs destroy button when filled is true
			{
				if (input == i + 1)
				{
					Destroy(GameObject.Find("Input" + (i + 1).ToString()));
					return true;
				}
			}
		}
		return false;
	}

	public void CheckLevel()
	{
		lvl = GameManager.ReturnLvlByScene();
		// TODO gameobject thisGrid =  "instantiatedGrid{num/lvl}" attached to lvl
	}

	void Awake()
	{
		CheckLevel();
		grid = SaveManager.LoadGrid(lvl);
	}

	public void RevealGridNums()
	{
		// PopulateGrid();
		numsCorrect = SaveManager.LoadNumsFilled(lvl);
		if (numsCorrect == null)
		{
			SaveManager.SaveNumsFilled(GridFI.RevealSpotsFilled(lvl), lvl);
		}
		PopulateGridChecker();
		LogRevealedGrid();
	}

	public int[] ReturnTrueGrid()
	{
		int[] tempGrid = grid;
		return tempGrid;

	}

	public bool CheckNumsCorrect()
	{
		int amountOfNumsCorrect = 0;
		PopulateGridChecker();
		for (int i = 0; i < 81; i++)
		{
			if (gridChecker[i] == grid[i])
			{
				numsCorrect[i] = true;
				amountOfNumsCorrect++;
			}
		}
		if (amountOfNumsCorrect >= 81)
		{
			amountOfNumsCorrect = 0;
			int dif = GameManager.ReturnLvlByScene();
			SaveManager.SaveTotalWins();
			SaveManager.SavePlayWins(dif);
			SaveManager.ClearGrid(dif);
			if (GameObject.Find("WinAnimationController") != null)
			{
				GameObject WAC = GameObject.Find("WinAnimationController");
				WAC.GetComponent<WinAnimationController>().InstantiateStars();
			}
			return true;
		}
		else
		{
			return false;
		}

	}

	public bool IsSpaceCorrect(int space)
	{
		PopulateGridChecker();
		if (gridChecker[space] != grid[space])
		{
			Debug.Log("Space is incorrect");
			return false;
		}
		else
		{
			Debug.Log("Space is correct");
			return true;
		}
	}

	 void PopulateGridChecker()
	{
		for (int i = 0; i < 81; i++)
		{
			if (numsCorrect[i])
			{
				gridChecker[i] = grid[i];
			}
			else
			{
				gridChecker[i] = 0;
			}
			if (gridChecker[i] == grid[i])
			{
				numsCorrect[i] = true;
			}
			else
			{
				numsCorrect[i] = false;
			}
		}
		SaveManager.SaveNumsFilled(numsCorrect, lvl);

	}

	 void PopulateGrid()
	{
		for (int i = 0; i < 81; i++)
		{
			grid[i] = GameManager.gridsSaved[lvl, i];
		}
		Debugger.LogGrid(grid);
	}

	void LogRevealedGrid()
	{
		Debugger.LogGrid(gridChecker);
	}
}
