using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BaseSceneManager : MonoBehaviour
{
    [Header("Spawn positions")]
    public Transform InitialSpawnPosition;




    private bool _initialized;
    public async virtual Task<bool> Initialize()
    {
        return true;
    }

    public async Task Spawn()
    {

    }
}
