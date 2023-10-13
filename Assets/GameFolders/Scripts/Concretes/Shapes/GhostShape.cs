using System.Collections.Generic;
using UnityEngine;

public class GhostShape : MonoBehaviour
{
    #region Fields&Properties
    [SerializeField] private List<Transform> _children = new List<Transform>();
    private Transform _activeGhostShape;
    private Shape _currentShape;
    private bool _isGroundReached;
    private int yPos;
    #endregion

    #region Mono Methods
    void Start()
    {
        RegisterEvents();
        Init();
    }

    private void OnDisable()
    {
        DeregisterEvents();
    }

    #endregion

    #region Public Methods
    public void PlaceGhost()
    {
        _isGroundReached = false;
        _activeGhostShape.transform.rotation = _currentShape.transform.rotation;
        _activeGhostShape.position = _currentShape.transform.position;
        yPos = (int)_activeGhostShape.transform.position.y;
        CheckGround();
    }
    #endregion

    #region Private Methods

    private void Init()
    {
        if (_children.Count != 7)
        {
            Debug.Log("The ghost object has not properly set");
            Destroy(gameObject);
        }

        foreach (var child in _children)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void RegisterEvents()
    {
        SpawnManager.Instance.OnShapeSpawned += AssignShape;
        LevelManager.Instance.OnPlayerPassedLevel += DeactivateShape;
        GameManager.Instance.OnGameStopped += DeactivateShape;
    }

    private void DeregisterEvents()
    {
        SpawnManager.Instance.OnShapeSpawned -= AssignShape;
        LevelManager.Instance.OnPlayerPassedLevel -= DeactivateShape;
        GameManager.Instance.OnGameStopped -= DeactivateShape;
    }

    private void AssignShape(int activeChild, Shape shapeToAssign)
    {
        if(_activeGhostShape != null)
        {
            _activeGhostShape.gameObject.SetActive(false);
            _currentShape.AssignGhostShape(null);
        } 
        _currentShape = shapeToAssign;
        _currentShape.AssignGhostShape(this);
        _activeGhostShape = _children[activeChild];
        _activeGhostShape.gameObject.SetActive(true);
    }

    private async void CheckGround()
    {
        int childx = 0;
        int childy = 0;
        int counter = 0;

        while (!_isGroundReached)
        {
            foreach (Transform child in _activeGhostShape)
            {
                childx = (int)child.position.x;
                childy = (int)child.position.y - counter;
                childx = Mathf.Clamp(childx, 0, Board.Instance.Width - 1);
                childy = Mathf.Clamp(childy, 0, Board.Instance.Height - 1);
                _isGroundReached = CheckForNextPosition(childx, childy);
                if (_isGroundReached)
                    break;
            }

            if (!_isGroundReached)
            {
                counter++;
                yPos--;
            }   
        }
        _activeGhostShape.transform.position = new Vector2(_activeGhostShape.position.x, yPos);
    }

    private bool CheckForNextPosition(int x, int y)
    {
        if (y > Board.Instance.Height - 1)
            return false;

        if (y == 0)
            return true;

        if (Board.Instance.allBgTiles[x, y - 1].IsFilled)
            return true;

        return false;
    }

    private void DeactivateShape()
    {
        _activeGhostShape.gameObject.SetActive(false);
    }
    #endregion
}
