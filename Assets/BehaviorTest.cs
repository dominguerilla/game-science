using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class BehaviorTest : MonoBehaviour {

    public GameObject position1, position2;

    private NavMeshAgent agent;
    private BehaviorAgent bagent;
    private SmartCharacter schar;
    private Animator anim;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        schar = GetComponent<SmartCharacter>();
        anim = GetComponent<Animator>();
        bagent = new BehaviorAgent(WalkBackAndForth());
        bagent.StartBehavior();
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Moving", agent.hasPath);
	}

    [Affordance]
    protected Node WalkBackAndForth()
    {
        return new DecoratorLoop(
            new Sequence(
                new WalkTo(agent, position1)/*,
                new LeafWait(2000),
                new WalkTo(agent, position2),
                new LeafWait(2000)
                */)
            );
    }

}
