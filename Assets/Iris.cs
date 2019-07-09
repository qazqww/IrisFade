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

    void Start()
    {
        Init();        
    }

    void Update()
    {
        FindNode(target.position);
    }

    void Init(int width = 50, int height = 50)
    {
        this.width = width;
        this.height = height;
        gridLayout = transform.GetComponentInChildren<GridLayoutGroup>();
        nodePrefab = Resources.Load("Node") as GameObject;

        CreateGrid();
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
                    image.color = Color.red;
                else
                    image.color = Color.black;
            }
        }
        return null;
    }

    public Image FadeNode(Vector3 worldPos)
    {
        // 월드 좌표계를 스크린 좌표계로 변경 시
        // x,y값은 좌표값으로, z값은 카메라와의 거리값으로 설정

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                Image image = nodeArr[row, col];
                Bounds bound = new Bounds(image.transform.position, image.rectTransform.sizeDelta);
                if (bound.Contains(Input.mousePosition) && Input.GetMouseButtonDown(0))
                {
                    image.color = Color.red;
                    Image[] neighbors = {nodeArr[row-1, col], nodeArr[row+1, col],
                                        nodeArr[row, col-1], nodeArr[row, col+1] };
                    for(int i=0; i<neighbors.Length; i++)
                    {

                    }
                }
            }
        }
        return null;
    }

    public void FadeNodeMore(Image node, int row, int col)
    {
        node.color = Color.red;
        Image[] neighbors = { nodeArr[row-1, col], nodeArr[row+1, col],
                              nodeArr[row, col-1], nodeArr[row, col+1] };
    }

    // 마우스로 클릭하면 색이 변하고
    // 주변의 노드들 4개를 얻어와 (nodeArr)
    // 그 노드를 담아 다시 함수에 넣는다
}

// UI
// Anchor : 스크린 상에서의 기준 값
// Pivot : 각 객체의 기준 값