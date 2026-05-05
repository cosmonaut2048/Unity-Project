namespace Services.SaveComponents
{
    [System.Serializable]
    public class GameData
    {
        public OfficeRuntimeData office;

        public void SetGameData()
        {
            office = new OfficeRuntimeData();
            office.SetDataFromOffice();
        }
    }
}