using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int NumberOfP = 100;
    public int NumberOfScientists = 20;
    public Text ScoreText;
    private int score;

    //Affichage du temps total depuis le début du jeu
    public Text timerText;
    private float startTime;

    public GameObject guide;
    
    public Vector3 peopleValues;

    public bool[] commonMapCity;

    public GameObject garden;

    public GameObject prefab;


    //Données
    private int countBeforeSicks;
    public void setCountBeforeSicks(int newBS) { countBeforeSicks = newBS; }
    public int getCountBeforeSicks() { return countBeforeSicks; }
    private int countSicks;
    public void setCountSicks(int newS) { countSicks = newS; }
    public int getCountSicks() { return countSicks; }
    private int countDeath;
    public void setCountDeath(int newD) { countDeath = newD; }
    public int getCountDeath() { return countDeath; }
    private int countScientists;
    public void setCountScientists(int newS) { countBeforeSicks = newS; }
    public int getCountScientists() { return countScientists; }

    public Text txtBeforeSicks;
    public Text txtSicks;
    public Text txtDeath;
    public Text txtScientists;


    void Start () {

        //Initialisations des données
        countScientists = 1;
        countBeforeSicks = 0;
        countSicks = 1;
        countDeath = 0;
        UpdateData();

        //Initialisation de la carte de connaissance du monde par les scientifiques
        commonMapCity = new bool[] { false, false, false, false, false };

        //Initialisation lieux du jeu
        garden = GameObject.FindWithTag("Place1");
        guide = GameObject.FindWithTag("Guide"); // Dans ce cas, on défini manuellement un guide parmi les scientifiques
        guide.GetComponent<Renderer>().material.color = Color.yellow;
        guide.GetComponent<PlayerController>().setIsScientist(true);
        guide.GetComponent<PlayerController>().setIsGuide(true);

        startTime = Time.time;

        score = 0;
        UpdateScore();
        int randomTag = 0;
        int nbScientists = 0;

        for (int i =0; i< NumberOfP; i++)
        {
            Vector3 v = new Vector3(Random.Range(-50F, 50F),4, Random.Range(-50F, 50F));
            
            PlayerController pc = Instantiate(prefab, v, Quaternion.identity).GetComponent<PlayerController>();
            //pc.setState("sain");
            pc.gameObject.name = "P_Clone_" + i;
            //Affectaction des différents tags
            randomTag = Random.Range(0, 1);
            if (randomTag == 0 && nbScientists < NumberOfScientists)
            {
                pc.gameObject.tag = "Scientist";
                pc.gameObject.GetComponent<Renderer>().material.color = Color.gray;
                nbScientists++;
            }
            else
            {
                pc.gameObject.tag = "Classic";
                pc.setImmuneDefences(Random.Range(1, 101));
            }
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
        countSicks += newSickNumber;
        UpdateData();
    }
    public void addBeforeSick(int newContaminedNumber)
    {
        countBeforeSicks += newContaminedNumber;
        UpdateData();
    }
    public void addDeath(int newDeathNumber)
    {
        countDeath += newDeathNumber;
        UpdateData();
    }
    public void addScientist(int newScientistNumber)
    {
        countScientists += newScientistNumber;
        UpdateData();
    }

    void UpdateData()
    {
        //Affichage des données du jeu
        txtBeforeSicks.text = "Contaminés : " + countBeforeSicks;
        txtSicks.text = "Malades : " + countSicks + "/" + (NumberOfP - NumberOfScientists - countDeath + 1) + " (" + (100 * countSicks / (NumberOfP + 1 - NumberOfScientists - countDeath)) + "%)";
        txtDeath.text = "Morts : " + countDeath + "/" + (NumberOfP - NumberOfScientists + 1) + " (" + (100 * countDeath / (NumberOfP + 1 - NumberOfScientists)) + "%)";
        txtScientists.text = "Groupe : " + countScientists + "/" + (NumberOfScientists+1) + " (" + (100 * countScientists / (NumberOfScientists + 1)) + "%)";
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
