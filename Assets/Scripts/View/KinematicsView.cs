using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TMPro.EditorUtilities;
using UnityEngine;

public class KinematicsView : MonoBehaviour
{
    [SerializeField] private GameObject[] _sceneObjects;
    private List<GameObject> _objectsToRemove;

    private LineRenderer _lineRenderer;
    private GameObject _arrow;

    private bool isMoving = false;

    private Vector2 _velocity;

    public void DoKinematicsScene()
    {
        SpawnObjects();

        GameObject text = _objectsToRemove[1];
        StartCoroutine(FirstTextCoroutine(text));
    }

    private void SpawnObjects()
    {
        GameObject pointer = Instantiate(_sceneObjects[0]);
        GameObject text1 = Instantiate(_sceneObjects[1]);
        _objectsToRemove = new List<GameObject>();

        pointer.transform.parent = transform;
        text1.transform.parent = transform;

        _objectsToRemove.Add(pointer);
        _objectsToRemove.Add(text1);

        pointer.AddComponent<RectTransform>().localPosition = new Vector2(-550, 270);
        text1.GetComponent<RectTransform>().localPosition = new Vector2(0, 230);

        text1.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    private void SpawnText2()
    {
        var text2 = Instantiate(_sceneObjects[2]);
        text2.transform.parent = transform;

        text2.GetComponent<RectTransform>().localPosition = new Vector2(0, 230);
        text2.GetComponent<RectTransform>().localScale = Vector3.one;

        _objectsToRemove.Add(text2);

        StartCoroutine(SecondTextCoroutine());
    }
    public void DestroyObjects()
    {
        foreach(var destroyableObject in _objectsToRemove)
        {
            Destroy(destroyableObject);
        }
        _objectsToRemove = null;
    }

    private void StartMovement()
    {
        _velocity.y = -0.1f;
        _velocity.x = 0;

        isMoving = true;
        _lineRenderer = Instantiate(_sceneObjects[3]).GetComponent<LineRenderer>();
        _arrow = Instantiate(_sceneObjects[4]);

        _objectsToRemove.Add(_arrow);
        _objectsToRemove.Add(_lineRenderer.gameObject);

        _lineRenderer.GetComponent<LineRenderer>().positionCount = 2;
    }

    private void Update()
    {
        if(isMoving)
        {
            var currentPosition = _objectsToRemove[0].transform.position;

            var newPosition = new Vector2(
                currentPosition.x + _velocity.x * 20.0f, 
                currentPosition.y + _velocity.y * 20.0f);
            
            _lineRenderer.SetPosition(0, new Vector3(currentPosition.x, currentPosition.y, 0));
            _lineRenderer.SetPosition(1,  newPosition);

            _arrow.transform.eulerAngles = new Vector3(0, 0, Arrow.GetAngle(_velocity));
            _arrow.transform.position = newPosition;

            _objectsToRemove[0].transform.position = Vector2.MoveTowards(
                _objectsToRemove[0].transform.position, 
                newPosition, 
                (Mathf.Abs(_velocity.y) + Mathf.Abs(_velocity.x)) / 3);

            if(newPosition.y <= -8) 
            {
                _velocity.y = 0; _velocity.x = 0.1f;
            }
        }
    }

    private IEnumerator FirstTextCoroutine(GameObject text)
    {
        yield return new WaitForSeconds(5.0f);
        text.GetComponent<Animator>().SetTrigger("ChangeText");
        StartCoroutine(WaitBeforeRemoveCoroutine(text));
        SpawnText2();
    }

    private IEnumerator SecondTextCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        StartMovement();
    }

    private IEnumerator WaitBeforeRemoveCoroutine(GameObject objectToRemove)
    {
        yield return new WaitForSeconds(1.0f);
        _objectsToRemove.Remove(objectToRemove);
        Destroy(objectToRemove);
    }

    public void StopMovement()
    {
        isMoving = false;
    }
}