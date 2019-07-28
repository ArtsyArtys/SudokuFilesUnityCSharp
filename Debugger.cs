using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Debugger {

	public static void LogGrid(int[] grid, string str = "")
	{
		string[] numberToStrings = new string[81];
		string numberStrings = str + "\n";
		for (int i = 0; i < 81; i++) {
			if (grid[i] != 0)
			{
				numberToStrings[i] = grid[i].ToString();
			}
			else
			{
				numberToStrings[i] = "0";
			}
		}
		for (int j = 0; j < 81; j++) {
			numberStrings += ("[ " + numberToStrings[j] + " ] ");
			if ((j + 1) % 9 == 0 ) {
				numberStrings += "\n";
			}
		}
		Debug.Log(numberStrings);
	}

	public static void LogBoolGrid(bool[] grid, string str = "")
	{
		string[] boolStrings = new string[81];
		string fullString = str + "\n";
		for (int i = 0; i < 81; i++)
		{
			if (grid[i])
			{
				boolStrings[i] = "Y";
			}
			else
			{
				boolStrings[i] = "N";
			}
		}
		for (int i = 0; i < 81; i++)
		{
			fullString += ("[ " + boolStrings[i] + " ] ");
			if (i % 9 == 0)
			{
				fullString += "\n";
			}
		}
		Debug.Log(fullString);
	}

	public static void LogGridCompleted(int[] grid, bool[] boolGrid, string str = "")
	{
		string[] gridString = new string[81];
		string fullString = str + "\n";
		for (int i = 0; i < 81; i++)
		{
			if (boolGrid[i])
			{
				gridString[i] = grid[i].ToString();
			}
			else
			{
				gridString[i] = "N";
			}
		}
		for (int i = 0; i < 81; i++)
		{
			fullString += ("[ " + gridString[i] + " ]");
			if (i % 9 == 0)
			{
				fullString += "\n";
			}
		}
		Debug.Log(fullString);
	}

	public static void ButtonClicked()
	{
		Debug.Log("You clicked a button!");
	}
}
