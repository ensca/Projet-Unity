using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScientificBehaviour : MonoBehaviour {

    #region Classe Player
    public class Player
    {
        NavMeshAgent agent;
        string state;           //sain, porteur, malade, mort, immunise      
        //Ajouter ce qui concerne les vaccins
        bool isScientist;
        bool isGuide;           //Pour gérer les regroupements
        bool isMagician;
        float incubationPeriod;
        Vector3 guideDestination;
        //Vector3 Position; ???
        //Pour après : ajouter les conditions sur les défences immnunitaires etc ...


        public Player(NavMeshAgent nav)
        {
            this.agent = nav;
            this.state = "sain";
            this.isScientist = false;
            this.isGuide = false;
            this.isMagician = false;
            this.incubationPeriod = 0f;
            this.guideDestination = new Vector3(0.0f, 0.0f, 0.0f);
        }

        public Player()
        {
            this.agent = new NavMeshAgent();
            this.state = "sain";
            this.isScientist = false;
            this.isGuide = false;
            this.isMagician = false;
            this.incubationPeriod = 0f;
            this.guideDestination = new Vector3(0.0f, 0.0f, 0.0f);
        }

        public NavMeshAgent getAgent() { return this.agent; }
        public string getState() { return this.state; }
        public bool getIsScientist() { return this.isScientist; }
        public bool getIsGuide() { return this.isGuide; }
        public bool getIsMagician() { return this.isMagician; }
        public float getIncubationPerid() { return this.incubationPeriod; }
        public Vector3 getGuideDestination() { return this.guideDestination; }

        public void setAgent(NavMeshAgent newAgent) { this.agent = newAgent; }
        public void setState(string newState) { this.state = newState; }
        public void setIsScientist(bool newIsScientist) { this.isScientist = newIsScientist; }
        public void setIsGuide(bool newIsGuide) { this.isGuide = newIsGuide; }
        public void setIsMagician(bool newIsMagician) { this.isScientist = newIsMagician; }
        public void setIncubationPerid(float newIncubationPeriod) { this.incubationPeriod = newIncubationPeriod; }
        public void setGuideDestination(Vector3 newGuideDestination) { this.guideDestination = newGuideDestination; }
    }
    #endregion


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
