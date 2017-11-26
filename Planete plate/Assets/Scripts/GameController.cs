using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text ScoreText;
    private int score;

    //Affichage du temps total depuis le début du jeu
    public Text timerText;
    private float startTime;

    public GameObject guide;

    public GameObject person;
    public Vector3 peopleValues;
    public int peopleCount;

    public bool[] commonMapCity;

    public GameObject garden;

    public GameObject prefab;
    public Player character;


    //Données
    public int cptContamines;
    public int cptMalades;
    public int cptMorts;
    public int cptScientifiques;

    public Text txtContamines;
    public Text txtMalades;
    public Text txtMorts;
    public Text txtScientifiques;

    
    void Start () {

        //Initialisations des données
        cptScientifiques = 1;
        cptContamines = 0;
        cptMalades = 1;
        cptMorts = 0;
        UpdateData();

        //Initialisation de la carte de connaissance du monde par les scientifiques
        commonMapCity = new bool[] { false, false, false, false, false };

        //Initialisation lieux du jeu
        garden = GameObject.FindWithTag("Place1");

        guide = GameObject.FindWithTag("Guide"); // Dans ce cas, on défini manuellement un guide parmi les scientifiques
        guide.GetComponent<Renderer>().material.color = Color.yellow;
        guide.GetComponent<PlayerController>().thisPlayer.setIsScientist(true);
        guide.GetComponent<PlayerController>().thisPlayer.setIsGuide(true);

        startTime = Time.time;

        score = 0;
        UpdateScore();

        character = new Player(prefab);
        for (int i =0; i<100; i++)
        {
            Vector3 v = new Vector3(Random.Range(-100F, 100F),3, Random.Range(-100F, 100F));

            //Instantiate(prefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
            Instantiate(prefab, v, Quaternion.identity);
        }

        //StartCoroutine(peopleInstantiation());
    }
	
	// Update is called once per frame
	void Update () {
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string secondes = (t % 60).ToString("f2");

        timerText.text = "Time : " + minutes + ":" + secondes;

        //P_9Time.text = "P_9 : " + GameObject.Find("P_9").GetComponent<PlayerController>().thisPlayer.getSickDate();
    }

    public void addScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        ScoreText.text = "Collisions : " + score.ToString();
    }

    public void addSick(int newSickNumber)
    {
        cptMalades += newSickNumber;
        UpdateData();
    }
    public void addContamine(int newContaminedNumber)
    {
        cptContamines += newContaminedNumber;
        UpdateData();
    }
    public void addDeath(int newDeathNumber)
    {
        cptMorts += newDeathNumber;
        UpdateData();
    }
    public void addScientist(int newScientistNumber)
    {
        cptScientifiques += newScientistNumber;
        UpdateData();
    }

    void UpdateData()
    {
        //Affichage des données du jeu
        txtContamines.text = "Contaminés : " + cptContamines.ToString();
        txtMalades.text = "Malades : " + cptMalades.ToString();
        txtMorts.text = "Morts : " + cptMorts.ToString();
        txtScientifiques.text = "Groupe : " + cptScientifiques.ToString();
    }

    public GameObject getGuide()
    {
        return guide;
    }

    public void UpdateMapCity(bool[] newMap)
    {
        for (int i=0; i<5; i++)
        {
            if(newMap[i]==true && commonMapCity[i]==false)
            {
                commonMapCity[i] = true;
            }
        }
        Debug.Log("commonMap : " + commonMapCity[0] + " " + commonMapCity[1] + " " + commonMapCity[2] + " " + commonMapCity[3] + " " + commonMapCity[4]);
    }
}
