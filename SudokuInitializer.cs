using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuInitializer : MonoBehaviour {


	public int[] grid;
	bool[] sorted;
	bool[] registered;
	int colOrigin;
	int rowOrigin;
	int tries = 0;
	List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
	int[] squareNums;
	string numStrings;
	bool backtrack;
	bool breakRowCol;
	bool decrementI;
	bool contRowCol;
	bool breakLoop;
	bool breakAll;

	void Awake() {
		// trials = 0;
		// Shuffler Shuffle = GetComponent<Shuffler>();
		grid = new int[81];
		sorted = new bool[81];
		// numberToStrings = new string[81];
		registered = new bool[10];
	}

	void Start() {
	}

	public void Update() {
		if (Input.GetKeyDown("space")) {
			tries += 1;
			grid = GenerateGrid();
			PrintGrid();
		}
		if (Input.GetKeyDown(KeyCode.J)) {
			tries += 1;
			GenerateGrid();
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			IsPerfect(grid);
			PrintGrid();
		}
	}

	// public void Shuffle() {
	// 	for (int i = 0; i < arr.Count; i++) {
	// 		int tempNum = arr[i];
	// 		int k = Random.Range(i, arr.Count);
	// 		arr[i] = arr[k];
	// 		arr[k] = tempNum;
	// 	}
	// }

	public int[] GenerateImperfectGrid() {
		for (int i = 0; i < 81; i++) {
			if (i % 9 == 0) {
				list = Shuffler.ShuffleList(list);
			}
			int perBox = (((i / 3) % 3) * 9) + (((i % 27) / 9) * 3) + ((i / 27) * 27) + (i %3);
			grid[perBox] = list[i % 9];
		}
		// Debug.Log("Generated imperfect grid");
		return grid;
	}

	public string CopyToStrings(int[] grid) {
		string[] numberToStrings = new string[81];
		string numberStrings = "";
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
		// Debug.Log(numberStrings);
		return numberStrings;
	}

	public void PrintGrid() {
		Debug.Log(CopyToStrings(grid));
	}

	public bool IsPerfect(int[] fullGrid) {
		if (CheckBoxes(fullGrid) && CheckRows(fullGrid) &&
		CheckColumns(fullGrid)) {
			// Debug.Log("IsPerfect is true");
			return true;
		}
		return false;
	}

	public bool CheckBoxes(int[] impGrid) {
		if (impGrid.Length != 81) {
			Debug.Log("incorrect grid length");
			return false;
		}
		else {
			for (int i = 0; i < 9; i++) {
				RegisterNums();
				int boxOrigin = (i * 3) % 9 + ((i * 3) / 9) * 27;
				for (int j = 0; j < 9; j++) {
					int boxStep = boxOrigin + (j / 3) * 9 + (j % 3);
					int boxNum = impGrid[boxStep];
					registered[boxNum] = true;
				}
				for (int k = 0; k < registered.Length; k++) {
					if (!registered[k]) {
						Debug.Log("Box failed");
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool CheckRows(int[] impGrid) {
		for (int i = 0; i < 9; i++) {
			RegisterNums();
			rowOrigin = i * 9;
			for (int j = 0; j < 9; j++) {
				int rowStep = rowOrigin + j;
				int rowNum = impGrid[rowStep];
				registered[rowNum] = true;
			}
			for (int k = 0; k < registered.Length; k++) {
				if (!registered[k]) {
					Debug.Log("Row failed. on iteration " + i.ToString() + " " + k.ToString());
					return false;
				}
			}
		}
		return true;
	}

	public bool CheckColumns(int[] impGrid) {
		for (int i = 0; i < 9; i++) {
			RegisterNums();
			colOrigin = i;
			for (int j = 0; j < 9; j++) {
				int colStep = colOrigin + j * 9;
				int colNum = impGrid[colStep];
				registered[colNum] = true;
			}
			for (int k = 0; k < registered.Length; k++) {
				if (!registered[k]) {
					Debug.Log("Column failed. on iteration " + i.ToString() + " " + k.ToString());
					return false;
				}
			}
		}
		return true;
	}

	public int[] GenerateGrid() {
		// GenerateImperfectGrid();
		for (int i = 0; i < 9; i++) {
			backtrack = false;
			decrementI = false;

			for (int a = 0; a < 2; a++) {
				RegisterNums();
				colOrigin = i;
				rowOrigin = i * 9;

				RowCol(a, i);
				if (decrementI) {
					i -= 2;
					break;
				}
				Check(a, i);
				if (breakAll)
				{
					return GenerateGrid();
				}
			}
		}
		if (IsPerfect(grid)) {
			return grid;
		}
		else if (!IsPerfect(grid))
		{
			return GenerateGrid();
		}
		return null;
	}


	public void RowCol(int a, int i) {
		for (int j = 0; j < 9; j++) {
			int step = (a % 2 == 0 ? rowOrigin + j : colOrigin + j * 9);
			int num = grid[step];
			contRowCol = false;

			if (!registered[num]) {
				registered[num] = true;
			}
			else { // duplicate found
				BAS(j, i, a, num);
				if (contRowCol) {
					continue;
				}
				if (breakRowCol) {
					return;
				}
			}
		}
	}

	public void BAS(int j, int i, int a, int num) {
		for (int l = j; l >=0; l--) {
			int scan = (a %2 == 0 ? i * 9 + l : i + 9 * l);
			if (grid[scan] == num) {
				for (int q = (a % 2 == 0 ? (i % 3 + 1) * 3 : 0); q < 9; q++) {
					if (a % 2 == 1 && q % 3 <= i % 3) {
						continue;
					}
					int boxOrigin = ((scan % 9) / 3) * 3 + (scan / 27) * 27;
					int boxStep = boxOrigin + (q / 3) * 9 + (q % 3);
					int boxNum = grid[boxStep];

					if ((!sorted[scan] && !sorted[boxStep] && !registered[boxNum])
					|| (sorted[scan] && !registered[boxNum] && (a % 2 == 0 ? boxStep % 9 == scan % 9 : boxStep / 9 == scan / 9))) {
						grid[scan] = boxNum;
						grid[boxStep] = num;
						registered[boxNum] = true;
						contRowCol = true;
						return;
					}
					else if (q == 8) {
						PAS(j, i, a, num);
						if (contRowCol || breakRowCol) {
							return;
						}
					}
				}
			}
		}
	}

	public void PAS(int j, int i, int a, int num) {
		int searchingNo = num;
		bool[] swapIndex = new bool[81];

		for (int z = 0; z < 18; z++) {
			breakLoop = false;
			for (int p = 0; p <= j; p++) {
				if (breakLoop) {
					break;
				}
				int pace = (a % 2 == 0 ? rowOrigin + p : colOrigin + p * 9);
				if (grid[pace] == searchingNo) {
					int adjacentCell = -1;
					int adjacentNo = -1;
					int decrement = (a % 2 == 0 ? 9 : 1);

					for (int t = 1; t < 3 - (i % 3); t++) {
						adjacentCell = pace + (a % 2 == 0 ? (t + 1) * 9 : t + 1);

						if ((a % 2 == 0 && adjacentCell >= 81) || (a % 2 == 1 && adjacentCell % 9 == 0)) {
							adjacentCell -= decrement;
						}
						else {
							adjacentNo = grid[adjacentCell];
							if (i % 3 != 0 || t != 1 || swapIndex[adjacentCell] || registered[adjacentNo]) {
								adjacentCell -= decrement;
							}
						}
						adjacentNo = grid[adjacentCell];

						if (!swapIndex[adjacentCell]) {
							swapIndex[adjacentCell] = true;
							grid[pace] = adjacentNo;
							grid[adjacentCell] = searchingNo;
							searchingNo = adjacentNo;

							if (!registered[adjacentNo]) {
								registered[adjacentNo] = true;
								contRowCol = true;
								return;
							}
							breakLoop = true;
							break;
						}
					}
				}
			}
		}
		backtrack = true;
		breakRowCol = true;
	}

	public void Check(int a, int i) {
		if (a % 2 == 0) {
			for (int k = 0; k < 9; k++) {
				sorted[i * 9 + k] = true;
			}
		}
		else if (!backtrack) {
			for (int k = 0; k < 9; k++) {
				sorted[i + k * 9] = true;
			}
		}
		else {
			backtrack = false;
			for (int m = 0; m < 9; m++)
			{
				if ((i * 9 + m) < 81 || i < 0 || a < 0 || a > 2)
				{
					sorted[i * 9 + m] = false;
				}
				else
				{
					breakAll = true;
					return;
				}
			}
			for (int m = 0; m < 9; m++)
			{
				if (((i - 1) * 9 + m) < 81 || i < 0 || a < 0 || a > 2)
				{
					sorted[(i - 1) * 9 + m] = false;
				}
				else
				{
					breakAll = true;
					return;
				}
			}
			for (int m = 0; m < 9; m++)
			{
				if ((i - 1 + m * 9) < 81 || i < 0 || a < 0 || a > 2)
				{
					sorted[i - 1 + m * 9] = false;
				}
				else
				{
					breakAll = true;
					return;
				}
			}
			// i -= 2;
			decrementI = true;
			breakRowCol = false;
		}
	}

	public void RegisterNums() {
		for (int reg = 0; reg < 10; reg++) {
			registered[reg] = false;
		}
		registered[0] = true;
	}
}
