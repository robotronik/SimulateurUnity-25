using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

// Define the structure to parse JSON response
[System.Serializable]
public struct Pos
{
    public int x { get; set; }
    public int y { get; set; }
    public int theta { get; set; }
}
public class RestAPI : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();
    [SerializeField] private string ip = "127.0.0.1";
    [SerializeField] private string port = "8080";
    private bool isFetching = false;
    public bool valid;
    void Start(){}
    void Update(){
        if (!isFetching)
        {
            isFetching = true;
            _ = FetchData(); // Fire and forget, but track with isFetching
        }
    }
    void FixedUpdate()
    {}

    private async Task FetchData()
    {
        string url = $"http://{ip}:{port}/position";
        Debug.Log($"Fetching data at {url} ...");
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            Pos pos = JsonUtility.FromJson<Pos>(jsonResponse);
            // Use Main Thread for Unity operations if needed
            // await UnityMainThreadDispatcher.Instance.EnqueueAsync(() => UpdatePosition(pos));
            Debug.Log($"Position: x={pos.x}, y={pos.y}, theta={pos.theta}");
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
