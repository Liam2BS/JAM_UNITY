using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class Poulet : MonoBehaviour
{
    private NavMeshAgent agent;
    public float walkRadius = 10f;
    public GameObject vfxBlood;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(!agent.hasPath)
        {
            agent.SetDestination(RandomNavmeshLocation(walkRadius));
        }
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void Respawn()
    {
        transform.position = RandomNavmeshLocation(100f);
        agent.SetDestination(RandomNavmeshLocation(walkRadius));
    }

    public void Blood()
    {
        GameObject vfx = GameObject.Instantiate(vfxBlood, transform.parent);
        vfx.transform.position = transform.position;
        vfx.transform.localScale = new Vector3(2,2,2);
        vfx.GetComponent<VisualEffect>().Play();
        Destroy(vfx, 3f);
    }
}
