using System;
using V9Common;

namespace V9ManagerIVR.Models.Calendar
{
    public class ListCalendarModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public V9_ScheduleActionType CalendarType { get; set; }
    }
}
