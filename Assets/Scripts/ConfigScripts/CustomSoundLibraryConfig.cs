using System.Collections.Generic;
using ConfigScripts;
using FMODUnity;

//[CreateAssetMenu(fileName = "CustomSoundLibrary", menuName = "Configs/Custom Sound Library")]
public class CustomSoundLibraryConfig : BaseConfig
{
    public List<EventReference> lobbyMusicCompositions = new ();
    public List<EventReference> lapRaceMusicCompositions = new ();
    public List<EventReference> freeRideMusicCompositions = new ();
}
