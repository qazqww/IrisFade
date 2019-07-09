using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Iris : MonoBehaviour
{
    GridLayoutGroup gridLayout;

    [SerializeField]    int width = 50;
    [SerializeField]    int height = 50;
    int rowCount = 0;
    int colCount = 0;

    GameObject nodePrefab;
    [SerializeField]    Transform target;

    Image[,] nodeArr;

    void Init(int width = 50, int height = 50)
    {
        this.width = width;
        this.height = height;
        gridLayout = transform.GetComponentInChildren<GridLayoutGroup>();
        nodePrefab = Resources.Load("Node") as GameObject;

        CreateGrid();
    }

    void Start()
    {
        Init();        
    }

    void Update()
    {
        FindNode(target.position);
    }

    void CreateGrid()
    {
        Transform root = gridLayout.transform;

        int remainderW = Screen.width % width;
        int remainderH = Screen.height % height;

        colCount = Screen.width / width + (remainderW > 0 ? 1 : 0);
        rowCount = Screen.height / height + (remainderH > 0 ? 1 : 0);

        nodeArr = new Image[rowCount, colCount];
        gridLayout.constraintCount = colCount;

        for(int row = 0; row < rowCount; row++)
        {
            for(int col = 0; col < colCount; col++)
            {
                GameObject obj = Instantiate(nodePrefab, root);
                nodeArr[row, col] = obj.GetComponent<Image>();
            }
        }
    }

    public Image FindNode(Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        for(int row = 0; row < rowCount; row++)
        {
            for(int col = 0; col < colCount; col++)
            {
                Image image = nodeArr[row, col];
                Bounds bound = new Bounds(image.transform.position, image.rectTransform.sizeDelta);
                if (bound.Contains(worldPos))
                    image.color = Color.red;
                else
                    image.color = Color.black;
            }
        }
        return null;
    }
}

// UI
// Anchor : 스크린 상에서의 기준 값
// Pivot : 각 객체의 기준 값