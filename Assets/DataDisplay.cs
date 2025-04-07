using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDisplay : MonoBehaviour
{
    RestAPI restAPI;
    public List<GameObject> stocks = new List<GameObject>();
    private List<GameObject> lidarPoints = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        restAPI = this.GetComponent<RestAPI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!restAPI.valid) return;
        for (int i = 0; i < stocks.Count; i++)
        {
            GameObject go = stocks[i];
            bool present = restAPI.global.table.avail_stocks[i];
            go.SetActive(present);
        }

        // Instantiate a pool of gameobjects (spheres) for each point of lidardata
        foreach (GameObject go in lidarPoints) { 
            Destroy(go);
        }
        lidarPoints.Clear();
        foreach (Pos p in restAPI.global.lidar)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.transform.position = new Vector3((float)(p.x)/1000f, 0.35f, (float)(p.y)/1000f);
            go.transform.localScale = new Vector3(0.02f, 0.1f, 0.02f);
            lidarPoints.Add(go);
        }

    }
}
