using System.Collections.Generic;

public static class WeightedRandomizer
{
    public static WeightedRandomizer<R> From<R>(Dictionary<R, int> spawnRate)
    {
        return new WeightedRandomizer<R>(spawnRate);
    }
}

public class WeightedRandomizer<T> 
{
    private static System.Random _random = new System.Random();
    private Dictionary<T, int> _weights;

    public WeightedRandomizer(Dictionary<T, int> weights)
    {
        _weights = weights;
    }

    public T TakeOne()
    {
        var sortedSpawnRate = Sort(_weights);
        int sum = 0;
        foreach (var spawn in _weights)
        {
            sum += spawn.Value;
        }

        int roll = _random.Next(0, sum);

        T selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
        foreach (var spawn in sortedSpawnRate)
        {
            if (roll < spawn.Value)
            {
                selected = spawn.Key;
                break;
            }
            roll -= spawn.Value;
        }

        return selected;
    }

    private List<KeyValuePair<T, int>> Sort(Dictionary<T, int> weights)
    {
        var list = new List<KeyValuePair<T, int>>(weights);

        list.Sort(
            delegate (KeyValuePair<T, int> firstPair,
                KeyValuePair<T, int> nextPair)
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
        );

        return list;
    }
}