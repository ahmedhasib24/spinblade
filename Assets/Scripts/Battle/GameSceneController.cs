using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameSceneController : MonoBehaviour
{
    private static GameSceneController _instance;

    public static GameSceneController Instance
    {
        get { return _instance; }
    }

    public CameraFollower cameraFollower;

    public int stadiumIndex = 0;
    public List<GameObject> stadiums;

    public SpinBlade player;
    public List<SpinBlade> allBlades;
    public int playerIndex = 0;
    public int currentOpponentIndex = 0;
    public List<SpinBlade> currentOpponents = new List<SpinBlade>();

    public GameSceneUIController uiController;

    public List<SpinController> blades;
    public List<GameObject> attackRings;
    public List<GameObject> weightDisks;
    public List<GameObject> baseRings;
    public List<GameObject> characters;

    public Vector3 playerInitialPosition;
    public List<Vector3> opponentInitialPositions;

    public SpinController playerBlade;
    public List<SpinController> opponentBlades;

    public float vsPanelShowTime = 5f;
    public bool isGameStarted = false;
    public bool isGameOver = false;
    public bool isPaused = false;

    //Fireballs
    public GameObject fireballPrefab;
    public List<GameObject> fireballsPool;
    public int maxFireballs = 6;

    //Fireball effect
    public GameObject fireballEffectPrefab;
    public List<GameObject> fireballEffectsPool;
    public int maxFireballEffects = 6;

    //Fireball effect
    public GameObject collisionEffectPrefab;
    public List<GameObject> collisionEffectsPool;
    public int maxCollisionEffect = 6;

    public int opponentCount = 0;

    public int opponentEliminated = 0;

    public GameMode gameMode;

    //Icons
    public SpinController playerIcon;
    public SpinController opponent1Icon;
    public SpinController opponent2Icon;
    public SpinController opponent3Icon;

    //Icon positions
    public Transform VSPlayerIconPos;
    public Transform VSOpponentIconPos;

    public Transform GamePlayerIconPos;
    public Transform GameOpponent1IconPos;
    public Transform GameOpponent2IconPos;
    public Transform GameOpponent3IconPos;


    GameObject playerCharacter;
    GameObject opponentCharacter;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        stadiumIndex = PlayerPrefs.GetInt("SelectedArena");
        playerIndex = PlayerPrefs.GetInt("SelectedSpin");
        //Debug.Log(playerIndex);
        gameMode = GameController.Instance.gameMode;
        if (gameMode != GameMode.Arcade)
        {
            //Debug.Log("CurrentOpponentIndex");
            currentOpponentIndex = PlayerPrefs.GetInt("CurrentOpponentIndex", 0);
        }
        InitializeGame();
    }

    void InitializeGame()
    {
        stadiums[stadiumIndex].SetActive(true);

        allBlades = GameController.Instance.allBlades;
        player = allBlades[playerIndex];
        if (gameMode != GameMode.Arcade)
        {
            opponentCount = 1;
            maxFireballs = 6;
            maxFireballEffects = 6;
            maxCollisionEffect = 2;
            currentOpponents.Add(GameController.Instance.basicBlades[currentOpponentIndex]);
            //Instantiate(attackRings[]);
            playerIcon = Instantiate(blades[player.id], Vector3.zero, Quaternion.identity) as SpinController;
            playerIcon.gameObject.GetComponent<SpinController>().enabled = false;
            playerIcon.gameObject.GetComponent<TrailRenderer>().enabled = false;
            playerIcon.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            playerIcon.gameObject.layer = LayerMask.NameToLayer("Water");
            Instantiate(attackRings[allBlades[player.id].attackRing.id], playerIcon.transform).layer = LayerMask.NameToLayer("Water");
            playerCharacter = Instantiate(characters[allBlades[player.id].attackRing.id], playerIcon.transform);
            playerCharacter.layer = LayerMask.NameToLayer("Water");
            playerCharacter.transform.localPosition = new Vector3(0f, 0.06f, 0f);
            playerCharacter.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            //playerCharacter.transform.localScale = new Vector3(15f, 15f, 15f);
            Instantiate(weightDisks[allBlades[player.id].weightDisk.id], playerIcon.transform).layer = LayerMask.NameToLayer("Water");
            Instantiate(baseRings[allBlades[player.id].baseRing.id], playerIcon.transform).layer = LayerMask.NameToLayer("Water");
            //playerIcon.transform.position = new Vector3(-6.5f, 27f, 0f);
            playerIcon.transform.position = VSPlayerIconPos.position;

            opponent1Icon = Instantiate(blades[currentOpponents[0].id], Vector3.zero, Quaternion.identity) as SpinController;
            opponent1Icon.gameObject.GetComponent<SpinController>().enabled = false;
            opponent1Icon.gameObject.GetComponent<TrailRenderer>().enabled = false;
            opponent1Icon.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            opponent1Icon.gameObject.layer = LayerMask.NameToLayer("Water");
            Instantiate(attackRings[allBlades[currentOpponents[0].id].attackRing.id], opponent1Icon.transform).layer = LayerMask.NameToLayer("Water");
            opponentCharacter = Instantiate(characters[allBlades[currentOpponents[0].id].attackRing.id], opponent1Icon.transform);
            opponentCharacter.layer = LayerMask.NameToLayer("Water");
            opponentCharacter.transform.localPosition = new Vector3(0f, 0.06f, 0f);
            opponentCharacter.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            //opponentCharacter.transform.localScale = new Vector3(15f, 15f, 15f);
            Instantiate(weightDisks[allBlades[currentOpponents[0].id].weightDisk.id], opponent1Icon.transform).layer = LayerMask.NameToLayer("Water");
            Instantiate(baseRings[allBlades[currentOpponents[0].id].baseRing.id], opponent1Icon.transform).layer = LayerMask.NameToLayer("Water");
            //opponent1Icon.transform.position = new Vector3(6.6f, 26.2f, 0f);
            opponent1Icon.transform.position = VSOpponentIconPos.position;
            uiController.ShowVSPanel();
        }
        else
        {
            opponentCount = 3;
            maxFireballs = 12;
            maxFireballEffects = 12;
            maxCollisionEffect = 8;
        }
        CreateFireballs();
        CreateFireballEffects();
        StartCoroutine(LoadVSPlay());
    }

    void CreateFireballs()
    {
        for (int i = 0; i < maxFireballs; i++)
        {
            GameObject fb = Instantiate(fireballPrefab);
            fb.SetActive(false);
            fireballsPool.Add(fb);
        }
    }

    void CreateFireballEffects()
    {
        for (int i = 0; i < maxFireballEffects; i++)
        {
            GameObject fb = Instantiate(fireballEffectPrefab);
            fb.SetActive(false);
            fireballEffectsPool.Add(fb);
        }
    }

    void CreateCollisionEffects()
    {
        for (int i = 0; i < maxCollisionEffect; i++)
        {
            GameObject fb = Instantiate(collisionEffectPrefab);
            fb.SetActive(false);
            collisionEffectsPool.Add(fb);
        }
    }

    public GameObject GetFireball()
    {
        for (int i = 0; i < fireballsPool.Count; i++)
        {
            if (fireballsPool[i].activeInHierarchy == false)
            {
                return fireballsPool[i];
            }
        }
        return Instantiate(fireballPrefab);
    }

    public GameObject GetFireballEffect()
    {
        for (int i = 0; i < fireballEffectsPool.Count; i++)
        {
            if (fireballEffectsPool[i].activeInHierarchy == false)
            {
                return fireballEffectsPool[i];
            }
            //else
            //    return Instantiate(fireballEffectPrefab);
        }
        return Instantiate(fireballEffectPrefab); ;
    }

    public GameObject GetCollisionEffect()
    {
        for (int i = 0; i < collisionEffectsPool.Count; i++)
        {
            if (collisionEffectsPool[i].activeInHierarchy == false)
            {
                return collisionEffectsPool[i];
            }
            //else
            //return Instantiate(collisionEffectPrefab);
        }
        return Instantiate(collisionEffectPrefab);
    }

    public void LoadBattleScene()
    {
        SceneManager.LoadScene("Battle");
    }

    public void LoadArcadeScene()
    {
        SceneManager.LoadScene("Arcade");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    IEnumerator LoadVSPlay()
    {
        if (gameMode != GameMode.Arcade)
        {
            yield return new WaitForSeconds(vsPanelShowTime);
        }
        //currentOpponents = new List<SpinBlade>();
        opponentBlades = new List<SpinController>();
        for (int i = 0; i < opponentCount; i++)
        {
            int rand = Random.Range(0, 9);
            if (gameMode != GameMode.Arcade)
            {
                rand = currentOpponentIndex;
            }
            SpinController opponent = Instantiate(blades[rand], opponentInitialPositions[i], Quaternion.identity) as SpinController;
            Instantiate(attackRings[allBlades[rand].attackRing.id], opponent.transform);
            Instantiate(weightDisks[allBlades[rand].weightDisk.id], opponent.transform);
            Instantiate(baseRings[allBlades[rand].baseRing.id], opponent.transform);
            opponentBlades.Add(opponent);
            if (gameMode == GameMode.Arcade)
            {
                currentOpponents.Add(allBlades[rand]);
            }
            opponentBlades[i].bladeData = allBlades[rand];
            opponentBlades[i].isAI = true;
            opponentBlades[i].id = i;
        }

        playerBlade = Instantiate(blades[player.id], playerInitialPosition, Quaternion.identity) as SpinController;
        Instantiate(attackRings[player.attackRing.id], playerBlade.transform);
        Instantiate(weightDisks[player.weightDisk.id], playerBlade.transform);
        Instantiate(baseRings[player.baseRing.id], playerBlade.transform);
        playerBlade.targets = opponentBlades;
        playerBlade.bladeData = player;

        if (gameMode == GameMode.Arcade)
        {
            CreatePlayerIcons();
        }


        for (int i = 0; i < opponentCount; i++)
        {
            for (int j = 0; j < opponentCount; j++)
            {
                opponentBlades[i].targets.Add(playerBlade);
                if (i != j)
                {
                    opponentBlades[i].targets.Add(opponentBlades[j]);
                }
            }
        }
        //opponentBlades[i].targets.Add(playerBlade);
        //playerBlade.gameObject.SetActive(false);
        //playerBlade.transform.position = playerInitialPosition;
        //playerBlade.gameObject.SetActive(true);



        //opponentBlade.gameObject.SetActive(false);
        //opponentBlade.transform.position = opponentInitialPosition;
        //opponentBlade.gameObject.SetActive(true);

        cameraFollower.target = playerBlade.transform;
        if (gameMode != GameMode.Arcade)
        {
            uiController.CloseVSPanel();
            //playerIcon.transform.position = new Vector3(-7.1f, 30.6f, 0f);
            playerIcon.transform.position = GamePlayerIconPos.position;
            playerIcon.transform.localScale = new Vector3(10f, 10f, 10f);
            //opponent1Icon.transform.position = new Vector3(7.1f, 30.6f, 0f);
            opponent1Icon.transform.position = GameOpponent1IconPos.position;
            opponent1Icon.transform.localScale = new Vector3(10f, 10f, 10f);
            playerCharacter.SetActive(false);
            opponentCharacter.SetActive(false);
        }
        uiController.ShowGamePanel();
        isGameStarted = true;
        //StartCoroutine(UpdateEnergies());
    }

    private void Update()
    {
        UpdateEnergies();
        if (CrossPlatformInputManager.GetButtonDown("Pause"))
        {
            TogglePause();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void CreatePlayerIcons()
    {
        playerIcon = Instantiate(blades[player.id], Vector3.zero, Quaternion.identity) as SpinController;
        playerIcon.gameObject.GetComponent<SpinController>().enabled = false;
        playerIcon.gameObject.GetComponent<TrailRenderer>().enabled = false;
        playerIcon.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        playerIcon.gameObject.layer = LayerMask.NameToLayer("Water");
        Instantiate(attackRings[allBlades[player.id].attackRing.id], playerIcon.transform).layer = LayerMask.NameToLayer("Water");
        Instantiate(weightDisks[allBlades[player.id].weightDisk.id], playerIcon.transform).layer = LayerMask.NameToLayer("Water");
        Instantiate(baseRings[allBlades[player.id].baseRing.id], playerIcon.transform).layer = LayerMask.NameToLayer("Water");
        //playerIcon.transform.position = new Vector3(-7.1f, 30.6f, 0f);
        playerIcon.transform.position = GamePlayerIconPos.position;
        playerIcon.transform.localScale = new Vector3(10f, 10f, 10f);

        opponent1Icon = Instantiate(blades[currentOpponents[0].id], Vector3.zero, Quaternion.identity) as SpinController;
        opponent1Icon.gameObject.GetComponent<SpinController>().enabled = false;
        opponent1Icon.gameObject.GetComponent<TrailRenderer>().enabled = false;
        opponent1Icon.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        opponent1Icon.gameObject.layer = LayerMask.NameToLayer("Water");
        Instantiate(attackRings[allBlades[currentOpponents[0].id].attackRing.id], opponent1Icon.transform).layer = LayerMask.NameToLayer("Water");
        Instantiate(weightDisks[allBlades[currentOpponents[0].id].weightDisk.id], opponent1Icon.transform).layer = LayerMask.NameToLayer("Water");
        Instantiate(baseRings[allBlades[currentOpponents[0].id].baseRing.id], opponent1Icon.transform).layer = LayerMask.NameToLayer("Water");
        //opponent1Icon.transform.position = new Vector3(7.1f, 30.6f, 0f);
        opponent1Icon.transform.position = GameOpponent1IconPos.position;
        opponent1Icon.transform.localScale = new Vector3(10f, 10f, 10f);

        opponent2Icon = Instantiate(blades[currentOpponents[1].id], Vector3.zero, Quaternion.identity) as SpinController;
        opponent2Icon.gameObject.GetComponent<SpinController>().enabled = false;
        opponent2Icon.gameObject.GetComponent<TrailRenderer>().enabled = false;
        opponent2Icon.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        opponent2Icon.gameObject.layer = LayerMask.NameToLayer("Water");
        Instantiate(attackRings[allBlades[currentOpponents[1].id].attackRing.id], opponent2Icon.transform).layer = LayerMask.NameToLayer("Water");
        Instantiate(weightDisks[allBlades[currentOpponents[1].id].weightDisk.id], opponent2Icon.transform).layer = LayerMask.NameToLayer("Water");
        Instantiate(baseRings[allBlades[currentOpponents[1].id].baseRing.id], opponent2Icon.transform).layer = LayerMask.NameToLayer("Water");
        //opponent2Icon.transform.position = new Vector3(-7.1f, 28.6f, 0f);
        opponent2Icon.transform.position = GameOpponent2IconPos.position;
        opponent2Icon.transform.localScale = new Vector3(10f, 10f, 10f);

        opponent3Icon = Instantiate(blades[currentOpponents[2].id], Vector3.zero, Quaternion.identity) as SpinController;
        opponent3Icon.gameObject.GetComponent<SpinController>().enabled = false;
        opponent3Icon.gameObject.GetComponent<TrailRenderer>().enabled = false;
        opponent3Icon.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        opponent3Icon.gameObject.layer = LayerMask.NameToLayer("Water");
        Instantiate(attackRings[allBlades[currentOpponents[2].id].attackRing.id], opponent3Icon.transform).layer = LayerMask.NameToLayer("Water");
        Instantiate(weightDisks[allBlades[currentOpponents[2].id].weightDisk.id], opponent3Icon.transform).layer = LayerMask.NameToLayer("Water");
        Instantiate(baseRings[allBlades[currentOpponents[2].id].baseRing.id], opponent3Icon.transform).layer = LayerMask.NameToLayer("Water");
        //opponent3Icon.transform.position = new Vector3(7.1f, 28.6f, 0f);
        opponent3Icon.transform.position = GameOpponent3IconPos.position;
        opponent3Icon.transform.localScale = new Vector3(10f, 10f, 10f);
    }

    void UpdateEnergies()
    {
        if (GameSceneController.Instance.isGameStarted == false || GameSceneController.Instance.isPaused == true)
        {
            return;
        }
        uiController.UpdateEnergyBars(playerBlade, opponentBlades);

        if (playerBlade.Energy >= player.skillCost)
        {
            uiController.UpdateSkillButton(true);
        }
        else
        {
            uiController.UpdateSkillButton(false);
        }
        if (playerBlade.Energy >= player.attackCost)
        {
            uiController.UpdateAttackButton(true);
        }
        else
        {
            uiController.UpdateAttackButton(false);
        }
        if (playerBlade.Energy >= player.defenseCost)
        {
            uiController.UpdateDefenseButton(true);
        }
        else
        {
            uiController.UpdateDefenseButton(false);
        }
    }

    public void UpdatePlayerHealth(float value)
    {
        uiController.UpdatePlayerHealthBar(value);
    }

    public void UpdateOpponentHealth(int id, float value)
    {
        uiController.UpdateOpponentHealthBar(id, value);
    }

    public void GameOver(bool win, int playerRank)
    {
        StartCoroutine(GameOverRoutine(win, playerRank));
    }

    IEnumerator GameOverRoutine(bool win, int playerRank)
    {

        yield return StartCoroutine(GetScreenshot());

        isGameOver = true;
        //Calculate reward
        int gems = CalculateReward(gameMode, playerRank);
        GameController.Instance.Gems += gems;
        uiController.ShowGameOverPanel(win, gems);
    }

    int CalculateReward(GameMode gameMode, int playerRank)
    {
        int reward = 0;
        if (gameMode == GameMode.Arcade)
        {
            switch (playerRank)
            {
                case 1:
                    reward = 30;
                    break;
                case 2:
                    reward = 20;
                    break;
                case 3:
                    reward = 10;
                    break;
                case 4:
                    reward = 0;
                    break;
            }
        }
        else if (gameMode == GameMode.Match)
        {
            switch(playerRank)
            {
                case 0:
                    reward = 0;
                    break;
                case 1:
                    reward = 15;
                    break;
            }
        }
        else if (gameMode == GameMode.Tournament)
        {
            switch (playerRank)
            {
                case 1:
                    reward = 40;
                    break;
                case 2:
                    reward = 20;
                    break;
                case 3:
                    reward = 10;
                    break;
                case 4:
                    reward = 0;
                    break;
            }
        }
        return reward;
    }

    public void TogglePause()
    {
        if (isPaused == false)
        {
            isPaused = true;
            Time.timeScale = 0;
            //Debug.Log("Paused");
            //Open pause panel
            uiController.OpenPausePanel();
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1;
            uiController.ClosePausePanel();
        }
    }

    #region Screenshot

    string ScreenshotName = "screenshot.png";

    //---------- Helper Variables ----------//
    private float width
    {
        get
        {
            return Screen.width;
        }
    }

    private float height
    {
        get
        {
            return Screen.height;
        }
    }

    //---------- Screenshot ----------//
    public void Screenshot()
    {
        Clear_SavedScreenShots();

        // Short way
        StartCoroutine(GetScreenshot());
    }

    //---------- Get Screenshot ----------//
    public IEnumerator GetScreenshot()
    {
        Clear_SavedScreenShots();
        yield return new WaitForEndOfFrame();

        // Get Screenshot
        Texture2D screenshot;
        screenshot = new Texture2D((int)width, (int)height, TextureFormat.ARGB32, false);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
        screenshot.Apply();

        // Save Screenshot
        Save_Screenshot(screenshot);
    }

    //---------- Save Screenshot ----------//
    private void Save_Screenshot(Texture2D screenshot)
    {
        string screenShotPath = Application.persistentDataPath + "/" + ScreenshotName;
        //string screenShotPath = Application.persistentDataPath + "/" + DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss") + "_" + ScreenshotName;
        File.WriteAllBytes(screenShotPath, screenshot.EncodeToPNG());
    }

    //---------- Clear Saved Screenshots ----------//
    public void Clear_SavedScreenShots()
    {
        string path = Application.persistentDataPath;

        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.png");

        foreach (FileInfo f in info)
        {
            File.Delete(f.FullName);
        }
    }
    #endregion
}
