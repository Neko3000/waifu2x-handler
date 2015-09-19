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
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Threading;

using System.Windows.Media.Animation;

namespace waifu2x_handler
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private string[] ImageFIles;
        private List<ImageItem> ImageItemList;
        private int NowNumberOfImage;
        private string OutputDirectory;
        private string ProcessArgumentBehindPart;
        delegate void RunProgram();

        public MainWindow()
        {
            InitializeComponent();
            ImageItemList = new List<ImageItem>();
        }

        public void CollapsedAnimationEnd(object sender,EventArgs e)
        {
            MessageBox.Show("ends");
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if(ImageItemList!=null)
            {
                NowNumberOfImage = 0;
                ProcessPrepare();
            }           
        }
        private void FinishSingle(object sender,EventArgs e)
        {
            RunProgram _action = new RunProgram(ImageItemList[NowNumberOfImage].Complete);
            Dispatcher.BeginInvoke(_action);
            //ImageItemList[NowNumberOfImage].Complete();
            NowNumberOfImage++;

            if (NowNumberOfImage<ImageItemList.Count)
            {
                string inputFileFullName = ImageItemList[NowNumberOfImage].FullName;
                string inputFileName = ImageItemList[NowNumberOfImage].Name;

                ProcessAction("-i " + inputFileFullName + " -o " + OutputDirectory + inputFileName + ProcessArgumentBehindPart);
            }
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {         
            var openFile = new  System.Windows.Forms.OpenFileDialog();
            openFile.Filter = "image files|*.png;*jpg;*.jpeg;*.tif;*.tiff;;*.bmp;*.tga";
            openFile.Multiselect = true;
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if(openFile.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                ImageFIles =(string[]) openFile.FileNames.Clone();
                foreach(string singleImageFile in ImageFIles)
                {
                    BitmapImage thumbnail = new BitmapImage(new Uri(singleImageFile,UriKind.Absolute));

                    FileInfo tempFileInfo = new FileInfo(singleImageFile);
                    ImageItem singleImageItem = new ImageItem(thumbnail.Clone(), tempFileInfo.Name, tempFileInfo.DirectoryName,tempFileInfo.FullName, tempFileInfo.Extension);

                    ImageItemList.Add(singleImageItem);
                }
            }
            ImageListViewer.ItemsSource = ImageItemList;
            ImageListViewer.Items.Refresh();
        }
        private void SetOutputDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            var setDirectory = new System.Windows.Forms.FolderBrowserDialog();
            if(setDirectory.ShowDialog()== System.Windows.Forms.DialogResult.OK)
            {
                OutputPathTextBox.Text = setDirectory.SelectedPath;
            }
        }
        private void ProcessPrepare()
        {

            string inputFileFullName = ImageItemList[NowNumberOfImage].FullName;
            string outputDirectnary=OutputPathTextBox.Text;
            string inputFileName = ImageItemList[NowNumberOfImage].Name;

            string scaleSize = ScaleSizeTextBox.Text;
            string noise = ((ComboBoxItem)NoiseTextBox.Items[NoiseTextBox.SelectedIndex]).Content.ToString().Replace("#","");

            string selectedModeName="";
            string selectedMode = "";
            var tempModeRadioButtons =TransformModeGroup.Children;
            foreach(UIElement singRadioButton in tempModeRadioButtons)
            {
                if(singRadioButton.GetType()==typeof(RadioButton))
                {
                    if(((RadioButton)singRadioButton).IsChecked==true)
                    {
                        selectedModeName = ((RadioButton)singRadioButton).Name;
                    }
                }
            }            
            switch(selectedModeName)
            {
                case "TwoDRGB":
                    selectedMode = "models/anime_style_art_rgb";
                    break;
                case "TwoDLightness":
                    selectedMode = "models/anime_style_art";
                    break;
                case "ThreeDSpecific":
                    selectedMode = "models/ukbench";
                    break;
            }

            string selectedProcessName = "";
            string selectedProcess = "";
            var tempProcessRadioButtons = ProcessGroup.Children;
            foreach (UIElement singRadioButton in tempProcessRadioButtons)
            {
                if (singRadioButton.GetType() == typeof(RadioButton))
                {
                    if (((RadioButton)singRadioButton).IsChecked == true)
                    {
                        selectedProcessName = ((RadioButton)singRadioButton).Name;
                    }
                }
            }
            switch (selectedProcessName)
            {
                case "CPU":
                    selectedProcess = "cpu";
                    break;
                case "GPU":
                    selectedProcess = "gpu";
                    break;
                case "CUDNN":
                    selectedProcess = "cudnn";
                    break;
            }

            if(noise=="None")
            {
                ProcessArgumentBehindPart =" -m " + "scale" + " --scale_ratio " + scaleSize+ " --model_dir "+selectedMode+" -p "+selectedProcess;
            }
            else
            {
                ProcessArgumentBehindPart =" -m " + "noise_scale" + " --scale_ratio " + scaleSize+" --noise_level "+noise + " --model_dir " + selectedMode + " -p " + selectedProcess;
            }

            OutputDirectory = OutputPathTextBox.Text;

            ProcessAction("-i " + inputFileFullName + " -o " + outputDirectnary + inputFileName + ProcessArgumentBehindPart);
        }
        private void ProcessAction(string processArguments)
        {
            Process MainProcess = new Process();

            ProcessStartInfo MainStartInfo = new ProcessStartInfo();
            MainStartInfo.FileName = @"origin\waifu2x-caffe\waifu2x-caffe-cui.exe";
            MainStartInfo.UseShellExecute = true;
            MainStartInfo.CreateNoWindow = true;
            //MainStartInfo.Arguments = @"-i H:\ap01_002a02.png -m noise_scale --scale_ratio 1.6 --noise_level 2";
            //MessageBox.Show(@"-i " + inputFileFullName + " -o " + outputDirectnary + inputFileName + " -m " + "scale_noise" + " --scale_ratio 1.6 --noise_level 2");
            //MainStartInfo.Arguments = @"-i " + inputFileFullName + " -o " + outputDirectnary + inputFileName + " -m " + "noise_scale" + " --scale_ratio 1.6 --noise_level 2";         
            MainStartInfo.Arguments = processArguments;
            MainProcess.StartInfo = MainStartInfo;

            MainProcess.Exited += new EventHandler(FinishSingle);
            MainProcess.EnableRaisingEvents = true;
            MainProcess.Start();
        }
    }
}
