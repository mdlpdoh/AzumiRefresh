using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;

namespace com.dogonahorse
{
    public class LevelInitData
    {
        public int ChapterNumber;
        public int LevelNumber;
        public string ChapterAnimalName;
        public int MaxTaps;
        public int TwoStarLevel;
        public int TwoStarBonus;
        public int ThreeStarLevel;
        public int ThreeStarBonus;
        public int CoinsInLevel;
    }

    public class ChapterInitData
    {
        public int ChapterNumber;
        public int ChapterBonusScore;
        public int ChapterBonusCoins;
        public string ChapterAnimalName;

        public Color ChapterMainColor;
        public Color ChapterSecondColor;
        public Color ChapterThirdColor;
        public List<LevelInitData> LevelInitList = new List<LevelInitData>();
    }
    public class LevelPlayerData
    {
        public int ChapterNumber;
        public int LevelNumber;
        public int HighScore;
        public int MaxStarsEarned;
        public bool LevelIsOpen;

        public bool LevelIsNewlyOpen = false;
    }

    public class ChapterPlayerData
    {
        public int ChapterNumber;
        public List<LevelPlayerData> LevelPlayerDataList = new List<LevelPlayerData>();
    }

    public class LevelManager : MonoBehaviour
    {

        public int expectedChapters = 4;
        private static LevelManager instance = null;
        //private List<LevelPlayerData> LevelPlayerList = new List<LevelPlayerData> ();
        private List<ChapterInitData> ChapterInitList = new List<ChapterInitData>();

        private List<ChapterPlayerData> ChapterPlayerDataList = new List<ChapterPlayerData>();
        private string datapath;

        private static int lastLevelNumber = 0;
        private static int lastChapterNumber = 0;

        private static LevelPlayerData newOpenLevel = null;



        public static LevelManager Instance
        {
            // return reference to private instance 
            get
            {
                return instance;
            }
        }
        //public  GameState defaultState;

        void Awake()
        {
            // Check if existing instance of class exists in scene 35 
            // If so, then destroy this instance
            if (instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            // Make this active and only instance
            instance = this;
            DontDestroyOnLoad(gameObject);
            datapath = Application.persistentDataPath;

            //get Basic Chapter Data
            string chapters;
            TextAsset jsonAsset = Resources.Load("data/leveldata") as TextAsset;
            chapters = jsonAsset.text;
            SetUpChapters(chapters);

            //get player Data, if it exists
            string playerData;
            if (File.Exists(datapath + "/playerdata.json"))
            {
                print("retrieving existing player data");
                playerData = System.IO.File.ReadAllText(datapath + "/playerdata.json");
                SetUpPlayerData(playerData);
            }
            else
            {
                print("setting up new player data");
                SetUpNewPlayerData();
            }
        }
        void Start()
        {
            EventManager.ListenForEvent(AzumiEventType.LevelWon, OnLevelWon);
            EventManager.ListenForEvent(AzumiEventType.ResetProgress, OnResetProgress);
        }

        public void OnResetProgress(AzumiEventType Event_Type, Component Sender, object Param = null)
        {
            //   print("OnResetProgress");

            SetUpNewPlayerData();
            WritePlayerLevelSettings();
        }
        public void OnLevelWon(AzumiEventType Event_Type, Component Sender, object Param = null)
        {

            //  print("=============OnLevelWon ");
            ScoreManager myScoreManager = Sender as ScoreManager;
            int totalScore = myScoreManager.TotalScore;
            int numberOfStars = myScoreManager.NumberOfStars;
            LevelPlayerData currentLevelData = ChapterPlayerDataList[lastChapterNumber - 1].LevelPlayerDataList[lastLevelNumber - 1];
            if (totalScore > currentLevelData.HighScore)
            {
                currentLevelData.HighScore = totalScore;
            }
            if (numberOfStars > currentLevelData.MaxStarsEarned)
            {
                currentLevelData.MaxStarsEarned = numberOfStars;
            }
            newOpenLevel = GetNextLevel(lastChapterNumber, lastLevelNumber);
     
            if (newOpenLevel != null)
            {
                newOpenLevel.LevelIsOpen = true;
                newOpenLevel.LevelIsNewlyOpen = true;
                
            }
            WritePlayerLevelSettings();
        }

        LevelPlayerData GetNextLevel(int chapterNumber, int levelNumber)
        {
            if (levelNumber < 10)
            {
                return ChapterPlayerDataList[chapterNumber - 1].LevelPlayerDataList[levelNumber];
            }
            else if (chapterNumber < 4)
            {
                return ChapterPlayerDataList[chapterNumber].LevelPlayerDataList[0];
            }
            else
            {
                return null;
            }
        }

        public static int GetPlayerLevelMaxStars(int chapterNumber, int levelNumber)
        {
            return Instance.ChapterPlayerDataList[chapterNumber - 1].LevelPlayerDataList[levelNumber - 1].MaxStarsEarned;
        }

        public static int GetPlayerLevelHighScore(int chapterNumber, int levelNumber)
        {
            return Instance.ChapterPlayerDataList[chapterNumber - 1].LevelPlayerDataList[levelNumber - 1].HighScore;
        }

        public static bool GetPlayerLevelStatus(int chapterNumber, int levelNumber)
        {
            return Instance.ChapterPlayerDataList[chapterNumber - 1].LevelPlayerDataList[levelNumber - 1].LevelIsOpen;
        }
       public static bool GetPlayerLevelStatusChanged(int chapterNumber, int levelNumber)
        {
            return Instance.ChapterPlayerDataList[chapterNumber - 1].LevelPlayerDataList[levelNumber - 1].LevelIsNewlyOpen;
        }

        public static int GetMaxTaps(int chapterNumber, int levelNumber)
        {
            LevelInitData currentLevel = Instance.ChapterInitList[chapterNumber - 1].LevelInitList[levelNumber - 1];
            return currentLevel.MaxTaps;
        }

        public static int GetCoinsInLevel(int chapterNumber, int levelNumber)
        {
            LevelInitData currentLevel = Instance.ChapterInitList[chapterNumber - 1].LevelInitList[levelNumber - 1];
            return currentLevel.CoinsInLevel;
        }

        public static string GetChapterAnimalName(int chapterNumber, int levelNumber)
        {
            LevelInitData currentLevel = Instance.ChapterInitList[chapterNumber - 1].LevelInitList[levelNumber - 1];
            return currentLevel.ChapterAnimalName;
        }

        public static Color GetChapterMainColor(int chapterNumber)
        {
            return Instance.ChapterInitList[chapterNumber - 1].ChapterMainColor;

        }
        public static Color GetChapterSecondColor(int chapterNumber)
        {
            return Instance.ChapterInitList[chapterNumber - 1].ChapterSecondColor;
        }

        public static Color GetChapterThirdColor(int chapterNumber)
        {
            return Instance.ChapterInitList[chapterNumber - 1].ChapterThirdColor;
        }
        public static void SetLevelIDNumbers(int chapterNumber, int levelNumber)
        {
            lastChapterNumber = chapterNumber;
            lastLevelNumber = levelNumber;
        }

        public static void InitializateDevPanelValues(DevelopmentPanelManager newDevPanel)
        {
            //if both numbers are zero it means level is being tested from within the editor
            //in this case the scoremanager shoud go with the ScoreManager's inspector values
            if (lastChapterNumber != 0 && lastLevelNumber != 0)
            {
                LevelInitData currentLevel = Instance.ChapterInitList[lastChapterNumber - 1].LevelInitList[lastLevelNumber - 1];
                newDevPanel.MaxTaps = currentLevel.MaxTaps;
                newDevPanel.TwoStarLevel = currentLevel.TwoStarLevel;
                newDevPanel.TwoStarBonus = currentLevel.TwoStarBonus;
                newDevPanel.ThreeStarLevel = currentLevel.ThreeStarBonus;
            }
        }


        public static void InitializateLevelValues(ScoreManager newScoreManager)
        {

            //if both numbers are zero it means level is being tested from within the editor
            //in this case the scoremanager shoud go with the ScoreManager's inspector values
            if (lastChapterNumber != 0 && lastLevelNumber != 0)
            {

                //print(Instance.ChapterInitList.Count);
                LevelInitData currentLevel = Instance.ChapterInitList[lastChapterNumber - 1].LevelInitList[lastLevelNumber - 1];

                newScoreManager.ChapterAnimalName = currentLevel.ChapterAnimalName;
                newScoreManager.MaxTaps = currentLevel.MaxTaps;
                newScoreManager.TwoStarLevel = currentLevel.TwoStarLevel;
                newScoreManager.TwoStarBonus = currentLevel.TwoStarBonus;
                newScoreManager.ThreeStarLevel = currentLevel.ThreeStarBonus;
                newScoreManager.CoinsInLevel = currentLevel.CoinsInLevel;
                newScoreManager.ChapterMainColor = GetChapterMainColor(lastChapterNumber);
                newScoreManager.ChapterSecondColor = GetChapterSecondColor(lastChapterNumber);
                newScoreManager.ChapterSecondColor = GetChapterThirdColor(lastChapterNumber);
                newScoreManager.ChapterNumber = lastChapterNumber;
                newScoreManager.LevelNumber = lastLevelNumber;


            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Length >= 10)
            {
                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                string chapterNumberString = sceneName.Substring(6, 2);
                string levelNumberString = sceneName.Substring(8, 2);
                int chapterNumber;
                int levelNumber;
                if (int.TryParse(chapterNumberString, out chapterNumber) &&
                int.TryParse(levelNumberString, out levelNumber))
                {


                    LevelInitData currentLevel = Instance.ChapterInitList[chapterNumber - 1].LevelInitList[levelNumber - 1];
                    newScoreManager.ChapterAnimalName = currentLevel.ChapterAnimalName;
                    newScoreManager.MaxTaps = currentLevel.MaxTaps;
                    newScoreManager.TwoStarLevel = currentLevel.TwoStarLevel;
                    newScoreManager.TwoStarBonus = currentLevel.TwoStarBonus;
                    newScoreManager.ThreeStarLevel = currentLevel.ThreeStarBonus;
                    newScoreManager.CoinsInLevel = currentLevel.CoinsInLevel;
                    newScoreManager.ChapterMainColor = GetChapterMainColor(chapterNumber);
                    newScoreManager.ChapterSecondColor = GetChapterSecondColor(chapterNumber);
                    newScoreManager.ChapterSecondColor = GetChapterSecondColor(chapterNumber);
                    newScoreManager.ChapterSecondColor = GetChapterThirdColor(chapterNumber);
                    newScoreManager.ChapterNumber = chapterNumber;
                    newScoreManager.LevelNumber = levelNumber;
                }
            }

        }
        void SetUpPlayerData(string playerData)
        {
            JSONNode json = JSONNode.Parse(playerData);
            //simple check to see if data is OK
            if (json.Count == expectedChapters)
            {
                for (int i = 0; i < json.Count; i++)
                {
                    ChapterPlayerData nextChapter = new ChapterPlayerData();
                    nextChapter.ChapterNumber = json[i]["ChapterNumber"].AsInt;
                    nextChapter.LevelPlayerDataList = SetUpPlayerLevels(json[i]["Levels"], nextChapter.ChapterNumber);
                    ChapterPlayerDataList.Add(nextChapter);
                }
            }
            //player data is not OK, rebuild it
            else
            {
                print("PlayerData Json is damaged-- setting up new player data");
                SetUpNewPlayerData();
            }
        }

        List<LevelPlayerData> SetUpPlayerLevels(JSONNode levelInfo, int chapterNumber)
        {
            List<LevelPlayerData> newLevels = new List<LevelPlayerData>();

            for (int i = 0; i < levelInfo.Count; i++)
            {
                LevelPlayerData nextLevel = new LevelPlayerData();
                nextLevel.ChapterNumber = chapterNumber;
                nextLevel.LevelNumber = levelInfo[i]["LevelNumber"].AsInt;
                nextLevel.HighScore = levelInfo[i]["HighScore"].AsInt;
                nextLevel.MaxStarsEarned = levelInfo[i]["MaxStarsEarned"].AsInt;
                nextLevel.LevelIsOpen = levelInfo[i]["LevelIsOpen"].AsBool;

                newLevels.Add(nextLevel);
            }
            return newLevels;
        }


        void SetUpNewPlayerData()
        {
            ChapterPlayerDataList = new List<ChapterPlayerData>();
            for (int i = 0; i < 4; i++)
            {

                ChapterPlayerData nextChapter = new ChapterPlayerData();
                nextChapter.ChapterNumber = i + 1;
                List<LevelPlayerData> newLevels = new List<LevelPlayerData>();
                for (int j = 0; j < 10; j++)
                {
                    LevelPlayerData nextLevel = new LevelPlayerData();
                    nextLevel.ChapterNumber = nextChapter.ChapterNumber;
                    nextLevel.LevelNumber = j + 1;
                    nextLevel.HighScore = 0;
                    nextLevel.MaxStarsEarned = 0;
                    newLevels.Add(nextLevel);

                    if (i == 0 && j == 0)
                    {
                        nextLevel.LevelIsOpen = true;
                    }
                    else
                    {
                        nextLevel.LevelIsOpen = false;
                    }
                }
                nextChapter.LevelPlayerDataList = newLevels;
                ChapterPlayerDataList.Add(nextChapter);
            }

        }
        void SetUpChapters(string chapterInfo)
        {
            JSONNode json = JSONNode.Parse(chapterInfo);
            //print("chapterInfo " + chapterInfo);
            for (int i = 0; i < json.Count; i++)
            {
                //print(json[i]["ChapterAnimalName"]);
                ChapterInitData nextChapter = new ChapterInitData();

                nextChapter.ChapterNumber = json[i]["ChapterNumber"].AsInt;
                nextChapter.ChapterAnimalName = json[i]["ChapterAnimalName"];
                nextChapter.ChapterMainColor = HexUtility.HexToColor(json[i]["ChapterMainColor"]);
                nextChapter.ChapterSecondColor = HexUtility.HexToColor(json[i]["ChapterSecondColor"]);
                nextChapter.ChapterThirdColor = HexUtility.HexToColor(json[i]["ChapterThirdColor"]);
                nextChapter.LevelInitList = SetUpLevels(json[i]["Levels"], nextChapter.ChapterNumber, nextChapter.ChapterAnimalName);
                ChapterInitList.Add(nextChapter);
            }
        }

        List<LevelInitData> SetUpLevels(JSONNode levelInfo, int chapterNumber, string chapterAnimal)
        {
            List<LevelInitData> newLevels = new List<LevelInitData>();

            for (int i = 0; i < levelInfo.Count; i++)
            {
                LevelInitData nextLevel = new LevelInitData();
                nextLevel.ChapterNumber = chapterNumber;
                nextLevel.ChapterAnimalName = chapterAnimal;
                nextLevel.LevelNumber = levelInfo[i]["LevelNumber"].AsInt;
                nextLevel.MaxTaps = levelInfo[i]["MaxTaps"].AsInt;
                nextLevel.ThreeStarLevel = levelInfo[i]["ThreeStarLevel"].AsInt;
                nextLevel.TwoStarLevel = levelInfo[i]["TwoStarLevel"].AsInt;
                nextLevel.ThreeStarBonus = levelInfo[i]["ThreeStarBonus"].AsInt;
                nextLevel.TwoStarBonus = levelInfo[i]["TwoStarBonus"].AsInt;
                nextLevel.CoinsInLevel = levelInfo[i]["CoinsInLevel"].AsInt;
                newLevels.Add(nextLevel);
            }
            return newLevels;
        }

        public static void UpdateScore(int newPoints)
        {
            if (lastChapterNumber != 0 && lastLevelNumber != 0)
            {
                Instance.ChapterInitList[lastChapterNumber - 1].LevelInitList[lastLevelNumber - 1].MaxTaps = newPoints;
            }
        }

        public void WriteLevelSettings()
        {

            JSONArray newJson = GetChaptersData(new JSONArray());

            System.IO.File.WriteAllText(datapath + "/leveldata.json", newJson.ToString(""));

        }
        void OnDestroy()
        {
            WritePlayerLevelSettings();
        }

        public void WritePlayerLevelSettings()
        {
            JSONArray newJson = GetPlayerChaptersData(new JSONArray());
            System.IO.File.WriteAllText(datapath + "/playerdata.json", newJson.ToString(""));
        }


        JSONArray GetPlayerChaptersData(JSONArray baseArray)
        {
            //JSONArray baseArray = new JSONArray();
            for (int i = 0; i < ChapterPlayerDataList.Count; i++)
            {
                JSONClass newNode = new JSONClass();
                ChapterPlayerData currentChapter = ChapterPlayerDataList[i];
                newNode["ChapterNumber"].AsInt = currentChapter.ChapterNumber;
                newNode["Levels"] = GetPlayerLevelsData(new JSONArray(), currentChapter);
                baseArray.Add(newNode);
            }
            //newClass.Add(baseArray);
            return baseArray;
        }

        JSONArray GetPlayerLevelsData(JSONArray newArray, ChapterPlayerData currentChapter)
        {

            for (int i = 0; i < currentChapter.LevelPlayerDataList.Count; i++)
            {
                LevelPlayerData currentLevel = currentChapter.LevelPlayerDataList[i];

                JSONClass newNode = new JSONClass();
                newNode["LevelNumber"].AsInt = currentLevel.LevelNumber;
                newNode["HighScore"].AsInt = currentLevel.HighScore;
                newNode["MaxStarsEarned"].AsInt = currentLevel.MaxStarsEarned;
                newNode["LevelIsOpen"].AsBool = currentLevel.LevelIsOpen;
                newArray.Add(newNode);
            }
            return newArray;
        }

        JSONArray GetChaptersData(JSONArray baseArray)
        {
            //JSONArray baseArray = new JSONArray();
            for (int i = 0; i < ChapterInitList.Count; i++)
            {
                JSONClass newNode = new JSONClass();
                ChapterInitData currentChapter = ChapterInitList[i];
                newNode["ChapterNumber"].AsInt = currentChapter.ChapterNumber;
                newNode["ChapterBonusScore"].AsInt = currentChapter.ChapterBonusScore;
                newNode["ChapterBonusCoins"].AsInt = currentChapter.ChapterBonusCoins;
                newNode["ChapterAnimalName"] = currentChapter.ChapterAnimalName;
                newNode["ChapterMainColor"] = HexUtility.ColorToHex(currentChapter.ChapterMainColor);
                newNode["ChapterSecondColor"] = HexUtility.ColorToHex(currentChapter.ChapterSecondColor);
                newNode["ChapterThirdColor"] = HexUtility.ColorToHex(currentChapter.ChapterThirdColor);

                newNode["Levels"] = GetLevelsData(new JSONArray(), currentChapter);
                baseArray.Add(newNode);
            }
            //newClass.Add(baseArray);
            return baseArray;
        }
        JSONArray GetLevelsData(JSONArray newArray, ChapterInitData currentChapter)
        {

            for (int i = 0; i < currentChapter.LevelInitList.Count; i++)
            {
                LevelInitData currentLevel = currentChapter.LevelInitList[i];

                JSONClass newNode = new JSONClass();
                newNode["LevelNumber"].AsInt = currentLevel.LevelNumber;
                newNode["MaxTaps"].AsInt = currentLevel.MaxTaps;
                newNode["ThreeStarLevel"].AsInt = currentLevel.ThreeStarLevel;
                newNode["TwoStarLevel"].AsInt = currentLevel.TwoStarLevel;
                newNode["ThreeStarBonus"].AsInt = currentLevel.ThreeStarBonus;
                newNode["TwoStarBonus"].AsInt = currentLevel.TwoStarBonus;
                newNode["CoinsInLevel"].AsInt = currentLevel.CoinsInLevel;
                newArray.Add(newNode);
            }
            return newArray;
        }
    }
}