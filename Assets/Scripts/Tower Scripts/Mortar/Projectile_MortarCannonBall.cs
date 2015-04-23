using UnityEngine;
using System.Collections;

public class Projectile_MortarCannonBall : MonoBehaviour {

    public GameObject explosion;
    public float damage = 0;

	void Start () {

	}

	void Update () {
	}

    void OnCollisionEnter(Collision other)
    { 
        GameObject e = Instantiate(explosion, other.contacts[0].point, Quaternion.identity) as GameObject;
        e.GetComponent<ScaleOverTime>().damage = damage;
        Destroy(gameObject);
    }
}
