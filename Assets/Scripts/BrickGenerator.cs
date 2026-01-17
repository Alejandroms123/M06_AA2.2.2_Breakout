using UnityEngine;

public class BrickGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _brick;

    [SerializeField] private int _rows;
    [SerializeField] private int _columns;

    [SerializeField] private float _spacing;
    [SerializeField] private float _saturationStep;
    [SerializeField] private Vector2 _startPosition;

    public void GenerateBricks()
    {
        Vector2 prefabSize = _brick.transform.localScale;

        float totalWidth = _columns * prefabSize.x + (_columns - 1) * _spacing;
        float totalHeight = _rows * prefabSize.y + (_rows - 1) * _spacing;

        Vector2 topLeft = new Vector2(
            _startPosition.x - totalWidth / 2 + prefabSize.x / 2,
            _startPosition.y + totalHeight / 2 - prefabSize.y / 2);

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                Vector2 position = new Vector2(
                    topLeft.x + col * (prefabSize.x + _spacing),
                    topLeft.y - row * (prefabSize.y + _spacing));

                GameObject brick = Instantiate(_brick, position, Quaternion.identity, transform);
                Brick b = brick.GetComponent<Brick>();
                if (b != null)
                    b.Initialize(row, col, _rows, _columns);
            }
        }
    }

    public bool AllBricksDestroyed()
    {
        return transform.childCount == 0;
    }
}