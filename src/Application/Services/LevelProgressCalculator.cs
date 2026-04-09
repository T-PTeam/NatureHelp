namespace Application.Services;

public static class LevelProgressCalculator
{
    public static (int Level, int XpIntoLevel, int XpToNext) ComputeProgress(int totalXp, int[] thresholds)
    {
        if (thresholds.Length == 0)
            return (1, totalXp, 0);

        var level = 1;
        for (var i = 1; i < thresholds.Length; i++)
        {
            if (totalXp >= thresholds[i])
                level = i + 1;
        }

        level = Math.Clamp(level, 1, thresholds.Length);

        var floorXp = thresholds[level - 1];
        var xpInto = totalXp - floorXp;
        var xpToNext = level < thresholds.Length ? thresholds[level] - totalXp : 0;
        if (xpToNext < 0) xpToNext = 0;

        return (level, xpInto, xpToNext);
    }

    public static int ComputeLevel(int totalXp, int[] thresholds)
    {
        if (thresholds.Length == 0)
            return 1;
        var level = 1;
        for (var i = 1; i < thresholds.Length; i++)
        {
            if (totalXp >= thresholds[i])
                level = i + 1;
        }

        return Math.Clamp(level, 1, thresholds.Length);
    }
}
