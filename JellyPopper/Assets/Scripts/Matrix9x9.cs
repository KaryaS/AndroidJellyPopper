using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix9x9 {

    private JELLYTYPE[,] Grid = new JELLYTYPE[9, 5];
    public GameObject[,] GridGame = new GameObject[9, 5];
    private Vector2[,] GridPos = new Vector2[9, 5];
    int iXGridSize = 8;
    int iYGridSize = 4;
    private float fJellyRadius = 0.695f;
    public bool bRotating = false;

    public enum JELLYTYPE
    {
        NONE,
        YELLOW,
        GREEN,
        RED,
        BLUE,
        PINK,
        BLACK,
        MULTI
    }

    public Matrix9x9()
    {
        SetZero();
    }

    public void SetZero()
    {
        foreach (JELLYTYPE j in Grid)
        {
            j.Equals(JELLYTYPE.NONE);
        }
         Vector2 v2GridOrigin = new Vector2(-2.756714f, 2.744076f);
        for (int x = 0; x < iXGridSize + 1; x++)
        {
            for(int y = 0; y < iYGridSize +1; y++)
            {
                GridPos[x,y] = new Vector2(v2GridOrigin.x + (fJellyRadius * x), v2GridOrigin.y - (fJellyRadius * y));
            }
        }

    }


    public void RotateGrid()
    {
        bRotating = true;
        for (int x = 0; x < iXGridSize; x++)
        {
            for (int y = 0; y < iYGridSize; y++)
            {
                if (GridGame[x, y] != null)
                {
                    GridGame[x, y].transform.position = GridPos[iXGridSize - y, iYGridSize - x];
                    GridGame[x, y].GetComponent<CS_Jelly>().v2BoardPos = new Vector2(iXGridSize - y, iYGridSize - x);
                    //SetValue(x, y, TempStore[x, y].GetComponent<CS_Jelly>().Colour, TempStore[iGridSize - y, iGridSize - x]);
                    //SetValue(iGridSize - y, iGridSize - x, TempStore[x, y].GetComponent<CS_Jelly>().Colour, TempStore[x, y]);
                    //Doesnt work
                    //GameObject OppositeJelly;
                    //if (GridGame[iGridSize - y, iGridSize - x])
                    //{
                    //    OppositeJelly = Object.Instantiate(GridGame[iGridSize - y, iGridSize - x]);
                    //}
                    //else
                    //{
                    //    OppositeJelly = null;
                    //}
                    //GameObject JellyReplace = Object.Instantiate(GridGame[x, y]);
                    //Object.Destroy(GridGame[x, y]);
                    //GridGame[x, y] = OppositeJelly;
                    //if(GridGame[iGridSize - y, iGridSize - x])
                    //{
                    //    Object.Destroy(GridGame[iGridSize - y, iGridSize - x]);
                    //}
                    //GridGame[iGridSize - y, iGridSize - x] = JellyReplace;

                }

            }
        }
        JELLYTYPE[,] TempColStore = Grid;
        for (int x = 0; x < iXGridSize; x++)
        {
            for (int y = 0; y < iYGridSize; y++)
            {
               Grid[x, y] = TempColStore[iXGridSize - x, iYGridSize - y];
            }
        }
        bRotating = false;
    }


    public void ClearPosition(Vector2 a_v2)
    {
        Grid[(int)a_v2.x, (int)a_v2.y] = JELLYTYPE.NONE;
        GridGame[(int)a_v2.x, (int)a_v2.y] = null;
    }

    public JELLYTYPE GetValue(int a_row, int a_column)
    {
        JELLYTYPE J = Grid[a_row, a_column];
        return J; 
    }

    public JELLYTYPE GetValue(Vector2 a_v2)
    {
        JELLYTYPE J = Grid[(int)a_v2.x, (int)a_v2.y];
        return J;
    }

    public GameObject GetGameObject(Vector2 a_v2)
    {
        GameObject g = GridGame[(int)a_v2.x, (int)a_v2.y];
        return g;
    }

    public GameObject GetGameObject(int a_row, int a_column)
    {
        if(a_row > 8 || a_column > 4 || a_column < 0 || a_row < 0)
        {
            return null;
        }
        GameObject g = GridGame[a_row, a_column];
        return g;
    }

    public void SetValue(int a_row, int a_column, JELLYTYPE a_jelly, GameObject a_gameobject)
    {
        Grid[a_row, a_column] = a_jelly;
        GridGame[a_row, a_column] = a_gameobject;

    }

    public void SetValue(Vector2 a_v2, JELLYTYPE a_jelly, GameObject a_gameobject)
    {
        Grid[(int)a_v2.x, (int)a_v2.y] = a_jelly;
        GridGame[(int)a_v2.x, (int)a_v2.y] = a_gameobject;
    }

}
