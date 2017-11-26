using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class collisions : MonoBehaviour
{

    private GameController gameController;
    private int countCol;
    private int countScientist;

    private Player thisPlayer;

    // Use this for initialization
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        //Vérification de l'existence du gameController
        if (gameControllerObject != null)
            gameController = gameControllerObject.GetComponent<GameController>();
        if (gameController == null)
            Debug.Log("Cannot find 'GameController' script");

        thisPlayer = GetComponent<PlayerController>().thisPlayer;
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        //Si le navMeshAgent rencontré n'est pas un scientifique, on ne fait rien
        if (collision.gameObject.CompareTag("Scientist") == false) { }
        //Si le navMeshAgent rencontré est un scientifique
        else
        {
            if ((GetComponent<Renderer>().CompareTag("Guide") || GetComponent<Renderer>().material.color == Color.magenta) && collision.gameObject.GetComponent<Renderer>().material.color != Color.magenta && collision.gameObject.GetComponent<Renderer>().CompareTag("Guide")== false)
            {
                collision.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                countScientist++;
                gameController.addScientist(countScientist);

                //On ajoute ce que le scientifique qui vient de rejoindre le groupe connaît de la ville
                gameController.UpdateMapCity(collision.gameObject.GetComponent<PlayerController>().thisPlayer.getMapCity());
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
        if (collision.gameObject.GetComponent<Renderer>().CompareTag("Classic") == true || collision.gameObject.GetComponent<Renderer>().CompareTag("Scientist") == false)    //Les scientifiques ne peuvent pas être malades pour le moment
        {
            //Si un navMeshAgent rencontre un thisPlayer malade (les scientifiques et le guide ne peuvent pas tomber malade)
            //Le navMeshAgent contracte la maladie mais n'est pas encore déclarer malade
            //Il devient orange pendant cette période d'incubation
            if (GetComponent<PlayerController>().thisPlayer.getState() == "sick" && collision.gameObject.GetComponent<Renderer>().CompareTag("Scientist") == false && collision.gameObject.GetComponent<Renderer>().CompareTag("Guide") == false && GetComponent<Renderer>().CompareTag("Guide") == false)
            {
                collision.gameObject.GetComponent<PlayerController>().thisPlayer.setState("beforeSick");
                collision.gameObject.GetComponent<PlayerController>().thisPlayer.setSickDate(Time.time);
                collision.gameObject.GetComponent<Renderer>().material.color = new Color32(255, 89, 0, 0);
            }

            //Si un navMeshAgent rencontre un navMeshAgent malade (les scientifiques et le guide ne peuvent pas tomber malade)
            //Le navMeshAgent contracte la maladie mais n'est pas encore déclarer malade
            //Il devient orange pendant cette période d'incubation
            if (collision.gameObject.GetComponent<PlayerController>().thisPlayer.getState() == "sick" && GetComponent<Renderer>().CompareTag("Scientist") == false && collision.gameObject.GetComponent<Renderer>().CompareTag("Guide") == false && GetComponent<Renderer>().CompareTag("Guide") == false)
            {
                GetComponent<Renderer>().material.color = new Color32(255, 89, 0, 0);
                GetComponent<PlayerController>().thisPlayer.setState("beforeSick");
                GetComponent<PlayerController>().thisPlayer.setSickDate(Time.time);
            }

            //On compte le nombre de collisions
            countCol++;
            gameController.addScore(countCol);
        }

        if (collision.gameObject.GetComponent<Renderer>().CompareTag("Wall") == true)
        {
            NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

            // Pick the first indice of a random triangle in the nav mesh
            int t = Random.Range(0, navMeshData.indices.Length - 3);

            // Select a random point on it
            Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
            Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

            GetComponent<Renderer>().transform.position = point;
        }
    }



    void OnTriggerEnter(Collider other)
    {

        Debug.Log("Trigger enter");
        if (other.gameObject.GetComponent<Renderer>().CompareTag("Garden"))
        {
            Debug.Log("OK zone jardin !!! :D");
            bool[] tab = new bool[4];
            tab = thisPlayer.getMapCity();
            tab[0] = true;
            thisPlayer.setMapCity(tab);
        }

        if (other.gameObject.GetComponent<Renderer>().CompareTag("Key"))
        {
            Debug.Log("OK zone key !!! :D");
            bool[] tab = new bool[4];
            tab = thisPlayer.getMapCity();
            tab[1] = true;
            thisPlayer.setMapCity(tab);
        }

        if (other.gameObject.GetComponent<Renderer>().CompareTag("Labo"))
        {
            Debug.Log("OK zone labo !!! :D");
            bool[] tab = new bool[4];
            tab = thisPlayer.getMapCity();
            tab[3] = true;
            thisPlayer.setMapCity(tab);
        }
    }
}

