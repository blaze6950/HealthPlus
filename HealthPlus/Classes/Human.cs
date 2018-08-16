using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace HealthPlus
{
    [Serializable]
    public class Human
    {
        private String _name;
        private List<Medicament> _medicaments;
        private List<OneDay> _oneDays;
        private Uri _avatar;

        public Human(String name)
        {
            Name = name;
            _medicaments = new List<Medicament>();
            _oneDays = new List<OneDay>();
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<Medicament> Medicaments
        {
            get { return _medicaments; }
            set { _medicaments = value; }
        }

        public List<OneDay> OneDays
        {
            get { return _oneDays; }
        }

        public Uri Avatar
        {
            get { return _avatar; }
            set { _avatar = value; }
        }

        public void AddMedicament(Medicament medicament)
        {
            _medicaments.Add(medicament);
            switch (medicament.OfAdmission)
            {
                case FrequencyOfAdmission.EveryDay:
                    AddEveryDay(medicament);
                    break;
                case FrequencyOfAdmission.OnCertainDays:
                    AddCertainDays(medicament);
                    break;
                case FrequencyOfAdmission.DailyInterval:
                    AddDailyInterval(medicament);
                    break;
            }
        }

        private void AddEveryDay(Medicament medicament) // функция добавления препарата, если выбран режим каждый день
        {
            var startDate = medicament.StartDate;
            for (int i = 0; i < medicament.DurationOfAdmission; i++)
            {
                startDate = startDate.AddDays(1);
                var oneDay = FindADay(startDate);
                if (oneDay != null)
                {
                    TimesADay(oneDay, medicament);
                }
                else
                {
                    oneDay = new OneDay();
                    oneDay.Day = startDate;
                    _oneDays.Add(oneDay);
                    TimesADay(oneDay, medicament);
                }
            }
        }

        private void AddCertainDays(Medicament medicament) // функция добавления расписания, при выбранных днях недели
        {
            var startDate = medicament.StartDate;
            startDate = startDate.AddDays(1);
            for (int i = 0; i < medicament.DurationOfAdmission; i++)
            {
                for (int j = 0; j < medicament.Days.Count; j++)
                {
                    if (startDate.DayOfWeek.ToString() == medicament.Days[j].ToString())
                    {
                        var oneDay = FindADay(startDate);
                        if (oneDay != null)
                        {
                            TimesADay(oneDay, medicament);
                        }
                        else
                        {
                            oneDay = new OneDay();
                            oneDay.Day = startDate;
                            _oneDays.Add(oneDay);
                            TimesADay(oneDay, medicament);
                        }
                        break;
                    }
                }
                startDate = startDate.AddDays(1);
            }
        }

        private void AddDailyInterval(Medicament medicament)
        {
            var startDate = medicament.StartDate;
            startDate = startDate.AddDays(1);
            for (int i = 0; i < medicament.DurationOfAdmission; i++)
            {
                var oneDay = FindADay(startDate);
                if (oneDay != null)
                {
                    TimesADay(oneDay, medicament);
                }
                else
                {
                    oneDay = new OneDay();
                    oneDay.Day = startDate;
                    _oneDays.Add(oneDay);
                    TimesADay(oneDay, medicament);
                }
                startDate = startDate.AddDays(medicament.DaysOfDailyInterval);
            }
        }

        private OneDay FindADay(DateTime dayFinding) // функция, которая находит день, если он уже существует или создает новый, добавляя в него препарат и время приема
        {
            for (int i = 0; i < _oneDays.Count; i++)
            {
                if (_oneDays[i].Day == dayFinding)
                {
                    return _oneDays[i];
                }
            }
            return null;
        }

        private void TimesADay(OneDay oneday, Medicament medicament) // функция, которая добавляет в расписание дня несколько приемов препарата, если они есть
        {
            var hours = 12.0 / medicament.TimesADay;
            for (int i = 0; i < medicament.TimesADay; i++)
            {
                oneday.LightMedicaments.Add(new LightMedicament(TimeSpan.FromHours(9 + i * hours), medicament));
            }
        }
        
    }
}