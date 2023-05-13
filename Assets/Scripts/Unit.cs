using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public event Action<Unit> MovementFinished;

    [SerializeField]
    private int movementPoints = 20;
    [SerializeField]
    private float movementDuration = 1;
    [SerializeField]
    private float rotationDuration = 0.3f;

    private GlowHighlight _glowHighlight;
    private Queue<Vector3> _pathPositions = new();

    public int MovementPoints => movementPoints;

    private void Awake()
    {
        _glowHighlight = GetComponent<GlowHighlight>();
    }

    public void Select()
    {
        _glowHighlight.ToggleGlow(true);
    }

    public void Deselect()
    {
        _glowHighlight.ToggleGlow(false);
    }

    public void MoveThroughPath(IEnumerable<Vector3> currentPath)
    {
        _pathPositions = new Queue<Vector3>(currentPath);
        var firstTarget = _pathPositions.Dequeue();
        StartCoroutine(RotationCoroutine(firstTarget));
    }

    private IEnumerator RotationCoroutine(Vector3 endPosition)
    {
        var currentTransform = transform;
        var currentPosition = currentTransform.position;
        var currentRotation = currentTransform.rotation;

        var startRotation = currentRotation;
        endPosition.y = currentPosition.y;
        var direction = endPosition - currentPosition;
        var endRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1) == false)
        {
            float timeElapsed = 0;

            while (timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                var lerpStep = timeElapsed / rotationDuration;
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                yield return null;
            }

            transform.rotation = endRotation;
        }

        StartCoroutine(MovementCoroutine(endPosition));
    }

    private IEnumerator MovementCoroutine(Vector3 endPosition)
    {
        var startPosition = transform.position;
        endPosition.y = startPosition.y;

        float timeElapsed = 0;

        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            var lerpStep = timeElapsed / movementDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
            yield return null;
        }

        transform.position = endPosition;

        if (_pathPositions.Count > 0)
        {
            var nextTarget = _pathPositions.Dequeue();
            StartCoroutine(RotationCoroutine(nextTarget));
        }
        else
        {
            MovementFinished?.Invoke(this);
        }
    }
}
