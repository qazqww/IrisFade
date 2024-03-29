﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Iris : MonoBehaviour
{
    GridLayoutGroup gridLayout;
    [SerializeField] Transform target;
    Image nodePrefab;
    Image[,] nodeArr;

    int size = 50;
    int rowCount = 0;
    int colCount = 0;
    int targetRow;
    int targetCol;
    bool isFade = false;

    Color transparent = new Color(0, 0, 0, 0);

    void Start()
    {
        Init();        
    }

    void Update()
    {
        if (!isFade)
            FindNode(target.position);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 200, 200), "FadeIn"))
        {
            isFade = true;
            StartCoroutine(FadeIn(targetRow, targetCol));
        }

        if (GUI.Button(new Rect(0, 200, 200, 200), "FadeOut"))
        {
            isFade = true;
            StartCoroutine(FadeOut(targetRow, targetCol));
        }
    }

    void Init(int size = 50)
    {
        this.size = size;
        gridLayout = GetComponentInChildren<GridLayoutGroup>(true);
        nodePrefab = Resources.Load<Image>("Node");
        
        CreateGrid();
    }

    void CreateGrid()
    {
        Transform root = gridLayout.transform;

        int remainderW = Screen.width % size;
        int remainderH = Screen.height % size;

        colCount = Screen.width / size + (remainderW > 0 ? 1 : 0);
        rowCount = Screen.height / size + (remainderH > 0 ? 1 : 0);

        nodeArr = new Image[rowCount, colCount];
        gridLayout.constraintCount = colCount;

        for(int col = 0; col < colCount; col++)
        {
            for(int row = 0; row < rowCount; row++)
            {
                Image image = Instantiate(nodePrefab, gridLayout.transform);
                image.name = string.Format("Node[{0}, {1}]", row, col);
                if(image != null)
                    nodeArr[row, col] = image;
            }
        }
    }

    public Image FindNode(Vector3 worldPos)
    {
        // 월드 좌표계를 스크린 좌표계로 변경 시
        // x,y값은 좌표값으로, z값은 카메라와의 거리값으로 설정

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        for(int row = 0; row < rowCount; row++)
        {
            for(int col = 0; col < colCount; col++)
            {
                Image image = nodeArr[row, col];
                Bounds bound = new Bounds(image.transform.position, image.rectTransform.sizeDelta);
                if (bound.Contains(worldPos))
                {
                    targetRow = row;
                    targetCol = col;                   
                    break;
                }
            }
        }
        return null;
    }

    bool CheckNode(int row, int col)
    {
        if (row < 0 || row >= rowCount || col < 0 || col >= colCount)
            return false;
        return true;
    }

    IEnumerator FadeIn(int row, int col)
    {
        nodeArr[row, col].color = transparent;
        yield return new WaitForSeconds(0.1f);
        for (int i = 1; i < colCount; i++)
        {
            // LEFT
            for(int r = -i; r <= i; r++)
            {
                if (CheckNode(row + r, col - i))
                    nodeArr[row + r, col - i].color = transparent;
            }

            // RIGHT
            for (int r = -i; r <= i; r++)
            {
                if (CheckNode(row + r, col + i))
                    nodeArr[row + r, col + i].color = transparent;
            }      
            
            // UP
            for (int c = -i; c <= i; c++)
            {
                if (CheckNode(row - i, col + c))
                    nodeArr[row - i, col + c].color = transparent;
                
            }

            // DOWN
            for (int c = -i; c <= i; c++)
            {
                if (CheckNode(row + i, col + c))
                    nodeArr[row + i, col + c].color = transparent;
            }
            yield return new WaitForSeconds(0.05f);
        }
        isFade = false;
    }

    IEnumerator FadeOut(int row, int col)
    {
        int count = Mathf.Max(row, rowCount - row, col, colCount - col);
        for (int i = count; i > 0; i--)
        {
            // LEFT
            for (int r = -i; r <= i; r++)
            {
                if (CheckNode(row + r, col - i))
                    nodeArr[row + r, col - i].color = Color.black;
            }

            // RIGHT
            for (int r = -i; r <= i; r++)
            {
                if (CheckNode(row + r, col + i))
                    nodeArr[row + r, col + i].color = Color.black;
            }

            // UP
            for (int c = -i; c <= i; c++)
            {
                if (CheckNode(row - i, col + c))
                    nodeArr[row - i, col + c].color = Color.black;

            }

            // DOWN
            for (int c = -i; c <= i; c++)
            {
                if (CheckNode(row + i, col + c))
                    nodeArr[row + i, col + c].color = Color.black;
            }
            yield return new WaitForSeconds(0.05f);
        }
        nodeArr[row, col].color = Color.black;
        isFade = false;
    }
}

// UI
// Anchor : 스크린 상에서의 기준 값
// Pivot : 각 객체의 기준 값