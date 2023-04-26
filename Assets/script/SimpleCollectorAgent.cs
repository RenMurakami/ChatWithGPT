using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

using System.Collections;

public class SimpleCollectorAgent : Agent
{
    [Tooltip("The platform to be moved around")]
    public GameObject platform;
    public GameObject player;
    
    private Vector3 startPosition;
    private SimpleCharacterController characterController;
    new private Rigidbody rigidbody;

    public Vector3 minPosition;
    public Vector3 maxPosition;

    /// <summary>
    /// Called once when the agent is first initialized
    /// </summary>
    public override void Initialize()
    {
        startPosition = transform.position;
        characterController = GetComponent<SimpleCharacterController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Called every time an episode begins. This is where we reset the challenge.
    /// </summary>
    public override void OnEpisodeBegin()
    {


        // Reset agent position, rotation
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        Vector3 newPos = new Vector3(4, 0.5f, 4);
        // Reset platform position (5 meters away from the agent in a random direction)
        Vector3 randomPosition = new Vector3(
            Random.Range(minPosition.x, maxPosition.x),
            Random.Range(minPosition.y, maxPosition.y),
            Random.Range(minPosition.z, maxPosition.z)
        );
        platform.transform.position = startPosition + randomPosition + newPos + Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) * Vector3.forward * 5f;
        float onGround = platform.transform.position.y - 1;
    }

/// <summary>
/// Controls the agent with human input
/// </summary>
/// <param name="actionsOut">The actions parsed from keyboard input</param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {

        // Read input values and round them. GetAxisRaw works better in this case
        // because of the DecisionRequester, which only gets new decisions periodically.
        int vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        bool jump = Input.GetKey(KeyCode.Space);

        // Convert the actions to Discrete choices (0, 1, 2)
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = vertical >= 0 ? vertical : 2;
        actions[1] = horizontal >= 0 ? horizontal : 2;
        actions[2] = jump ? 1 : 0;
    }

    /// <summary>
    /// React to actions coming from either the neural net or human input
    /// </summary>
    /// <param name="actions">The actions received</param>
    public override void OnActionReceived(ActionBuffers actions)
    {
        //If it takes too long, it will add negative reward every iteration.
        AddReward(-1 / 20000000);

        if (Vector3.Distance(platform.transform.position, transform.position) > 200f || this.transform.position.y<-0.1)
        {
            Debug.Log("Text: TOO FAR");
            EndEpisode();
        }
        //AddReward(1 / Vector3.Distance(platform.transform.position, transform.position));
        // Convert actions from Discrete (0, 1, 2) to expected input values (-1, 0, +1)
        // of the character controller
        if (Vector3.Distance(player.transform.position, transform.position) < 10f && Vector3.Distance(player.transform.position, transform.position) != 0f)
        {
            Debug.Log("Text: player STOP");
            this.rigidbody.velocity = Vector3.zero;
            this.rigidbody.angularVelocity = Vector3.zero;
            
            //this.rigidbody.position = transform.position;
            //this.transform.position = transform.position;
            //this.transform.eulerAngles = Vector3.zero;
        }
        else
        {
            float vertical = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1;
            float horizontal = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1;
            bool jump = actions.DiscreteActions[2] > 0;

            characterController.ForwardInput = vertical;
            characterController.TurnInput = horizontal;
            characterController.JumpInput = jump;
        }
        
    }

    /// <summary>
    /// Respond to entering a trigger collider
    /// </summary>
    /// <param name="other">The object (with trigger collider) that was touched</param>
    private void OnTriggerEnter(Collider other)
    {
        // If the other object is a collectible, reward and end episode

        Debug.Log("Text: hit");
        if (other.tag == "apple")
        {

           // Debug.Log("Text: apple");
            AddReward(1f);
            //EndEpisode();
        }

        if(other.tag == "player"){

          //  Debug.Log("Text: player");
            this.rigidbody.velocity = Vector3.zero;
            this.rigidbody.angularVelocity = Vector3.zero; 
           // print(Time.time);
            new WaitForSecondsRealtime(5);
           // print(Time.time);
        }
    }
}