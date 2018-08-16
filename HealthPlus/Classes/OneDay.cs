using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace HealthPlus
{
    [Serializable]
    public class OneDay
    {
        private DateTime _day;
        private List<LightMedicament> _lightMedicaments;

        public OneDay()
        {
            _lightMedicaments = new List<LightMedicament>();
        }

        public DateTime Day
        {
            get { return _day; }
            set { _day = value; }
        }

        public List<LightMedicament> LightMedicaments
        {
            get { return _lightMedicaments; }
        }
    }
}