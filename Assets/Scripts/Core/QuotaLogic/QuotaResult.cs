namespace Core.QuotaLogic
{
    public class QuotaResult
    {
        private bool _isSuccess;
        private Quota _quota;
        private int _quotaContribution;
        private double _quotaCompletion; // размер_вклада / размер_квоты.
        
        public bool IsSuccess { get; set; }
        public Quota Quota { get; set; }
        public int QuotaContribution { get; set; }
    }
}