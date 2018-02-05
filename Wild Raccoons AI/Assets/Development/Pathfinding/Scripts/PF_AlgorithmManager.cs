using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PF_AlgorithmManager : MonoBehaviour
{
    public struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 start_, Vector3 end_, Action<Vector3[], bool> callback_)
        {
            pathStart = start_;
            pathEnd = end_;
            callback = callback_;
        }
    }

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    public static PF_AlgorithmManager instance;
    public PF_Algorithm pathfinding;

    private bool currentlyProcessingPath = false;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PF_Algorithm>();
    }

    public static void RequestPath(Vector3 pathStart_, Vector3 pathEnd_, Action<Vector3[], bool> callback_)
    {
        // Create the request.
        PathRequest newRequest = new PathRequest(pathStart_, pathEnd_, callback_);

        // Add the request to the queue;
        instance.pathRequestQueue.Enqueue(newRequest);

        instance.TryProcessNext();
    }

    public void TryProcessNext()
    {
        // Ensure that we are not already processing a path, and that a path request exists.
        if(!currentlyProcessingPath && pathRequestQueue.Count > 0)
        {
            // Assign the current path request to be the first in the queue.
            currentPathRequest = pathRequestQueue.Dequeue();

            // Set currently processing flag to true.
            currentlyProcessingPath = true;

            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path_, bool success_)
    {
        // Perform the callback.
        currentPathRequest.callback(path_, success_);

        // Set currently processing flag to false;
        currentlyProcessingPath = false;

        // Move onto the next request.
        TryProcessNext();
    }
}
