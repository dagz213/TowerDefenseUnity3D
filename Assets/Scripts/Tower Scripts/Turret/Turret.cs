using UnityEngine;
using System.Collections;

public class Turret : Tower {
	// Use this for initialization
	void Start () {
        Height = 2;
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


            if(Time.time >= NextFireTime && target.position.y <= Height)
            {
                FireProjectile();
            }

        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy"&& other.gameObject.transform.position.y <= Height)
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

    void CalculateAimPosition(Vector3 targetPos)
    {
        Vector3 aimPoint = new Vector3(targetPos.x + AimError, targetPos.y + AimError, targetPos.z + AimError);
        Quaternion look = Quaternion.LookRotation(aimPoint - head.position);
        DesiredRotation = look;
    }

    void CalculateAimError()
    {
        AimError = Random.Range(-errorAmount, errorAmount);
    }

    void FireProjectile()
    {
        NextFireTime = Time.time + reloadTime;
        NextMoveTime = Time.time + firePauseTime;

        CalculateAimError();

        foreach (Transform muzzle in muzzlePositions)
        {
            GameObject go = (GameObject)Instantiate(projectile, muzzle.position, muzzle.rotation);
            go.GetComponent<Projectile_Turret>().damage = damage;
        }
    }
}
