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
        public List<LevelInitData> LevelInitList = new List<LevelInitData>();
    }
    public class LevelPlayerData
    {
        public int ChapterNumber;
        public int LevelNumber;
        public int HighScore;
        public int MaxStarsEarned;
        public bool LevelIsOpen;
    }

    public class ChapterPlayerData
    {
        public int ChapterNumber;
        public List<LevelPlayerData> LevelPlayerDataList = new List<LevelPlayerData>();
    }

    public class LevelManager : MonoBehaviour
    {

        private static LevelManager instance = null;
        //private List<LevelPlayerData> LevelPlayerList = new List<LevelPlayerData> ();
        private List<ChapterInitData> ChapterInitList = new List<ChapterInitData>();

        private List<ChapterPlayerData> ChapterPlayerDataList = new List<ChapterPlayerData>();
        private string datapath;

        private static int lastLevelNumber = 0;
        private static int lastChapterNumber = 0;

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

        }

        void Start()
        {
            //get Basic Chapter Data
            string chapters;
            TextAsset jsonAsset = Resources.Load("data/leveldata") as TextAsset;
            chapters = jsonAsset.text;
            SetUpChapters(chapters);

            //get player Data, if it exists
            string playerData;
            if (File.Exists(datapath + "/playerdata.json"))
            {
                         print ("retrieving existing player data");
                playerData = System.IO.File.ReadAllText(datapath + "/playerdata.json");
                SetUpPlayerData(playerData);
            }
            else
            {
                print ("setting up new player data");
                SetUpNewPlayerData();
            }


            /*
                        if (File.Exists(datapath + "/leveldata.json"))
                        {
                            chapters = System.IO.File.ReadAllText(datapath + "/leveldata.json");
                        }
                        else
                        {
                            TextAsset jsonAsset = Resources.Load("data/leveldata") as TextAsset;
                            chapters = jsonAsset.text;
                        }
            */
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

            }

        }
        void SetUpPlayerData(string playerData)
        {
            JSONNode json = JSONNode.Parse(playerData);
            //print("chapterInfo " + chapterInfo);
            for (int i = 0; i < json.Count; i++)
            {
                //print(json[i]["ChapterAnimalName"]);
                ChapterPlayerData nextChapter = new ChapterPlayerData();

                nextChapter.ChapterNumber = json[i]["ChapterNumber"].AsInt;
                nextChapter.LevelPlayerDataList = SetUpPlayerLevels(json[i]["Levels"], nextChapter.ChapterNumber);
                ChapterPlayerDataList.Add(nextChapter);
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
            ChapterPlayerData nextChapter = new ChapterPlayerData();
            nextChapter.ChapterNumber = 1;

            LevelPlayerData nextLevel = new LevelPlayerData();
            nextLevel.ChapterNumber = 1;
            nextLevel.LevelNumber = 1;
            nextLevel.HighScore = 0;
            nextLevel.MaxStarsEarned = 0;

            nextLevel.LevelIsOpen = true;
            List<LevelPlayerData> newLevels = new List<LevelPlayerData>();
            newLevels.Add(nextLevel);
            nextChapter.LevelPlayerDataList = newLevels;
            ChapterPlayerDataList.Add(nextChapter);

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