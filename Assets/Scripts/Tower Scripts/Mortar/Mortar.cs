using UnityEngine;
using System.Collections;

public class Mortar : Tower {

	//public GameObject projectile;
    public GameObject mortarCannon;

	public float lobAmount = 1000.0f;
   

    public bool set1 = false;
    public bool set2 = false;
    public bool set3 = false;
    public bool set4 = false;
    public bool set5 = false;
    public bool set6 = false;

	void Start () {
        //InvokeRepeating("FireProjectile", fireStart, fireRadius);
	}
	
	// Update is called once per frame
	void Update () {
        if (targets.Count > 0)
        {
            if (targets [0] == null)
            {
                Transform go = targets [0];
                targets.Remove(go);
            }
            
            if (target == null && targets [0] != null)
            {   
                target = targets [0];
            }

            if (target != null && Time.time >= NextFireTime)
            {
                FireProjectile();   
            }
        } else
        {
            resetRange();
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            NextFireTime = Time.time + (reloadTime * .5f);
            Transform go = other.gameObject.transform;
            if(!targets.Contains(go)){
                targets.Add(go);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == target)
        {
            Transform go = other.gameObject.transform;
            targets.Remove(go);
        }
    }


    void FireProjectile() {

        NextFireTime = Time.time + reloadTime;
        GameObject newProjectile = Instantiate(projectile, mortarCannon.transform.position, projectile.transform.rotation) as GameObject;
        newProjectile.GetComponent<Projectile_MortarCannonBall>().damage = damage;
        Vector3 verticalForce = new Vector3(0, lobAmount, 0);
        Vector3 horizontalDirection = (target.position - transform.position).normalized;
        float tempSpeed = lobAmount / Physics.gravity.y;

        float distanceX = horizontalDirection.x;
        float distanceY = horizontalDirection.y;

        newProjectile.GetComponent<Rigidbody>().AddForce(verticalForce - (horizontalDirection * tempSpeed * determineRange()), ForceMode.VelocityChange);

        resetRange();
    }

    float determineRange()
    {
        float range = 0.0f;

        if (set1 && set2 && set3 && set4 && set5 && set6)
            range = .5f;
        if (!set1 && set2 && set3 && set4 && set5 && set6)
            range = 1f;
        if (!set1 && !set2 && set3 && set4 && set5 && set6)
            range = 1.5f;
        if (!set1 && !set2 && !set3 && set4 && set5 && set6)
            range = 2f;
        if (!set1 && !set2 && !set3 && !set4 && set5 && set6)
            range = 2.75f;
        if (!set1 && !set2 && !set3 && !set4 && !set5 && set6)
            range = 3.55f;

        return range;
    }

    void resetRange()
    {
        set1 = false;
        set2 = false;
        set3 = false;
        set4 = false;
        set5 = false;
        set6 = false;
    }

    bool isReset()
    {
        if(!set1 && !set2 && !set3 && !set4 && !set5 && !set6)
            return true;
        else return false;
    }
}