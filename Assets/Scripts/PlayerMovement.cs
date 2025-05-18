using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : BoardMovementManager
{
    [HideInInspector] public int currentPos = 0;

    public bool IsAlive;
    public int playerScore;
    //number of steps away from Home
    [HideInInspector] public int distHome;
    [HideInInspector] public int playerId;

    Button button;
    //Is First Enemy hit
    bool isFEH;

    public void InitialzeInfo(int playerId,int currentPos)
    {
        isFEH = false;
        IsAlive = true;
        distHome = 0;
        playerScore = 0;
        this.playerId = playerId;
        this.currentPos = currentPos;

        button = this.AddComponent<Button>();
        button.onClick.AddListener(OnTouch);
    }
    public void MovePlayerForward(int steps)
    {
        StartCoroutine(SmoothPlayerMovement(steps));
    }

    IEnumerator SmoothPlayerMovement(int steps)
    {
        bool isPossible = true;
        if (!IsStepPossisble(distHome,steps))
            isPossible = false;
        
        //Move player
        while (isPossible && steps > 0)
        {
            if (isFEH && distHome > BoardInfo.Instance.entryDist - 1)
            {
                if (distHome > 22)
                {
                    this.transform.SetParent(BoardInfo.Instance.destination);
                    break;
                }
                    
                if (distHome == (BoardInfo.Instance.entryDist))
                {
                    currentPos = EnterIntoInnerLayer(playerId);
                }
                else 
                {
                    currentPos++;
                    currentPos %= BoardInfo.Instance.innerLayer.Length;
                }
                this.transform.SetParent(BoardInfo.Instance.innerLayer[currentPos]);
            }
            else
            {
                if (distHome > BoardInfo.Instance.entryDist - 1)
                    distHome = 0;
                currentPos++;
                currentPos %= (BoardInfo.Instance.entryDist + 1) ;
                this.transform.SetParent(BoardInfo.Instance.outerLayer[currentPos]);
            }
            yield return new WaitForSeconds(delay);
            distHome++;
            steps--;
        }

        if (IsDestinationArrive(this.transform))
        {
            //Increase Score
            playerScore += BoardInfo.Instance.hitScore;
            IsAlive = false;

            //Celebration Animation?
        }


        if (IsEnemyEncounter(this.transform, playerId) && !IsSafePos(this.transform))
        {
            isFEH = true;
            playerScore += BoardInfo.Instance.hitScore;

            //Move another player tokens to home in reverse path
            int childCount = this.transform.parent.childCount;
            for (int i = 0; i < childCount - 1 ; i++)
            {   
              Transform child = this.transform.parent.GetChild(0);
              StartCoroutine(GoToHome(child));
              yield return new WaitForSeconds(delay);
            }

            //Player Hit Animation?
        }

        GameManager.Instance.OtherPlayerTurn();
    }


    //Add event

    public void OnTouch()
    {
        //GameManager.Instance.ManageTokenAnim(false);
        if (GameManager.Instance.currentPlayer == playerId)
        {
            MovePlayerForward(GameManager.Instance.diceValue);
        }
    }

    public void StartAnim()
    {
        if(IsAlive)
            GetComponent<Animator>().SetBool("myTurn",true);
    }

    public void StopAnim()
    {
        if (IsAlive) 
            GetComponent<Animator>().SetBool("myTurn",false);
    }

}
