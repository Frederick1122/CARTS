namespace UI.Windows.Settings
{
    public class SettingsWindowController : UIController<SettingsWindowView, SettingsWindowModel>
    {
        protected override SettingsWindowModel GetViewData()
        {
            return new SettingsWindowModel();
        }
    }
}