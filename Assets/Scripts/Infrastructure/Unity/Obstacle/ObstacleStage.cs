using KTC.Presentation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTC.Infrastructure
{
    public sealed class ObstacleStage : MonoBehaviour, IGameCycleStage
    {
        [SerializeField]
        private ObstacleColumnSpawner _obstacleColumnSpawner = null;

        [SerializeField]
        private Transform _spawnPositionBottom = null;

        [SerializeField]
        private Transform _passPosition = null;

        [SerializeField, Min(0.01f)]
        private float _spawnInterval = 2f;

        [SerializeField, Min(0.01f)]
        private float _moveSpeed = 3f;

        [SerializeField]
        private float _worldMinY = -5f;

        [SerializeField]
        private float _worldMaxY = 5f;

        private readonly List<ObstacleColumn> _columns = new();
        private bool _isRunning = false;
        private float _spawnElapsed = 0f;
        private int _nextColumnId = 0;
        private int _runVersion = 0;

        public event Action<int> OnPassedColumn;

        public bool IsOutsideVerticalBounds(float positionY)
        {
            if (!float.IsFinite(_worldMinY) || !float.IsFinite(_worldMaxY) ||
                _worldMinY >= _worldMaxY)
            {
                return false;
            }

            return positionY < _worldMinY || positionY > _worldMaxY;
        }

        public void Reset()
        {
            StopRunning();
            _runVersion++;

            foreach (ObstacleColumn column in _columns)
            {
                foreach (ObstacleInstaller tile in column.Tiles)
                {
                    if (tile != null)
                    {
                        tile.OnReturnedToPool -= HandleTileReturned;
                        tile.ReturnToPool();
                    }
                }
            }

            _columns.Clear();
            _nextColumnId = 0;
            _spawnElapsed = 0f;
        }

        public void SpawnObstacleColumn()
        {
            if (_obstacleColumnSpawner == null || _spawnPositionBottom == null)
            {
                return;
            }

            if (_obstacleColumnSpawner.TrySpawnColumn(
                _spawnPositionBottom.position,
                out IReadOnlyList<ObstacleInstaller> spawnedTiles))
            {
                ObstacleColumn column = new(
                    _nextColumnId++, _spawnPositionBottom.position.x, spawnedTiles);
                _columns.Add(column);

                foreach (ObstacleInstaller tile in column.Tiles)
                {
                    tile.OnReturnedToPool += HandleTileReturned;
                }
            }
        }

        public void StartRunning()
        {
            if (_isRunning || _obstacleColumnSpawner == null ||
                _spawnPositionBottom == null || _passPosition == null ||
                !float.IsFinite(_spawnInterval) || _spawnInterval <= 0f ||
                !float.IsFinite(_moveSpeed) || _moveSpeed <= 0f)
            {
                return;
            }

            _spawnElapsed = 0f;
            _isRunning = true;
        }

        public void StopRunning()
        {
            _isRunning = false;
        }

        private void FixedUpdate()
        {
            if (!_isRunning || _passPosition == null)
            {
                return;
            }

            float distance = _moveSpeed * Time.fixedDeltaTime;
            float passX = _passPosition.position.x;
            int runVersion = _runVersion;

            for (int columnIndex = _columns.Count - 1; columnIndex >= 0; columnIndex--)
            {
                ObstacleColumn column = _columns[columnIndex];
                float previousX = column.X;
                column.X -= distance;

                for (int tileIndex = column.Tiles.Count - 1; tileIndex >= 0; tileIndex--)
                {
                    ObstacleInstaller tile = column.Tiles[tileIndex];
                    if (tile == null)
                    {
                        column.Tiles.RemoveAt(tileIndex);
                        continue;
                    }

                    tile.transform.position += Vector3.left * distance;
                }

                if (!column.HasPassed && previousX > passX && column.X <= passX)
                {
                    column.HasPassed = true;
                    OnPassedColumn?.Invoke(column.Id);

                    if (!_isRunning || runVersion != _runVersion)
                    {
                        return;
                    }
                }

                if (column.HasPassed && column.Tiles.Count == 0)
                {
                    _columns.RemoveAt(columnIndex);
                }
            }

            _spawnElapsed += Time.fixedDeltaTime;
            if (_spawnElapsed >= _spawnInterval)
            {
                _spawnElapsed %= _spawnInterval;
                SpawnObstacleColumn();
            }
        }

        private void HandleTileReturned(ObstacleInstaller tile)
        {
            tile.OnReturnedToPool -= HandleTileReturned;

            foreach (ObstacleColumn column in _columns)
            {
                if (column.Tiles.Remove(tile))
                {
                    return;
                }
            }
        }

        private void OnDisable()
        {
            StopRunning();
        }

        private void OnDestroy()
        {
            Reset();
        }

        private sealed class ObstacleColumn
        {
            public int Id { get; }
            public float X { get; set; }
            public bool HasPassed { get; set; }
            public List<ObstacleInstaller> Tiles { get; }

            public ObstacleColumn(
                int id, float x, IReadOnlyList<ObstacleInstaller> tiles)
            {
                Id = id;
                X = x;
                Tiles = new List<ObstacleInstaller>(tiles);
            }
        }
    }
}
