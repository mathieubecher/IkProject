using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Anchor
{
    [SerializeField] private float minIncline;
    [SerializeField] private float maxIncline;

    [SerializeField] private float minRotate;
    [SerializeField] private float maxRotate;

    [SerializeField] private bool limitIncline = false;
    [SerializeField] private bool limitRotate = false;

    public float MinIncline { get => minIncline; set => minIncline = value; }
    public float MaxIncline { get => maxIncline; set => maxIncline = value; }
    public float MinRotate { get => minRotate; set => minRotate = value; }
    public float MaxRotate { get => maxRotate; set => maxRotate = value; }
    public bool LimitIncline { get => limitIncline; set => limitIncline = value; }
    public bool LimitRotate { get => limitRotate; set => limitRotate = value; }
}
