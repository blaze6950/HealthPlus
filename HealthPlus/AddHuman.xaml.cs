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
    /// Interaction logic for AddHuman.xaml
    /// </summary>
    public partial class AddHuman : Window
    {
        private List<Human> _people;
        private Uri _avatar;

        public AddHuman(List<Human> people)
        {
            if (people == null)
            {
                people = new List<Human>();
            }
            _people = people;
            InitializeComponent();
        }

        private void AddHuman_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxName.Text != String.Empty)
            {
                var newHuman = new Human(TextBoxName.Text);
                newHuman.Avatar = _avatar;
                _people.Add(newHuman);
            }
            this.Close();
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Картинки(*.JPG;*.PNG)|*.JPG;*.PNG" + "|Все файлы (*.*)|*.* ";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = false;
            if (myDialog.ShowDialog() == true)
            {
                _avatar = new Uri(myDialog.FileName);
                ((Button) sender).Content = myDialog.FileName;
            }
        }
    }
}
