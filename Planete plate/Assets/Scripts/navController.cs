using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class navController : MonoBehaviour {
    
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
            if (gameController.cptScientifiques <= 3)
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
            else
            {
                if(gameController.commonMapCity[0]==true)
                {
                    v = gameController.garden.transform.position;
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


    // if (thisPlayer.getAgent().gameObject.GetComponent<Renderer>().material.color != Color.magenta || thisPlayer.getIsScientist() != true || thisPlayer.getIsGuide() != false)
    //{
    //Si l'agent est arrivé à destination ou s'il est bloqué à un endroit, il change de destination
    /* if (arrivee == true || thisPlayer.getAgent().transform.position == position)
     {
         v = GetRandomLocation();
         while ((transform.position - v).magnitude < 30)
         {
             v = GetRandomLocation();
         }
         thisPlayer.getAgent().SetDestination(v);*/
    //if (thisPlayer.getIsGuide() == true)
    //    thisPlayer.setGuideDestination(v);
    //else
    //{
    //    thisPlayer.getAgent().SetDestination(thisPlayer.getGuideDestination());
    //}

    //position = thisPlayer.getAgent().transform.position;
    // }




    #region Déplacement Fluide
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
        if (GetComponent<NavMeshAgent>().remainingDistance <= GetComponent<NavMeshAgent>().stoppingDistance + pathEndThreshold )
        {
            // Arrived
            hasPath = false;
            return true;
        }

        return false;
    }


    /*
    void patrol()
    {

        Vector3 target = waypoint[currentWaypoint].position;
        target.y = transform.position.y; // Keep waypoint at character's height
        Vector3 moveDirection = target - transform.position;

        if (moveDirection.magnitude < 0.5f)
        {
            if (curTime == 0)
                curTime = Time.time; // Pause over the Waypoint
            if ((Time.time - curTime) >= pauseDuration)
            {
                currentWaypoint++;
                curTime = 0;
            }
        }
        else
        {
            var rotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
            character.Move(moveDirection.normalized * patrolSpeed * Time.deltaTime);
        }
    }*/

    #endregion

    #region Rencontres
    /* OK 
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.gameObject.CompareTag("Scientist") == false) { }
        else
        {
            if (GetComponent<Renderer>().CompareTag("Scientist") && this.GetComponent<Renderer>().material.color != Color.magenta)
            {
                //if (collisionCounter == 0)
                //{
                if (thisPlayer.getGuideDestination().x == 0.0f && thisPlayer.getGuideDestination().y == 0.0f && thisPlayer.getGuideDestination().z == 0.0f)
                {
                    //collisionCounter = 1;
                    thisPlayer.setIsGuide(true);
                    thisPlayer.setGuideDestination(this.GetComponent<NavMeshAgent>().destination);
                    collision.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                }
                //}
                else
                {
                    collision.gameObject.GetComponent<NavMeshAgent>().SetDestination(thisPlayer.getAgent().destination);
                    collision.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                }

            }
        }

    }
    */

    /*void OnCollisionEnter(Collision collision)
    {
        //if (collision.GetType(collision.gameObject.GetComponent<NavMeshAgent>()) == ){        }

        if (thisPlayer.getIsScientist()==true)
        { 
            collision.gameObject.GetComponent<NavMeshAgent>().SetDestination(agent.destination);    
            collision.gameObject.GetComponent<Renderer>().material.color = Color.black;
        }

    }*/

    #endregion


    #region Tests
    /*
        #region Attributs
        public bool scientifique = true;
        #endregion

        #region Tentatives Déplacement Fluide
        /*
         public Camera cam;
         public NavMeshAgent navMeshAgent; 

         // Use this for initialization
         void Start () {
             cam = Camera.main;
             navMeshAgent = GetComponent<NavMeshAgent>();		
         }

         // Update is called once per frame
         void Update () {
             if (Input.GetKeyDown(KeyCode.Mouse0))
             {
                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                 RaycastHit hit;

                 if (Physics.Raycast(ray, out hit))
                 {
                     navMeshAgent.SetDestination(hit.point);
                 }
             }
         }
        */

    /*
     public float wanderRadius=20;
     public float wanderTimer=5;

     private Transform target;
     private NavMeshAgent agent;
     private float timer;


     // Use this for initialization
     void Start()
     {
         agent = GetComponent<NavMeshAgent>();
         timer = wanderTimer;


     }

     // Update is called once per frame
     void Update()
     {
         timer += Time.deltaTime;

         if (timer >= wanderTimer)
         {
             Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
             agent.SetDestination(newPos);
             timer = 0;
         }
     }

     public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
     {
         Vector3 randDirection = Random.insideUnitSphere * dist;

         randDirection += origin;

         NavMeshHit navHit;

         NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

         return navHit.position;
     }

     /*
     void update()
     {
         timer += Time.deltaTime;

         if (timer >= wanderTimer)
         {
             agent.SetDestination(RandomNavmeshLocation(4f));
             timer = 0;
         }

     }

     public Vector3 RandomNavmeshLocation(float radius)
     {
         Vector3 randomDirection = Random.insideUnitSphere * radius;
         randomDirection += transform.position;
         NavMeshHit hit;
         Vector3 finalPosition = Vector3.zero;
         if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
         {
             finalPosition = hit.position;
         }
         return finalPosition;
     }
    #endregion

    #region Déplacement Fluide
    //[SerializeField]
    float Speed = 10;

    Vector3 wayPoint;

    //[SerializeField]
    int Range = 10;
    Vector3 position;

    void Start()
    {
        //initialise the target way point
        wander();
    }

    void Update()
    {
        // this is called every frame
        // do move code here
        transform.position += transform.TransformDirection(Vector3.forward) * Speed * Time.deltaTime;
        if ((transform.position - wayPoint).magnitude < 5)
        {
            // when the distance between us and the target is less than 5
            // create a new way point target
            wander();


        }
    }

    void wander()
    {
        // does nothing except pick a new destination to go to

        wayPoint = new Vector3(Random.Range(transform.position.x - Range, transform.position.x + Range), 1, Random.Range(transform.position.z - Range, transform.position.z + Range));
        wayPoint.y = 1;
        // don't need to change direction every frame seeing as you walk in a straight line only
        transform.LookAt(wayPoint);
        Debug.Log(wayPoint + " and " + (transform.position - wayPoint).magnitude);
    }
    #endregion

    
    #region Rencontre
    void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "scientifique")
        {
            collision.gameObject.transform.position = this.position;
        }
       
        /*
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2)
        {
            if (collision.gameObject.CompareTag("Player"))      //Ils deviennent noir quand il se touchent
                this.GetComponent<Renderer>().material.color = Color.black;

            if (collision.gameObject.CompareTag("Scientifique"))
                collision.gameObject.transform.position = this.position;

            /*collisionCount++;                                 //Test pour les collisions, dès que le tic tac touche un autre objet il change couleur
            if ((collisionCount%2).Equals(0))
                this.GetComponent<Renderer>().material.color = Color.black;
            else
                this.GetComponent<Renderer>().material.color = Color.red;
        }


    #endregion */
    #endregion


    /*
    public Transform target;
    NavMeshAgent agent;
    Vector3 wayPoint;
    int speed = 20;
    int range = 10;


    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update ()
    {
        //agent.SetDestination();
        transform.position += transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;
        if ((transform.position - wayPoint).magnitude < 3)
        {
            Wander();
        }
    }

    /*
    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }*/

    /*void Wander()
    {
        // does nothing except pick a new destination to go to

        //wayPoint = new Vector3(Random.Range(transform.position.x - range, transform.position.x + range), 1, Random.Range(transform.position.z - range, transform.position.z + range));

        wayPoint = new Vector3(Random.Range(-20.0f, 20.0f), 0, Random.Range(-20.0f, 20.0f));

        //wayPoint.y = 1;
        // don't need to change direction every frame seeing as you walk in a straight line only
        //transform.LookAt(wayPoint);
        //Debug.Log(wayPoint + " and " + (transform.position - wayPoint).magnitude);
    }*/
    /*
    public Transform[] waypoint;        // The amount of Waypoint you want
    public float patrolSpeed = 3f;       // The walking speed between Waypoints
    public bool loop = true;       // Do you want to keep repeating the Waypoints
    public float dampingLook = 6.0f;          // How slowly to turn
    public float pauseDuration = 0;   // How long to pause at a Waypoint
*/
}



