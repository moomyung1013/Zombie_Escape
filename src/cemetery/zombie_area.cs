using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class zombie_area : MonoBehaviour
{

    NavMeshAgent nav;
    GameObject target;
    //Animator animator;

    private float time = 3;
    private bool count = false;

    // Use this for initialization
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        target = GameObject.Find("C_man_1_FBX2013");

        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (count == true)
        {
            if (nav.destination != target.transform.position)
            {
                nav.SetDestination(target.transform.position);
            }
        }
        else if (count == false)
        {
            time -= Time.deltaTime;
            if (Mathf.Round(time) == 0)
            {
                count = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coin")) //코인과 충돌
        {
            collision.gameObject.SetActive(false);   
        }
    }
}