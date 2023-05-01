
using System.Collections;
using UnityEngine;


public class newBoidsScript : MonoBehaviour
{
    [Header("General Settings")]
    public Vector2 behavioralCh = new Vector2(2.0f, 6.0f);
    public bool debug;

    [Header("School Settings")]
    [Range(1, 150)] public int schoolNum = 2;
    [Range(0, 5000)] public int fragmentedSchool = 30;
    [Range(0, 1)] public float fragmentedSchoolYLimit = 0.5f;
    [Range(0, 1.0f)] public float migrationFrequency = 0.1f;
    [Range(0, 1.0f)] public float posChangeFrequency = 0.5f;
    [Range(0, 100)] public float smoothChFrequency = 0.5f;

    [Header("Fish Controls")]
    public GameObject fishPrefab;
    [Range(1, 9999)] public int fishNum = 10;
    [Range(0, 150)] public float fishSpeed = 1;
    [Range(0, 100)] public int scatteredFish = 10;
    [Range(0, 1)] public float scatterdFishYLimit = 1;
    [Range(0, 10)] public float soaring = 0.5f;
    [Range(0.01f, 500)] public float verticalWawe = 20;
    public bool rotationClamp = false;
    [Range(0, 360)] public float rotationClampValue = 50;
    public Vector2 scaleRandom = new Vector2(1.0f, 1.5f);

    Transform thisTransform, dangerTransform;
    int dangerBird;
    public Transform[] birdsTransform, flocksTransform;
    public Vector3[] rdTargetPos, flockPos, velFlocks;
    float[] birdsSpeed, birdsSpeedCur, spVelocity;
    public int[] curentFlock;
    float dangerSpeedCh, dangerSoaringCh;
    float timeTime;
    static WaitForSeconds delay0;


    void Awake()
    {
        //--------------

        thisTransform = transform;
        CreateFlock();
        CreateFish();
        StartCoroutine(BehavioralChange());

    }


    void LateUpdate()
    {
        //--------------  

        FlocksMove();
        BirdsMove();

        //--------------
    }


    void FlocksMove()
    {
        //--------------  

        for (int f = 0; f < schoolNum; f++)
        {
            flocksTransform[f].localPosition = Vector3.SmoothDamp(flocksTransform[f].localPosition, flockPos[f], ref velFlocks[f], smoothChFrequency);
        }

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void BirdsMove()
    {
        //--------------

        float deltaTime = Time.deltaTime;
        timeTime += deltaTime;
        Vector3 translateCur = Vector3.forward * fishSpeed * dangerSpeedCh * deltaTime;
        Vector3 verticalWaweCur = Vector3.up * ((verticalWawe * 0.5f) - Mathf.PingPong(timeTime * 0.5f, verticalWawe));
        float soaringCur = soaring * deltaTime;

        //--------------

        for (int b = 0; b < fishNum; b++)
        {
            if (birdsSpeedCur[b] != birdsSpeed[b]) birdsSpeedCur[b] = Mathf.SmoothDamp(birdsSpeedCur[b], birdsSpeed[b], ref spVelocity[b], 0.5f);
            birdsTransform[b].Translate(translateCur * birdsSpeed[b]);
            Vector3 tpCh = flocksTransform[curentFlock[b]].position + rdTargetPos[b] + verticalWaweCur - birdsTransform[b].position;
            Quaternion rotationCur = Quaternion.LookRotation(Vector3.RotateTowards(birdsTransform[b].forward, tpCh, soaringCur, 0));
            if (rotationClamp == false) birdsTransform[b].rotation = rotationCur;
            else birdsTransform[b].localRotation = FishRotationClamp(rotationCur, rotationClampValue);
        }

        //--------------
    }


    IEnumerator BehavioralChange()
    {
        //--------------

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(behavioralCh.x, behavioralCh.y));

            //School Behaviour

            for (int f = 0; f < schoolNum; f++)
            {
                if (Random.value < posChangeFrequency)
                {
                    Vector3 rdvf = Random.insideUnitSphere * fragmentedSchool;
                    flockPos[f] = new Vector3(rdvf.x, Mathf.Abs(rdvf.y * fragmentedSchoolYLimit), rdvf.z);
                }
            }

            //Fish Behaviour

            for (int b = 0; b < fishNum; b++)
            {
                birdsSpeed[b] = Random.Range(3.0f, 7.0f);
                Vector3 lpv = Random.insideUnitSphere * scatteredFish;
                rdTargetPos[b] = new Vector3(lpv.x, lpv.y * scatterdFishYLimit, lpv.z);
                if (Random.value < migrationFrequency) curentFlock[b] = Random.Range(0, schoolNum);
            }
        }

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void CreateFlock()
    {
        //--------------

        flocksTransform = new Transform[schoolNum];
        flockPos = new Vector3[schoolNum];
        velFlocks = new Vector3[schoolNum];
        curentFlock = new int[fishNum];

        for (int f = 0; f < schoolNum; f++)
        {
            GameObject nobj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            nobj.SetActive(debug);
            flocksTransform[f] = nobj.transform;
            Vector3 rdvf = Random.onUnitSphere * fragmentedSchool;
            flocksTransform[f].position = thisTransform.position;
            flockPos[f] = new Vector3(rdvf.x, Mathf.Abs(rdvf.y * fragmentedSchoolYLimit), rdvf.z);
            flocksTransform[f].parent = thisTransform;
        }
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void CreateFish()
    {
        //--------------

        birdsTransform = new Transform[fishNum];
        birdsSpeed = new float[fishNum];
        birdsSpeedCur = new float[fishNum];
        rdTargetPos = new Vector3[fishNum];
        spVelocity = new float[fishNum];

        for (int b = 0; b < fishNum; b++)
        {
            birdsTransform[b] = Instantiate(fishPrefab, thisTransform).transform;
            Vector3 lpv = Random.insideUnitSphere * scatteredFish;
            birdsTransform[b].localPosition = rdTargetPos[b] = new Vector3(lpv.x, lpv.y * scatterdFishYLimit, lpv.z);
            birdsTransform[b].localScale = Vector3.one * Random.Range(scaleRandom.x, scaleRandom.y);
            birdsTransform[b].localRotation = Quaternion.Euler(0, Random.value * 360, 0);
            curentFlock[b] = Random.Range(0, schoolNum);
            birdsSpeed[b] = Random.Range(3.0f, 7.0f);
        }

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    static Quaternion FishRotationClamp(Quaternion rotationCur, float rotationClampValue)
    {
        //--------------

        Vector3 angleClamp = rotationCur.eulerAngles;
        rotationCur.eulerAngles = new Vector3(Mathf.Clamp((angleClamp.x > 180) ? angleClamp.x - 360 : angleClamp.x, -rotationClampValue, rotationClampValue), angleClamp.y, 0);
        return rotationCur;

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
