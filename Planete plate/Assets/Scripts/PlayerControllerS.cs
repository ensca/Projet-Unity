using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerS : MonoBehaviour
{
    //Attributs du personnages
    private bool scientifique;

    //Mouvement fluide
    #region Deplacement

    private float timeToChangeDirection;
    int collisionCount = 0;

    public void Start()
    {
        ChangeDirection();
    }

    
    public void Update()
    {
        timeToChangeDirection -= Time.deltaTime;

        if (timeToChangeDirection <= 0)
        {
            ChangeDirection();
        }

        GetComponent<Rigidbody>().velocity = transform.forward * 10;
    }


    
    private void ChangeDirection()
    {
        float angle = Random.Range(0f, 360f);

        Quaternion quat = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 newForward = quat * Vector3.forward;
        newForward.y = 0;
        newForward.Normalize();
        transform.forward = newForward;
        timeToChangeDirection = 1.5f;
    }
    
    #endregion

    #region Rencontre 

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2)
        {
            //if (collision.gameObject.CompareTag("Player"))      //Ils deviennent noir quand il se touchent
                this.GetComponent<Renderer>().material.color = Color.black;
            /*collisionCount++;                                 //Test pour les collisions, dès que le tic tac touche un autre objet il change couleur
            if ((collisionCount%2).Equals(0))
                this.GetComponent<Renderer>().material.color = Color.black;
            else
                this.GetComponent<Renderer>().material.color = Color.red;*/
        }
    }
    #endregion
}
