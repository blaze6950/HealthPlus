using System;

namespace HealthPlus
{
    [Serializable]
    public class LightMedicament
    {
        private TimeSpan _time;
        private Medicament _medicament;
        public bool isDone = false;

        public LightMedicament(TimeSpan time, Medicament medicament)
        {
            _time = time;
            _medicament = medicament;
        }

        public TimeSpan Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public Medicament Medicament1
        {
            get { return _medicament; }
        }
    }
}