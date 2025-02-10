using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _packagePrefabs;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private float _spawnOffset = 0.3f;//Lateral range for package spawning
    private float _timer;

    private void Start()
    {
        SpawnPackage();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            SpawnPackage();
            _timer = 0;
        }
    }

    private void SpawnPackage()
    {
        //Randomly choose from array of packages
        int _index = Random.Range(0, _packagePrefabs.Length);
        
        //Add random lateral offset for spawning
        float _xOffset = Random.Range(-_spawnOffset, _spawnOffset);
        Vector3 _offset = new Vector3(_xOffset, 0, 0);
        
        Instantiate(_packagePrefabs[_index], transform.position + _offset, Quaternion.identity);
    }
}
