using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField]
    public PathManager pathManager;

    List<Waypoint> thePath;
    Waypoint target;

    public float moveSpeed, rotateSpeed;

    public Animator anim;
    bool isRunning = false;

    public bool batmanNPC;

    // Start is called before the first frame update
    void Start()
    {
        isRunning = false;
        anim.SetBool("isRunning", false);
        if (batmanNPC)
        {
            isRunning = true;
            anim.SetBool("isRunning", true);
        }
        thePath = pathManager.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            target = thePath[0];
        }
    }

    void RotateTowardsTarget()
    {
        float stepSize = rotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void MoveForward()
    {
        float stepSize = Time.deltaTime * moveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, target.pos);
        if (distanceToTarget < stepSize)
        {
            return;
        }
        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !batmanNPC)
        {
            isRunning = !isRunning;
            anim.SetBool("isRunning", isRunning);
        }
        if (isRunning)
        {
            RotateTowardsTarget();
            MoveForward();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (batmanNPC)
        {
            if (RandomBool())
            {
                isRunning = false;
                anim.SetBool("isRunning", isRunning);
                Invoke("DelayedRun",1.5f);
            }
        }
        target = pathManager.GetNextTarget();
        if (other.tag == "Wall")
        {
            isRunning = false;
            anim.SetBool("isRunning", false);
        }
    }

    void DelayedRun()
    {
        isRunning = true;
        anim.SetBool("isRunning", isRunning);
    }

    bool RandomBool()
    {
        return (Random.value > 0.5f);
    }
}
