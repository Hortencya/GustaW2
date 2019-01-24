using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journey  {

    public List<Node> path;
    public Transform owner, target;
    // Use this for initialization
    public Journey(Transform _star) {
        owner = _star;
    }
    public Journey(List<Node> _path, Transform _start, Transform _end) {
        path = _path;
        owner = _start;
        target = _end;
    }
    public bool OwnedBy(Transform ownerPos)
    {
        if (owner == ownerPos)
        {
            return true;
        }
        else return false;
    }
    public bool HaveTarget()
    {
        if (target == null) return false; else return true;
    }
    public void SetTarget(Transform targetPos)
    {
        target = targetPos;
    }
}
