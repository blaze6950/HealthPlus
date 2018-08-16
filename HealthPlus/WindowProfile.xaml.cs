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
using Microsoft.Win32;

namespace HealthPlus
{
    /// <summary>
    /// Interaction logic for WindowProfile.xaml
    /// </summary>
    public partial class WindowProfile : Window
    {
        private Human _human;

        public WindowProfile(Human human)
        {
            InitializeComponent();
            _human = human;
            TextBlockName.Text = _human.Name;
            BitmapImage bi3 = new BitmapImage();
            try
            {
                bi3.BeginInit();
                bi3.UriSource = human.Avatar;
                bi3.EndInit();
                Avatar.Stretch = Stretch.Uniform;
                Avatar.Source = bi3;
            }
            catch (Exception)
            {
                
            }
            TextBlock text;
            foreach (var medicament in _human.Medicaments)
            {
                text = new TextBlock();
                text.Text = medicament.Title;
                text.ToolTip = medicament.Notes;
                ListBoxMedicaments.Items.Add(text);
            }
        }

        private void ListBoxMedicaments_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxMedicaments.Items.Count != 0)
            {
                var text = ((TextBlock)ListBoxMedicaments.SelectedItem).Text;
                var tooltip = ((TextBlock)ListBoxMedicaments.SelectedItem).ToolTip;
                Medicament medicamentFind = null;
                foreach (var medicament in _human.Medicaments)
                {
                    if (medicament.Title == text && medicament.Notes == tooltip)
                    {
                        medicamentFind = medicament;
                        break;
                    }
                }
                new AddMedicament(medicamentFind, _human).Show();
            }
        }

        private void ListBoxMedicaments_Delete(object sender, MouseButtonEventArgs e)
        {
            var text = ((TextBlock)ListBoxMedicaments.SelectedItem).Text;
            var tooltip = ((TextBlock)ListBoxMedicaments.SelectedItem).ToolTip;
            Medicament medicamentFind = null;
            foreach (var medicament in _human.Medicaments)
            {
                if (medicament.Name == text && medicament.Notes == tooltip)
                {
                    medicamentFind = medicament;
                    break;
                }
            }
            if (medicamentFind != null)
            {
                var res = MessageBox.Show("Are u sure? Delete " + medicamentFind.Name + "?", "Delete", MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    _human.Medicaments.Remove(medicamentFind);
                }
                _human.OneDays.Clear();
                var medicaments = _human.Medicaments;
                _human.Medicaments = new List<Medicament>();
                foreach (var medicament1 in medicaments)
                {
                    _human.AddMedicament(medicament1);
                }
                ListBoxMedicaments.Items.Clear();
                foreach (var medicament in _human.Medicaments)
                {
                    ListBoxMedicaments.Items.Add(medicament.Name);
                }
            }
        }

        private void Avatar_Click(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Картинки(*.JPG;*.PNG)|*.JPG;*.PNG" + "|Все файлы (*.*)|*.* ";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = false;
            if (myDialog.ShowDialog() == true)
            {
                _human.Avatar = new Uri(myDialog.FileName);
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = _human.Avatar;
                bi3.EndInit();
                Avatar.Stretch = Stretch.Uniform;
                Avatar.Source = bi3;
            }
        }
    }
}
