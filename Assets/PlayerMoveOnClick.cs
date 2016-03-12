using UnityEngine;
using System.Collections;

public class PlayerMoveOnClick : MonoBehaviour {

    private bool followMode = false;
    private NavMeshAgent agent;
    private Animator anim;

    public GameObject Player;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        /*if (followMode && Player) agent.SetDestination(Player.transform.position);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                followMode = false;
            }

        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (Player) agent.SetDestination(Player.transform.position);
            followMode = false;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            followMode = true;
        }*/ //test
        anim.SetBool("Moving", agent.hasPath);
	}

    public void WalkTo(Transform location)
    {
        agent.SetDestination(location.position);
    }
}
