using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovmentController : MonoBehaviour
{
    [SerializeField]
    Camera _maincamera;
    private Animator animator;
    private NavMeshAgent agent;
    public GameObject camerapivot;
    [SerializeField] 
    ParticleSystem clickEffect;

    [SerializeField]
    Texture2D cursor;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursor, Vector3.zero, CursorMode.ForceSoftware);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();


        GetComponent<NavMeshAgent>().speed = 5.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                if (clickEffect != null)
                { Instantiate(clickEffect, hit.point + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation); }
            }

        }

        if (agent.remainingDistance > 3)
        {
            animator.SetFloat("Speed", 2.0f);
        }
        else if (agent.remainingDistance < 3 && agent.remainingDistance > 0.5f)
        {
            animator.SetFloat("Speed", 1.0f);
        }
        else if (agent.remainingDistance < 0.5f)
        {
            animator.SetFloat("Speed", 0.0f);
        }


        
    }
    void LateUpdate()
    {
        camerapivot.transform.eulerAngles = new Vector3(0, 0, 0);
    }

}

