using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridFiller : MonoBehaviour
{
  public GridRevealedNums GRN;
  public Transform[] gridSquares;
  public GameObject[] numSprites;
  public GameObject[] numNoteSprites;
  public Highlighter highlighter;
  public MistakesCounter mc;
  Transform[] children = new Transform[9];
  Transform child;

  int[] grid = new int[81];
  public int spacePicked;
  public bool isSpacePicked = false;

  void Start()
  {
    gridSquares = GetComponentsInChildren<Transform>();
    for (int i = 0; i < 81; i++)
    {
      gridSquares[i] = gridSquares[i + 1];
    }
    grid = GRN.grid;
    Debugger.LogGrid(grid);
    InitializeGrid();
  }

  void Update()
  {

  }

  // public bool CheckInputAgainstGrid(int input, int correctNum, int space)
  // {
  //
  // }

  public void PickSpace(int space)
  {
    spacePicked = space;
    isSpacePicked = true;
    highlighter.ShowHighlighted(space);

    if (GRN.numsCorrect[spacePicked])
    {
      int[] correctNums = new int[81];
      bool[] matchingNums = new bool[81];
      for (int i = 0; i < 81; i++)
      {
        if (GRN.numsCorrect[i])
        {
          correctNums[i] = GRN.grid[i];
        }
        else
        {
          correctNums[i] = 0;
        }
        if (correctNums[i] == GRN.grid[space] && i != spacePicked)
        {
          matchingNums[i] = true;
        }
      }
      // TODO if playerprefs.HighlightSameNums is true;
      highlighter.HighlightSameNums(matchingNums);
    }
  }

  public bool CheckNumsCorrect(int input)
  {
    if (!GameManager.notes)
    {
      if (grid[spacePicked] == input)
      {
        GRN.numsCorrect[spacePicked] = true;
        ClearSameNums(spacePicked, input);
        bool shouldClearNotes = GRN.CheckSameNumsRevealed(input);
        if (shouldClearNotes)
        {
          ClearNotesOfValue(input);
        }
        if (gridSquares[spacePicked].childCount == 0)
        {
          GameObject prefab = Instantiate(numSprites[grid[spacePicked] - 1], gridSquares[spacePicked]);
          prefab.name = input.ToString();
        }
        else
        {
          highlighter.isIncorrect[spacePicked] = false;
          for (int i = 0; i < gridSquares[spacePicked].childCount; i++)
          {
            Destroy(gridSquares[spacePicked].transform.GetChild(i).gameObject);
          }
          GameObject prefab = Instantiate(numSprites[grid[spacePicked] - 1], gridSquares[spacePicked]);
          prefab.name = input.ToString();
          highlighter.ShowHighlighted(spacePicked);
        }
        PickSpace(spacePicked);
        return true;
      }
      else if (!GRN.numsCorrect[spacePicked])
      {
        if (highlighter.isIncorrect[spacePicked])
        {
          child = gridSquares[spacePicked].transform.GetChild(0);
          if (child.name == input.ToString())
          {
            highlighter.isIncorrect[spacePicked] = false;
            for (int i = 0; i < gridSquares[spacePicked].childCount; i++)
            {
              Destroy(gridSquares[spacePicked].transform.GetChild(i).gameObject);
            }
            highlighter.ShowHighlighted(spacePicked);
          }
          else
          {
            highlighter.isIncorrect[spacePicked] = true;
            if (gridSquares[spacePicked].childCount != 0)
            {
              for (int i = 0; i < gridSquares[spacePicked].childCount; i++)
              {
                Destroy(gridSquares[spacePicked].transform.GetChild(i).gameObject);
              }
            }
            highlighter.ShowHighlighted(spacePicked);
            if (gridSquares[spacePicked].Find(input.ToString()) == null)
            {
              GameManager.mistakes += 1;
              mc.SaveData();
            }
            GameObject prefab = Instantiate(numSprites[input - 1], gridSquares[spacePicked]);
            prefab.name = input.ToString();
            return false;
          }
        }
        else
        {
          highlighter.isIncorrect[spacePicked] = true;
          if (gridSquares[spacePicked].childCount != 0)
          {
            for (int i = 0; i < gridSquares[spacePicked].childCount; i++)
            {
              Destroy(gridSquares[spacePicked].transform.GetChild(i).gameObject);
            }
          }
          highlighter.ShowHighlighted(spacePicked);
          if (gridSquares[spacePicked].Find(input.ToString()) == null)
          {
            GameManager.mistakes += 1;
            mc.SaveData();
          }
          GameObject prefab = Instantiate(numSprites[input - 1], gridSquares[spacePicked]);
          prefab.name = input.ToString();
          return false;
        }
      }
      for (int i = 0; i < 9; i++)
      {
        child = gridSquares[spacePicked].Find(i.ToString() + "Note");
        children[i] = child;
        Destroy(children[i].gameObject);
      }
    }
    else
    {
      NoteIO(input);
    }
    return false;
  }

  void InitializeGrid()
  {
    for (int i = 0; i < 81; i++) // instantiate revealed nums
    {
      if (GRN.numsCorrect[i] && gridSquares[i] != null)
      {
        GameObject prefab = Instantiate(numSprites[grid[i] - 1], gridSquares[i]);
        prefab.name = grid[i].ToString()[0].ToString();
      }
    }
    bool[,] initialNotes = new bool[81, 9];
    initialNotes = SaveManager.LoadNotes(GameManager.ReturnLvlByScene());
    if (initialNotes != null)
    {
      for (int i = 0; i < 81; i++) // instantiate revealed notes
      {
        for (int j = 0; j < 9; j++)
        {
          if (initialNotes[i, j])
          {
            child = gridSquares[i].Find(j.ToString() + "Note");
            if (child == null)
            {
              GameObject prefab = Instantiate(numNoteSprites[j], gridSquares[i]);
              prefab.name = j.ToString() + "Note";
            }
            else
            {
              Destroy(child.gameObject);
            }
          }
        }
      }
    }
  }

  void NoteIO(int input)
  {
    child = gridSquares[spacePicked].Find(input.ToString());
    if (!GRN.numsCorrect[spacePicked] && !highlighter.isIncorrect[spacePicked])
    {
      child = gridSquares[spacePicked].Find(input.ToString() + "Note");
      int lvl = GameManager.ReturnLvlByScene();
      if (child == null)
      {
        GameObject prefab = Instantiate(numNoteSprites[input - 1], gridSquares[spacePicked]);
        prefab.name = input.ToString() + "Note";
        SaveManager.SaveNotes(lvl, input - 1, spacePicked);
      }
      else
      {
        SaveManager.SaveNotes(lvl, input - 1, spacePicked, false);
        Destroy(child.gameObject);
      }
    }
  }

  void ClearSameNums (int space, int input)
  {
    int rowOrigin = (space / 9) * 9;
    int colOrigin = space % 9;
    int boxOrigin = ((space % 9) / 3) * 3 + (space / 27) * 27;
    for (int i = 0; i < 81; i++)
    {
      if (i == space)
      {
        Transform[] tempSquares = new Transform[9];

        // find/destroy same nums in box
        for (int j = 0; j < 9; j++)
        {
          tempSquares[j] = gridSquares[boxOrigin + (j / 3) * 9 + (j % 3)];
          if (tempSquares[j] != gridSquares[spacePicked])
          {
            for (int k = 0; k < tempSquares[j].childCount; k++)
            {
              children[k] = tempSquares[j].GetChild(k);
              if (children[k].name == input.ToString() || children[k].name == input.ToString() + "Note")
              {
                highlighter.isIncorrect[boxOrigin + (j / 3) * 9 + (j % 3)] = false;
                Destroy(children[k].gameObject);
              }
            }
          }
        }

        // find/destroy same nums in row
        for (int j = 0; j < 9; j++)
        {
          tempSquares[j] = gridSquares[rowOrigin + j];
          if (tempSquares[j] != gridSquares[spacePicked])
          {
            for (int k = 0; k < tempSquares[j].childCount; k++)
            {
              children[k] = tempSquares[j].GetChild(k);
              if (children[k].name == input.ToString() || children[k].name == input.ToString() + "Note")
              {
                highlighter.isIncorrect[rowOrigin + j] = false;
                Destroy(children[k].gameObject);
              }
            }
          }
        }

        // find/destroy same nums in column
        for ( int j = 0; j < 9; j++)
        {
          tempSquares[j] = gridSquares[colOrigin + (9 * j)];
          if (tempSquares[j] != gridSquares[spacePicked])
          {
            for (int k = 0; k < tempSquares[j].childCount; k++)
            {
              children[k] = tempSquares[j].GetChild(k);
              if (children[k].name == input.ToString() || children[k].name == input.ToString() + "Note")
              {
                highlighter.isIncorrect[colOrigin + (9 * j)] = false;
                Destroy(children[k].gameObject);
              }
            }
          }
        }

        highlighter.ShowHighlighted(spacePicked);

      }
    }
  }

  void ClearNotesOfValue(int noteValue)
  {
    for (int i = 0; i < 81; i++)
    {
      if (gridSquares[i].childCount > 0)
      {
        for (int k = 0; k < gridSquares[i].childCount; k++)
        {
          children[k] = gridSquares[i].GetChild(k);
          if (children[k].name == (noteValue.ToString() + "Note"))
          {
            Destroy(children[k].gameObject);
          }
        }
      }
    }
  }



}
