using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]

//[XmlArray("Blades")]
public class SpinBlade
{
    [XmlElement("id")]
    public int id;
    [XmlElement("iconId")]
    public int iconId;
    [XmlElement("name")]
    public string name;
    [XmlElement("attackRing")]
    public AttackRing attackRing;
    [XmlElement("weightDisk")]
    public WeightDisk weightDisk;
    [XmlElement("baseRing")]
    public BaseRing baseRing;
    [XmlElement("level")]
    public int level;
    [XmlElement("initialEnergy")]
    public float initialEnergy;
    [XmlElement("energyRefillTime")]
    public float energyRefillTime;
    [XmlElement("skillCost")]
    public float skillCost;
    [XmlElement("attackCost")]
    public float attackCost;
    [XmlElement("defenseCost")]
    public float defenseCost;
    [XmlElement("locked")]
    public int locked;
    [XmlElement("price")]
    public int price;
}

[System.Serializable]
public class CustomBlade
{
    public AttackRing attackRing;
    public WeightDisk weightDisk;
    public BaseRing baseRing;
}

[System.Serializable]
public struct Stadium
{
    public int id;
    public string name;
    public int price;
}

public struct BladeLevel
{
    public int id;
    public int iconId;
    public string name;
    public string meshId;
}

[System.Serializable]
public class AttackRing
{
    [XmlElement("id")]
    public int id;
    [XmlElement("attack")]
    public float attack;
}

[System.Serializable]
public class WeightDisk
{
    [XmlElement("id")]
    public int id;
    [XmlElement("defense")]
    public float defense;
}

[System.Serializable]
public class BaseRing
{
    [XmlElement("id")]
    public int id;
    [XmlElement("stamina")]
    public float stamina;
}

public enum GameMode
{
    Arcade,
    Match,
    Tournament
}

[XmlRoot("PlayerBladesContainer")]
public class BasicBladesContainer
{
    [XmlArray("PlayerBlades")]
    [XmlArrayItem("SpinBlade")]
    public List<SpinBlade> basicBlades;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(BasicBladesContainer));
        using (var stream = new FileStream(path, FileMode.OpenOrCreate))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static BasicBladesContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(BasicBladesContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as BasicBladesContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static BasicBladesContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(BasicBladesContainer));
        return serializer.Deserialize(new StringReader(text)) as BasicBladesContainer;
    }
}

[XmlRoot("CustomBladesContainer")]
public class CustomBladesContainer
{
    [XmlArray("CustomBlades")]
    [XmlArrayItem("CustomBlade")]
    public List<CustomBlade> customBlades;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(CustomBladesContainer));
        using (var stream = new FileStream(path, FileMode.OpenOrCreate))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static CustomBladesContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(CustomBladesContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as CustomBladesContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static CustomBladesContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(CustomBladesContainer));
        return serializer.Deserialize(new StringReader(text)) as CustomBladesContainer;
    }
}


public class GameController : MonoBehaviour
{
    public Text textMessage;
    private static GameController _instance;

    public static GameController Instance
    {
        get { return _instance; }
    }

    public MenuUIController menuUiController;
    public MenuUIController MenuUIController
    {
        get
        {
            if(menuUiController == null)
            {
                menuUiController = FindObjectOfType<MenuUIController>();
            }
            return menuUiController;
        }
    }

    public List<SpinBlade> allBlades = new List<SpinBlade>();

    public List<SpinBlade> basicBlades = new List<SpinBlade>();
    public List<SpinBlade> tournamentBlades = new List<SpinBlade>();
    public List<AttackRing> attackRings;
    public List<WeightDisk> weightDisks;
    public List<BaseRing> baseRings;

    public List<AttackRing> workshopAttackRings;
    public List<WeightDisk> workshopWeightDisks;
    public List<BaseRing> workshopBaseRings;

    public List<Stadium> stadiums;

    //public TextAsset playerBladesXML;
    public BasicBladesContainer basicBladesContainer = new BasicBladesContainer();

    public List<CustomBlade> customBlades = new List<CustomBlade>();
    public CustomBladesContainer customBladesContainer = new CustomBladesContainer();

    public GameMode gameMode = GameMode.Arcade;

    private int gemCount = 10000;
    public int Gems
    {
        get { return gemCount = PlayerPrefs.GetInt("Gems", 100); }
        set
        {
            gemCount = value;
            PlayerPrefs.SetInt("Gems", gemCount);
        }
    }

    public bool isTournamentEnabled = false;
    public int tournamentRound = 0;
    public List<int> eliminatedIds = new List<int>();

    private string basicBladesXMLPath;
    private string customBladesXMLPath;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        basicBladesXMLPath = DataPath("BasicBladesContainer.xml");
        customBladesXMLPath = DataPath("CustomBladesContainer.xml");
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        //PlayerPrefs.DeleteAll();
        //Gems = PlayerPrefs.GetInt("Gems", 10000);
        //SaveBasicBladesToFile();
        if (!File.Exists(basicBladesXMLPath))
        {
            SaveBasicBladesToFile();
            LoadBasicBladesFromFile();
        }
        else
        {
            LoadBasicBladesFromFile();
        }
        if (!File.Exists(customBladesXMLPath))
        {
            SaveCustomBladesToFile();
            LoadCustomBladesFromFile();
        }
        else
        {
            LoadCustomBladesFromFile();
        }
        //SaveCustomBladesToFile();
        //LoadCustomBladesFromFile();
    }

    public void InitializeTournament()
    {
        int selectedPlayerId = PlayerPrefs.GetInt("SelectedSpin");
        if (isTournamentEnabled == true)
        {
            SpinBlade playerBlade = allBlades[selectedPlayerId];
            tournamentBlades[0] = playerBlade;
            return;
        }
        else
        {
            List<int> randomIds = new List<int>();
            tournamentBlades = new List<SpinBlade>();
            SpinBlade spinBlade = allBlades[selectedPlayerId];
            tournamentBlades.Add(spinBlade);
            for (int i = 0; i < 7; i++)
            {
                int rand = 0;
                do
                {
                    rand = Random.Range(0, 10);
                    randomIds.Add(rand);
                }
                while (!randomIds.Contains(rand));
            }
            //basicBlades.OrderBy(x => rnd.Next()).Take(5));
            for (int i = 0; i < randomIds.Count; i++)
            {
                SpinBlade opponentBlade = basicBlades[randomIds[i]];
                tournamentBlades.Add(opponentBlade);
            }
            isTournamentEnabled = true;
        }
    }

    public void SetupTournamentRound()
    {
        int currentOpponentId = 0;
        if (tournamentRound == 0)
        {
            currentOpponentId = tournamentBlades[1].id;
        }
        else if (tournamentRound == 1)
        {
            if (eliminatedIds[1] % 2 == 0)
            {
                currentOpponentId = tournamentBlades[eliminatedIds[1] + 1].id;
            }
            else if (eliminatedIds[1] % 2 == 1)
            {
                currentOpponentId = tournamentBlades[eliminatedIds[1] - 1].id;
            }
        }
        else if (tournamentRound == 2)
        {
            for (int i = 4; i < tournamentBlades.Count; i++)
            {
                if (!eliminatedIds.Contains(i))
                {
                    currentOpponentId = tournamentBlades[i].id;
                }
            }
        }
        PlayerPrefs.SetInt("CurrentOpponentIndex", currentOpponentId);
    }

    public void CompleteTournamentRound()
    {
        if (tournamentRound == 0)
        {
            eliminatedIds.Add(1);
            for (int i = 1; i < 4; i++)
            {
                float rand = Random.value;
                if (rand > 0.5f)
                {
                    int id = i * 2;
                    eliminatedIds.Add(id);
                }
                else
                {
                    int id = (i * 2) + 1;
                    eliminatedIds.Add(id);
                }
            }
            tournamentRound += 1;
        }
        else if (tournamentRound == 1)
        {
            if (eliminatedIds[1] % 2 == 0)
            {
                eliminatedIds.Add(eliminatedIds[1] + 1);
            }
            else if (eliminatedIds[1] % 2 == 1)
            {
                eliminatedIds.Add(eliminatedIds[1] - 1);
            }
            float rand = Random.value;
            if (rand > 0.5f)
            {
                if (eliminatedIds[2] % 2 == 0)
                {
                    eliminatedIds.Add(eliminatedIds[2] + 1);
                }
                else if (eliminatedIds[2] % 2 == 1)
                {
                    eliminatedIds.Add(eliminatedIds[2] - 1);
                }
            }
            else
            {
                if (eliminatedIds[3] % 2 == 0)
                {
                    eliminatedIds.Add(eliminatedIds[3] + 1);
                }
                else if (eliminatedIds[3] % 2 == 1)
                {
                    eliminatedIds.Add(eliminatedIds[3] - 1);
                }
            }
            tournamentRound += 1;
        }
    }

    public void CompleteTournament()
    {
        isTournamentEnabled = false;
        tournamentRound = 0;
        tournamentBlades.Clear();
        eliminatedIds.Clear();
        gameMode = GameMode.Arcade;
    }

    private void LoadBasicBladesFromFile()
    {
        //textMessage.text = basicBladesXMLPath;
        //TextAsset ta = Resources.Load("BasicBladesContainer") as TextAsset;
        //basicBladesContainer = BasicBladesContainer.LoadFromText(ta.text);
        basicBladesContainer = BasicBladesContainer.Load(basicBladesXMLPath);
        basicBlades = basicBladesContainer.basicBlades;
        //if(basicBlades.Count == 0)
        //{
        //    textMessage.text = 0.ToString();
        //}
        //else
        //{
        //    textMessage.text = basicBladesContainer.basicBlades.Count.ToString();
        //}

        attackRings = new List<AttackRing>();
        weightDisks = new List<WeightDisk>();
        baseRings = new List<BaseRing>();
        for (int i = 0; i < basicBlades.Count; i++)
        {
            SpinBlade blade = basicBlades[i];
            allBlades.Add(blade);
            if (blade.locked == 0)
            {
                //Debug.Log("Adding Item");
                workshopAttackRings.Add(blade.attackRing);
                workshopWeightDisks.Add(blade.weightDisk);
                workshopBaseRings.Add(blade.baseRing);
                //menuUiController.AddItemToworkshop(i);
            }
        }
    }

    public void SaveBasicBladesToFile()
    {
        //Debug.Log("Saving");
        basicBladesContainer.basicBlades = basicBlades;
        //basicBladesContainer.Save(Path.Combine(DataPath(), "BasicBladesContainer.xml"));
        basicBladesContainer.Save(basicBladesXMLPath);
    }

    private void LoadCustomBladesFromFile()
    {
        //TextAsset ta = Resources.Load("BasicBladesContainer") as TextAsset;
        //customBladesContainer = CustomBladesContainer.LoadFromText(ta.text);
        customBladesContainer = CustomBladesContainer.Load(customBladesXMLPath);
        //playerBladesContainer.Load(Path.Combine(Application.dataPath, "PlayerBladesContainer.xml"));
        customBlades = customBladesContainer.customBlades;
        for (int i = 0; i < customBlades.Count; i++)
        {
            CustomBlade cBlade = customBlades[i];
            AddCustomBladeToAllBlades(cBlade);
        }
    }

    public void SaveCustomBladesToFile()
    {
        //Debug.Log("Saving");
        customBladesContainer.customBlades = customBlades;
        customBladesContainer.Save(customBladesXMLPath);
    }

    public void PurchaseSpin(int index)
    {
        basicBlades[index].locked = 0;
        Gems -= basicBlades[index].price;
        MenuUIController.UpdateSelectSpinPanel(index);
        //Add items to workshop
        workshopAttackRings.Add(basicBlades[index].attackRing);
        workshopWeightDisks.Add(basicBlades[index].weightDisk);
        workshopBaseRings.Add(basicBlades[index].baseRing);
        //menuUiController.AddItemToworkshop(index);
        SaveBasicBladesToFile();
    }
    public void PurchaseStadium(int index)
    {
        Gems -= stadiums[index].price;
        MenuUIController.UpdateArenaPanel(index);
    }

    public void AddCustomBlade(int arId, int wdId, int bId)
    {
        CustomBlade customBlade = new CustomBlade();
        customBlade.attackRing = new AttackRing();
        customBlade.attackRing.id = basicBlades[arId].attackRing.id;
        customBlade.attackRing.attack = basicBlades[arId].attackRing.attack;
        customBlade.weightDisk = new WeightDisk();
        customBlade.weightDisk.id = basicBlades[wdId].weightDisk.id;
        customBlade.weightDisk.defense = basicBlades[wdId].weightDisk.defense;
        customBlade.baseRing = new BaseRing();
        customBlade.baseRing.id = basicBlades[bId].baseRing.id;
        customBlade.baseRing.stamina = basicBlades[bId].baseRing.stamina;
        CustomBlade blade = customBlades.Find(x => x.attackRing == customBlade.attackRing && x.weightDisk == customBlade.weightDisk && x.baseRing == customBlade.baseRing);
        //if (!customBlades.Contains(customBlade))
        if (blade == null)
        {
            customBladesContainer.customBlades.Add(customBlade);
            AddCustomBladeToAllBlades(customBlade);
            SaveCustomBladesToFile();
            //LoadCustomBladesFromFile();
        }
    }

    private void AddCustomBladeToAllBlades(CustomBlade cBlade)
    {
        SpinBlade blade = basicBlades[cBlade.attackRing.id];
        SpinBlade newBlade = new SpinBlade();
        newBlade.id = blade.id;
        newBlade.iconId = blade.iconId;
        newBlade.name = blade.name;
        newBlade.attackRing = new AttackRing();
        newBlade.attackRing.id = cBlade.attackRing.id;
        newBlade.attackRing.attack = cBlade.attackRing.attack;
        newBlade.weightDisk = new WeightDisk();
        newBlade.weightDisk.id = cBlade.weightDisk.id;
        newBlade.weightDisk.defense = cBlade.weightDisk.defense;
        newBlade.baseRing = new BaseRing();
        newBlade.baseRing.id = cBlade.baseRing.id;
        newBlade.baseRing.stamina = cBlade.baseRing.stamina;
        newBlade.level = blade.level;
        newBlade.initialEnergy = blade.initialEnergy;
        newBlade.energyRefillTime = blade.energyRefillTime;
        newBlade.skillCost = blade.skillCost;
        newBlade.attackCost = blade.attackCost;
        newBlade.defenseCost = blade.defenseCost;
        newBlade.locked = blade.locked;
        newBlade.price = blade.price;
        allBlades.Add(newBlade);
    }

    //private void OnApplicationQuit()
    //{
    //    SaveBasicBladesToFile();
    //    //SaveCustomBladesToFile();
    //}

    private string DataPath(string path2)
    {
        string filePath = "";
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //filePath = Path.Combine(Application.dataPath, "Resources/" + path2);
            filePath = Path.Combine(Application.dataPath, path2);
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            filePath = Path.Combine(Application.dataPath, path2);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            filePath = Path.Combine(Application.persistentDataPath, path2);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            filePath = Path.Combine(Application.persistentDataPath, path2);
        }
        else
        {
            filePath = Path.Combine(Application.dataPath, path2);
        }
        return filePath;
    }

    public void AddPrchasedItem()
    {
        //Gems +=
    }
}
