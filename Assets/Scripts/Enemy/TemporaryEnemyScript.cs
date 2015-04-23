using UnityEngine;
using System.Collections;
using Pathfinding;

public class TemporaryEnemyScript : MonoBehaviour {

    public Transform compass;
    public Transform body;
    public float turnSpeed = 10.0f;

    public Vector3 targetPosition;
    public Seeker seeker;
    public CharacterController controller;
    public Path path;
    public float nextWaypointDistance = 3.0f;
    public int currentWaypoint = 0;

    public float moveSpeed = 10f;

    //HP Things
    public GUITexture hpBar;
    public float currentHP = 100.0f;
    public float maxHP = 100.0f;
    private float percentHP;
    private float hpLength;
    public GraphNode a;
    public GraphNode b;

	// Use this for initialization
	void Start () {
        targetPosition = GameObject.FindWithTag("AITarget").transform.position;
        GetNewPath();
        a = AstarPath.active.GetNearest(transform.position).node;
        b = AstarPath.active.GetNearest(targetPosition).node;
	}

    public void GetNewPath()
    {
        seeker.StartPath(transform.position, targetPosition, OnPathComplete);
    }

    void OnPathComplete(Path newPath)
    {
        if (!newPath.error)
        {
            path = newPath;
            currentWaypoint = 0;
        }
    }

    void OnBecameInvisible()
    {
        hpBar.GetComponent<HoveringTexture>().clampToScreen = true;
    }

    void OnBecameVisible()
    {
        hpBar.GetComponent<HoveringTexture>().clampToScreen = false;
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
        dir *= moveSpeed * Time.fixedDeltaTime;

        controller.SimpleMove(dir);

        compass.LookAt(path.vectorPath [currentWaypoint]);
        body.rotation = Quaternion.Lerp(body.rotation, compass.rotation, Time.deltaTime * 5f);

        if (Vector3.Distance(transform.position, path.vectorPath [currentWaypoint]) < nextWaypointDistance)
            currentWaypoint++;

    }

	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Rigidbody>().WakeUp();
        //transform.Translate(new Vector3(0, 0, -0.001f));

        percentHP = currentHP / maxHP;
        hpLength = percentHP * 50f;

        Rect inset = hpBar.pixelInset;
        inset.width = hpLength;
        hpBar.pixelInset = inset;

	    if (currentHP <= 0)
        {
			InGameGUI.money += 100;
			InGameGUI.score += 50;
            Destroy(gameObject);
        }
	}
}
