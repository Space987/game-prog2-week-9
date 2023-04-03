using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class CatEnemyScript : MonoBehaviour
{
    public GameObject player;
    public float maxAngle = 45;
    public float maxDistance = 2;
    public float timer = 1.0f;
    public float visionCheckRate = 1.0f;

    // For patroling the AI
    public Transform[] point;
    private int desPoint;
    private NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        // For patroliing 
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GoNextPoint();

        player = GameObject.FindGameObjectWithTag("Player");
        
    }


    // Update is called once per frame
    void Update()
    { 
        
        if(SeePlayer())
        {
            Vector3 position = player.transform.position;
            agent.destination = position;
        }
        else {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoNextPoint();
            }
        }
    }

    public bool SeePlayer()
    {
        Vector3 vecPlayerTurret = player.transform.position - transform.position;
        if (vecPlayerTurret.magnitude > maxDistance)
        {
            return false;
        }
        Vector3 normVecPlayerTurret = Vector3.Normalize(vecPlayerTurret);
        float dotProduct = Vector3.Dot(transform.forward,normVecPlayerTurret);
        var angle = Mathf.Acos(dotProduct);
        float deg = angle * Mathf.Rad2Deg;
        if (deg < maxAngle)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position,normVecPlayerTurret);
        
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    return true;
                }
                
            }
        }
        return false;
       
    }

    void GoNextPoint(){

        //returns if no points have been set up 
        if (point.Length == 0)
        {
            return;
        }

        // Set the agent to the current selected destination point
        agent.destination = point[desPoint].position;

        // Choose the next point in the destination
        // Restarts when needed
        desPoint = (desPoint +1) % point.Length;

    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
