using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    string state;           //sain, porteur, malade, mort, immunise      
                            //Ajouter ce qui concerne les vaccins
    int immuneDefences;     //entre 1 et 3. 1 = très mauvaises défenses / 3 = très bonnes défences.
    bool isScientist;
    bool isGuide;           //Pour gérer le regroupement des scientifiques
    float sickDate;         //Date à laquelle il a été contaminé. Pur la période d'incubation et pour la mort
    Vector3 guideDestination;
    //Pour après : ajouter les conditions sur les défences immnunitaires etc ...
    private bool[] mapCity; //Chaque scientifique disposed'une carte mentale de représentation de la ville



    public Player()
    {
        this.state = "sain";
        this.immuneDefences = 0;
        this.isScientist = false;
        this.isGuide = false;
        this.sickDate = 0f;
        this.guideDestination = new Vector3(0.0f, 0.0f, 0.0f);
        this.mapCity = new bool[] { false, false, false, false, false };
    }

    public string getState() { return this.state; }
    public int getImmuneDefences() { return this.immuneDefences; }
    public bool getIsScientist() { return this.isScientist; }
    public bool getIsGuide() { return this.isGuide; }
    public float getSickDate() { return this.sickDate; }
    public Vector3 getGuideDestination() { return this.guideDestination; }
    public bool[] getMapCity() { return this.mapCity; }

    public void setState(string newState) { this.state = newState; }
    public void setImmuneDefences(int newImmuneDefences) { this.immuneDefences = newImmuneDefences; }
    public void setIsScientist(bool newIsScientist) { this.isScientist = newIsScientist; }
    public void setIsGuide(bool newIsGuide) { this.isGuide = newIsGuide; }
    public void setSickDate(float newIncubationPeriod) { this.sickDate = newIncubationPeriod; }
    public void setGuideDestination(Vector3 newGuideDestination) { this.guideDestination = newGuideDestination; }
    public void setMapCity(bool[] newMapCity) { this.mapCity = newMapCity; }
}

public class PlayerController : Player {

    private GameController gameController;

    public void Start() {

        //Récupération du gameController
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        //Vérification de l'existence du gameController
        if (gameControllerObject != null)
            gameController = gameControllerObject.GetComponent<GameController>();
        if (gameController == null)
            Debug.Log("Cannot find 'GameController' script");

        if (gameObject.CompareTag("Sick")) //Pour le premier malade
        {
            GetComponent<Renderer>().material.color = Color.red;
            setState("sick");
            setSickDate(Time.time);
        }
    }

    void Update()
    {
        if (getState() == "beforeSick")
        {
            if ((Time.time - getSickDate()) >= 5)
            {
                GetComponent<Renderer>().material.color = Color.red;
                setState("sick");
                gameController.addSick(1);  //1 malade de plus
                gameController.addBeforeSick(-1); //1 contaminé de moins
            }
        }
        if (getState() == "sick")
        {
            if ((Time.time - getSickDate()) >= 30)
            {
                GetComponent<Renderer>().material.color = Color.black;
                setState("dead");
                gameController.addDeath(1); //1 mort de plus
                gameController.addSick(-1); //1 malade de moins
            }
        }
        if (getState() == "dead")
        {
            if ((Time.time - getSickDate()) >= 35)
            {
                Destroy(gameObject);
            }
        }
    }
}
