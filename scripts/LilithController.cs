using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LilithController : MonoBehaviour
{
    public enum BossState { Idle, Phase1, Phase2, Dead }
    public BossState currentState;

    public float maxHealth = 50f;
    public float currentHealth;
    public float shieldHealth = 0f;
    public float maxshield = 50f;

    public TMP_Text healthtext;
    public TMP_Text shieldtext;

    public Slider healthslider;
    public Slider shieldslider;

    public GameObject shieldEffect;
    public GameObject minionPrefab;
    public Transform[] minionSpawnPoints;
    public int maxMinions = 3;
    private int currentMinions = 0;

    public Transform wanderer;
    public Animator animator;

    public sorcererabilities sorcerer;

    private float attackCooldown = 3f;
    private float attackTimer = 0f;

    private float shieldRegenCooldown = 10f; // Regenerate shield after 10 seconds
    private bool shieldRegenerationTriggered = false; // Prevent multiple triggers
    private bool shieldActive = false;

    private float divebombCooldown = 7f;
    private float divebombTimer = 0f;
    public float divebombRadius = 5f;

    private bool hit = false;
    public GameObject shield;
    private bool reflectiveAuraActive = false;
    public GameObject reflectiveaura;

    private float reflectiveAuraCooldown = 20f;
    private float reflectiveAuraTimer = 0f;
    private bool lilithplayerhit = false;

    private bool bloodSpikesAvailable = false;
    private float bloodSpikesCooldown = 10f; // Cooldown for Blood Spikes
    private float bloodSpikesTimer = 0f;
    public GameObject bloodspikes;
    private List<GameObject> spawnedSpikes = new List<GameObject>();
    public AudioSource BGaudioSource;
    public AudioSource sfxAudioSource;

    public AudioClip BossLevBack;
    public AudioClip BossDamageSound;
    public AudioClip BossDieSound;
    public AudioClip BossDiveSound;
    public AudioClip BossSummonSound;
    public AudioClip BossHandsSound;
    public AudioClip BossShieldSound;
    public AudioClip BossAuraSound;
    public AudioClip GobDies;


    void Start()
    {
        currentState = BossState.Idle;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        shield.SetActive(false);
        reflectiveaura.SetActive(false);
        BGaudioSource.clip = BossLevBack;
        BGaudioSource.loop = true; // Loop the background music
        BGaudioSource.Play();

    }

    void Update()
    {
        divebombTimer += Time.deltaTime;
        //LAZEM NERAGA3 DEEEEEE
        if (currentHealth < 50 && currentState == BossState.Idle)
        {
            currentState = BossState.Phase1;
        }

        if (currentHealth <= 0 && (currentState == BossState.Phase1 || currentState == BossState.Idle))
        {
            currentState = BossState.Phase2;
            currentHealth = 50;
        }

        switch (currentState)
        {
            case BossState.Idle:
                LookAtWanderer();
                break;
            case BossState.Phase1:
                HandlePhase1();
                break;
            case BossState.Phase2:
                HandlePhase2();
                break;
            case BossState.Dead:
                break;
        }

        healthslider.value = currentHealth / maxHealth;
        shieldslider.value = shieldHealth / maxshield;
        healthtext.text = $"{currentHealth}/{maxHealth}";
        shieldtext.text = $"{shieldHealth}/{maxshield}";

        HandleReflectiveAuraCooldown();
        HandleBloodSpikesCooldown();
    }

    public void PerformDivebomb()
    {

        PlaySoundEffect(BossDiveSound);
        lilithplayerhit = false;
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, divebombRadius);
        foreach (Collider obj in hitObjects)
        {
            if (obj.CompareTag("Player") && !lilithplayerhit)
            {
                DealDamageToWanderer(20);
                lilithplayerhit = true;
            }
        }

        Debug.Log("Divebomb attack performed!");
        currentState = BossState.Idle;
        StartCoroutine(ResetToPhase1AfterCooldown());
    }
    void HandleBloodSpikesCooldown()
    {
        if (!bloodSpikesAvailable && currentState == BossState.Phase2 && !reflectiveAuraActive)
        {
            bloodSpikesTimer += Time.deltaTime;
            if (bloodSpikesTimer >= bloodSpikesCooldown)
            {
                bloodSpikesAvailable = true;
                bloodSpikesTimer = 0f;
            }
        }
    }


    void HandleReflectiveAuraCooldown()
    {
        if (!reflectiveAuraActive && currentState == BossState.Phase2) // Activate only in Phase 2
        {
            reflectiveAuraTimer += Time.deltaTime;
            if (reflectiveAuraTimer >= reflectiveAuraCooldown)
            {
                ActivateReflectiveAura();
                reflectiveAuraTimer = 0f;
            }
        }
    }

    void ActivateReflectiveAura()
    {
        PlaySoundEffect(BossAuraSound);
        reflectiveAuraActive = true;
        reflectiveaura.SetActive(true);
        Debug.Log("Reflective Aura is ready and active in Phase 2!");
    }

    void LookAtWanderer()
    {
        if (wanderer != null)
        {
            Vector3 direction = (wanderer.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
    }

    void HandlePhase1()
    {
        if (currentState != BossState.Phase1) // Validate state
            return;

        LookAtWanderer();

        if (currentMinions == 0 && !hit)
        {
            StartCoroutine(Cooldown());
            hit = true;
        }
        else if (divebombTimer >= divebombCooldown)
        {
            animator.SetInteger("phase", 1);
            divebombTimer = 0f;
        }
    }


    void HandlePhase2()
    {

        LookAtWanderer();

        if (reflectiveAuraActive)
        {
            reflectiveaura.SetActive(true);
        }
        else if (bloodSpikesAvailable)
        {
            PerformBloodSpikes();
            StartCoroutine(Cooldown2());
        }
        if (shieldActive && shieldHealth > 0)
        {
            PlaySoundEffect(BossShieldSound, 0.4f);
            shield.SetActive(true);
        }
        else
        {
            shield.SetActive(false);

            if (!shieldRegenerationTriggered)
            {
                StartCoroutine(RegenerateShieldAfterDelay());
            }
        }
    }
    void PerformBloodSpikes()
    {
        PlaySoundEffect(BossHandsSound);
        animator.SetInteger("phase", 2); // Trigger Blood Spikes animation
        Debug.Log("Lilith uses Blood Spikes!");
        bloodSpikesAvailable = false; // Reset cooldown

        // Parameters for spike placement
        int spikeCount = 5; // Number of spikes in a row
        float spikeSpacing = 2.0f; // Distance between each spike
        Vector3 startPosition = transform.position + transform.forward * 2f; // Start 2 units in front of Lilith
        Vector3 direction = transform.forward; // Spawn in the direction Lilith is facing

        // Spawn spikes
        for (int i = 0; i < spikeCount; i++)
        {
            Vector3 spawnPosition = startPosition + (direction * spikeSpacing * i);
            Quaternion spikeRotation = Quaternion.Euler(-90, 0, 0);
            GameObject spike = Instantiate(bloodspikes, spawnPosition, spikeRotation); // Instantiate spike
            spawnedSpikes.Add(spike); // Add to the list
        }

        // Check if Wanderer is hit by spikes
        Collider[] hitObjects = Physics.OverlapSphere(transform.position + transform.forward * (spikeCount * spikeSpacing / 2), spikeCount * spikeSpacing / 2);
        foreach (Collider obj in hitObjects)
        {
            if (obj.CompareTag("Player"))
            {
                DealDamageToWanderer(15); // Damage value of Blood Spikes
                Debug.Log("Wanderer hit by Blood Spikes!");
            }
        }

        // Start cooldown to destroy spikes
        StartCoroutine(Cooldownspikes());
    }

    IEnumerator Cooldownspikes()
    {
        yield return new WaitForSeconds(7f); // Cooldown duration
        DestroySpawnedSpikes();
        animator.SetInteger("phase", 0); // Reset animation
    }

    void DestroySpawnedSpikes()
    {
        foreach (GameObject spike in spawnedSpikes)
        {
            if (spike != null)
            {
                Destroy(spike); // Destroy each spike
            }
        }
        spawnedSpikes.Clear(); // Clear the list after destroying
    }

    IEnumerator Cooldown2()
    {
       
        yield return new WaitForSeconds(7f);
        animator.SetInteger("phase", 0);

    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(5f);
        SummonMinions();
    }

    IEnumerator ResetToPhase1AfterCooldown()
    {
        yield return new WaitForSeconds(0.5f);

        if (currentState == BossState.Phase1) // Ensure state is still Phase 1
        {
            animator.SetInteger("phase", 0);
        }
    }


    IEnumerator RegenerateShieldAfterDelay()
    {
        shieldRegenerationTriggered = true;
        yield return new WaitForSeconds(shieldRegenCooldown);
        shieldHealth = maxshield;
        PlaySoundEffect(BossShieldSound, 0.4f);
        shieldActive = true;
        shieldRegenerationTriggered = false;
        Debug.Log("Lilith's shield regenerated!");
    }

    void PerformAttack(string attackName, int damage)
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            animator.SetInteger("phase", 2);
            DealDamageToWanderer(damage);
            attackTimer = attackCooldown;
        }
    }

    void DealDamageToWanderer(int damage)
    {
        sorcerer.newhealth -= damage;
        Debug.Log($"Lilith dealt {damage} damage to the Wanderer.");
    }

    void SummonMinions()
    {
        PlaySoundEffect(BossSummonSound,0.6f);
        if (currentState == BossState.Phase1)
        {
            for (int i = 0; i < maxMinions; i++)
            {
                Instantiate(minionPrefab, minionSpawnPoints[Random.Range(0, minionSpawnPoints.Length)].position, Quaternion.identity);
            }
            currentMinions = maxMinions;
        }
    }

    public void TakeDamage(float damage)

    {
        
        if (currentMinions > 0)
        {
            Debug.Log("Minions must be defeated before attacking Lilith.");
            return;
        }

        if (reflectiveAuraActive)
        {
            ReflectiveAura(damage);
            return;
        }

        if (shieldActive && shieldHealth > 0)
        {
            if (reflectiveAuraActive)
            {
                Debug.Log("Reflective Aura is active, shield does not take damage.");
                return;
            }
            PlaySoundEffect(BossDamageSound);
            shieldHealth -= damage;
            
            if (shieldHealth <= 0)
            {
                shieldHealth = 0;
                Debug.Log("Lilith's shield is destroyed!");
            }
        }
        else
        {
            PlaySoundEffect(BossDamageSound);
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                if (currentState == BossState.Phase1)
                {
                    TransitionToPhase2();
                }
                else
                {
                    Die();
                }
            }
        }
    }

    void ReflectiveAura(float incomingDamage)
    {
        float totalReflectedDamage = incomingDamage + 15f;
        sorcerer.newhealth -= totalReflectedDamage;
        Debug.Log($"Reflective Aura reflected {totalReflectedDamage} damage!");
        reflectiveAuraActive = false;
        reflectiveaura.SetActive(false);
    }

    void TransitionToPhase2()
    {
        StopPhase1Actions(); // Ensure Phase 1 actions are canceled
        currentMinions = 0;
        shieldHealth = maxshield;
        currentState = BossState.Phase2;
        currentHealth = maxHealth;
        PlaySoundEffect(BossShieldSound, 0.4f);
        shieldActive = true;
        Debug.Log("Lilith transitioned to Phase 2!");
    }


    public void Die()
    {
        PlaySoundEffect(BossDieSound);
        currentHealth = 0;

        currentState = BossState.Dead;
        Debug.Log("Lilith is defeated!");
        animator.SetTrigger("death");

    }

    public void OnMinionDeath()
    {
        if (currentMinions > 0)
        {
            PlaySoundEffect(GobDies);
            currentMinions--;
            Debug.Log($"A minion has died. Remaining minions: {currentMinions}");
        }

        if (currentMinions == 0)
        {
            hit = false;
        }
    }

    void StopPhase1Actions()
    {
        StopCoroutine(Cooldown());
        StopCoroutine(ResetToPhase1AfterCooldown());
        hit = false; // Reset Phase 1-specific flags
        divebombTimer = 0f; // Reset timers
    }
    /*    public void PlaySound(string soundName)
        {
            var clip = (AudioClip)GetType().GetField(soundName)?.GetValue(this);

            if (clip != null)
            {
                //audioSource.clip = clip; // Dynamically assign the clip
                //audioSource.Play();      // Play the sound
            }
            else
            {
                Debug.LogWarning($"Sound '{soundName}' not found! Ensure it is assigned in the Inspector.");
            }
        }*/

    public void PlaySoundEffect(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
        {
            sfxAudioSource.PlayOneShot(clip, volume); // Play the clip with specific volume
        }
    }


}
