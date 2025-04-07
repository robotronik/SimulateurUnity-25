using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UIElements;

// Define the structure to parse JSON response
[System.Serializable]
public struct Pos
{
    public int x;
    public int y;
    public int theta;
}
[System.Serializable]
public struct tableState_t
{
    public Pos pos_opponent;
    public ulong startTime;
    public robot_t robot;

    public int strategy;

    public List<bool> avail_stocks;
    public bool done_banderole;
    public List<int> builtTribuneHeights;
}
[System.Serializable]
public struct robot_t
{
    public int colorTeam;
    public Pos pos;
    public Pos target;
    public double vit_x, vit_y;
    public int braking_distance;
    public int direction_side;
    public int plank_count;
}

public class RestAPI : MonoBehaviour
{
    [System.Serializable]
    public class Wrapper
    {
        public int status;
        public tableState_t table;
        public int score;
        public List<Pos> lidar;
        public List<Pos> navigation;
    }

    private static readonly HttpClient client = new HttpClient();
    [SerializeField] private string ip = "127.0.0.1";
    [SerializeField] private string port = "8080";
    private bool isFetching = false;
    public bool valid;

    public Pos robotPos;

    [SerializeField] public Wrapper global;

    void Start(){}
    void Update(){
    }
    void FixedUpdate()
    {
        if (!isFetching)
        {
            isFetching = true;
            _ = FetchData(); // Fire and forget, but track with isFetching
        }
    }

    private async Task FetchData()
    {
        string url = $"http://{ip}:{port}/get_global";
        Debug.Log($"Fetching data at {url} ...");
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            global = JsonUtility.FromJson<Wrapper>(jsonResponse);
            // Debug.Log($"Response: {jsonResponse}");
            // Parse the JSON response
            // Assuming the JSON structure matches the Wrapper class
            // You can access the data like this:
            // int status = global.status;
            Debug.Log($"Status: {global.status}");
            robotPos = global.table.robot.pos;
            // Use Main Thread for Unity operations if needed
            // await UnityMainThreadDispatcher.Instance.EnqueueAsync(() => UpdatePosition(pos));
            Debug.Log($"Position: x={robotPos.x}, y={robotPos.y}, theta={robotPos.theta}");
            valid = true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
            valid = false;
        }
        finally
        {
            isFetching = false;
        }
    }
}
