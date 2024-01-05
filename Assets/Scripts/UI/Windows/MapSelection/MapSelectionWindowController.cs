namespace UI.Windows.MapSelection
{
    public class MapSelectionWindowController : UIController<MapSelectionWindowView, MapSelectionWindowModel>
    {
        protected override MapSelectionWindowModel GetViewData()
        {
            return new MapSelectionWindowModel();
        }
    }
}