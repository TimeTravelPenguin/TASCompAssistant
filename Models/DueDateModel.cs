using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    class DueDateModel : PropertyChangedBase
    {
        private DateTime? _startDate = new DateTime();
        public DateTime? StartDate
        {
            get => _startDate;
            set => SetValue(ref _startDate, value);
        }

        private DateTime? _endDate = new DateTime();
        public DateTime? EndDate
        {
            get => _endDate;
            set => SetValue(ref _endDate, value);
        }

        private DateTime? _dueTime = new DateTime();
        public DateTime? DueTime
        {
            get => _dueTime;
            set => SetValue(ref _dueTime, value);
        }

        public void ClearDueDates()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            DueTime = DateTime.Now;
        }
    }
}
