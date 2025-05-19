using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    bool isSettingOn;
    bool isMusicOn;
    public bool isSoundOn;
    public bool isVibrationOn;

    string homeScene = "HomeScene";

    private GameObject homePanel;
    public GameObject settingPanelPrefab;
    private GameObject currentSettingPanel;
    private GameObject playerSelectionPanel;
    private Transform contentHolder;

    private Button settingButton;
    private Toggle musicButton;
    private Toggle vibrationButton;
    private Toggle soundButton;
    private Button playOffline;
    private Button playOnline;
    private Button home;
    private Button option;
    private Button shareButton;

    private Button previousButton;
    private Button startButton;


    private void Awake()
    {
        Debug.Log("Run Ho gaya");
        Instance = this;

        SetInitParamValue();
        GetSetUIComponents();
    }
    void SetInitParamValue()
    {
        isSettingOn = false;
        isMusicOn = true;
        isSoundOn = true;
        isVibrationOn = true;
    }
    void GetSetUIComponents()
    {
        if (homeScene == SceneManager.GetActiveScene().name)
        {
            playOffline = GameObject.FindGameObjectWithTag("playOffline").GetComponent<Button>();
            playOnline  = GameObject.FindGameObjectWithTag("playOnline").GetComponent<Button>();
            playOnline.onClick.AddListener(OnPlayOnlineButtonClikced);

            playerSelectionPanel = GameObject.FindGameObjectWithTag("playerSelectionPanel");
            homePanel = GameObject.FindGameObjectWithTag("homePanel");
            playerSelectionPanel.SetActive(false);
            playOffline.onClick.AddListener(OnPlayOfflineButtonClicked);
        }
        else
        {
            home = GetComponent<Button>();
            option = GetComponent<Button>();

            home.onClick.AddListener(OnHomeButtonClicked);
            option.onClick.AddListener(OnOptionsButtonClicked);
        }

        contentHolder = GameObject.FindGameObjectWithTag("contentHolder").transform;
        settingButton = GameObject.FindGameObjectWithTag("settingButton").GetComponent<Button>();
        settingButton.onClick.AddListener(OnSettingButtonClicked);
    }
    
    #region Home UI Button Functions
    public void OnSettingButtonClicked()
    {
        Debug.Log("On Setting Button Clicked");
        isSettingOn = ! isSettingOn;

        if (isSettingOn)
        {
            currentSettingPanel = Instantiate(settingPanelPrefab);
            currentSettingPanel.transform.SetParent(contentHolder);

            currentSettingPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            currentSettingPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);
            currentSettingPanel.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            GetSetSettingComponents();
        }
        else
        {
            ReleaseSettingComponents();
            Destroy(currentSettingPanel, .1f);
        }

    }
    public void OnPlayOfflineButtonClicked()
    {
        homePanel.SetActive(false);
        playerSelectionPanel.SetActive(true);

        GetSetSelectionPanelComponents();
    }

    void ReleaseSelectionPanelComponents()
    {
        previousButton.onClick.RemoveAllListeners();
        startButton.onClick.RemoveAllListeners();

        previousButton = null;
        startButton = null;
    }
    void GetSetSelectionPanelComponents()
    {
        previousButton = GameObject.FindGameObjectWithTag("previousButton").GetComponent<Button>();
        startButton = GameObject.FindGameObjectWithTag("startButton").GetComponent<Button>();

        previousButton.onClick.AddListener(OnPreviousButtonClicked);
        startButton.onClick.AddListener(OnStartButtonClicked);
    }
    public void OnPlayOnlineButtonClikced()
    {
        Debug.Log("Implement in Future");
    }
    #endregion

    #region Setting UI Buttons Functions
    void ReleaseSettingComponents()
    {
        musicButton = null;
        soundButton = null;
        vibrationButton = null;

        if (musicButton == null)
        {
            Debug.Log("Components Removed");
        }
        /*  vibrationButton.onClick.RemoveAllListeners();
           musicButton.onClick.RemoveAllListeners();
           soundButton.onClick.RemoveAllListeners();*/
        shareButton.onClick.RemoveAllListeners();
    }
    void GetSetSettingComponents()
    {
        musicButton = GameObject.FindGameObjectWithTag("musicButton").GetComponent<Toggle>();
        soundButton = GameObject.FindGameObjectWithTag("soundButton").GetComponent<Toggle>();
        vibrationButton = GameObject.FindGameObjectWithTag("vibrationButton").GetComponent<Toggle>();
        shareButton = GameObject.FindGameObjectWithTag("shareButton").GetComponent<Button>();

        /*  vibrationButton.onValueChanged.AddListener(OnVibrationButtonClicked);
          *//*musicButton.onClick.AddListener(OnMusicButtonClicked);
          soundButton.onClick.AddListener(OnSoundButtonClicked);*/

        shareButton.onClick.AddListener(OnshareButtonClicked);
    }

    public void OnshareButtonClicked()
    {
        Debug.Log("ShareFunctionality Added");
        //Implement Share functionality
    }

    public void OnSoundButtonClicked()
    {
        isSoundOn = !isSoundOn;
    }

    public void OnMusicButtonClicked()
    {
        isMusicOn = !isMusicOn;
        if (isMusicOn)
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioSource>().Play();
        else
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioSource>().Stop();
    }

    public bool OnVibrationButtonClicked()
    {
        isVibrationOn = !isVibrationOn;
        return true;
    }
    #endregion

    #region GamePlayOption Button
    public void OnOptionsButtonClicked()
    {
        
    }

    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    //Also Contain Setting Button
    #endregion

    #region playerSelectionPanel
    public void OnSelectPlayerCountClicked()
    {
        GameManager.Instance.StartGame(4);
    }

    public void OnPreviousButtonClicked()
    {
        ReleaseSelectionPanelComponents();
        homePanel.SetActive(true);
        playerSelectionPanel.SetActive(false);
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("GamePlayScene");
    }
    #endregion
}
