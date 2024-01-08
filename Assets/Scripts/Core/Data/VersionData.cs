using UnityEngine;

[CreateAssetMenu(fileName = "Version", menuName = "Version")]
public class VersionData : ScriptableObject
{
    [SerializeField] private string _version;

    public string GetVersion() => _version;
}
