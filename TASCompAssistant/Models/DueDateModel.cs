using System;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class DueDateModel : PropertyChangedBase
    {
        private DateTime? _dueTime = new DateTime();

        private DateTime? _endDate = new DateTime();
        private DateTime? _startDate = new DateTime();

        public DateTime? StartDate
        {
            get => _startDate;
            set => SetValue(ref _startDate,
                new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 0, 0, 0, 0));
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set => SetValue(ref _endDate,
                new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 0, 0, 0, 0));
        }

        public DateTime? DueTime
        {
            get => _dueTime;
            set => SetValue(ref _dueTime,
                new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day, value.Value.Hour,
                    value.Value.Minute, value.Value.Second, 0));
        }

        public DueDateModel()
        {
            ClearDueDates();
        }

        public void ClearDueDates()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            DueTime = DateTime.Now;
        }
    }
}