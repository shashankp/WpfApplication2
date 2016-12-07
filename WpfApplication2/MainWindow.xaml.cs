﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WpfApplication2
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

        private void Button1_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var files = new List<string>();
                if (Directory.Exists(tbSolutionPath.Text))
                {
                    files = Directory.GetFiles(tbSolutionPath.Text).ToList();
                } else if (File.Exists(tbSolutionPath.Text))
                {
                    files.Add(tbSolutionPath.Text);
                }

                var resultText = TfsHelper.GetHistory(files);
                
                tbResult.Text = resultText;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
