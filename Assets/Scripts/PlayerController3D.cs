using UnityEngine;
using System.Collections;

public class PlayerController3D : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public GameObject attackPrefab;      // Prefab to spawn
    public Transform attackSpawnPoint;   // Where the prefab spawns

    // ─────────── LEARNING / RECORDING ───────────
    private float lastMoveInputTime;
    private float lastAttackInputTime;

    private int atkDistanceCount; // ONLY for min/avg/max attack distance


    private int atkCount;
    private int moveInputCount;

    [Header("Attack Lockout")]
    public float postHitAttackLockDuration = 1f;
    private float attackLockTimer;



    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Attack")]
    public float attackDuration = 0.25f;

    // ─────────── State ───────────
    private Vector3 moveInput;
    private Vector3 externalMoveInput;
    private bool useExternalMovement;
    private bool isAttacking;

    [Header("Enemy Reference")]
    public Transform enemy; // assign in inspector

    // ─────────── POSITION WINDOW SAMPLING ───────────
    public float positionWindowDuration = 3f;

    private Vector3 positionAccumulator;
    private int positionSampleCount;
    private float windowTimer;



    public Camera mainCamera;
    public Vector3 respawnOffset = Vector3.zero;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(GameData.gameState != 0) return;

        if (!useExternalMovement)
            ReadInput();

        // Player input attack
        if (Input.GetMouseButtonDown(0))
            TryAttack();

        if (GameData.gameState == 0)
        {
            RecordWorldPositionWindow();
        }

        if (attackLockTimer > 0f)
        {
            attackLockTimer -= Time.deltaTime;
        }

    }

    void FixedUpdate()
    {
        Vector3 finalMove =
            useExternalMovement ? externalMoveInput : moveInput;

        TryMove(finalMove);
    }

    // ─────────── AI / BEHAVIOR GRAPH ───────────
    public void SetExternalMovement(Vector3 move)
    {
        externalMoveInput = move;
        useExternalMovement = true;
    }

    public void ClearExternalMovement()
    {
        externalMoveInput = Vector3.zero;
        useExternalMovement = false;
    }

    // ─────────── MOVEMENT ───────────
    public bool TryMove(Vector3 moveVector)
    {
        if (isAttacking)
            return false;

        Move(moveVector);

        if (moveVector.sqrMagnitude > 0f)
            Rotate8Directions(moveVector);

        return true;
    }

    void Move(Vector3 moveVector)
    {
        Vector3 velocity = moveVector * moveSpeed;
        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y,
            velocity.z
        );
    }

    // ─────────── 8-DIRECTION ROTATION ───────────
    void Rotate8Directions(Vector3 moveDir)
    {
        float angle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        float snappedAngle = Mathf.Round(angle / 45f) * 45f;
        transform.rotation = Quaternion.Euler(0f, snappedAngle, 0f);
    }

    // ─────────── INPUT ───────────
    void ReadInput()
{
    Vector3 newInput = new Vector3(
        Input.GetAxisRaw("Horizontal"),
        0f,
        Input.GetAxisRaw("Vertical")
    ).normalized;

    if (GameData.gameState == 0 && newInput != moveInput && newInput.sqrMagnitude > 0f)
    {
        float diff = Time.time - lastMoveInputTime;
        lastMoveInputTime = Time.time;

        UpdateStats(
            ref LearningData.minInputDiff,
            ref LearningData.avgInputDiff,
            ref LearningData.maxInputDiff,
            diff,
            ref moveInputCount
        );
    }

    moveInput = newInput;
}

public void LockAttack(float duration)
{
    attackLockTimer = Mathf.Max(attackLockTimer, duration);
}


    // ─────────── ATTACK ───────────
    public bool TryAttack()
    {
        //Debug.Log(atkCount);
        if(GameData.gameState == 0)
        {
            if (isAttacking || attackLockTimer > 0f)
            return false;
        }
        
        if (isAttacking)
            return false;

        if (GameData.gameState == 0)
        {
            float diff = Time.time - lastAttackInputTime;
            lastAttackInputTime = Time.time;

            // Ignore the very first attack (diff will be huge / invalid)
            if (LearningData.atkTimingCount > 0)
            {
                LearningData.minTimeBetweenAttacks =
                    Mathf.Min(LearningData.minTimeBetweenAttacks, diff);

                LearningData.avgTimeBetweenAttacks =
                    (LearningData.avgTimeBetweenAttacks * LearningData.atkTimingCount + diff)
                    / (LearningData.atkTimingCount + 1);
            }

            LearningData.atkTimingCount++;


            UpdateStats(
                ref LearningData.minAtkDiff,
                ref LearningData.avgAtkDiff,
                ref LearningData.maxAtkDiff,
                diff,
                ref atkCount
            );

            RecordAttackDistance();
            //Debug.Log("called");
        }

        StartCoroutine(Attack());
        return true;
    }


    IEnumerator Attack()
    {
        isAttacking = true;

        // Stop movement while attacking
        rb.linearVelocity = Vector3.zero;

        // Spawn attack prefab
        if (attackPrefab != null && attackSpawnPoint != null)
        {
            GameObject spawned = Instantiate(
                attackPrefab,
                attackSpawnPoint.position,
                attackSpawnPoint.rotation
            );
            Destroy(spawned, attackDuration);
        }

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }

    void RecordAttackDistance()
    {
        if (enemy == null) return;

        float distance = Vector3.Distance(transform.position, enemy.position);


            LearningData.minAtkDistance = Mathf.Min(LearningData.minAtkDistance, distance);
            LearningData.maxAtkDistance = Mathf.Max(LearningData.maxAtkDistance, distance);
            LearningData.avgAtkDistance = (LearningData.avgAtkDistance * atkDistanceCount + distance) / (atkDistanceCount + 1);
            LearningData.atkCount = atkCount;
        

        atkDistanceCount++;

        //Debug.Log("min: " + LearningData.minAtkDistance + 
                  // " avg: " + LearningData.avgAtkDistance + 
                  // " max: " + LearningData.maxAtkDistance +
                  // " count: " + atkDistanceCount);
    }



    void RecordWorldPositionWindow()
    {
        // Accumulate every frame
        positionAccumulator += transform.position;
        positionSampleCount++;

        windowTimer += Time.deltaTime;

        if (windowTimer >= positionWindowDuration)
        {
            Vector3 windowAverage = positionAccumulator / positionSampleCount;

            LearningData.avgWorldPositionsPerWindow.Add(windowAverage);

            // Reset window
            positionAccumulator = Vector3.zero;
            positionSampleCount = 0;
            windowTimer = 0f;
        }
    }



    // ─────────── EXTERNAL QUERY ───────────
    public bool IsAttacking()
    {
        return isAttacking;
    }


    void LateUpdate()
    {
        KeepInsideCamera();
    }

    void KeepInsideCamera()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        bool outOfView =
            viewportPos.z < 0f ||
            viewportPos.x < 0f || viewportPos.x > 1f ||
            viewportPos.y < 0f || viewportPos.y > 1f;

        if (outOfView)
        {
            TeleportBack();
        }
    }

    void TeleportBack()
    {
        Vector3 safePosition = mainCamera.transform.position + mainCamera.transform.forward * 5f;
        safePosition.y = transform.position.y; // keep height

        rb.linearVelocity = Vector3.zero;
        transform.position = safePosition + respawnOffset;
        //transform.position = new Vector3(0, 1, -6);
    }


    void UpdateStats(ref float min, ref float avg, ref float max, float value, ref int count)
{
    if (count == 0)
    {
        min = max = avg = value;
    }
    else
    {
        min = Mathf.Min(min, value);
        max = Mathf.Max(max, value);
        avg = (avg * count + value) / (count + 1);
    }

    count++;
}

}
