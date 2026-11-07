using KTC.Domain;
using System.Collections.Generic;
using UnityEngine;

namespace KTC.Infrastructure
{
    public class ObstacleColumnSpawner : MonoBehaviour
    {
        [SerializeField]
        private ObstacleSpawner _tileSpawner = null;

        public bool TrySpawnColumn(Vector3 bottomPosition)
        {
            if (_tileSpawner == null || _tileSpawner.StageSettings == null)
            {
                return false;
            }

            StageSettings settings = _tileSpawner.StageSettings;
            int rowCount = settings.ObstacleRowCount;
            int gapRowCount = settings.ObstacleGapRowCount;
            float rowSpacing = settings.ObstacleRowSpacing;

            if (gapRowCount < 1 || gapRowCount >= rowCount || rowSpacing <= 0)
            {
                return false;
            }

            // TODO: ’[‚Ü‚ÅŒŠ‚É‚È‚ç‚È‚¢‚æ‚¤‚É‚·‚é.
            int gapStartRow = Random.Range(0, rowCount - gapRowCount + 1);
            int gapEndExclusive = gapStartRow + gapRowCount;

            bool shouldFillGap =
                Random.Range(0, StageSettings.PERCENT_SCALE) < settings.DestructibleGapFillPercent;

            int tileCount = shouldFillGap ? rowCount : rowCount - gapRowCount;

            List<ObstacleInstaller> spawnedTiles = new(tileCount);

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
                    foreach (ObstacleInstaller spawnedTile in spawnedTiles)
                    {
                        spawnedTile.ReturnToPool();
                    }

                    return false;
                }

                spawnedTiles.Add(tile);
            }

            return true;
        }
    }
}
