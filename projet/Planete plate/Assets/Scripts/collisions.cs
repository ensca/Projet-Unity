using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class collisions : MonoBehaviour
{

    private GameController gameController;

    private PlayerController thisPlayer;

    // Use this for initialization
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        //Vérification de l'existence du gameController
        if (gameControllerObject != null)
            gameController = gameControllerObject.GetComponent<GameController>();
        if (gameController == null)
            Debug.Log("Cannot find 'GameController' script");

        thisPlayer = GetComponent<PlayerController>();
        if (thisPlayer == null)
            Debug.Log("Player not found");
    }

    void OnCollisionEnter(Collision collision)
    {
        PlayerController otherPC = collision.gameObject.GetComponent<PlayerController>();

        if (thisPlayer == null)
        {
            Debug.Log("Collision enter without PlayerController defined, with " + collision.gameObject.name);
            return;
        }

        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        //Si le navMeshAgent rencontré est un scientifique
        if (collision.gameObject.CompareTag("Scientist") && (gameObject.CompareTag("Scientist") || gameObject.CompareTag("Guide")))
        {
            //if (gameObject.CompareTag("Guide") || otherPC.getIsScientist())
            if ((GetComponent<PlayerController>().getIsScientist() == true) && otherPC.getIsScientist() == false)
            {
                otherPC.setIsScientist(true);
                collision.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                gameController.addScientist(1);

                //On ajoute ce que le scientifique qui vient de rejoindre le groupe connaît de la ville
                gameController.UpdateMapCity(otherPC.getMapCity());
                //Pas sûr que ce soit utile...
                gameController.UpdateMapCity(thisPlayer.getMapCity());

                /*
                if (GetComponent<Renderer>().material.color != Color.magenta)
                {
                    GetComponent<Renderer>().material.SetColor("magenta", Color.magenta);

                }*/
            }
            //else(collision.gameObject.GetComponent<Renderer>().material.color!=Color.magenta)

        }

        //Collision entre des navMeshAgent
        else if (collision.gameObject.CompareTag("Classic") && !collision.gameObject.CompareTag("Guide") && !gameObject.CompareTag("Guide"))    //Les scientifiques ne peuvent pas être malades pour le moment
        {
            //Si un navMeshAgent rencontre un thisPlayer malade ou en période d'incubation (les scientifiques et le guide ne peuvent pas tomber malade)
            //Le navMeshAgent contracte la maladie mais n'est pas encore déclaré malade
            //Il devient orange pendant cette période d'incubation
            if ((thisPlayer.getState() == "sick" || thisPlayer.getState() == "beforeSick") && collision.gameObject.CompareTag("Scientist") == false && (otherPC.getState() != "sick" && otherPC.getState() != "beforeSick" && otherPC.getState() != "dead"))
            {
                int rd = Random.Range(1, 4);
                if (rd >= otherPC.getImmuneDefences())
                {
                    otherPC.setState("beforeSick");
                    otherPC.setSickDate(Time.time);
                    collision.gameObject.GetComponent<Renderer>().material.color = new Color32(255, 89, 0, 0);
                    gameController.addBeforeSick(1);
                }
                else
                    collision.gameObject.GetComponent<Renderer>().material.color = Color.green;
            }

            //Si un navMeshAgent rencontre un navMeshAgent malade ou en période d'incubation(les scientifiques et le guide ne peuvent pas tomber malade)
            //Le navMeshAgent contracte la maladie mais n'est pas encore déclaré malade
            //Il devient orange pendant cette période d'incubation
            bool sickness = (otherPC.getState() == "sick" || otherPC.getState() == "beforeSick");   //L'autre est malade ou en période d'incubation
            sickness = sickness && !gameObject.CompareTag("Scientist");     //Ce personnage n'est pas un scientifique
            sickness = sickness && (thisPlayer.getState() != "sick" && thisPlayer.getState() != "beforeSick" && otherPC.getState() != "dead");  //ce joueur n'est pas encore malade, ni en période d'incubation, ni mort
            if (sickness)
            {
                int rd = Random.Range(1, 4);
                if (rd >= GetComponent<PlayerController>().getImmuneDefences())
                {
                    GetComponent<Renderer>().material.color = new Color32(255, 89, 0, 0);
                    thisPlayer.setState("beforeSick");
                    thisPlayer.setSickDate(Time.time);
                    gameController.addBeforeSick(1);
                }
                else
                    GetComponent<Renderer>().material.color = Color.green;
            }
            
        }

        if (collision.gameObject.CompareTag("Wall") == true || collision.gameObject.CompareTag("House") == true)
        {
            NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

            // Pick the first indice of a random triangle in the nav mesh
            int t = Random.Range(0, navMeshData.indices.Length - 3);

            // Select a random point on it
            Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
            Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

            GetComponent<NavMeshAgent>().destination = point;
        }


        //Collision d'un membre du groupe des scientifiques avec un batiment
        //Si c'est une maison, on regarde laquelle c'est, 
        //On regarde si on peut y entrer : nb de personnes dans le groupe, possède la clé
        // Si on a pu y pénétrer, on met à jour liste des maisons visitées

        if (thisPlayer.getIsScientist() == true)
        {
            //Debug.Log("scientifique == OK");
            if (collision.gameObject.CompareTag("House") == true)
            {
                Debug.Log("maison == OK");
                if (collision.gameObject.name == "House")
                {
                    Destroy(collision.gameObject);
                    Debug.Log("A détruire == OK");
                }
            }
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (thisPlayer == null)
        {
            Debug.Log("Trigger enter without PlayerController defined");
            return;
        }

        if (other.CompareTag("Garden"))
        {
            //Debug.Log("OK zone jardin !!! :D");
            bool[] tab = thisPlayer.getMapCity();
            tab[0] = true;
            thisPlayer.setMapCity(tab);
        }

        if (other.CompareTag("Key"))
        {
            //Debug.Log("OK zone key !!! :D");
            bool[] tab = thisPlayer.getMapCity();
            tab[1] = true;
            thisPlayer.setMapCity(tab);
        }

        if (other.CompareTag("Labo"))
        {
            //Debug.Log("OK zone labo !!! :D");
            bool[] tab = thisPlayer.getMapCity();
            tab[3] = true;
            thisPlayer.setMapCity(tab);
        }
    }
}

