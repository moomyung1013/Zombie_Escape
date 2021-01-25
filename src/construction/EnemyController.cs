using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    NavMeshAgent nav;
    public GameObject target;
    private float time = 3;
    private bool count = false;

    // Use this for initialization
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        target = GameObject.Find("MainPlayer");
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            SceneManager.LoadScene("ConGameOverScene");
        }
    }
}
