using UnityEditor.Presets;
using UnityEngine;

public class fireball : MonoBehaviour
{
    public GameObject fireballahmed;
    public GameObject player;
    // Assign the Fireball prefab
    public float fireballSpeed = 10f; // Speed of the Fireball
    public bool fireballEnabled=true;
    public float cooldownTime = 1f;  // Cooldown time in seconds
    private float currentCooldownTime = 0f;  // Time remaining on the cooldown
    public bool fireballactive = false;


    void Start()
    {
        // Ensure that the player is assigned (either through Inspector or dynamically)
        if (player == null)
        {
            // Try to find the player object if it's not assigned in the Inspector
            player = GameObject.FindGameObjectWithTag("Player"); // Assumes the player has the "Player" tag
            if (player == null)
            {
                Debug.LogError("Player object not found! Make sure the Player is tagged properly.");
            }
        }
    }



    void Update()


    {
        if (currentCooldownTime > 0f)
        {
            currentCooldownTime -= Time.deltaTime; // Reduce cooldown over time
        }

        if (player == null)
        {
            Debug.LogError("Player Removed");
        }
        //if (Input.GetKeyDown(KeyCode.E))
        //{


        //    fireballEnabled = false ;
        //    // fireballref.fireballEnabled = false;

        //}
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            if (currentCooldownTime <= 0f)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("Fireball launched at: " + hit.point);
                    // Check if the clicked object is an enemy

                    if (hit.collider.CompareTag("goblin") || hit.collider.CompareTag("demon")|| hit.collider.CompareTag("boss"))
                    {
                        Debug.Log("Fireball hit a joe!");
                        // Spawn and launch the Fireball
                        //if (fireballEnabled)
                        SpawnFireball(hit.point);
                        fireballactive = true;
                        currentCooldownTime = cooldownTime;
                    }
                }
            }
        }
    }



    void SpawnFireball(Vector3 targetPosition)
    {
        // Instantiate the Fireball at the player's position

        print("countss");
        GameObject fireball = Instantiate(fireballahmed, player.transform.position, Quaternion.identity);
        if (fireball == null)
        {
            Debug.LogError("Fireball prefab is not assigned in the Inspector!");
        }
        // Calculate direction to the target
        Vector3 direction = (targetPosition - player.transform.position).normalized;

        // Apply velocity to the Fireball
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        rb.velocity = direction * fireballSpeed;
       // Destroy(fireball );
    }

    // Destroy the Fireball upon collision
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("goblin"))
    //    {
    //        print("inside if destroy");
    //        // Destroy the Fireball
    //        Destroy(gameObject);
    //    }
    //}
}
