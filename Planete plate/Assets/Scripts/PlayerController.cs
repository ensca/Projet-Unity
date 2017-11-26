using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player
{
    GameObject gameObject;
    string state;           //sain, porteur, malade, mort, immunise      
                            //Ajouter ce qui concerne les vaccins
    bool isScientist;
    bool isGuide;           //Pour gérer le regroupement des scientifiques
    float sickDate;         //Date à laquelle il a été contaminé. Pur la période d'incubation et pour la mort
    Vector3 guideDestination;
    //Pour après : ajouter les conditions sur les défences immnunitaires etc ...
    private bool[] mapCity; //Chaque scientifique disposed'une carte mentale de représentation de la ville



    public Player(GameObject go)
    {
        this.gameObject = go;
        this.state = "sain";
        this.isScientist = false;
        this.isGuide = false;
        this.sickDate = 0f;
        this.guideDestination = new Vector3(0.0f, 0.0f, 0.0f);
        this.mapCity = new bool[] { false, false, false, false, false };
    }

    public Player()
    {
        this.gameObject = new GameObject();
        this.state = "sain";
        this.isScientist = false;
        this.isGuide = false;
        this.sickDate = 0f;
        this.guideDestination = new Vector3(0.0f, 0.0f, 0.0f);
        this.mapCity = new bool[] { false, false, false, false, false };

    }

    public GameObject getGameObject() { return this.gameObject; }
    public string getState() { return this.state; }
    public bool getIsScientist() { return this.isScientist; }
    public bool getIsGuide() { return this.isGuide; }
    public float getSickDate() { return this.sickDate; }
    public Vector3 getGuideDestination() { return this.guideDestination; }
    public bool[] getMapCity() { return this.mapCity; }

    public void setGameObject(GameObject newAgent) { this.gameObject = newAgent; }
    public void setState(string newState) { this.state = newState; }
    public void setIsScientist(bool newIsScientist) { this.isScientist = newIsScientist; }
    public void setIsGuide(bool newIsGuide) { this.isGuide = newIsGuide; }
    public void setSickDate(float newIncubationPeriod) { this.sickDate = newIncubationPeriod; }
    public void setGuideDestination(Vector3 newGuideDestination) { this.guideDestination = newGuideDestination; }
    public void setMapCity(bool[] newMapCity) { this.mapCity = newMapCity; }
}

public class PlayerController : MonoBehaviour {

    public Player thisPlayer;

    public void Start() {
        thisPlayer = new Player(GetComponent<GameObject>());
        
        if(GetComponent<Renderer>().CompareTag("Sick")) //Pour le premier malade
        {
            GetComponent<Renderer>().material.color = Color.red;
            thisPlayer.setState("sick");
            thisPlayer.setSickDate(Time.time);
        }
    }

    void Update()
    {

        if (thisPlayer.getState() == "beforeSick")
        {
            if ((Time.time - thisPlayer.getSickDate()) >= 5)
            {
                GetComponent<Renderer>().material.color = Color.red;
                thisPlayer.setState("sick");
            }
        }
        if (thisPlayer.getState() == "sick")
        {
            if ((Time.time - thisPlayer.getSickDate()) >= 30)
            {
                GetComponent<Renderer>().material.color = Color.black;
                thisPlayer.setState("dead");
            }
        }
        if (thisPlayer.getState() == "dead")
        {
            if ((Time.time - thisPlayer.getSickDate()) >= 35)
            {
                Destroy(GetComponent<Rigidbody>().gameObject);
            }
        }
    }
}
