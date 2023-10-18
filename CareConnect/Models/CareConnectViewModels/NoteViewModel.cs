using CareConnect.Enums;

namespace CareConnect.Models.CareConnectViewModels
{
    public class NoteViewModel
    {
        public string SourceId { get; set; }
        public string Comment { get; set; }
        public NoteSource Soure { get; set; }
    }
}
