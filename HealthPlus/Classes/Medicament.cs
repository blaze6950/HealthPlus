using System;
using System.Collections.Generic;

namespace HealthPlus
{
    [Serializable]
    public class Medicament
    {
        #region Fields

        private string _title; // пользовательское название препарата
        private string _name; // название препарата
        private int _num; // количество принимаемого препарата
        private UnitOfPreparation _unitOfPreparation; // наименование единицы препарата
        private DateTime _startDate; // дата начала приема препарата
        private FrequencyOfAdmission _frequencyOfAdmission; // периодичность приема препарата
        private int _daysOfDailyInterval; // если выбран дневной интервал, то тут хранится кол-во дней в интервале
        private List<CertainDays> _certainDays; // если выбрано по определенным дня, то тут хранятся дни, по которым надо делать прием
        private int _durationOfAdmission; // длительность приема препарата
        private int _timesADay; // сколько раз в день
        private string _notes; // примечания
        #endregion

        #region Constructors
        public Medicament()
        {
            _certainDays = new List<CertainDays>();
        }

        public Medicament(string name)
        {
            _name = name;
            _certainDays = new List<CertainDays>();
        }
        #endregion

        #region Properties

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Num
        {
            get { return _num; }
            set { _num = value; }
        }

        public UnitOfPreparation UnitOfPreparation
        {
            get { return _unitOfPreparation; }
            set { _unitOfPreparation = value; }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public FrequencyOfAdmission OfAdmission
        {
            get { return _frequencyOfAdmission; }
            set { _frequencyOfAdmission = value; }
        }

        public int DaysOfDailyInterval
        {
            get { return _daysOfDailyInterval; }
            set { _daysOfDailyInterval = value; }
        }

        public List<CertainDays> Days
        {
            get { return _certainDays; }
        }

        public int DurationOfAdmission
        {
            get { return _durationOfAdmission; }
            set { _durationOfAdmission = value; }
        }

        public int TimesADay
        {
            get { return _timesADay; }
            set { _timesADay = value; }
        }

        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }
        #endregion
    }
}