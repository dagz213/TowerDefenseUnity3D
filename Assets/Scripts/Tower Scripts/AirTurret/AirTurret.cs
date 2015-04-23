using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirTurret : Tower{

    void Start()
    {
        Height = 3;
    }
	// Update is called once per frame
	void Update () {
        if (targets.Count > 0)
        {
            if(targets[0] == null)
            {
                Transform go = targets[0];
                targets.Remove(go);
            }

            if (target == null)
            {
                target = targets[0];
            }

            if(Time.time >= NextMoveTime)
            {
                CalculateAimPosition(target.position);
                Quaternion rotation = Quaternion.Lerp(head.rotation, DesiredRotation, Time.deltaTime * turnSpeed);
                head.transform.localRotation = rotation;
            }
            
            
            if(Time.time >= NextFireTime && target.position.y >= Height)
            {
                FireProjectile();
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.transform.position.y >= Height)
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
            if (other.CompareTag("Enemy")) {
                Transform go = other.gameObject.transform;
                targets.Remove(go);
            }
        }
    }
    
    void CalculateAimPosition(Vector3 targetPos)
    {
        Vector3 aimPoint = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        DesiredRotation = Quaternion.LookRotation(aimPoint - head.position);
    }
    
    void FireProjectile()
    {
        NextFireTime = Time.time + reloadTime;
        NextMoveTime = Time.time + firePauseTime;

        int m = Random.Range(0, 6);
        Vector3 position = muzzlePositions [m].position;
        GameObject missile = (GameObject)Instantiate(projectile, position, muzzlePositions[m].rotation);
        missile.GetComponent<Projectile_AirTurret>().damage = damage;
        missile.GetComponent<Projectile_AirTurret>().target = target;
    }
}
