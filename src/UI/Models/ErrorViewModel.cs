namespace MusicStore.Models
{
    public class ErrorViewModel
    {
        public string CorrelationId { get; set; }
        
        public bool CorrelationIdPresent => !string.IsNullOrEmpty(CorrelationId);

        public string ErrorMessage { get; set; }
    }
}