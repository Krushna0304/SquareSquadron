using System.Collections;
using UnityEngine;

public class BoardMovementManager : MonoBehaviour
{
    protected float delay;

    private void Start()
    {
        delay = BoardInfo.Instance.delay;   
    }
    protected bool IsStepPossisble(int distHome,int steps)
    {
        if(distHome + steps <= BoardInfo.Instance.destDist)
            return true;
        return false;
       
    }
    protected bool IsEnemyEncounter(Transform currentPlayer,int playerId)
    {
        if(currentPlayer.parent.transform.GetChild(0).transform.GetComponent<PlayerMovement>().playerId != playerId)
            return true;
        return false;
    }

    protected bool IsSafePos(Transform currentPos)
    {
        for (int i = 0; i < BoardInfo.Instance.safePosIndex.Length; i++)
        {
            if (currentPos == BoardInfo.Instance.outerLayer[BoardInfo.Instance.safePosIndex[i]])
                return true;
        }
        return false;
    }

    protected bool IsDestinationArrive(Transform currentPos)
    {
        if(currentPos == BoardInfo.Instance.destination)
            return true;
        return false;
    }

    protected int EnterIntoInnerLayer(int playerId)
    {
        return BoardInfo.Instance.playerExitIndex[playerId];
    }
    protected int ExitToInnerLayer(int playerId)
    {
        return BoardInfo.Instance.playerEnteredIndex[playerId];
    }

    protected IEnumerator GoToHome(Transform other)
    {
        int playerId = other.GetComponent<PlayerMovement>().playerId;
        int distHome = other.GetComponent<PlayerMovement>().distHome;
        int currentPos = other.GetComponent<PlayerMovement>().currentPos;

        while (distHome > 0)
        {
            if (distHome > BoardInfo.Instance.entryDist + 1)
            {                
                currentPos--;
                if(currentPos == -1)
                     currentPos = BoardInfo.Instance.innerLayer.Length - 1;
                
                other.transform.SetParent(BoardInfo.Instance.innerLayer[currentPos]);
            }
            else
            {
                if (distHome == (BoardInfo.Instance.entryDist + 1))
                {
                    currentPos = ExitToInnerLayer(playerId);
                }
                else
                { 
                    currentPos--;
                    if (currentPos == -1)
                        currentPos = BoardInfo.Instance.entryDist;
                }
                other.transform.SetParent(BoardInfo.Instance.outerLayer[currentPos]);
            }
            yield return new WaitForSeconds(delay);
            distHome--;
        }
        other.GetComponent<PlayerMovement>().currentPos = currentPos;
        other.GetComponent<PlayerMovement>().distHome = distHome;

    }
}
