using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInstantiator : MonoBehaviour {

	int[] arr;
	public SudokuInitializer SudokuInitializer;
	// public GridCreator0 GridCreator0;
	public GridRevealedNums GRN;
	int lvl;
	// public Debugger DB;



	void Awake()
	{
		arr = new int[81];
		lvl = GameManager.ReturnLvlByScene();
		SudokuInitializer.grid = SudokuInitializer.GenerateImperfectGrid();
		SudokuInitializer.grid = SudokuInitializer.GenerateGrid();
		arr = SudokuInitializer.grid;
		if (SaveManager.LoadGrid(lvl) == null)
		{
			Debug.Log("grid saved in slot: " + lvl.ToString() + " is null");
			SaveManager.SaveGrid(arr, lvl);
			GRN.grid = arr;
			SaveManager.SaveTimer(0, lvl);
		}
		else
		{
			GRN.grid = SaveManager.LoadGrid(lvl);
		}
		GRN.RevealGridNums();
	}

}
