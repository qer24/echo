using System.Collections.Generic;
using UnityEngine;

public class CameraOffsetsController : MonoBehaviour
{
    public bool Debug;

    private readonly List<Offset> _offsets = new();
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.localPosition;
    }

    private void Update()
    {
        var pos = _startPosition;
        var rot = Quaternion.identity;

        foreach (var offset in _offsets)
        {
            switch (offset)
            {
                case PositionalOffset positionalOffset:
                    pos += positionalOffset.Position;

                    break;
                case RotationalOffset rotationalOffset:
                    rot *= rotationalOffset.Rotation;

                    break;
            }
        }

        transform.localPosition = pos;
        transform.localRotation = rot;
    }

    private void OnGUI()
    {
        if (!Debug) return;

        var style = new GUIStyle
        {
            fontSize = 30
        };

        GUILayout.Label($"Offsets: {_offsets.Count}", style);

        foreach (var offset in _offsets)
        {
            switch (offset)
            {
                case PositionalOffset positionalOffset:
                    GUILayout.Label(positionalOffset.Position.ToString(), style);

                    break;
                case RotationalOffset rotationalOffset:
                    GUILayout.Label(rotationalOffset.Rotation.ToString(), style);

                    break;
            }
        }
    }

    public void AddOffset(Offset offset)
    {
        if (!_offsets.Contains(offset)) _offsets.Add(offset);
    }

    public void RemoveOffset(Offset offset)
    {
        if (_offsets.Contains(offset)) _offsets.Remove(offset);
    }

    public void ResetAllOffsets()
    {
        foreach (var offset in _offsets)
        {
            switch (offset)
            {
                case PositionalOffset positionalOffset:
                    positionalOffset.Position = Vector3.zero;

                    break;
                case RotationalOffset rotationalOffset:
                    rotationalOffset.Rotation = Quaternion.identity;

                    break;
            }
        }

        transform.localPosition = _startPosition;
        transform.localRotation = Quaternion.identity;
    }

    public abstract class Offset { }

    public class PositionalOffset : Offset
    {
        public Vector3 Position = Vector3.zero;

        public static implicit operator Vector3(PositionalOffset offset)
        {
            return offset.Position;
        }
    }

    public class RotationalOffset : Offset
    {
        public Quaternion Rotation = Quaternion.identity;

        public static implicit operator Quaternion(RotationalOffset offset)
        {
            return offset.Rotation;
        }
    }
}
