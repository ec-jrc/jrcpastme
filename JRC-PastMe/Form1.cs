/*
* JRC-PaStMe © European Union, 2018
* 
* Licensed under the EUPL, Version 1.2 or – as soon they
will be approved by the European Commission - subsequent
versions of the EUPL (the "Licence");
* You may not use this work except in compliance with the Licence.
* You may obtain a copy of the Licence at:
* 
* https://joinup.ec.europa.eu/software/page/eupl
* 
* Unless required by applicable law or agreed to in
writing, software distributed under the Licence is
distributed on an "AS IS" basis,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
either express or implied.
* See the Licence for the specific language governing
permissions and limitations under the Licence.
*/

using System;
using System.IO;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Matlab;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.VisualBasic.PowerPacks;
using System.Runtime.InteropServices;
//DEBUG PAsswordTraining
using DotNetDoctor.csmatio.io;
using DotNetDoctor.csmatio.types;
using System.Text.RegularExpressions; //Useful for regual expressions in BF_strengthMetric (BRUTE FORCE)


namespace JRC_PastMe
{
    //public static class ModifyProgressBarColor
    //{
    //    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
    //    static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
    //    public static void SetState(this ProgressBar pBar, int state)
    //    {
    //        SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
    //    }
    //}
    public partial class Form1 : Form
    {
        private string FileOfBannedPasswords = ConfigurationManager.AppSettings["FileOfBannedPasswords"];
        private string adapt_matrix_path = ConfigurationManager.AppSettings["adapt_matrix_path"];
        private string CFHMM_matrix_path = ConfigurationManager.AppSettings["CFHMM_matrix_path"];
        private string SimpleHMM_matrix_path = ConfigurationManager.AppSettings["SimpleHMM_matrix_path"];
        private int LminNum = Convert.ToInt16(ConfigurationManager.AppSettings["LminNum"]);
        private int LminLower = Convert.ToInt16(ConfigurationManager.AppSettings["LminLower"]);
        private int LminUpper = Convert.ToInt16(ConfigurationManager.AppSettings["LminUpper"]);
        private int LminSpecial = Convert.ToInt16(ConfigurationManager.AppSettings["LminSpecial"]);
        private int LminTwo = Convert.ToInt16(ConfigurationManager.AppSettings["LminTwo"]);
        private int LminThree = Convert.ToInt16(ConfigurationManager.AppSettings["LminThree"]);
        private int LminAll = Convert.ToInt16(ConfigurationManager.AppSettings["LminAll"]);
        private int char_encoding = Convert.ToInt16(ConfigurationManager.AppSettings["char_encoding"]);
        private bool update_settings;
        private bool first_pwd = true;
        private Color blue_color = Color.FromArgb(0, 68, 148);
        private Color yellow_color = Color.FromArgb(251, 193, 29);
        private ShapeContainer canvas = new ShapeContainer();


        private int MM_memsize = Convert.ToInt16(ConfigurationManager.AppSettings["MM_memsize"]);
        public Matrix<double> M_bis;
        //With the old version of the code we need to initiate a matrix of double
        //public Matrix<double> M;
        //In the new version we use the creator of the mtrx dll
        SMtrx1D M;
        public int active_panel = 1;

        public Form1()
        {
            InitializeComponent();
            Form2 splash = new Form2();
            splash.Show();
            splash.Refresh();
            Boolean loading_done = true;
            //Load the matrix from the beginning to save time when computing a pwd strength
            try
            {
                initialize_matrix(adapt_matrix_path);
            }
            catch (Exception)
            {
                MessageBox.Show("At least the MMmem matrix is missing! \r\n\r\n Make sure it is in the PaStMe folder or retrain it from the training tab.", "Trained matrix missing",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                console_textbox.AppendText("An error occur while loading the MMmem matrix: ");
                loading_done = false;
                checkbox_advanced_view.Checked = true;
                console_textbox.AppendText("\r\n [!] The adaptive matrix is apparently missing. Check if it is present " +
                    "with the correct name or consider re-training it.");
            }

            if (!File.Exists(CFHMM_matrix_path) || !File.Exists(SimpleHMM_matrix_path))
            {
                loading_done = false;
                checkbox_advanced_view.Checked = true;
                console_textbox.AppendText("\r\n [!] At least one of the hierarchical matrix is missing. Check if they are present " +
                    "with the correct names or consider re-training them.");

            }
            //Initialise the settings based on the App.config that has been already loaded into the variables
            //Update settings is set to false to avoid updating the config value while it is useless
            update_settings = false;
            numericUpDown_lower.Value = LminLower;
            numericUpDown_upper.Value = LminUpper;
            numericUpDown_number.Value = LminNum;
            numericUpDown_special.Value = LminSpecial;
            numericUpDown_2mix.Value = LminTwo;
            numericUpDown_3mix.Value = LminThree;
            numericUpDown_allmix.Value = LminAll;
            comboBox_encoding.SelectedIndex = char_encoding;
            blacklist_path.Text = Path.GetFileName(FileOfBannedPasswords);
            update_settings = true;

            //M = PasswordStrength.init_MM_strengthMetric(adapt_matrix_path);
            this.Size = new System.Drawing.Size(416, 550);
            this.MinimumSize = new Size(416, 550);
            this.MaximumSize = new Size(416, 550);

            //Placing all the panels in their correct positions
            // Making all of them invisible except the single one
            panel_multiple.Location = new Point(0, 260);
            panel_settings.Location = new Point(0, 260);
            panel_training.Location = new Point(0, 260);
            panel_single.Location = new Point(0, 260);

            //We generate the graduation for the progress bar
            progress_bar_graduations();

            // we intialize all the panel and buttons
            initialize_panel_buttons();
            //then based if the loading was done properly we activate the correct panel
            if (loading_done)
            {
                panel_single.Visible = true;
                button_single.Enabled = false;

                button_single.BackColor = yellow_color;
                button_single.ForeColor = System.Drawing.Color.Black;
                splash.Close();
            }
            else
            {
                panel_training.Visible = true;
                button_training.Enabled = false;
                button_training.BackColor = yellow_color;
                button_training.ForeColor = System.Drawing.Color.Black;
                splash.Close();
            }

        }

        private void initialize_matrix(object path)
        {
            string matrix_path = path.ToString();
            //We first (re-)initialize the dictionary
            M = new SMtrx1D();
            //Then we load the matrix in the dictionary
            M.load2(matrix_path);
        }


        private void initialize_panel_buttons()
        {
            //make them all invisible
            panel_single.Visible = false;
            panel_multiple.Visible = false;
            panel_settings.Visible = false;
            panel_training.Visible = false;

            button_single.Enabled = true;
            button_multiple.Enabled = true;
            button_settings.Enabled = true;
            button_training.Enabled = true;
            //reinitialising the color
            button_single.BackColor = blue_color;
            button_single.ForeColor = System.Drawing.Color.White;
            button_multiple.BackColor = blue_color;
            button_multiple.ForeColor = System.Drawing.Color.White;
            button_settings.BackColor = blue_color;
            button_settings.ForeColor = System.Drawing.Color.White;
            button_training.BackColor = blue_color;
            button_training.ForeColor = System.Drawing.Color.White;
        }

        private void progress_bar_graduations()
        {
            int height = progressBar1.Height -1;
            int width = progressBar1.Width;
            int start_x =progressBar1.Location.X;
            int start_y =progressBar1.Location.Y;
            int ng_grad = 9;
            List<LineShape> list_graduation = new List<LineShape>();

            canvas.Parent = panel_single;
            canvas.BringToFront();
            //drawing a rectangle around the progress bar for homogeneity
            LineShape border1 = new LineShape();
            LineShape border2 = new LineShape();
            LineShape border3 = new LineShape();
            LineShape border4 = new LineShape();
            border1.Parent = canvas;
            border2.Parent = canvas;
            border3.Parent = canvas;
            border4.Parent = canvas;

            border1.StartPoint = new System.Drawing.Point(start_x, start_y);
            border1.EndPoint = new System.Drawing.Point(start_x + width, start_y);
            border1.BorderColor = Color.FromArgb(179, 179, 179);

            border2.StartPoint = new System.Drawing.Point(start_x + width, start_y);
            border2.EndPoint = new System.Drawing.Point(start_x + width, start_y + height);
            border2.BorderColor = Color.FromArgb(179, 179, 179);

            border3.StartPoint = new System.Drawing.Point(start_x + width, start_y + height);
            border3.EndPoint = new System.Drawing.Point(start_x, start_y + height);
            border3.BorderColor = Color.FromArgb(179, 179, 179);

            border4.StartPoint = new System.Drawing.Point(start_x, start_y);
            border4.EndPoint = new System.Drawing.Point(start_x, start_y + height);
            border4.BorderColor = Color.FromArgb(179, 179, 179);
            //we iteratively draw the nb_grad graduations
            for (int i = 0; i < ng_grad; i++)
            {
                LineShape graduation = new LineShape();
                graduation.Parent = canvas;
                graduation.StartPoint = new System.Drawing.Point(start_x + (i+1)*(width/(ng_grad+1)), start_y);
                graduation.EndPoint = new System.Drawing.Point(start_x + (i + 1) * (width / (ng_grad + 1)), start_y + height);
                graduation.BorderColor = Color.FromArgb(179, 179, 179);
                list_graduation.Add(graduation);
            }

        }

        private double evaluate_pwd(string pwd_to_evaluate, string output_path ="")
        {
            if (File.Exists(CFHMM_matrix_path) && File.Exists(SimpleHMM_matrix_path) && M.count() != 0)
            {
                string text_to_save = "";
                if (output_path == "")
                {
                    if(checkBox_hiddenpwd.Checked)
                    {
                        //the password should be hidden, we don't display it
                        console_textbox.AppendText("\r\n[-] Evaluating the password: *hidden*\r\n");
                    }
                    else
                    {
                        console_textbox.AppendText("\r\n[-] Evaluating the password:\r\n     " + pwd_to_evaluate + "\r\n");
                    }
                }
                text_to_save += pwd_to_evaluate;

                if (output_path == "")
                {
                    console_textbox.AppendText("\r\n     * Test for LIST_strengthMetric");
                }
                
                double SlistInMain = PasswordStrength.LIST_strengthMetric(pwd_to_evaluate, FileOfBannedPasswords);
                if (output_path == "")
                {
                    console_textbox.AppendText("\r\n               -> Result: " + SlistInMain.ToString("0.###") + "\r\n");
                }
                text_to_save += ", " + SlistInMain.ToString();

                // TEST FOR BF_strengthMetric
                if (output_path == "")
                {
                    console_textbox.AppendText("\r\n     * Test for BF_strengthMetric");
                }

                double SbfInMain = PasswordStrength.BF_strengthMetric(pwd_to_evaluate, LminNum, LminLower, LminUpper, LminSpecial, LminTwo, LminThree, LminAll); // bool DebugOption

                if (output_path == "")
                {
                    console_textbox.AppendText("\r\n               -> Result: " + SbfInMain.ToString("0.###") + "\r\n");
                }
                text_to_save += ", " + SbfInMain.ToString();

                // TEST FOR MM_strengthMetric
                //PasswordStrength.MM_strengthMetric("total_matrix_mem3.mat");
                if (output_path == "")
                {
                    console_textbox.AppendText("\r\n     * Test for AMM_strengthMetric ");
                }
                double Sa = 0;
                if (ConfigurationManager.AppSettings["char_encoding"] == "0")
                {
                    Sa = PasswordStrength.MM_strengthMetric_Hashlist_Ascii(M, pwd_to_evaluate, MM_memsize);
                }
                else
                {
                    Sa = PasswordStrength.MM_strengthMetric_Hashlist_Uni1K(M, pwd_to_evaluate, MM_memsize);
                }
                double Sa_norm = -1.0;
                if (Sa != -1)
                {
                    if (double.IsInfinity(Sa))
                    {
                        Sa_norm = 10.0;
                    }
                    else
                    {
                        Sa_norm = 5.0 * Math.Tanh(0.01 * (Sa - 162.6786)) + 5.0;
                    }
                }
                if (output_path == "")
                {
                    //console_textbox.AppendText("  -> Sa: " + Sa.ToString());
                    console_textbox.AppendText("\r\n               -> Result: " + Sa_norm.ToString("0.###") + "\r\n");
                }

                //text_to_save += ", " + Sa.ToString();
                text_to_save += ", " + Sa_norm.ToString();

                // TEST FOR CFHMM_strengthMetric
                if (output_path == "")
                {
                    console_textbox.AppendText("\r\n     * Test for CFHMM_strengthMetric");
                }

                double Sn = PasswordStrength.CFHMM_strengthMetric_V2(pwd_to_evaluate, CFHMM_matrix_path);

                double Ss = 0;
                double Sn_norm;

                if (Sn == 0)
                {
                    Ss = PasswordStrength.SimpleHMM_strengthMetric_V2(pwd_to_evaluate, SimpleHMM_matrix_path);
                    Sn_norm = 5.0 * Math.Tanh(0.01 * (Ss - 13.0325) / 0.15) + 5.0;
                }
                else
                {
                    Sn_norm = 5.0 * Math.Tanh(0.01 * (Sn - 13.4383) / 0.15) + 5.0;
                }

                if (output_path == "")
                {
                    //console_textbox.AppendText(" -> Sn: " + Sn.ToString());
                    console_textbox.AppendText("\r\n               -> Result: " + Sn_norm.ToString("0.###") + "\r\n");
                }
                //text_to_save += ", " + Sn.ToString();
                text_to_save += ", " + Sn_norm.ToString();

                if (output_path == "")
                {
                    console_textbox.AppendText("\r\n     * Fusion of the scores");
                }
                int maximumLength = 40;
                double Smulti = 10.0;
                if (pwd_to_evaluate.Length <= maximumLength)
                {
                    Smulti = PasswordStrength.FusionWS(SlistInMain, SbfInMain, Sa_norm, Sn_norm);
                    //Smulti = PasswordStrength.fusionWS(SlistInMain, SbfInMain, Sa_norm, Sn_norm);
                }

                if (Smulti < 0)
                {
                    Smulti = 0.2;
                }
                if (output_path == "")
                {
                    console_textbox.AppendText("\r\n            ************************");
                    console_textbox.AppendText("\r\n            ***** SCORE: " + Smulti.ToString("0.000") + " *****");
                    console_textbox.AppendText("\r\n            ************************");

                }
                text_to_save += ", " + Smulti.ToString() + "\n";


                //Write the result into the file if output_path is not empty
                //It should have been previously checked that output_path is a file
                //but just to be sure we check again here
                if (output_path != "")
                {
                    File.AppendAllText(output_path, text_to_save);
                }

                //we are returning the score, so far sa_norm as we do not have the fusion
                return Smulti;
            }
            else
            {
                initialize_panel_buttons();
                panel_training.Visible = true;
                button_training.Enabled = false;
                button_training.BackColor = System.Drawing.Color.FromArgb(255, 204, 0);
                button_training.ForeColor = System.Drawing.Color.Black;
                checkbox_advanced_view.Checked = true;
                console_textbox.AppendText("\r\n [!] One of the matrix or the blacklist file is missing. Check if they are present in the " +
                    "folder or consider re-training them.");
                return 0;
            }


        }

        private void pwd_textbox_TextChanged(object sender, EventArgs e)
        {
            if (pwd_textbox.Text == "")
            {
                pwd_score_label.Text = "Password score: N/A";
            }
            else
            {
                double score;
                // We first check if the entered password match the set encoding
                int CodePointInitial;
                int CodePointFinal;

                if (ConfigurationManager.AppSettings["char_encoding"] == "0")
                {
                    CodePointInitial = 32;
                    CodePointFinal = 126;
                }
                else
                {
                    CodePointInitial = 32;
                    CodePointFinal = 1023;
                }

                int LengthOfCurrentPwd = pwd_textbox.Text.Length;
                int[] CurrentLineInCodingNumbers = new int[LengthOfCurrentPwd];
                for (int Index = 0; Index < LengthOfCurrentPwd; Index++)
                {
                    CurrentLineInCodingNumbers[Index] = pwd_textbox.Text[Index];
                }
                if (TrainingForPasswordStrength.CheckIfEveryIntOfArrayIsInsideARange(CurrentLineInCodingNumbers, CodePointInitial, CodePointFinal))
                {
                    console_textbox.Text = "";
                    score = Math.Round(evaluate_pwd(pwd_textbox.Text), 3);
                    progressBar1.Value = Convert.ToInt32(score * 10);
                    pwd_score_label.Text = "Password score: " + score.ToString();
                    int normalized_color_score = Convert.ToInt32(score * 512 / 10); // normalize the score in the range 0-512
                    progressBar1.ForeColor = System.Drawing.Color.FromArgb(Math.Min(512 - normalized_color_score, 255), Math.Min(normalized_color_score, 255), 0);
                    canvas.BringToFront();
                    this.Refresh();
                }
                else
                {
                    if(checkbox_advanced_view.Checked)
                    {
                        if (ConfigurationManager.AppSettings["char_encoding"] == "0")
                        {
                            console_textbox.AppendText("\r\n [!] Encoding issue, the password is not in ASCII and cannot be evaluated!\n");
                        }
                        else
                        {
                            console_textbox.AppendText("\r\n [!] Encoding issue, the password is not in the range of accepted characters and cannot be evaluated!\n");
                        }

                    }
                    else
                    {
                        if (ConfigurationManager.AppSettings["char_encoding"] == "0")
                        {
                            MessageBox.Show("Encoding issue, the password is not in ASCII and cannot be evaluated!", 
                                " Encoding issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Encoding issue, the password is not in the range of accepted characters and cannot be evaluated!", 
                                " Encoding issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }

                    pwd_score_label.Text = "Password score: N/A";
                    progressBar1.Value = 0;
                }
            }
        }

        private void button_import_case_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select the file containing the previous password";
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            if (ofd.ShowDialog() == DialogResult.OK)
            {            
                pwd_file_textbox.Text = ofd.FileName;
                output_filename_textbox.Text = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName) + "_results.txt";
                button_file_eval.Enabled = true;
                output_filename_textbox.Enabled = true;
            }
        }



        private void button_file_eval_Click(object sender, EventArgs e)
        {
            pwd_score_label.Text = "Password score: N/A";
            if (File.Exists(pwd_file_textbox.Text)) // we check if the user has selected a file
            {
                //we define the path to the output file as the directory ofther password file + the filename inserted in the textbox
                string output_filename = System.IO.Path.GetDirectoryName(pwd_file_textbox.Text) + '\\' + output_filename_textbox.Text;
                //if the output file does not exist we go straight to the evaluation
                if (!File.Exists(output_filename))
                {
                    //We first insert the csv header into the file
                    File.AppendAllText(output_filename, "Password, score_list, score_exhaustive, score_adaptive, score_cfhmm, final_score\n");
                    int nb_pwd = 0;
                    foreach (string line in File.ReadLines(pwd_file_textbox.Text))
                    {
                        nb_pwd += 1;
                        evaluate_pwd(line, output_filename);
                    }
                    if (checkbox_advanced_view.Checked)
                    {
                        console_textbox.AppendText("\n[-] Evaluation done - " + nb_pwd.ToString() + " have been processed \n");
                    }
                    else
                    {
                        MessageBox.Show("All the passwords from the file have been evaluated and stored in " +
                            output_filename_textbox.Text, "Evaluation done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // the user want to save but the output file exists and will be erased
                {
                    DialogResult result = MessageBox.Show("The file " + output_filename_textbox.Text + " already exists in the folder. \n You can click on OK to continue and delete the file, or cancel and change the output filename", "Warning", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                    {
                        console_textbox.Text = "";
                        this.Refresh();
                        // We delete the file and then create a new one with the header
                        File.Delete(output_filename);
                        File.AppendAllText(output_filename, "Password, score_list, score_exhaustive, score_adaptive, score_cfhmm, final_score\n");
                        foreach (string line in File.ReadLines(pwd_file_textbox.Text))
                        {evaluate_pwd(line, output_filename);}
                        if (checkbox_advanced_view.Checked)
                        {
                            console_textbox.AppendText("\n[-] Evaluation done\n");
                        }
                        else
                        {
                            MessageBox.Show("All the passwords from the file have been evaluated and stored in " +
                                output_filename_textbox.Text, "Evaluation done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                console_textbox.Text = "";
                this.Refresh();
                console_textbox.AppendText("[!] No file selected or file does not exist.");
            }

        }

        //Linking the two checkbox
        private void checkbox_advanced_view1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbox_advanced_view.Checked)
            {
                console_textbox.Visible = true;
                this.Size = new System.Drawing.Size(816, 550);
                this.MinimumSize = new Size(816, 550);
                this.MaximumSize = new Size(816, 550);
                //this.checkbox_advanced_view.Location = new Point(4,419);
            }
            else
            {
                console_textbox.Visible = false;
                this.Size = new System.Drawing.Size(416, 550);
                this.MinimumSize = new Size(416, 550);
                this.MaximumSize = new Size(416, 550);
                //checkbox_advanced_view.Location = new Point(4,208);
            }
        }

        // Training part

        private void button_import_training_file_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select the file for the training process";
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                training_file_textbox.Text = ofd.FileName;
                button_file_train.Enabled = true;
            }
        }

        private void button_file_train_Click(object sender, EventArgs e)
        {
            if (File.Exists(training_file_textbox.Text))
            {
                if(checkBox_ascii.Checked || checkBox_uni1k.Checked)
                {
                    checkbox_advanced_view.Checked = true;
                    Thread thread1 = new Thread(new ThreadStart(training_thread_function));
                    thread1.Start();
                }
                else
                {
                    if(checkbox_advanced_view.Checked)
                    {
                        console_textbox.AppendText("\r\n[!] No encoding selected - Select at least one encoding to be trained");
                    }
                    else
                    {
                        MessageBox.Show("Select at least one encoding to be trained", "No encoding selected",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                if (checkbox_advanced_view.Checked)
                {
                    console_textbox.AppendText("\r\n[!] No training file selected or the file does not exist");
                }
                else
                {
                    MessageBox.Show("No training file selected or the file does not exist", "Warning",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void training_thread_function()
        {
            //First disabling all the buttons allowing to launch the training
            checkBox_ascii.Invoke((MethodInvoker)delegate { checkBox_ascii.Enabled = false; });
            checkBox_uni1k.Invoke((MethodInvoker)delegate { checkBox_uni1k.Enabled = false; });
            button_import_training_file.Invoke((MethodInvoker)delegate { button_import_training_file.Enabled = false; });
            button_file_train.Invoke((MethodInvoker)delegate { button_file_train.Enabled = false; });
            try
            {
                if (checkBox_ascii.Checked)
                {
                    console_textbox.Invoke((MethodInvoker)delegate {
                        console_textbox.AppendText("\r\n[-] Starting the CFHMM training for ASCII");
                    });
                    TrainingForPasswordStrength.SimpleHMM_trainTotalsAndProbs_V2(training_file_textbox.Text, 0, "SimpleHMM_ascii", Dll_Callback);
                    TrainingForPasswordStrength.CFHMM_trainTotalsAndProbs_V2(training_file_textbox.Text, 0, "CFHMM_ascii", Dll_Callback);
                    //console_textbox.AppendText("\n [-] Starting the MMmem training for unicode\n");
                    console_textbox.Invoke((MethodInvoker)delegate {
                        console_textbox.AppendText("\r\n[-] Starting the MMmem training for ASCII");
                    });
                    TrainingForPasswordStrength.TrainMMmem_Hashlist_Ascii(training_file_textbox.Text, MM_memsize, "MMmem_ascii.bin", Dll_Callback);
                    //Re-enabling all the buttons as the training is complete
                    checkBox_ascii.Invoke((MethodInvoker)delegate { checkBox_ascii.Enabled = true; });
                    button_import_training_file.Invoke((MethodInvoker)delegate { button_import_training_file.Enabled = true; });
                    checkBox_uni1k.Invoke((MethodInvoker)delegate { checkBox_uni1k.Enabled = true; });
                    button_file_train.Invoke((MethodInvoker)delegate { button_file_train.Enabled = true; });

                    console_textbox.Invoke((MethodInvoker)delegate {
                        console_textbox.AppendText("\r\n[-] ASCII training complete");
                    });
                    if(char_encoding == 0)
                    {
                        console_textbox.Invoke((MethodInvoker)delegate {
                            console_textbox.AppendText("\r\n[-] Reloading the new matrix - it may take a while");
                        });
                        initialize_matrix(adapt_matrix_path);
                        console_textbox.Invoke((MethodInvoker)delegate {
                            console_textbox.AppendText("\r\n[-] Matrix loaded");
                        });
                    }
                }
                if (checkBox_uni1k.Checked)
                {
                    //console_textbox.AppendText("\n [-] Starting the CFHMM training for unicode\n");
                    console_textbox.Invoke((MethodInvoker)delegate {
                        console_textbox.AppendText("\r\n[-] Starting the CFHMM training for Uni1K");
                    });
                    TrainingForPasswordStrength.SimpleHMM_trainTotalsAndProbs_V2(training_file_textbox.Text,1, "SimpleHMM_uni1k", Dll_Callback);
                    TrainingForPasswordStrength.CFHMM_trainTotalsAndProbs_V2(training_file_textbox.Text, 1, "CFHMM_uni1k", Dll_Callback);
                    //console_textbox.AppendText("\n [-] Starting the MMmem training for unicode\n");
                    console_textbox.Invoke((MethodInvoker)delegate {
                        console_textbox.AppendText("\r\n[-] Starting the MMmem training for Uni1K");
                    });
                    TrainingForPasswordStrength.TrainMMmem_Hashlist_Uni1K(training_file_textbox.Text, MM_memsize, "MMmem_uni1k.bin", Dll_Callback);

                    checkBox_ascii.Invoke((MethodInvoker)delegate { checkBox_ascii.Enabled = true; });
                    checkBox_uni1k.Invoke((MethodInvoker)delegate { checkBox_uni1k.Enabled = true; });
                    button_import_training_file.Invoke((MethodInvoker)delegate { button_import_training_file.Enabled = true; });
                    button_file_train.Invoke((MethodInvoker)delegate { button_file_train.Enabled = true; });

                    console_textbox.Invoke((MethodInvoker)delegate {
                        console_textbox.AppendText("\r\n[-] Uni1K Training complete");
                    });
                    if (char_encoding == 1)
                    {
                        console_textbox.Invoke((MethodInvoker)delegate {
                            console_textbox.AppendText("\r\n[-] Reloading the new matrix - it may take a while");
                        });
                        initialize_matrix(adapt_matrix_path);
                        console_textbox.Invoke((MethodInvoker)delegate {
                            console_textbox.AppendText("\r\n[-] Matrix loaded");
                        });
                    }
                }
            }
            catch (Exception err)
            {
                string err_msg = "\r\n[!] An error occur while loading the training file \n" + err;
                //console_textbox.AppendText(err_msg);
                console_textbox.Invoke((MethodInvoker)delegate {
                    console_textbox.AppendText(err_msg);
                });
                //As it failed, we reenable everything
                checkBox_ascii.Invoke((MethodInvoker)delegate { checkBox_ascii.Enabled = true; });
                checkBox_uni1k.Invoke((MethodInvoker)delegate { checkBox_uni1k.Enabled = true; });
                button_import_training_file.Invoke((MethodInvoker)delegate { button_import_training_file.Enabled = true; });
                button_file_train.Invoke((MethodInvoker)delegate { button_file_train.Enabled = true; });
            }
        }

        public void Dll_Callback(string status)
        {
            if(console_textbox.Lines[console_textbox.Lines.Length - 1].StartsWith("[*]") && status.StartsWith("[*]"))
            {
                console_textbox.Invoke((MethodInvoker)delegate
                {console_textbox.Lines = console_textbox.Lines.Take(console_textbox.Lines.Count() - 1).ToArray(); });
            }
            console_textbox.Invoke((MethodInvoker)delegate {
                console_textbox.AppendText("\r\n" + status);
            });
        }
        //Group of functions handling the interface
        private void button_single_Click(object sender, EventArgs e)
        {
            //first we reinitialize all buttons and panels
            initialize_panel_buttons();

            //Then we update for the current one
            panel_single.Visible = true;
            button_single.BackColor = yellow_color;
            button_single.ForeColor = System.Drawing.Color.Black;
            button_single.Enabled = false;
        }
        private void button_multiple_Click(object sender, EventArgs e)
        {
            //first we reinitialize all buttons and panels
            initialize_panel_buttons();

            //Then we update for the current one
            panel_multiple.Visible = true;
            button_multiple.BackColor = yellow_color;
            button_multiple.ForeColor = System.Drawing.Color.Black;
            button_multiple.Enabled = false;
        }
        private void button_training_Click(object sender, EventArgs e)
        {
            //first we reinitialize all buttons and panels
            initialize_panel_buttons();

            //Then we update for the current one
            panel_training.Visible = true;
            button_training.BackColor = yellow_color;
            button_training.ForeColor = System.Drawing.Color.Black;
            button_training.Enabled = false;
        }
        private void button_settings_Click(object sender, EventArgs e)
        {
            //first we reinitialize all buttons and panels
            initialize_panel_buttons();

            //Then we update for the current one
            panel_settings.Visible = true;
            button_settings.BackColor = yellow_color;
            button_settings.ForeColor = System.Drawing.Color.Black;
            button_settings.Enabled = false;
        }


        /////////////////////////////////
        //Group of functions updating the App.Config according to the value modified in the settings tab
        private void update_settings_lower(object sender, EventArgs e)
        {
            if(update_settings)
            {
                //storing the value into the global variable
                LminLower = Convert.ToInt32(Math.Round(numericUpDown_lower.Value, 0));

                //saving the values in the app.config
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LminLower"].Value = LminLower.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        private void update_settings_upper(object sender, EventArgs e)
        {
            if (update_settings)
            {
                //storing the value into the global variable
                LminUpper = Convert.ToInt32(Math.Round(numericUpDown_upper.Value, 0));

                //saving the values in the app.config
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LminUpper"].Value = LminUpper.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        private void update_settings_num(object sender, EventArgs e)
        {
            if (update_settings)
            {
                //storing the value into the global variable
                LminNum = Convert.ToInt32(Math.Round(numericUpDown_number.Value, 0));

                //saving the values in the app.config
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LminNum"].Value = LminNum.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        private void update_settings_special(object sender, EventArgs e)
        {
            if (update_settings)
            {
                //storing the value into the global variable
                LminSpecial = Convert.ToInt32(Math.Round(numericUpDown_special.Value, 0));

                //saving the values in the app.config
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LminSpecial"].Value = LminSpecial.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        private void update_settings_two(object sender, EventArgs e)
        {
            if (update_settings)
            {
                //storing the value into the global variable
                LminTwo = Convert.ToInt32(Math.Round(numericUpDown_2mix.Value, 0));

                //saving the values in the app.config
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LminTwo"].Value = LminTwo.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        private void update_settings_three(object sender, EventArgs e)
        {
            if (update_settings)
            {
                //storing the value into the global variable
                LminThree = Convert.ToInt32(Math.Round(numericUpDown_3mix.Value, 0));

                //saving the values in the app.config
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LminThree"].Value = LminThree.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        private void update_settings_all(object sender, EventArgs e)
        {
            if (update_settings)
            {
                //storing the value into the global variable
                LminAll = Convert.ToInt32(Math.Round(numericUpDown_allmix.Value, 0));

                //saving the values in the app.config
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LminAll"].Value = LminAll.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        private void update_settings_encoding(object sender, EventArgs e)
        {
            if (update_settings)
            {
                //storing the value into the global variable
                char_encoding = comboBox_encoding.SelectedIndex;

                //saving the values in the app.config
                ConfigurationManager.AppSettings["char_encoding"] = char_encoding.ToString();
                if (char_encoding == 0)
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["adapt_matrix_path"].Value = "./MMmem_ascii.bin";
                    config.AppSettings.Settings["CFHMM_matrix_path"].Value = "./CFHMM_ascii.mat";
                    config.AppSettings.Settings["SimpleHMM_matrix_path"].Value = "./SimpleHMM_ascii.mat";
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    if (File.Exists("./MMmem_ascii.bin") == false || File.Exists("./CFHMM_ascii.mat") == false
                       || File.Exists("./SimpleHMM_ascii.mat") == false)
                    {
                        if (checkbox_advanced_view.Checked)
                        {
                            console_textbox.AppendText("\r\n [!] At least one the required file is missing for " +
                                "this encoding. Retrain the files if needed.");
                        }
                        else
                        {
                            MessageBox.Show("At least one the required file is missing for this encoding. Retrain the files if needed.",
                                "Trained matrix missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        console_textbox.AppendText("\r\n[-] Reloading the new matrix - it may take a while");
                        initialize_matrix(adapt_matrix_path);
                        console_textbox.AppendText("\r\n[-] Matrix loaded");
                    }
                }
                else
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["adapt_matrix_path"].Value = "./MMmem_uni1k.bin";
                    config.AppSettings.Settings["CFHMM_matrix_path"].Value = "./CFHMM_uni1k.mat";
                    config.AppSettings.Settings["SimpleHMM_matrix_path"].Value = "./SimpleHMM_uni1k.mat";
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    if (File.Exists("./MMmem_uni1k.bin") == false || File.Exists("./CFHMM_uni1k.mat") == false
                        || File.Exists("./SimpleHMM_uni1k.mat") == false)
                    {
                        if (checkbox_advanced_view.Checked)
                        {
                            console_textbox.AppendText("\r\n [!] At least one the required file is missing for " +
                                "this encoding. Retrain the files if needed.");
                        }
                        else
                        {
                            MessageBox.Show("At least one the required file is missing for this encoding. Retrain the files if needed.",
                                "Trained matrix missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        console_textbox.AppendText("\r\n[-] Reloading the new matrix - it may take a while");
                        initialize_matrix(adapt_matrix_path);
                        console_textbox.AppendText("\r\n[-] Matrix loaded");
                    }
                }
            }


        }
        private void button_restore_Click(object sender, EventArgs e)
        {
            numericUpDown_lower.Value = 11;
            numericUpDown_upper.Value = 12;
            numericUpDown_number.Value = 10;
            numericUpDown_special.Value = 9;
            numericUpDown_2mix.Value = 8;
            numericUpDown_3mix.Value = 7;
            numericUpDown_allmix.Value = 6;
            comboBox_encoding.SelectedIndex = 0;
        }
        private void button_blacklist_path_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select the file containing the blacklist";
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                blacklist_path.Text = Path.GetFileName(ofd.FileName);
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["FileOfBannedPasswords"].Value = ofd.FileName;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                FileOfBannedPasswords = ofd.FileName;
            }
        }

        private void pwd_textbox_Enter(object sender, EventArgs e)
        {
            if (first_pwd)
            {
                first_pwd = false;
                pwd_textbox.Text = "";
            }
        }

        private void about_Click(object sender, EventArgs e)
        {
            Form3 about_form = new Form3();
            about_form.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = checkBox_hiddenpwd.Checked;
            if(checkBox_hiddenpwd.Checked == false)
            {
                pwd_textbox.Text = "";
            }
        }
    }
}
