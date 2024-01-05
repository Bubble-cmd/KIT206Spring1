using KIT206Spring.Entities;
using KIT206Spring.Controllers;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq.Expressions;



namespace KIT206Spring
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Publication> selectedResearcherPublications;
        private ObservableCollection<Researcher> researchers;
        private List<Publication> publicaitonList;
        private List<Researcher> resList;

        public BitmapImage ImageData { get; set; }


        // select a researcher
        private void researcherListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (researcherListView.SelectedItem != null)
            {
                Researcher selectedResearcher = (Researcher)researcherListView.SelectedItem;
                ResearcherControl.DisplayResearcherDetails(selectedResearcher, resList);
                selectedResearcher.Pubs.Clear();
                publicaitonList = PublicationsControl.FetchPublications(selectedResearcher);

                selectedResearcherPublications.Clear();

                foreach (var publication in publicaitonList)
                {
                    selectedResearcherPublications.Add(publication);
                }

                // researcher details
                name.Text = "Name: " + selectedResearcher.FirstName + " " + selectedResearcher.LastName;
                //title not displaying at all, mustn't be pulling from db?
                resTitle.Text = "Title: " + selectedResearcher.Title;
                unit.Text = "Unit: " + selectedResearcher.SchoolUnit;
                Enum campusPr = selectedResearcher.Camp;
                campus.Text = "Campus: " + campusPr.ToString();
                email.Text = "Email: " + selectedResearcher.Email;
                //current job title not working, not sure its calculating?
                job.Text = "Job: " + selectedResearcher.Job_Title;

                commencedInt.Text = "Commenced with institution: " + selectedResearcher.CommencedWithInstitution.ToString("d");
                commencedCurr.Text = "Commenced current job: " + selectedResearcher.CommenceCurrentPosition.ToString("d");
                // Convert this to a float of years. To do in Staff.cs
                tenure.Text = "Tenure: " + selectedResearcher.Tenure;


                publi.Text = "Publications: " + selectedResearcher.Pubs.Count;
                if (selectedResearcher is Student student)
                {
                    degree.Text = "Degree: " + student.Degree;
                    supervisions.Text = "Supervisions: N/A";
                    supervisor.Text = "Supervisor: " + student.Supervisor;
                }
                else if (selectedResearcher is Staff staff)
                {
                    degree.Text = "Degree: N/A";
                    supervisor.Text = "Supervisor: N/A";
                    supervisions.Text = "Supervisions: " + staff.SuperCount;
                }

                //ImageData = new BitmapImage(new Uri(selectedResearcher.PhotoURL));
                var photo = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedResearcher.PhotoURL);
                bitmap.EndInit();
                ImageSource imageSource = bitmap;

                ResercherPhoto.Source = imageSource;

                Console.WriteLine("image URL " + selectedResearcher.PhotoURL);

                Console.WriteLine("Selected researcher: " + selectedResearcher.FirstName);
            }
        }
        private void tempButton_Click(object sender, RoutedEventArgs e)
        {
            Researcher selectedResearcher = (Researcher)researcherListView.SelectedItem;
            if (selectedResearcher != null)
            {
                PerformanceDetailsWindow PdetailsView = new PerformanceDetailsWindow();
                ResearcherControl.DetailsController(selectedResearcher);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(selectedResearcher.PhotoURL);
                image.EndInit();
                PdetailsView.profilePic.Source = image;

                PdetailsView.DataContext = selectedResearcher;
                PdetailsView.Show();
            }
            else
            {
                MessageBox.Show("No researcher selected");
            }
        }

        private static string Wrap(string v, int size)
        {
            v = v.TrimStart();
            if (v.Length <= size) return v;
            var nextspace = v.LastIndexOf(' ', size);
            if (-1 == nextspace) nextspace = Math.Min(v.Length, size);
            return v.Substring(0, nextspace) + ((nextspace >= v.Length) ?
            "" : "\n" + Wrap(v.Substring(nextspace), size));
        }

        private void PublicationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PublicationListView.SelectedItem != null)
            {
                Publication selectedPublication = (Publication)PublicationListView.SelectedItem;
                DateTime now = DateTime.Now;



                DOI.Text = "DOI : " + selectedPublication.DOI;
                pubTitle.Text = "Publication Title: " + Wrap(selectedPublication.Title, 75);
                authors.Text = "Authors: " + selectedPublication.Authors;
                pubYear.Text = "Publication Year: " + selectedPublication.AvailabilityDate.Year;
                ranking.Text = "Ranking: " + selectedPublication.Ranking;
                pubType.Text = "Publication Type: " + selectedPublication.Type;
                citeAS.Text = "Cite As: " + Wrap(selectedPublication.CiteAs, 75);
                avaDate.Text = "Availability Date: " + selectedPublication.AvailabilityDate.Date.ToShortDateString();
                pubAge.Text = "Publication Age: " + Math.Round((now - selectedPublication.AvailabilityDate).TotalDays);
            }
        }

        public MainWindow()
        {
            resList = ResearcherControl.FetchResearchers();
            researchers = new ObservableCollection<Researcher>(resList);
            selectedResearcherPublications = new ObservableCollection<Publication>();
            InitializeComponent();
            researcherListView.ItemsSource = researchers;
            PublicationListView.ItemsSource = selectedResearcherPublications;

        }
        private void LevelFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

            string selectedLevel = selectedItem.Content.ToString();

            researcherListView.ItemsSource = ResearcherControl.FilterLevel(selectedLevel, researchers);
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                researcherListView.ItemsSource = ResearcherControl.FilterList(researchers, SearchBox.Text);
            }
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(FirstNumberTextBox.Text, out int firstNumber) &&
                    int.TryParse(SecondNumberTextBox.Text, out int secondNumber))
            {
                publicaitonList = PublicationsControl.FilterByYear(firstNumber, secondNumber, publicaitonList);

                selectedResearcherPublications.Clear();
                foreach (var publication in publicaitonList)
                {
                    selectedResearcherPublications.Add(publication);
                }
            }
            else
            {
                Console.WriteLine("Please enter valid integers.");
            }
        }

        private void PublicationDateColumnHeader_Click(object sender, RoutedEventArgs e)
        {

            publicaitonList = PublicationsControl.invert_sort(publicaitonList);

            selectedResearcherPublications.Clear();
            foreach (var publication in publicaitonList)
            {
                selectedResearcherPublications.Add(publication);
            }
        }

        private void Button_Copy_Email_Click(object sender, RoutedEventArgs e)
        {
            string copiedEmail;
            Researcher selectedResearcher = (Researcher)researcherListView.SelectedItem;
            if (selectedResearcher != null)
            {
                copiedEmail = selectedResearcher.Email;
                Clipboard.SetText(copiedEmail);
                MessageBox.Show("Text copied to clipboard." + copiedEmail);
            }
            else
            {
                MessageBox.Show("No researcher selected");
            }
        }

        private void Report_Button_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Researcher> researchers = new ObservableCollection<Researcher>(ResearcherControl.FetchResearchers());
            ReportsView reports = new ReportsView(researchers);
            reports.Show();

        }
    }
}
