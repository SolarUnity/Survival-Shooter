using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//计数器
public class ScoreManager : MonoBehaviour
{
	//静态的数据，大家一起改啊!
    public static int score;


    Text text;


    void Awake ()
    {
        text = GetComponent <Text> ();
        score = 0;
    }


    void Update ()
    {
        text.text = "Score: " + score;
    }
}
