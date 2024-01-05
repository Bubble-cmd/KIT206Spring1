using KIT206Spring.Controllers;
using KIT206Spring.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace KIT206Spring
{
    /// <summary>
    /// Interaction logic for ReportsView.xaml
    /// </summary>
    public partial class ReportsView : Window
    {
        private ObservableCollection<Researcher> staff;
        private List<List<Staff>> ReportsList = new List<List<Staff>>();
        public ReportsView(ObservableCollection<Researcher> staff)
        {
            ReportsList = ResearcherControl.SortReport(staff);
            InitializeComponent();
            this.staff = staff;
            //StaffListView.ItemsSource = staff;
        }
        private void ReportSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            string selectedLevel = selectedItem.Content.ToString();
            StaffListView.ItemsSource = ResearcherControl.GenReport(selectedLevel, ReportsList);
        }
        private void Button_Copy_Email_Click(object sender, RoutedEventArgs e)
        {
            string copiedEmail = "";
            if (StaffListView.ItemsSource != null)
            {
                foreach (Researcher researcher in StaffListView.ItemsSource)
                {
                    copiedEmail += researcher.Email += ";";
                }
                Clipboard.SetText(copiedEmail);
                MessageBox.Show("Emails(s) copied to clipboard.");
            }
            else
            {
                MessageBox.Show("No emails to copy");
            }
        }
    }

}
