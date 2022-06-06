using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatQueue
{
    private List<Character> _queue = new List<Character>();

    public int Count => _queue.Count;
    public Character Peek() => _queue[0];
    public void Enqueue(Character character) => _queue.Add(character);
    public Character Dequeue()
    {
        var character = _queue[0];
        _queue.RemoveAt(0);
        return character;
    }
    // Fisher–Yates shuffle algorithm. Relatively costly for a List.
    public void Shuffle()
    {
        for (int i = 0; i < _queue.Count - 1; i++)
        {
            int r = Random.Range(i, _queue.Count);
            (_queue[r], _queue[i]) = (_queue[i], _queue[r]);
        }
    }
}