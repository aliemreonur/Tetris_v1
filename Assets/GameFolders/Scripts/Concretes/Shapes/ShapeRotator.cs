using UnityEngine;

public class ShapeRotator : IShapeRotator
{
    public Transform entityTransform { get; }
    private Shape _shape;
    private Quaternion _initialRotation;

    public ShapeRotator(Shape shape)
    {
        _shape = shape;
        entityTransform = _shape.transform;
        _initialRotation = entityTransform.rotation;
        _shape.OnSpawn += SpawnRotation;
    }

    public void SpawnRotation()
    {
        entityTransform.rotation = _initialRotation;
    }

    public void RotateShape()
    {
        if (!CanRotate())
            return;

        Vector3 currentPos = entityTransform.position;
        Quaternion currentRot = entityTransform.rotation;

        entityTransform.Rotate(0, 0, 90);
        _shape.CorrectiveSideMovement();

        foreach (var piece in _shape.shapePieces)
        {
            if (piece.CheckForRotatedPosition() == false)
            {
                entityTransform.position = currentPos;
                entityTransform.rotation = currentRot;
                return;
            }
        }
    }

    private bool CanRotate()
    {
        if (entityTransform.position.y > Board.Instance.Height - 2)
        {
            return false;
        }
        return true;
    }

}
