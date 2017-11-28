using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class navController : MonoBehaviour
{

    private float curTime;
    private int currentWaypoint = 0;

    private Vector3 v;
    private bool arrivee;
    private Vector3 position;


    private GameController gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        //Vérification de l'existence du gameController
        if (gameControllerObject != null)
            gameController = gameControllerObject.GetComponent<GameController>();
        if (gameController == null)
            Debug.Log("Cannot find 'GameController' script");


        v = new Vector3();
        position = new Vector3();
        arrivee = false;

    }

    void Update()
    {
        arrivee = AtEndOfPath();

        if (GetComponent<NavMeshAgent>().CompareTag("Guide")) //Il est guide
        {
            //if (gameController.getCountScientists() <= 3)
            //{
                if (arrivee == true || GetComponent<NavMeshAgent>().transform.position == position)
                {
                    v = GetRandomLocation();
                    while ((transform.position - v).magnitude < 30)
                    {
                        v = GetRandomLocation();
                    }
                }
            //}
            else
            {
                if (gameController.commonMapCity[0] == true)
                {
                    v = gameController.place1.transform.position;
                }
                else
                {
                    System.Random r = new System.Random();
                    int rang= r.Next(0, 5); ;
                    while (gameController.commonMapCity[rang] == true)
                    {
                        rang = r.Next(0, 5);
                    }
                    if (rang == 0)
                    {
                        v = gameController.place1.transform.position;
                    }
                    if (rang == 1)
                    {
                        v = gameController.place1.transform.position;
                    }
                    if (rang == 2)
                    {
                        v = gameController.place1.transform.position;
                    }
                    if (rang == 3)
                    {
                        v = gameController.place1.transform.position;
                    }
                    if (rang == 4)
                    {
                        v = gameController.place1.transform.position;
                    }


                }
            }
        }
        else //il n'est pas guide
        {
            if (GetComponent<NavMeshAgent>().GetComponent<Renderer>().material.color == Color.magenta)
            {
                v = gameController.getGuide().GetComponent<NavMeshAgent>().nextPosition;
            }
            else // S'il est ni guide, ni magenta
            {
                if (arrivee == true || GetComponent<NavMeshAgent>().transform.position == position)
                {
                    v = GetRandomLocation();
                    while ((transform.position - v).magnitude < 30)
                    {
                        v = GetRandomLocation();
                    }
                }
            }
        }
        GetComponent<NavMeshAgent>().SetDestination(v);
        position = GetComponent<NavMeshAgent>().transform.position;

    }





    public Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }

    //Pour savoir si l'agent est arrivé à destination
    public float pathEndThreshold = 5f;
    private bool hasPath = false;
    public bool AtEndOfPath()
    {
        if (GetComponent<NavMeshAgent>().remainingDistance <= GetComponent<NavMeshAgent>().stoppingDistance + pathEndThreshold)
        {
            // Arrived
            hasPath = false;
            return true;
        }

        return false;
    }



}



