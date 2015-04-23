using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour {

    public GameObject projectile;
    public float damage = 0;
    public int cost = 0;
    public float reloadTime = 1f;
    public float turnSpeed = 5f;
    public float firePauseTime = .25f;
    public float errorAmount = .001f;
    public Transform target;
    public Transform[] muzzlePositions;
    public Transform head;
    
    public float NextFireTime { get; set; }
    public float NextMoveTime { get; set; }
    public float AimError { get; set; }
    public Quaternion DesiredRotation { get; set; }
    public float Height { get; set; }

    public List<Transform> targets = new List<Transform>();
}
