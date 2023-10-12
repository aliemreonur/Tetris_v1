using System.Collections.Generic;
using UnityEngine;


public class GhostShape : MonoBehaviour
{
    #region Fields&Properties
    [SerializeField] private List<Transform> _children = new List<Transform>();
    private Transform _activeShape;
    private Shape _currentShape;
    private bool _isGroundReached;
    private int xPos, yPos;
    #endregion

    #region Mono Methods
    void Start()
    {
        SpawnManager.Instance.OnShapeSpawned += AssignShape;
        if (_children.Count != 7)
        {
            Debug.Log("The ghost object has not properly set");
            Destroy(gameObject);
        }
 
        foreach(var child in _children)
        {
            child.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Public Methods
    public void PlaceGhost()
    {
        _isGroundReached = false;
        _activeShape.transform.rotation = _currentShape.transform.rotation;
        _activeShape.position = _currentShape.transform.position;
        xPos = (int)_activeShape.transform.position.x;
        yPos = (int)_activeShape.transform.position.y;
        CheckGround();
    }
    #endregion

    #region Private Methods

    private void AssignShape(int activeChild, Shape shapeToAssign)
    {
        if(_activeShape != null)
        {
            _activeShape.gameObject.SetActive(false);
            _currentShape.AssignGhostShape(null);
        } 
        _currentShape = shapeToAssign;
        _currentShape.AssignGhostShape(this);
        _activeShape = _children[activeChild];
        _activeShape.gameObject.SetActive(true);
    }

    private void CheckGround()
    {
        int childx = 0;
        int childy = 0;
        int counter = 0;

        while (!_isGroundReached)
        {
            foreach (Transform child in _activeShape)
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
        _activeShape.transform.position = new Vector2(_activeShape.position.x, yPos);
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
    #endregion
}
