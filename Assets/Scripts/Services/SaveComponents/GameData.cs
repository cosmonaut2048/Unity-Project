namespace Services.SaveComponents
{
    [System.Serializable]
    public class GameData
    {
        public OfficeRuntimeData office;
        public WorkerPlacementData workerPlacement;

        public void SetGameData()
        {
            office = new OfficeRuntimeData();
            office.SetDataFromOffice();

            workerPlacement = new WorkerPlacementData();
            workerPlacement.SetDataFromPlacement();
        }
    }
}