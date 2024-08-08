using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KinematicsView : MonoBehaviour
{
    [SerializeField] private GameObject[] _sceneObjects;
    private List<GameObject> _objectsToRemove;

    private LineRenderer _lineRenderer;
    private GameObject _arrow;

    private event Action OnTextCoroutineDone;

    private bool _firstIsMoving = false;
    private bool _secoundIsMoving = false;

    private Vector2 _velocity;

    public void DoKinematicsScene()
    {
        SpawnObjects();
        SpawnText(_sceneObjects[1]);

        OnTextCoroutineDone += TextFirst;
        StartCoroutine(TextVisionCoroutine(7.0f));
    }

    private void SpawnObjects()
    {
        GameObject pointer = Instantiate(_sceneObjects[0]);
        _objectsToRemove = new List<GameObject>();

        pointer.transform.parent = transform;

        _objectsToRemove.Add(pointer);

        pointer.AddComponent<RectTransform>().localPosition = new Vector2(-550, 270);
    }

    public void DestroyObjects()
    {
        foreach(var destroyableObject in _objectsToRemove)
        {
            Destroy(destroyableObject);
        }
        _objectsToRemove = null;
        _secoundIsMoving = false;
    }

    private void TextFirst()
    {
        ChangeText(_objectsToRemove[1]);

        SpawnText(_sceneObjects[2]);
        StartCoroutine(TextVisionCoroutine(7.0f));


        OnTextCoroutineDone += StartMovement;
        OnTextCoroutineDone += TextSecond;
        OnTextCoroutineDone -= TextFirst;
    }

    private void TextSecond()
    {
        StartCoroutine(TextVisionCoroutine(5.0f));

        OnTextCoroutineDone += DeleteSecondText;
        OnTextCoroutineDone += TextThird;
        OnTextCoroutineDone -= TextSecond;
    }

    private void DeleteSecondText()
    {
        ChangeText(_objectsToRemove[1]);

        OnTextCoroutineDone -= DeleteSecondText;
    }

    private void TextThird()
    {
        SpawnText(_sceneObjects[5]);

        StartCoroutine(TextVisionCoroutine(10.0f));


        OnTextCoroutineDone += DeleteThirdText;
        OnTextCoroutineDone += TextForth;
        OnTextCoroutineDone -= TextThird;
    }

    private void DeleteThirdText()
    {
        ChangeText(_objectsToRemove[3]);
        
        OnTextCoroutineDone -= DeleteThirdText;
    }

    private void TextForth()
    {
        SpawnText(_sceneObjects[6]);

        StartCoroutine(TextVisionCoroutine(15.0f));

        OnTextCoroutineDone += DeleteForthText;
        OnTextCoroutineDone -= TextForth;
    }

    private void DeleteForthText()
    {
        ChangeText(_objectsToRemove[3]);

        StartCoroutine(TextVisionCoroutine(1.5f));


        OnTextCoroutineDone += StartSecondMovement;
        OnTextCoroutineDone -= DeleteForthText;
    }

    private void StartSecondMovement()
    {
        _secoundIsMoving = true;
        
        OnTextCoroutineDone += TextSixth;
        OnTextCoroutineDone += DeleteFifth;

        var firstPoint = Instantiate(_sceneObjects[0]);
        firstPoint.transform.position = new Vector2(-12, 0);
        var firstText = Instantiate(_sceneObjects[7]);
        firstText.transform.SetParent(firstPoint.transform);
        firstText.transform.localPosition = new Vector3(0, 1, 0);

        firstPoint.transform.SetParent(transform);
        _objectsToRemove.Add(firstPoint);
        _objectsToRemove.Add(firstText);

        var secondPoint = Instantiate(_sceneObjects[0]);
        secondPoint.transform.position = new Vector2(-12, -5);
        var secondText = Instantiate(_sceneObjects[7]);
        secondText.transform.SetParent(secondPoint.transform);
        secondText.transform.localPosition = new Vector3(0, 1, 0);

        secondPoint.transform.SetParent(transform);
        _objectsToRemove.Add(secondPoint);
        _objectsToRemove.Add(secondText);

        firstPoint.GetComponent<Rigidbody2D>().velocityX = 1.0f;
        secondPoint.GetComponent<Rigidbody2D>().velocityX = 1.0f;

        SpawnText(_sceneObjects[8]);

        StartCoroutine(TextVisionCoroutine(20.0f));

        OnTextCoroutineDone -= StartSecondMovement;
    }

    private void DeleteFifth()
    {
        ChangeText(_objectsToRemove[7]);

        OnTextCoroutineDone -= DeleteFifth;
    }

    private void TextSixth()
    {
        StartCoroutine(TextVisionCoroutine(15.0f));

        SpawnText(_sceneObjects[9]);
        var round = Instantiate(_sceneObjects[10], transform);
        round.transform.localPosition = new Vector2(0, -10.0f);
        _objectsToRemove.Add(round);

        OnTextCoroutineDone -= TextSixth;
    }

    private void ChangeText(GameObject objectToDelete)
    {
        objectToDelete.GetComponent<Animator>().SetTrigger("ChangeText");
        StartCoroutine(WaitBeforeRemoveCoroutine(objectToDelete));
    }

    private void StartMovement()
    {
        _velocity.y = -4f;
        _velocity.x = 0;

        _firstIsMoving = true;
        _lineRenderer = Instantiate(_sceneObjects[3]).GetComponent<LineRenderer>();
        _arrow = Instantiate(_sceneObjects[4]);

        _objectsToRemove.Add(_arrow);
        _objectsToRemove.Add(_lineRenderer.gameObject);

        _lineRenderer.GetComponent<LineRenderer>().positionCount = 2;

        OnTextCoroutineDone -= StartMovement;
    }

    private void FixedUpdate()
    {
        if(_firstIsMoving)
        {
            var currentPosition = _objectsToRemove[0].transform.position;

            var newPosition = new Vector2(
                currentPosition.x + _velocity.x * 0.5f, 
                currentPosition.y + _velocity.y * 0.5f);
            
            _lineRenderer.SetPosition(0, new Vector3(currentPosition.x, currentPosition.y, 0));
            _lineRenderer.SetPosition(1,  newPosition);

            _arrow.transform.eulerAngles = new Vector3(0, 0, Arrow.GetAngle(_velocity));
            _arrow.transform.position = newPosition;

            _objectsToRemove[0].GetComponent<Rigidbody2D>().velocity = _velocity;

            _velocity.y += 0.02f;
            _velocity.x += 0.05f;

            

            if(newPosition.x >= 25)
            {
                _firstIsMoving = false;
                _lineRenderer.SetPosition(0, new Vector3(1000,1000,0));
            }
        }

        if(_secoundIsMoving)
        {
            var firstPoint = _objectsToRemove[3];
            var firstText = _objectsToRemove[4].GetComponent<TextMeshProUGUI>();
            var secondPoint = _objectsToRemove[5];
            var secondText = _objectsToRemove[6].GetComponent<TextMeshProUGUI>();

            firstText.text = (firstPoint.GetComponent<Rigidbody2D>().velocityX).ToString();
            secondText.text = (Mathf.Round(secondPoint.GetComponent<Rigidbody2D>().velocityX)).ToString();

            
            secondPoint.GetComponent<Rigidbody2D>().velocityX += 0.001f;
        }
    }

    private GameObject SpawnText(GameObject text)
    {

        var textObject = Instantiate(text);
        var rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.SetParent(transform);
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition = new Vector2(0, 230);

        _objectsToRemove.Add(textObject);

        return textObject;
    }

    private IEnumerator TextVisionCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        OnTextCoroutineDone?.Invoke();
    }

    private IEnumerator WaitBeforeRemoveCoroutine(GameObject objectToRemove)
    {
        yield return new WaitForSeconds(1.0f);
        _objectsToRemove.Remove(objectToRemove);
        Destroy(objectToRemove);
    }

    public void StopMovement()
    {
        _firstIsMoving = false;
    }
}