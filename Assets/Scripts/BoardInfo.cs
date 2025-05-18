using UnityEngine;
using UnityEngine.UI;

public class BoardInfo : MonoBehaviour
{
    [Tooltip("Getting Score After Reaching one token at destination")]
    public int winScore;

    [Tooltip("Getting Score after hitting enemy as a Reward")]
    public int hitScore;

    [Tooltip("Delay in Player Movement")]
    public float delay;

    public static BoardInfo Instance;
    [HideInInspector] public int entryDist = 15;
    [HideInInspector] public int destDist = 24;

    private int index = 0;

    [HideInInspector] public int[] playerEnteredIndex;
    [HideInInspector] public int[] playerExitIndex;
    [HideInInspector] public Transform[][] boardGrid;
    [HideInInspector] public Transform[] outerLayer;
    [HideInInspector] public Transform[] innerLayer;
    [HideInInspector] public int[] safePosIndex;
    [HideInInspector] public int nSafePos;
    [HideInInspector] public Transform destination;


    [SerializeField] private Transform board;
    [SerializeField] private GameObject cellPrefab;

    //Create board of 5 x 5 Size
    private int Size => 5;
    private int Space => 20;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        delay = .2f;
        ManageBoardProperties();
        CreateGrid();
        OrganizeBoard();
        SetSafePos();
        SetEnteredIndex();
        SetExitIndex();
        SetDestiantion();
    }
    void ManageBoardProperties()
    {
        board = GameObject.FindGameObjectWithTag("board").transform;
    }
    void CreateGrid()
    {
        boardGrid = new Transform[Size][];
        for (int i = 0; i < Size; i++)
        {
            boardGrid[i] = new Transform[Size];
            for (int j = 0; j < Size; j++)
            {
                GameObject currentCell = Instantiate(cellPrefab);
                boardGrid[i][j] = currentCell.transform;
                currentCell.transform.SetParent(board);
                currentCell.transform.localScale = Vector3.one;

                currentCell.AddComponent<GridLayoutGroup>();
                currentCell.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.Flexible;
                currentCell.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
                currentCell.GetComponent<GridLayoutGroup>().padding = new RectOffset(Space,Space,Space,Space);
                currentCell.GetComponent<GridLayoutGroup>().spacing = new Vector2(Space,Space);
            }
        }
    }
    void OrganizeBoard()
    {
        OrganizeInnerLayer();
        SetDestiantion();
        OrganizeOuterLayer();
    }
    void SetSafePos()
    {
        safePosIndex = new int[4];
        safePosIndex[0] = 2;
        safePosIndex[1] = 6;
        safePosIndex[2] = 10;
        safePosIndex[3] = 14;
    }
    void SetEnteredIndex()
    {
        playerEnteredIndex = new int[4];
        playerEnteredIndex[0] = 1;
        playerEnteredIndex[1] = 5;
        playerEnteredIndex[2] = 9;
        playerEnteredIndex[3] = 13;
    }

    void SetExitIndex()
    {
        playerExitIndex = new int[4];
        playerExitIndex[0] = 0;
        playerExitIndex[1] = 6;
        playerExitIndex[2] = 4;
        playerExitIndex[3] = 2;
    }
    void OrganizeOuterLayer()
    {
        outerLayer = new Transform[16];
        index = 0;
        int i = 0, j = 0;

        while (i < Size)
        {
            outerLayer[index++] = boardGrid[i++][j];
        }
        j++;
        i--;

        while (j < Size)
        {
            outerLayer[index++] = boardGrid[i][j++];
        }
        i--;
        j--;

        while (i >= 0)
        {
            outerLayer[index++] = boardGrid[i--][j];
        }
        j--;
        i++;

        while (j > 0)
        {
            outerLayer[index++] = boardGrid[i][j--];
        }
    }
    void OrganizeInnerLayer()
    {
        innerLayer = new Transform[8];
        int i = 1, j = 1;
        index = 0;
        while (j < Size - 1 )
        {
            print(index +"     " + i +"     " + j);
            innerLayer[index++] = boardGrid[i][j++];
        }
        i++;
        j--;

        while (i < Size - 1 )
        {
            print(index + "     " + i + "     " + j);
            innerLayer[index++] = boardGrid[i++][j];
        }
        j--;
        i--;

        while (j > 0)
        {
            print(index + "     " + i + "     " + j);
            innerLayer[index++] = boardGrid[i][j--];
        }
        i--;
        j++;

        while (i > 1)
        {
            print(index + "     " + i + "     " + j);
            innerLayer[index++] = boardGrid[i--][j];
        }
    }
    void SetDestiantion()
    {
        destination = boardGrid[Size/2][Size/2];
    }
}
