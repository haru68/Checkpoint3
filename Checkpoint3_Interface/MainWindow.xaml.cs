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
using Checkpoint3;
using Nancy.Hosting.Self;
using Nancy;
using Nancy.Configuration;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Checkpoint3_Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        string NancyEntryPoint { get => @"http://localhost:1234/File/"; }
        FileModule FileModule { get => new FileModule(); }


        private void btn_OpenNancyFile(object sender, RoutedEventArgs e)
        {
            string FileName = TextBox_FileName.Text;
            ExecuteRequest(FileName, "GET");
        }

        private void btn_CreateNancyFile(object sender, RoutedEventArgs e)
        {
            string FileName = TextBox_FileName.Text;
            ExecuteRequest(FileName, "PUT");
        }

        private void ExecuteRequest(string pathExtension, string method)
        {
            string filepath = NancyEntryPoint + pathExtension;
            WebRequest request = WebRequest.Create(filepath);
            request.Method = method;
            request.ContentLength = 0;
            request.ContentType = "application/xml";
            WebResponse response = request.GetResponse();
            TextBlock_Answer.Text = ((HttpWebResponse)response).StatusDescription;
            using (Stream datastream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(datastream);
                string answer = reader.ReadToEnd();
                TextBlock_Answer.Text += answer;
            }
            response.Close();
        }

        private void btn_DeleteNancyFile(object sender, RoutedEventArgs e)
        {
            string FileName = TextBox_FileName.Text;
            
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(NancyEntryPoint);
                var response = client.DeleteAsync(FileName).Result;
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TextBlock_Answer.Text = FileName + " has been deleted";
                }
                else
                {
                    TextBlock_Answer.Text = "Cannot delete " + FileName + ". Status code = " + response.StatusCode;
                }
            }
        }

        private void btn_wpfBrowser(object sender, RoutedEventArgs e)
        {
            WebBrowserGrid.Visibility = Visibility.Visible;
            NancyGrid.Visibility = Visibility.Collapsed;
        }

        private void Btn_OpenFile(object sender, RoutedEventArgs e)
        {
            string filepath = TextBox_Filepath.Text;
            if(filepath == string.Empty)
            {
                filepath = "C:/";
            }
            WebBrowser.Navigate(filepath);
            
        }

        private void Btn_DeleteFile(object sender, RoutedEventArgs e)
        {
            string filepath = TextBox_FileName.Text;
            if (File.Exists(filepath))
            {
                File.Delete(TextBox_FileName.Text);
            }
            else
            {
                MessageBox.Show("Cannot delete file.");
            }
        }

        private void Btn_Forward(object sender, RoutedEventArgs e)
        {
            if (WebBrowser.CanGoForward)
            { WebBrowser.GoForward(); }
        }

        private void Btn_Bacward(object sender, RoutedEventArgs e)
        {
            if (WebBrowser.CanGoBack)
            { WebBrowser.GoBack(); }
        }
    }
}
