using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAi : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] float speed = 5f;
    [SerializeField] float stoppingDistance = 5f;
    [SerializeField, Min(1)] float patrolTime = 3f;
    [SerializeField] List<Transform> waypoints;

    [SerializeField, Min(5)] float attackRange = 5f;

    private NavMeshAgent agent;
    private Transform player;
    private bool _targetSpotted = false;
    private Transform _targetPoint;

    private Coroutine _patrolCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;

        agent.speed = speed;
        agent.stoppingDistance = stoppingDistance;


        _targetPoint = waypoints[Random.Range(0, waypoints.Count)];
        agent.SetDestination(_targetPoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(_targetSpotted)
        {
            Attack();
        }
        else
        {
            Move();
        }
    }

    private void Move()
    {
        if(Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
        {
            if(_patrolCoroutine is null)
            {
                _patrolCoroutine = StartCoroutine(Patrol(patrolTime));
            }
        }
    }

    private void Attack()
    {
        if(_patrolCoroutine != null)
        {
            StopCoroutine(_patrolCoroutine);
        }

        agent.SetDestination(player.position);
        transform.LookAt(player);

        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * attackRange, Color.magenta, 0.1f);

        if(Physics.Raycast(transform.position,transform.forward, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Haha");
            }
        }
    }

    private void Look()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * attackRange, Color.magenta, 0.1f);

        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                _targetSpotted = true;
            }
        }
    }

    public void SayHit(int num)
    {
        Debug.Log("Hit " + num);
    }

    private IEnumerator Patrol(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (waypoints.Count > 0)
        {
            Debug.Log("Moving to next waypoint");
            _targetPoint = waypoints[Random.Range(0, waypoints.Count)];
            agent.SetDestination(_targetPoint.position);
        }

        yield return null;
    }
}
