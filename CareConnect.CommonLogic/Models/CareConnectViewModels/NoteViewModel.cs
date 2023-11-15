using CareConnect.CommonLogic.Enums;

namespace CareConnect.CommonLogic.Models.CareConnectViewModels
{
    public class NoteViewModel
    {
        public string SourceId { get; set; }
        public string Comment { get; set; }
        public NoteSource Source { get; set; }
    }
}
