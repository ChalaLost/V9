using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9ManagerIVR.Models.CRM;

namespace V9ManagerIVR.Models.Extension
{
    public class ListExtensionModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Network { get; set; }
        public string Exten { get; set; }
        public int CurrentCall { get; set; }
        public List<ListExtensionDetailModel> Details = new();
    }

    public class ListExtensionDetailModel
    {

        public Guid? SelectedIVR { get; set; }
        public List<SelectModel> IVRs = new();
        public Guid? SelectedCalendar { get; set; }

        public List<SelectModel> Calendars = new();
        public int? Priority { get; set; }
    }


    public class UpdateExtensionModel
    {
        public Guid Id { get; set; }
        public int CurrentCall { get; set; }
        public List<UpdateExtensionDetailModel> Details { get; set; }

    }

    public class UpdateExtensionDetailModel
    {
        public Guid? CalendarId { get; set; }
        public Guid? IVRId { get; set; }
        public int Priority { get; set; }
    }

}
