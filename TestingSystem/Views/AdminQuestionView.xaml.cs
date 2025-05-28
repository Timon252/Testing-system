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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestingSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminQuestionView.xaml
    /// </summary>
    public partial class AdminQuestionView : UserControl
    {
        public AdminQuestionView()
        {
            InitializeComponent();
        }

        private void NextTipeQuestion_Click(object sender, RoutedEventArgs e)
        {
            QuesionTipeRadioSP.Visibility = Visibility.Hidden;
        }
    }
}
