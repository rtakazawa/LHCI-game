using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment_SpawnManager : MonoBehaviour
{
    [SerializeField] public bool _stopSpawning = false;
    [SerializeField] public GameObject _cloudsContainer;
    [SerializeField] public GameObject[] _cloudPrefabs;


    public float _rangeY = 100f; 
    public float _rangeZ = 200f;
    

    
    // Start is called before the first frame update
    void Start()
    {
        StartSpawning();
    }
    public void StartSpawning()
    {
        StartCoroutine(CloudsRoutine());
    }

    IEnumerator CloudsRoutine()
    {
        
        while (_stopSpawning == false)
        {
            var _posY = transform.position.y;
            var _sumY = _posY + _rangeY;
            var _rndY = Random.Range(_posY, _sumY);

            var _posZ = transform.position.z;
            var _sumZ = _posZ + _rangeZ;
            var _rndZ = Random.Range(_posZ, _sumZ);

            int _rndCloudIndex = Random.Range(0, _cloudPrefabs.Length);
            Vector3 posToSpawn = new Vector3(transform.position.x, _rndY, _rndZ);
            GameObject newCloud = Instantiate(_cloudPrefabs[_rndCloudIndex], posToSpawn, Quaternion.identity);
            newCloud.transform.parent = _cloudsContainer.transform;

            yield return new WaitForSeconds(Random.Range(7f,30f));
        }
    }


    // Update is called once per frame
    void Update()
    {
        

    }
}
