using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance; //public means it can be seen everywhere
    
    private int score = 0; //private means that it can only be seen in this script

    private const string DIR_LOGS = "/Logs"; //separate string variable for logs folder
    
    private const string FILE_SCORES = DIR_LOGS + "/highScores.txt"; //add slash so it doesnt break the file path, direct to logs folder

    private const string FILE_FAIL = DIR_LOGS + "/failure.txt";

    private const string FILE_ALL_SCORES = DIR_LOGS + "/allHighScores.csv"; //easy to import into spreadsheet programs
    
    String FILE_PATH_HIGH_SCORES = Application.dataPath + FILE_SCORES;

    String FILE_PATH_FAILURE = Application.dataPath + FILE_FAIL;

    String FILE_PATH_ALL_SCORES = Application.dataPath + FILE_ALL_SCORES;
    public int Score //property is way of wrapping variable up - a stand in, can transfer priv variable to public
        {
            get
            {
                return score;
            }

            set
            {
                score = value;

                string fileContents = "";

                if (File.Exists(FILE_PATH_ALL_SCORES))
                {
                    fileContents = File.ReadAllText(FILE_PATH_ALL_SCORES); //check this file
                    
                }

                fileContents += score + ","; //this is additive
                //csv import will accept commas and SOMETIMES pipes | - if there are interfering commas, use pipes
                
                File.WriteAllText(FILE_PATH_ALL_SCORES, fileContents);
                
                //Debug.Log("someone set the score");

                if (score > highScore)
                {
                    HighScore = score;
                }
            }

        }

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI loseText;

    public int targetScore = 5;

    public int levelNumber = 1;

    private const string PREF_KEY_HIGH_SCORE = "highScoreKey"; //makes the key a variable that youre unlikely to get wrong

    private int highScore = 0;
    public int HighScore 
    {
        
        get
        {

            if (highScore == 0) //if hs not set, grab it out of the player prefs or use default value. first time only
            {
                //highScore = PlayerPrefs.GetInt(PREF_KEY_HIGH_SCORE, 3);

                if (File.Exists(FILE_PATH_HIGH_SCORES))
                {
                    
                    string fileContents = File.ReadAllText(FILE_PATH_HIGH_SCORES); //read all content into string
                
                    highScore = int.Parse(fileContents); //turn string into integer
                    
                }
                else
                {
                    
                    highScore = 3; //if file hasnt been created here is the value so the game doesnt explode
                    
                }
                
                //this indicates this is a safe place to write files (aka the location the application is in)
                Debug.Log("file path: " + FILE_PATH_HIGH_SCORES);
                
            }
            return highScore;
        }

        set
        {
            highScore = value;
           
            Debug.Log("new high score!!!!!!!!! pog!!!!!!!!!!!!!!!");
            
            Debug.Log("File Path: " + FILE_PATH_HIGH_SCORES);
            
            //PlayerPrefs.SetInt(PREF_KEY_HIGH_SCORE, highScore);

            if (!File.Exists(FILE_PATH_HIGH_SCORES)) //adding ! means statement is false (aka if this file doesnt exist)
            {

                Directory.CreateDirectory(Application.dataPath + DIR_LOGS); 
                //will create relevant folder and file if there is none
                
                //File.Create(FILE_PATH_HIGH_SCORES);

            }
            
            File.WriteAllText(FILE_PATH_HIGH_SCORES, highScore + ""); //empty string so the code doesnt yell at you
            
        }
    }

    private void Awake() //happens before first frame, before start - put singleton stuff here
    {

        if (instance == null) //if instance var has nothing aka no singleton
        {

            instance = this; //change to current instance
            
            DontDestroyOnLoad(gameObject);

        }
        else //if theres already a singleton of this type
        {
            
            Destroy(gameObject);
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FILE_PATH_HIGH_SCORES = Application.dataPath + FILE_SCORES;
        FILE_PATH_ALL_SCORES  = Application.dataPath + FILE_ALL_SCORES;
        
        PlayerPrefs.SetInt(PREF_KEY_HIGH_SCORE, 3); //resets high score at beginning

    }

    // Update is called once per frame
    void Update()
    {

        //update text with leve, score, and target score
        scoreText.text = "level: " + levelNumber +
                         "\nscore: " + Score +
                         "\ntarget: " + targetScore +
                         "\nhigh score: " + HighScore;

        if (score == targetScore)
        {

            levelNumber++;
            
            SceneManager.LoadScene("Level" + levelNumber);

            targetScore = Mathf.RoundToInt(targetScore + targetScore * 1.5f); //mult. original target score by 1.5f (add f for float)
            //level 1 target: 5
            //level 2 target: 5 + 5(1.5) = 5 + 7.5 = 12.5 = 13
            //level 3 target: 13 + 13(1.5) = 13 + 19.5 = 32.5 = 33
        }

        if (score >= 0)
        {
            
            loseText.text = " ";
            
        }

        if (score < 0)
        {

            loseText.text = "you lose. f";
            
            File.WriteAllText(FILE_PATH_FAILURE, "L + ratio. hi matt");

        }

    }
}
