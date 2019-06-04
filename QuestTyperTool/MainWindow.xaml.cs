using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    static class adb
    {
        public static int initialized; //This only lets me know if ADB has been initialized since the app started.
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (input.Text != "")
            {
                string text = input.Text.Replace("\\", "\\\\");
                text = text.Replace("%", "\\%");
                text = text.Replace("\"", "\\\\\"");
                text = text.Replace("\'", "\\\'");
                text = text.Replace("(", "\\(");
                text = text.Replace(")", "\\)");
                text = text.Replace("&", "\\&");
                text = text.Replace("<", "\\<");
                text = text.Replace(">", "\\>");
                text = text.Replace(";", "\\;");
                text = text.Replace("*", "\\*");
                text = text.Replace("|", "\\|");
                text = text.Replace("~", "\\~");
                text = text.Replace("¬", "\\¬");
                text = text.Replace("`", "\\`");
                text = text.Replace(" ", "%s");
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";                                                                      //opens the Window's command prompt
                process.StartInfo.CreateNoWindow = true;                                                                     //doesn't create a window the user can see
                process.StartInfo.RedirectStandardInput = true;                                                              //allows the app to send input to the command prompt
                process.StartInfo.RedirectStandardOutput = true;                                                             //allows the app to read the command prompt output
                process.StartInfo.RedirectStandardError = true;                                                              //allows the app to read the command prompt errors
                process.StartInfo.UseShellExecute = false;                                                                   //needed to make the app work

                if (adb.initialized != 1)
                {
                    console.Text = "";                                                                                       //set the black box on the right side of the app to blank
                    process.Start();
                    process.StandardInput.WriteLine("adb devices");                                                          //write adb devices so adb initializes 
                    process.StandardInput.Close();                                                                           //stop doing things
                    process.WaitForExit();                                                                                   //wait for command prompt to stop doing things
                    string standard_error = process.StandardError.ReadToEnd().Replace(System.Environment.NewLine, "");       //remove the new line from errors to make them easier to filter
                    string standard_output = process.StandardOutput.ReadToEnd();                                             //give the output a variable name
                    console.Text = standard_output + standard_error;                                                         //push everything to the black box on the interface
                    if (standard_error == "'adb' is not recognized as an internal or external command,operable program or batch file.") //if this error appears
                    {
                        console.Text = "ADB NOT FOUND: Either put this program in the same folder as adb.exe or add adb.exe to system path!"; //display this more human readable message
                    }
                    else
                    {
                        string standard_output_filtered = process.StandardOutput.ReadToEnd().Replace(System.Environment.NewLine, ""); //remove the new line characters from output for same reason as before
                        if (standard_output_filtered == "List of devices attached")                                         //when adb returns this message only, it means no devices are connected
                        {
                            console.Text = "NO DEVICES FOUND: Make sure the Quest headset is in developer mode, turned on, and plugged into the computer!"; //human readable error message
                            adb.initialized = 1;                                                                            //definately know that adb is working since we got this message
                        }
                        else
                        {
                            process.Start();
                            process.StandardInput.WriteLine("adb shell input text \"" + text + "\"");                       //since everything else worked, so let's try to pass the first text entry
                            process.StandardInput.Close();
                            process.WaitForExit();
                            standard_error = process.StandardError.ReadToEnd().Replace(System.Environment.NewLine, "");     //refreshing the variables since the inputs changed
                            standard_output = process.StandardOutput.ReadToEnd();
                            console.Text = standard_output + standard_error;
                            adb.initialized = 1;                                                                            //adb is working and we've got our devices connected, so let's make sure we keep track of that
                            if (standard_error == "error: no devices/emulators found")                                      //this is more of a backup. Just incase the device is unplugged after the last check was passed or if the other error didn't trigger.
                            {
                                console.Text = "NO DEVICES FOUND: Make sure the Quest headset is in developer mode, turned on, and plugged into the computer!";
                            }
                        }
                    }
                }
                else                                                                                                       //adb has been working and initialized, let's keep sending those passwords or text
                {                                                                                                          //pretty much everything else is just a repeat of above
                    console.Text = "";
                    process.Start();
                    process.StandardInput.WriteLine("adb shell input text \"" + text + "\"");
                    process.StandardInput.Close();
                    process.WaitForExit();
                    string standard_error = process.StandardError.ReadToEnd().Replace(System.Environment.NewLine, "");
                    string standard_output = process.StandardOutput.ReadToEnd();
                    console.Text = standard_output + standard_error;
                    if (standard_error == "'adb' is not recognized as an internal or external command,operable program or batch file.")
                    {
                        console.Text = "ADB NOT FOUND: Either put this program in the same folder as adb.exe or add adb.exe to system path!";
                        adb.initialized = 0;
                    }
                    standard_output = standard_output.Replace(System.Environment.NewLine, "");
                    if (standard_output == "List of devices attached")
                    {
                        console.Text = "NO DEVICES FOUND: Make sure the Quest headset is in developer mode, turned on, and plugged into the computer!";
                        adb.initialized = 1;
                    }
                }
            }
            else
            {
                console.Text = "NO TEXT INPUT: Please type something first.";
            }
        }
    }
}
