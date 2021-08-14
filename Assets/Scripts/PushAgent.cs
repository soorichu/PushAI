using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class PushAgent : Agent
{
    public GameObject ground;
    public GameObject area;
    Bounds areaBounds;
    PushBlockSettings m_PushBlockSettings;
    public GameObject goal;
    public GameObject block;
    GoalDetect goalDetect;
    public bool useVectorObs;
    Rigidbody m_BlockRb;
    Rigidbody m_AgentRb;
    Material m_GroundMaterial;
    Renderer m_GroundRenderer;
    EnvironmentParameters m_ResetParams;

    private void Awake()
    {
        m_PushBlockSettings = FindObjectOfType<PushBlockSettings>();
    }

    public override void Initialize()
    {
        goalDetect = block.GetComponent<GoalDetect>();
        goalDetect.agent = this;

        m_AgentRb = GetComponent<Rigidbody>();
        m_BlockRb = block.GetComponent<Rigidbody>();
        m_GroundRenderer = ground.GetComponent<Renderer>();
        m_GroundMaterial = m_GroundRenderer.material;
        m_ResetParams = Academy.Instance.EnvironmentParameters;

        SetResetParameters();
    }

    public override void OnEpisodeBegin()
    {
        var rotation = Random.Range(0, 4);
        var rotationAngle = rotation * 90f;
        area.transform.Rotate(new Vector3(0f, rotationAngle, 0f));

        ResetBlock();
        transform.position = GetRandomSpawnPos();
        m_AgentRb.velocity = Vector3.zero;
        m_AgentRb.angularVelocity = Vector3.zero;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        MoveAgent(vectorAction);
        AddReward(-1f / MaxStep);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2;
        }
    }

    void ResetBlock()
    {
        block.transform.position = GetRandomSpawnPos();

    }

    public Vector3 GetRandomSpawnPos()
    {
        var foundNewSpawnLodation = false;
        var randomSpawnPos = Vector3.zero;
        while(foundNewSpawnLodation == false)
        {
            var randomPosX = Random.Range(-areaBounds.extents.x * m_PushBlockSettings.spawnAreaMarginMultiplier,
                                          areaBounds.extents.x * m_PushBlockSettings.spawnAreaMarginMultiplier);

            var randomPosZ = Random.Range(-areaBounds.extents.z * m_PushBlockSettings.spawnAreaMarginMultiplier,
                                          areaBounds.extents.z * m_PushBlockSettings.spawnAreaMarginMultiplier);

            randomSpawnPos = ground.transform.position + new Vector3(randomPosX, 1f, randomPosZ);
            if(Physics.CheckBox(randomSpawnPos, new Vector3(2.5f, 0.01f, 2.5f)) == false)
            {
                foundNewSpawnLodation = true;
            }

        }

        return randomSpawnPos;
    }

    void SetResetParameters()
    {
        SetGroundMaterialFriction();
        SetBlockProperties();
    }

    public void SetGroundMaterialFriction()
    {
        var groundCollider = ground.GetComponent<Collider>();
        groundCollider.material.dynamicFriction = m_ResetParams.GetWithDefault("dynamic_friction", 0);
        groundCollider.material.staticFriction = m_ResetParams.GetWithDefault("static_friction", 0);
    }

    public void SetBlockProperties()
    {
        var scale = m_ResetParams.GetWithDefault("block_scale", 2);
        m_BlockRb.transform.localScale = new Vector3(scale, 0.75f, scale);
        m_BlockRb.drag = m_ResetParams.GetWithDefault("block_drag", 0.5f);
    }

    public void ScoredAGoal()
    {
        AddReward(5f);
        EndEpisode();

        // Swap ground material for a bit to indicate we scored.
        StartCoroutine(GoalScoredSwapGroundMaterial(m_PushBlockSettings.goalScoredMaterial, 0.5f));
    }

    IEnumerator GoalScoredSwapGroundMaterial(Material mat, float time)
    {
        m_GroundRenderer.material = mat;
        yield return new WaitForSeconds(time);
        m_GroundRenderer.material = m_GroundMaterial;
    }

    public void MoveAgent(float[] act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;
        var action = Mathf.FloorToInt(act[0]);

        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
            case 5:
                dirToGo = transform.right * -0.75f;
                break;
            case 6:
                dirToGo = transform.right * -0.75f;
                break;
        }

        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        m_AgentRb.AddForce(dirToGo * m_PushBlockSettings.agentRunSpeed, ForceMode.VelocityChange);
    }




    float horizontal;
    float vertical;
    public float speed = 10f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        transform.position += (new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime);
    }
}
