using UnityEditor;

namespace GoogleImporter
{
    public class ConfigImportsMenu
    {
        private const string CREDENTIALS_PATH = "clean-orb-419119-b43ae7256d4a.json";
        private const string SPREADSHEET_ID = "15WU-0J0krNH0JJ-uvffQW4otN4nJWjXad0Kju570o0I";
        private const string ITEMS_SHEETS_NAME = "Cars";
        
        
        // [MenuItem("Google/Import Items Settings")]
        // private static async void LoadItemsSettings()
        // {
        //     var sheetsImporter = new GoogleSheetsImporter(CREDENTIALS_PATH, SPREADSHEET_ID);
        //
        //     await sheetsImporter.DownloadAndParseSheet(ITEMS_SHEETS_NAME);
        // } 
    }
}