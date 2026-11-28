using KTC.Domain;
using System.Collections.Generic;
using UnityEngine;

namespace KTC.Infrastructure
{
    public class ObstacleColumnSpawner : MonoBehaviour
    {
        [SerializeField]
        private ObstacleSpawner _tileSpawner = null;

        public bool TrySpawnColumn(
            Vector3 bottomPosition,
            out IReadOnlyList<ObstacleInstaller> spawnedTiles)
        {
            spawnedTiles = null;

            if (_tileSpawner == null || _tileSpawner.StageSettings == null)
            {
                return false;
            }

            StageSettings settings = _tileSpawner.StageSettings;
            int rowCount = settings.ObstacleRowCount;
            int gapRowCount = settings.ObstacleGapRowCount;
            float rowSpacing = settings.ObstacleRowSpacing;

            if (rowCount < 3 || gapRowCount < 1 || gapRowCount > rowCount - 2 || rowSpacing <= 0)
            {
                return false;
            }

            int gapStartRow = Random.Range(1, rowCount - gapRowCount);
            int gapEndExclusive = gapStartRow + gapRowCount;

            bool shouldFillGap =
                Random.Range(0, StageSettings.PERCENT_SCALE) < settings.DestructibleGapFillPercent;

            int tileCount = shouldFillGap ? rowCount : rowCount - gapRowCount;

            List<ObstacleInstaller> columnTiles = new(tileCount);

            for (int row = 0; row < rowCount; row++)
            {
                bool isGapRow = row >= gapStartRow && row < gapEndExclusive;

                if (isGapRow && !shouldFillGap)
                {
                    continue;
                }

                ObstacleType type = isGapRow
                    ? ObstacleType.Destructible
                    : ObstacleType.Normal;

                Vector3 position = bottomPosition;
                position.y += row * rowSpacing;

                if (!_tileSpawner.TrySpawn(type, position, out ObstacleInstaller tile))
                {
                    foreach (ObstacleInstaller spawnedTile in columnTiles)
                    {
                        spawnedTile.ReturnToPool();
                    }

                    return false;
                }

                columnTiles.Add(tile);
            }

            spawnedTiles = columnTiles;
            return true;
        }
    }
}
