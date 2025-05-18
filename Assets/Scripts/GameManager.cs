using System.Collections;
using System.Threading;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    int cnt = 0;
    public GameObject[] Tokens;
    private int NTP => 4;
    public static GameManager Instance;
    int numPlayer = 4;
    [HideInInspector] public int diceValue = 0;
    [HideInInspector] public int currentPlayer;

    private int MinDiceValue => 1;
    private int MaxDiceValue => 4;
    private void Awake()
    {
        currentPlayer = -1;
        Instance = this;
    }

    //Generating Token for each n player
    public void StartGame(int numPlayer)
    {
        this.numPlayer = numPlayer;
        for (int i = 0; i < numPlayer; i++)
        {
            int currentPos = BoardInfo.Instance.safePosIndex[i];
            for (int j = 0; j < NTP; j++)
            {
                GameObject currentToken = Instantiate(Tokens[i]);
                currentToken.transform.tag = i.ToString();
                currentToken.transform.SetParent(BoardInfo.Instance.outerLayer[currentPos]);
                currentToken.transform.localScale = Vector3.one;
                currentToken.GetComponent<RectTransform>().position = Vector2.zero;
                currentToken.AddComponent<PlayerMovement>();
                currentToken.GetComponent<PlayerMovement>().InitialzeInfo(i, currentPos);
            }
        }

        Invoke("OtherPlayerTurn", 1f);
    }

    //Function Managing Player Chance
    public void OtherPlayerTurn()
    {
        //using coroutine for smooth Dice Roll
        StartCoroutine(OtherPlayerTurnDelay());
    }
    public IEnumerator OtherPlayerTurnDelay()
    {
        currentPlayer++;
        currentPlayer %= numPlayer;

        Debug.Log(currentPlayer);
        #region Roll Dice

        yield return null;
        #endregion

        diceValue = GetRandomDiceValue();
        /*  cnt++;
          if (currentPlayer == 0)
          {
              diceValue = 18;
          }
          else if(cnt != 2 && cnt != 6)
          {
              diceValue = 20;
          }
          else
          {
              diceValue = 25;
          }*/
        // ManageTokenAnim(true);
    }

    //function to control Token Anim
    public void ManageTokenAnim(bool start) 
    {
        foreach (GameObject currentToken in GameObject.FindGameObjectsWithTag(currentPlayer.ToString()))
        {
            if(start)
                currentToken.GetComponent<PlayerMovement>().StartAnim();
            else
                currentToken.GetComponent<PlayerMovement>().StopAnim();
        }
    }

    //Function to get Dice Value
    public int GetRandomDiceValue()
    {
        return Random.RandomRange(MinDiceValue, MaxDiceValue);
    }
}
