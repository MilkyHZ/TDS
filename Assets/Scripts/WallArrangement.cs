using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(WallLayer))]
[ExecuteAlways]
public class WallArrangement : MonoBehaviour
{
    public Vector3 circleAxis = Vector3.up;

    private readonly List<Transform> children = new();
    private WallLayer _myWallLayer;

    private void OnValidate() => ArrangeCircle();

    private void LateUpdate() => ArrangeCircle();

    private void LookAtParent() 
    {
        foreach (var child in children)
        {
            child.LookAt(transform);
        }
    }

    private void GetChildren()
    {
        children.Clear();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
    }

    private void ArrangeCircle()
    {
        GetChildren();
        if (children.Count == 0) return;

        float targetRadius = 1f;
        var manager = FindFirstObjectByType<FortressLayerManager>();

        if (manager != null)
        {
            if (_myWallLayer == null) _myWallLayer = GetComponent<WallLayer>();

            targetRadius = FortressLayerManager.GetRadiusForTier(_myWallLayer.Tier, manager.BasePlayerRadius);
        }

        float angleStep = 360f / children.Count;

        for (int i = 0; i < children.Count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector3 pos = new(
                Mathf.Cos(angle) * targetRadius,
                0,
                Mathf.Sin(angle) * targetRadius
            );

            children[i].localPosition = pos;
        }

        LookAtParent();
    }
}