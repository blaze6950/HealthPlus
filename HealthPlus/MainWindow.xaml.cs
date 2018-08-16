using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mantin.Controls.Wpf.Notification;
using CheckBox = System.Windows.Controls.CheckBox;
using MessageBox = System.Windows.MessageBox;
using Timer = System.Threading.Timer;

namespace HealthPlus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Human> _people;
        private Timer _timer;
        private NotifyIcon _notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            if (_people == null)
            {
                _people = new List<Human>();
            }
            TextBoxDate_Click(null, null);
            _notifyIcon = new NotifyIcon();
            Uri u = new Uri("pill.ico", UriKind.Relative);
            _notifyIcon.Icon = new Icon(@"C:\Users\Дмитрий\Desktop\HealthPlus\HealthPlus\pill.ico");
            _notifyIcon.Text = @"HealthPlus";
            _notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            _notifyIcon.BalloonTipTitle = @"Hey!";
            _notifyIcon.BalloonTipText = @"I'm here!";
            _notifyIcon.DoubleClick += NotifyIconOnDoubleClick;
        }

        private void NotifyIconOnDoubleClick(object sender, EventArgs eventArgs)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            _notifyIcon.Visible = false;
        }

        private void TimerTick(object state)
        {
            foreach (var human in _people)
            {
                OneDay oneDayFind = null;
                foreach (var oneDay in human.OneDays)
                {
                    if (oneDay.Day == DateTime.Today)
                    {
                        oneDayFind = oneDay;
                        break;
                    }else if (oneDay.Day > DateTime.Today)
                    {
                        break;
                    }
                }
                if (oneDayFind == null)
                {
                    break;
                }
                foreach (var lightMedicament in oneDayFind.LightMedicaments)
                {
                    if (lightMedicament.isDone == false && lightMedicament.Time > DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(-5)) && lightMedicament.Time < DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(5)))
                    {
                        var res = MessageBox.Show("Time to " + lightMedicament.Medicament1.Name + " " + lightMedicament.Medicament1.Num +
                            lightMedicament.Medicament1.UnitOfPreparation + " | Note: " +
                            lightMedicament.Medicament1.Notes, human.Name, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (res == MessageBoxResult.Yes)
                        {
                            lightMedicament.isDone = true;
                        }
                    }
                }
            }
        }

        public void GoNotification(string title, string text, NotificationType nType)
        {
            // This example shows how to register the available events var toast = new ToastPopUp( "My Title", "This is the main content.", "Click this Hyperlink", NotificationType.Information);

            // This is what will be passed back through the HyperlinkClicked event. toast.HyperlinkObjectForRaisedEvent = new object(); toast.HyperlinkClicked += this.ToastHyperlinkClicked; toast.ClosedByUser += this.ToastClosedByUser; toast.Show();

            // Passing rich text as inlines and overrides the image. var inlines = new List(); inlines.Add(new Run() { Text = "This is the first line of my main content." }); inlines.Add(new Run() { Text = Environment.NewLine }); inlines.Add(new Run("This text will be italic.") { FontStyle = FontStyles.Italic });

            var toast = new ToastPopUp(title, text, nType);
            toast.Show();
            // If you don't need any events fired, you can do this. new ToastPopUp("My Title", "This is the main content.", NotificationType.Information) { Background = new LinearGradientBrush(Color.FromArgb(255, 4, 253, 82), Color.FromArgb(255, 10, 13, 248), 90), BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)), FontColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)) }.Show();
        }

        private void AMButton_Click(object sender, RoutedEventArgs e)
        {
            if (_people.Count != 0)
            {
                new AddMedicament(_people, (String)ListBoxProfiles.SelectedItem).ShowDialog();
            }
            else
            {
                GoNotification("Ooops", "Your profile list is empty! PLease, add a new profile and try again",
                    NotificationType.Information);
            }
            //new AddMedicament(_people).Show();
            if (_timer == null)
            {
                _timer = new Timer(TimerTick, null, 0, 60000);
            }
        }

        private void AddHuman_Click(object sender, RoutedEventArgs e)
        {
            var count = _people.Count;
            new AddHuman(_people).ShowDialog();
            if (count != _people.Count)
            {
                ListBoxProfiles.Items.Add(_people[_people.Count - 1].Name);
            }
        }

        private void ListBoxProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxTimesMedicament.Items.Clear();
            ProfileName.Text = ((String) ListBoxProfiles.SelectedItem);
            Human humanFind = null;
            foreach (var human in _people)
            {
                if (human.Name == ProfileName.Text)
                {
                    humanFind = human;
                    break;
                }
            }
            var selDate = DateTime.Parse(TextBlockSelectedDate.Text);
            if (humanFind != null)
            {
                BitmapImage bi3 = new BitmapImage();
                try
                {                    
                    bi3.BeginInit();
                    bi3.UriSource = humanFind.Avatar;
                    bi3.EndInit();
                    Avatar.Stretch = Stretch.Uniform;
                    Avatar.Source = bi3;
                }
                catch (Exception)
                {
                                        
                }
                OneDay oneDayFind = null;
                foreach (var oneDay in humanFind.OneDays)
                {
                    if (oneDay.Day == selDate)
                    {
                        oneDayFind = oneDay;
                        break;
                    }
                }
                if (oneDayFind != null)
                {
                    CheckBox cbMonday = null;
                    foreach (var lightMedicament in oneDayFind.LightMedicaments)
                    {
                        cbMonday = new CheckBox();
                        cbMonday.Checked += CbMondayOnChecked;
                        cbMonday.ToolTip = lightMedicament.Medicament1.Notes;
                        cbMonday.Content = lightMedicament.Time.ToString() + " | " + lightMedicament.Medicament1.Title + " - " + lightMedicament.Medicament1.Name + " " + lightMedicament.Medicament1.Num + " " + lightMedicament.Medicament1.UnitOfPreparation;
                        cbMonday.IsChecked = lightMedicament.isDone;
                        ListBoxTimesMedicament.Items.Add(cbMonday);
                    }
                }
            }
        }

        private void CbMondayOnChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            ProfileName.Text = ((String)ListBoxProfiles.SelectedItem);
            Human humanFind = null;
            foreach (var human in _people)
            {
                if (human.Name == ProfileName.Text)
                {
                    humanFind = human;
                    break;
                }
            }
            var selDate = DateTime.Parse(TextBlockSelectedDate.Text);
            if (humanFind != null)
            {
                OneDay oneDayFind = null;
                foreach (var oneDay in humanFind.OneDays)
                {
                    if (oneDay.Day == selDate)
                    {
                        oneDayFind = oneDay;
                        break;
                    }
                }
                if (oneDayFind != null)
                {
                    foreach (var lightMedicament in oneDayFind.LightMedicaments)
                    {
                        if (((CheckBox)sender).Content.ToString() == lightMedicament.Time.ToString() + " | " + lightMedicament.Medicament1.Title + " - " + lightMedicament.Medicament1.Name + " " + lightMedicament.Medicament1.Num + " " + lightMedicament.Medicament1.UnitOfPreparation && ((CheckBox)sender).ToolTip.ToString() == lightMedicament.Medicament1.Notes)
                        {
                            lightMedicament.isDone = ((CheckBox) sender).IsEnabled;
                        }
                    }
                }
            }
        }

        private void ButtonPreviousDay_Click(object sender, RoutedEventArgs e)
        {
            TextBlockSelectedDate.Text = DateTime.Parse(TextBlockSelectedDate.Text).AddDays(-1).ToShortDateString();
            ListBoxProfiles_SelectionChanged(null, null);
        }

        private void ButtonNextDate_Click(object sender, RoutedEventArgs e)
        {
            TextBlockSelectedDate.Text = DateTime.Parse(TextBlockSelectedDate.Text).AddDays(1).ToShortDateString();
            ListBoxProfiles_SelectionChanged(null, null);
        }

        private void TextBoxDate_Click(object sender, MouseButtonEventArgs e)
        {
            TextBlockSelectedDate.Text = DateTime.Today.ToShortDateString();
            ListBoxProfiles_SelectionChanged(null, null);
        }

        private void ListBoxProfiles_Delete(object sender, MouseButtonEventArgs e)
        {
            ProfileName.Text = ((String)ListBoxProfiles.SelectedItem);
            Human humanFind = null;
            foreach (var human in _people)
            {
                if (human.Name == ProfileName.Text)
                {
                    humanFind = human;
                    break;
                }
            }
            if (humanFind != null)
            {
                var res = MessageBox.Show("Are u sure? Delete " + humanFind.Name + "?", "Delete", MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    _people.Remove(humanFind);
                    Avatar.Source = null;
                }
                ListBoxProfiles.Items.Clear();
                foreach (var human in _people)
                {
                    ListBoxProfiles.Items.Add(human.Name);
                }
            }
        }

        private void ListBoxProfiles_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxProfiles.Items.Count != 0)
            {
                ProfileName.Text = ((String)ListBoxProfiles.SelectedItem);
                Human humanFind = null;
                foreach (var human in _people)
                {
                    if (human.Name == ProfileName.Text)
                    {
                        humanFind = human;
                        break;
                    }
                }
                new WindowProfile(humanFind).Show();
                ListBoxProfiles_SelectionChanged(null, null);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FileStream stream = null;

            try
            {
                stream = new FileStream("save.bin", FileMode.Create, FileAccess.Write);

                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, _people);
            }
            catch (Exception ex)
            {
                GoNotification("Ooops", "Could not create save(",
                    NotificationType.Information);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream("save.bin", FileMode.Open, FileAccess.Read);

                BinaryFormatter formatter = new BinaryFormatter();
                _people = (List<Human>)formatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                //GoNotification("Ooops", "No save file found(",
                //    NotificationType.Information);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            if (_people != null)
            {
                foreach (var human in _people)
                {
                    ListBoxProfiles.Items.Add(human.Name);
                }
                if (_timer == null)
                {
                    _timer = new Timer(TimerTick, null, 0, 60000);
                }
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                Hide();
                _notifyIcon.Visible = true;
                _notifyIcon.ShowBalloonTip(500);
            }
        }
    }
}
