using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mantin.Controls.Wpf.Notification;

namespace HealthPlus
{
    /// <summary>
    /// Interaction logic for AddMedicament.xaml
    /// </summary>
    public partial class AddMedicament : Window
    {
        private ListBox listBoxOnCertainDays = null;
        private TextBox textBoxDailyInterval = null;
        private bool _isEdited = false;
        private List<Human> _people;
        private FrequencyOfAdmission _frequencyOfAdmission;

        private Medicament _medicament;
        private Human _human;

        public AddMedicament(List<Human> people, String selectedHuman)
        {
            InitializeComponent();
            _people = people;
            foreach (var human in people)
            {
                ComboBoxPeople.Items.Add(human.Name);
            }
            foreach (string human in ComboBoxPeople.Items)
            {
                if (human == selectedHuman)
                {
                    ComboBoxPeople.SelectedItem = human;
                }
            }            
            ComboBoxMedicament.Items.Add("Tablets");
            ComboBoxMedicament.Items.Add("Spray");
            ComboBoxMedicament.Items.Add("Syrup");
            ComboBoxMedicament.Items.Add("Suspension");
            ComboBoxMedicament.Items.Add("Ampoule");
            ComboBoxMedicament.SelectedIndex = 0;

            ComboBoxCountName.Items.Add("tabl");
            ComboBoxCountName.Items.Add("time(s)");
            ComboBoxCountName.Items.Add("ml");
            ComboBoxCountName.Items.Add("mg");
            ComboBoxCountName.Items.Add("piece(s)");
            ComboBoxCountName.SelectedIndex = 0;

            for (int i = 1; i < 25; i++)
            {
                ComboBoxTimesOfADay.Items.Add(i);
            }
        }

        public AddMedicament(Medicament medicament, Human human)
        {
            InitializeComponent();
            ComboBoxMedicament.Items.Add("Tablets");
            ComboBoxMedicament.Items.Add("Spray");
            ComboBoxMedicament.Items.Add("Syrup");
            ComboBoxMedicament.Items.Add("Suspension");
            ComboBoxMedicament.Items.Add("Ampoule");

            ComboBoxCountName.Items.Add("tabl");
            ComboBoxCountName.Items.Add("time(s)");
            ComboBoxCountName.Items.Add("ml");
            ComboBoxCountName.Items.Add("mg");
            ComboBoxCountName.Items.Add("piece(s)");

            for (int i = 1; i < 25; i++)
            {
                ComboBoxTimesOfADay.Items.Add(i);
            }

            ComboBoxPeople.Items.Add(human.Name);
            ComboBoxPeople.SelectedIndex = 0;
            ComboBoxPeople.IsEnabled = false;

            TextBoxTitle.Text = medicament.Title;

            ComboBoxMedicament.SelectedItem = medicament.Name;

            TextBoxCountMedicament.Text = medicament.Num.ToString();

            ComboBoxCountName.SelectedItem = medicament.UnitOfPreparation.ToString();

            DatePickerStartDate.SelectedDate = medicament.StartDate;

            switch (medicament.OfAdmission)
            {
                case FrequencyOfAdmission.EveryDay:
                    EveryDay.IsChecked = true;
                    break;
                case FrequencyOfAdmission.OnCertainDays:
                    OnCertainDays.IsChecked = true;
                    foreach (var item in listBoxOnCertainDays.Items)
                    {
                        foreach (var day in medicament.Days)
                        {
                            if (((CheckBox)item).Content.ToString() == day.ToString())
                            {
                                ((CheckBox) item).IsChecked = true;
                            }
                        }
                    }
                    break;
                case FrequencyOfAdmission.DailyInterval:
                    DailyInterval.IsChecked = true;
                    textBoxDailyInterval.Text = medicament.DaysOfDailyInterval.ToString();
                    break;
            }

            TextBoxDuring.Text = medicament.DurationOfAdmission.ToString();

            ComboBoxTimesOfADay.SelectedItem = medicament.TimesADay;

            TextBoxNotes.Text = medicament.Notes;
            _medicament = medicament;
            _human = human;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_isEdited)
            {
                var res = MessageBox.Show("Really?", "Exit?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isEdited = true;
            if (((ComboBox)sender).Name == "ComboBoxMedicament")
            {
                ComboBoxCountName.SelectedIndex = ((ComboBox) sender).SelectedIndex;
            }
        }

        private void Selector_OnSelectionChanged(object sender, TextChangedEventArgs e)
        {
            _isEdited = true;
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            _isEdited = true;
            if (((RadioButton)e.OriginalSource).Name == "EveryDay")
            {
                var listBoxOrTextBoxOrNothing = this.ListBoxOrTextBoxOrNothing;
                if (listBoxOrTextBoxOrNothing != null) listBoxOrTextBoxOrNothing.Children.Clear();
                _frequencyOfAdmission = FrequencyOfAdmission.EveryDay;
            }
            else if (((RadioButton)e.OriginalSource).Name == "OnCertainDays")
            {
                var listBoxOrTextBoxOrNothing = this.ListBoxOrTextBoxOrNothing;
                if (listBoxOrTextBoxOrNothing != null)
                {
                    listBoxOrTextBoxOrNothing.Children.Clear();
                    listBoxOnCertainDays = new ListBox();
                    listBoxOnCertainDays.Name = "ListBoxCertainDays";
                    var cbMonday = new CheckBox();
                    cbMonday.Content = "Monday";
                    listBoxOnCertainDays.Items.Add(cbMonday);
                    var cbTuesday = new CheckBox();
                    cbTuesday.Content = "Tuesday";
                    listBoxOnCertainDays.Items.Add(cbTuesday);
                    var cbWednesday = new CheckBox();
                    cbWednesday.Content = "Wednesday";
                    listBoxOnCertainDays.Items.Add(cbWednesday);
                    var cbThursday = new CheckBox();
                    cbThursday.Content = "Thursday";
                    listBoxOnCertainDays.Items.Add(cbThursday);
                    var cbFriday = new CheckBox();
                    cbFriday.Content = "Friday";
                    listBoxOnCertainDays.Items.Add(cbFriday);
                    var cbSaturday = new CheckBox();
                    cbSaturday.Content = "Saturday";
                    listBoxOnCertainDays.Items.Add(cbSaturday);
                    var cbSunday = new CheckBox();
                    cbSunday.Content = "Sunday";
                    listBoxOnCertainDays.Items.Add(cbSunday);
                    listBoxOrTextBoxOrNothing.Children.Add(listBoxOnCertainDays);
                }
                _frequencyOfAdmission = FrequencyOfAdmission.OnCertainDays;
            }
            else
            {
                this.ListBoxOrTextBoxOrNothing.Children.Clear();
                textBoxDailyInterval = new TextBox();
                textBoxDailyInterval.MaxLines = 1;
                this.ListBoxOrTextBoxOrNothing.Children.Add(textBoxDailyInterval);
                _frequencyOfAdmission = FrequencyOfAdmission.DailyInterval;
            }
        }

        private void ButtonDone_Click(object sender, RoutedEventArgs e)
        {
            if (_medicament == null)
            {
                Human humanFind = null;
                try
                {
                    foreach (var human in _people)
                    {
                        if (ComboBoxPeople.SelectedItem.ToString() == human.Name)
                        {
                            humanFind = human;
                            break;
                        }
                    }
                    if (humanFind != null)
                    {
                        Medicament newMedicament = new Medicament(ComboBoxMedicament.SelectedItem.ToString());
                        newMedicament.Title = TextBoxTitle.Text;
                        newMedicament.Notes = TextBoxNotes.Text;
                        newMedicament.Num = Int32.Parse(TextBoxCountMedicament.Text);
                        newMedicament.StartDate = (DateTime)DatePickerStartDate.SelectedDate;
                        newMedicament.DurationOfAdmission = Int32.Parse(TextBoxDuring.Text);
                        newMedicament.UnitOfPreparation = UnitOfPreparation.Pieces;
                        newMedicament.OfAdmission = _frequencyOfAdmission;
                        newMedicament.TimesADay = Int32.Parse(ComboBoxTimesOfADay.SelectedItem.ToString());
                        switch (newMedicament.OfAdmission)
                        {
                            case FrequencyOfAdmission.EveryDay:
                                break;
                            case FrequencyOfAdmission.OnCertainDays:
                                if (listBoxOnCertainDays != null)
                                {
                                    int i = 0;
                                    foreach (var item in listBoxOnCertainDays.Items)
                                    {
                                        if ((bool) ((CheckBox) item).IsChecked)
                                        {
                                            newMedicament.Days.Add((CertainDays) i);
                                        }
                                        i++;
                                    }
                                }
                                break;
                            case FrequencyOfAdmission.DailyInterval:
                                if (textBoxDailyInterval != null)
                                {
                                    newMedicament.DaysOfDailyInterval = Int32.Parse(textBoxDailyInterval.Text);
                                }
                                break;
                        }
                        humanFind.AddMedicament(newMedicament);
                        _isEdited = false;
                        this.Close();
                    }
                }
                catch (Exception exception)
                {
                    new ToastPopUp("Error!", exception.Message + "\nPlease, fill the fields!", NotificationType.Error).Show();
                }
            }
            else
            {
                SaveChanges();
            }
        }

        private void SaveChanges()
        {
            try
            {
                _medicament.Name = ComboBoxMedicament.SelectedItem.ToString();
                _medicament.Notes = TextBoxNotes.Text;
                _medicament.Num = Int32.Parse(TextBoxCountMedicament.Text);
                _medicament.Title = TextBoxTitle.Text;
                if (DatePickerStartDate.SelectedDate != null)
                    _medicament.StartDate = (DateTime)DatePickerStartDate.SelectedDate;
                _medicament.DurationOfAdmission = Int32.Parse(TextBoxDuring.Text);
                _medicament.UnitOfPreparation = UnitOfPreparation.Pieces;
                _medicament.OfAdmission = _frequencyOfAdmission;
                _medicament.TimesADay = Int32.Parse(ComboBoxTimesOfADay.SelectedItem.ToString());
                switch (_medicament.OfAdmission)
                {
                    case FrequencyOfAdmission.EveryDay:
                        break;
                    case FrequencyOfAdmission.OnCertainDays:
                        if (listBoxOnCertainDays != null)
                        {
                            int i = 0;
                            foreach (var item in listBoxOnCertainDays.Items)
                            {
                                if ((bool)((CheckBox)item).IsChecked)
                                {
                                    _medicament.Days.Add((CertainDays)i);
                                }
                                i++;
                            }
                        }
                        break;
                    case FrequencyOfAdmission.DailyInterval:
                        if (textBoxDailyInterval != null)
                        {
                            _medicament.DaysOfDailyInterval = Int32.Parse(textBoxDailyInterval.Text);
                        }
                        break;
                }
                _isEdited = false;
                var medicaments = _human.Medicaments;
                _human.OneDays.Clear();
                _human.Medicaments = new List<Medicament>();
                foreach (var medicament1 in medicaments)
                {
                    _human.AddMedicament(medicament1);
                }
                this.Close();
            }
            catch (Exception e)
            {
                new ToastPopUp("Error!", e.Message, NotificationType.Error).Show();
            }
        }
    }
}
