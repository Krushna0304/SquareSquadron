using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void OnSelectPlayerCountClicked()
    {
        GameManager.Instance.StartGame(4);
    }
}
