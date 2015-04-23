using UnityEngine;
using System.Collections;

public class Projectile_Turret : MonoBehaviour {

	public float speed = 10.0f;
	public float range = 10.0f;
    public float damage;

	private float distance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward* Time.deltaTime * speed);
		distance += Time.deltaTime * speed;
	}



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            var script = other.gameObject.GetComponent<TemporaryEnemyScript>();
            script.currentHP -= (10 * damage);
        }
    }
}
