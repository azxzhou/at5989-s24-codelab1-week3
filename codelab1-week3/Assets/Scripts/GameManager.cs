using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance; //public means it can be seen everywhere
    
    private int score = 0; //private means that it can only be seen in this script

    public int Score //property is way of wrapping variable up - a stand in, can transfer priv variable to public
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
                
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

    private int highScore = 0;
    public int HighScore 
    {
        
        get
        {

            if (highScore == 0) //if hs not set, grab it out of the player prefs or use default value. first time only
            {
                highScore = PlayerPrefs.GetInt("scoreKey", 3);
            }
            return highScore;
        }

        set
        {
            highScore = value;
            //Debug.Log("new high score!!!!!!!!! pog!!!!!!!!!!!!!!!");
            PlayerPrefs.SetInt("scoreKey", highScore);
            
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
        
        PlayerPrefs.SetInt("scoreKey", 3); //resets high score at beginning

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

        }

    }
}
