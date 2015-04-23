using UnityEngine;
using System.Collections;

public class Projectile_AirTurret : MonoBehaviour {

    public GameObject explosion;
    public Transform target;
    public float range = 10;
    public float speed = 10;
    public float damage;

    private float distance;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        distance += Time.deltaTime * speed;
        if (distance >= range)
            Explode();

        if (target)
        {
            transform.LookAt(target);
        } else
        {
            Explode();
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Explode();
            var script = other.gameObject.GetComponent<TemporaryEnemyScript>();
            script.currentHP -= (10 * damage);
        }
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
