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

namespace JRC_PastMe
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button_file_eval = new System.Windows.Forms.Button();
            this.pwd_file_textbox = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.button_import_pwd_file = new System.Windows.Forms.Button();
            this.console_textbox = new System.Windows.Forms.TextBox();
            this.output_filename_textbox = new System.Windows.Forms.TextBox();
            this.checkbox_advanced_view = new System.Windows.Forms.CheckBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.checkBox_ascii = new System.Windows.Forms.CheckBox();
            this.training_file_textbox = new System.Windows.Forms.TextBox();
            this.button_file_train = new System.Windows.Forms.Button();
            this.button_import_training_file = new System.Windows.Forms.Button();
            this.button_single = new System.Windows.Forms.Button();
            this.pwd_score_label = new System.Windows.Forms.Label();
            this.pwd_textbox = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Label2 = new System.Windows.Forms.Label();
            this.panel_single = new System.Windows.Forms.Panel();
            this.panel_multiple = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.panel_training = new System.Windows.Forms.Panel();
            this.checkBox_uni1k = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.panel_settings = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.button_restore = new System.Windows.Forms.Button();
            this.button_blacklist_path = new System.Windows.Forms.Button();
            this.comboBox_encoding = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.blacklist_path = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown_special = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_allmix = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_3mix = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_number = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_2mix = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_upper = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_lower = new System.Windows.Forms.NumericUpDown();
            this.button_multiple = new System.Windows.Forms.Button();
            this.button_training = new System.Windows.Forms.Button();
            this.button_settings = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.about_label = new System.Windows.Forms.Label();
            this.checkBox_hiddenpwd = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel_single.SuspendLayout();
            this.panel_multiple.SuspendLayout();
            this.panel_training.SuspendLayout();
            this.panel_settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_special)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_allmix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_3mix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_number)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_2mix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_upper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_lower)).BeginInit();
            this.SuspendLayout();
            // 
            // button_file_eval
            // 
            this.button_file_eval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(148)))));
            this.button_file_eval.Enabled = false;
            this.button_file_eval.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_file_eval.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_file_eval.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_file_eval.ForeColor = System.Drawing.Color.White;
            this.button_file_eval.Location = new System.Drawing.Point(28, 99);
            this.button_file_eval.Name = "button_file_eval";
            this.button_file_eval.Size = new System.Drawing.Size(160, 42);
            this.button_file_eval.TabIndex = 15;
            this.button_file_eval.Text = "Evaluate file";
            this.button_file_eval.UseVisualStyleBackColor = false;
            this.button_file_eval.Click += new System.EventHandler(this.button_file_eval_Click);
            // 
            // pwd_file_textbox
            // 
            this.pwd_file_textbox.BackColor = System.Drawing.Color.White;
            this.pwd_file_textbox.Enabled = false;
            this.pwd_file_textbox.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pwd_file_textbox.ForeColor = System.Drawing.Color.Black;
            this.pwd_file_textbox.Location = new System.Drawing.Point(97, 47);
            this.pwd_file_textbox.Name = "pwd_file_textbox";
            this.pwd_file_textbox.ReadOnly = true;
            this.pwd_file_textbox.Size = new System.Drawing.Size(275, 32);
            this.pwd_file_textbox.TabIndex = 13;
            this.pwd_file_textbox.Text = "<Path Not Specified>";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.Black;
            this.Label1.Location = new System.Drawing.Point(24, 10);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(252, 24);
            this.Label1.TabIndex = 10;
            this.Label1.Text = "Path to the passwords file";
            // 
            // button_import_pwd_file
            // 
            this.button_import_pwd_file.BackColor = System.Drawing.Color.Transparent;
            this.button_import_pwd_file.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_import_pwd_file.BackgroundImage")));
            this.button_import_pwd_file.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_import_pwd_file.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_import_pwd_file.FlatAppearance.BorderSize = 0;
            this.button_import_pwd_file.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_import_pwd_file.Location = new System.Drawing.Point(28, 47);
            this.button_import_pwd_file.Name = "button_import_pwd_file";
            this.button_import_pwd_file.Size = new System.Drawing.Size(42, 32);
            this.button_import_pwd_file.TabIndex = 12;
            this.button_import_pwd_file.UseVisualStyleBackColor = false;
            this.button_import_pwd_file.Click += new System.EventHandler(this.button_import_case_Click);
            // 
            // console_textbox
            // 
            this.console_textbox.BackColor = System.Drawing.SystemColors.MenuText;
            this.console_textbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.console_textbox.Cursor = System.Windows.Forms.Cursors.Default;
            this.console_textbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.console_textbox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.console_textbox.Location = new System.Drawing.Point(400, 0);
            this.console_textbox.MaximumSize = new System.Drawing.Size(400, 550);
            this.console_textbox.MinimumSize = new System.Drawing.Size(400, 550);
            this.console_textbox.Multiline = true;
            this.console_textbox.Name = "console_textbox";
            this.console_textbox.ReadOnly = true;
            this.console_textbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.console_textbox.Size = new System.Drawing.Size(400, 550);
            this.console_textbox.TabIndex = 18;
            this.console_textbox.Visible = false;
            // 
            // output_filename_textbox
            // 
            this.output_filename_textbox.BackColor = System.Drawing.Color.White;
            this.output_filename_textbox.Enabled = false;
            this.output_filename_textbox.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output_filename_textbox.ForeColor = System.Drawing.Color.Black;
            this.output_filename_textbox.Location = new System.Drawing.Point(28, 185);
            this.output_filename_textbox.Name = "output_filename_textbox";
            this.output_filename_textbox.Size = new System.Drawing.Size(344, 32);
            this.output_filename_textbox.TabIndex = 27;
            this.output_filename_textbox.Text = "<filename>";
            // 
            // checkbox_advanced_view
            // 
            this.checkbox_advanced_view.AutoSize = true;
            this.checkbox_advanced_view.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.checkbox_advanced_view.Location = new System.Drawing.Point(3, 493);
            this.checkbox_advanced_view.Name = "checkbox_advanced_view";
            this.checkbox_advanced_view.Size = new System.Drawing.Size(101, 17);
            this.checkbox_advanced_view.TabIndex = 11;
            this.checkbox_advanced_view.Text = "Advanced View";
            this.checkbox_advanced_view.UseVisualStyleBackColor = true;
            this.checkbox_advanced_view.CheckedChanged += new System.EventHandler(this.checkbox_advanced_view1_CheckedChanged);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(142, 24);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(116, 80);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 14;
            this.pictureBox3.TabStop = false;
            // 
            // checkBox_ascii
            // 
            this.checkBox_ascii.AutoSize = true;
            this.checkBox_ascii.Checked = true;
            this.checkBox_ascii.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_ascii.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_ascii.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.checkBox_ascii.Location = new System.Drawing.Point(28, 149);
            this.checkBox_ascii.Name = "checkBox_ascii";
            this.checkBox_ascii.Size = new System.Drawing.Size(72, 23);
            this.checkBox_ascii.TabIndex = 27;
            this.checkBox_ascii.Text = "ASCII";
            this.toolTip1.SetToolTip(this.checkBox_ascii, "Consider the 95 printable ASCII characters when doing the training");
            this.checkBox_ascii.UseVisualStyleBackColor = true;
            // 
            // training_file_textbox
            // 
            this.training_file_textbox.BackColor = System.Drawing.Color.White;
            this.training_file_textbox.Enabled = false;
            this.training_file_textbox.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.training_file_textbox.ForeColor = System.Drawing.Color.Black;
            this.training_file_textbox.Location = new System.Drawing.Point(97, 47);
            this.training_file_textbox.Name = "training_file_textbox";
            this.training_file_textbox.ReadOnly = true;
            this.training_file_textbox.Size = new System.Drawing.Size(275, 32);
            this.training_file_textbox.TabIndex = 13;
            this.training_file_textbox.Text = "<Path Not Specified>";
            // 
            // button_file_train
            // 
            this.button_file_train.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(148)))));
            this.button_file_train.Enabled = false;
            this.button_file_train.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_file_train.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_file_train.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_file_train.ForeColor = System.Drawing.Color.White;
            this.button_file_train.Location = new System.Drawing.Point(212, 138);
            this.button_file_train.Name = "button_file_train";
            this.button_file_train.Size = new System.Drawing.Size(160, 42);
            this.button_file_train.TabIndex = 15;
            this.button_file_train.Text = "Train Matrices";
            this.button_file_train.UseVisualStyleBackColor = false;
            this.button_file_train.Click += new System.EventHandler(this.button_file_train_Click);
            // 
            // button_import_training_file
            // 
            this.button_import_training_file.BackColor = System.Drawing.Color.Transparent;
            this.button_import_training_file.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_import_training_file.BackgroundImage")));
            this.button_import_training_file.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_import_training_file.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_import_training_file.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_import_training_file.Location = new System.Drawing.Point(28, 47);
            this.button_import_training_file.Name = "button_import_training_file";
            this.button_import_training_file.Size = new System.Drawing.Size(42, 32);
            this.button_import_training_file.TabIndex = 12;
            this.button_import_training_file.UseVisualStyleBackColor = false;
            this.button_import_training_file.Click += new System.EventHandler(this.button_import_training_file_Click);
            // 
            // button_single
            // 
            this.button_single.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(148)))));
            this.button_single.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_single.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_single.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_single.ForeColor = System.Drawing.Color.White;
            this.button_single.Location = new System.Drawing.Point(28, 128);
            this.button_single.Name = "button_single";
            this.button_single.Size = new System.Drawing.Size(160, 42);
            this.button_single.TabIndex = 34;
            this.button_single.Text = "Single Password";
            this.button_single.UseVisualStyleBackColor = false;
            this.button_single.Click += new System.EventHandler(this.button_single_Click);
            // 
            // pwd_score_label
            // 
            this.pwd_score_label.AutoSize = true;
            this.pwd_score_label.BackColor = System.Drawing.Color.Transparent;
            this.pwd_score_label.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pwd_score_label.ForeColor = System.Drawing.Color.Black;
            this.pwd_score_label.Location = new System.Drawing.Point(24, 138);
            this.pwd_score_label.Name = "pwd_score_label";
            this.pwd_score_label.Size = new System.Drawing.Size(207, 24);
            this.pwd_score_label.TabIndex = 28;
            this.pwd_score_label.Text = "Password score: N/A";
            this.pwd_score_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pwd_textbox
            // 
            this.pwd_textbox.BackColor = System.Drawing.Color.White;
            this.pwd_textbox.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pwd_textbox.ForeColor = System.Drawing.Color.Black;
            this.pwd_textbox.Location = new System.Drawing.Point(28, 47);
            this.pwd_textbox.Name = "pwd_textbox";
            this.pwd_textbox.Size = new System.Drawing.Size(344, 32);
            this.pwd_textbox.TabIndex = 29;
            this.pwd_textbox.Text = "<Type here your password>";
            this.pwd_textbox.TextChanged += new System.EventHandler(this.pwd_textbox_TextChanged);
            this.pwd_textbox.Enter += new System.EventHandler(this.pwd_textbox_Enter);
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.White;
            this.progressBar1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.progressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.progressBar1.Location = new System.Drawing.Point(28, 173);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(344, 44);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 30;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.Color.Black;
            this.Label2.Location = new System.Drawing.Point(24, 10);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(219, 24);
            this.Label2.TabIndex = 9;
            this.Label2.Text = "Password to evaluate:";
            // 
            // panel_single
            // 
            this.panel_single.BackColor = System.Drawing.Color.Transparent;
            this.panel_single.Controls.Add(this.checkBox_hiddenpwd);
            this.panel_single.Controls.Add(this.progressBar1);
            this.panel_single.Controls.Add(this.Label2);
            this.panel_single.Controls.Add(this.pwd_textbox);
            this.panel_single.Controls.Add(this.pwd_score_label);
            this.panel_single.Location = new System.Drawing.Point(0, 260);
            this.panel_single.Name = "panel_single";
            this.panel_single.Size = new System.Drawing.Size(400, 230);
            this.panel_single.TabIndex = 15;
            // 
            // panel_multiple
            // 
            this.panel_multiple.BackColor = System.Drawing.Color.Transparent;
            this.panel_multiple.Controls.Add(this.button_file_eval);
            this.panel_multiple.Controls.Add(this.button_import_pwd_file);
            this.panel_multiple.Controls.Add(this.label14);
            this.panel_multiple.Controls.Add(this.Label1);
            this.panel_multiple.Controls.Add(this.pwd_file_textbox);
            this.panel_multiple.Controls.Add(this.output_filename_textbox);
            this.panel_multiple.Location = new System.Drawing.Point(401, 260);
            this.panel_multiple.Name = "panel_multiple";
            this.panel_multiple.Size = new System.Drawing.Size(400, 230);
            this.panel_multiple.TabIndex = 37;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(24, 158);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(225, 24);
            this.label14.TabIndex = 10;
            this.label14.Text = "Name of the output file ";
            // 
            // panel_training
            // 
            this.panel_training.BackColor = System.Drawing.Color.Transparent;
            this.panel_training.Controls.Add(this.checkBox_uni1k);
            this.panel_training.Controls.Add(this.label13);
            this.panel_training.Controls.Add(this.checkBox_ascii);
            this.panel_training.Controls.Add(this.label16);
            this.panel_training.Controls.Add(this.button_import_training_file);
            this.panel_training.Controls.Add(this.button_file_train);
            this.panel_training.Controls.Add(this.training_file_textbox);
            this.panel_training.Location = new System.Drawing.Point(827, 260);
            this.panel_training.Name = "panel_training";
            this.panel_training.Size = new System.Drawing.Size(400, 230);
            this.panel_training.TabIndex = 15;
            // 
            // checkBox_uni1k
            // 
            this.checkBox_uni1k.AutoSize = true;
            this.checkBox_uni1k.Checked = true;
            this.checkBox_uni1k.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_uni1k.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_uni1k.Location = new System.Drawing.Point(121, 149);
            this.checkBox_uni1k.Name = "checkBox_uni1k";
            this.checkBox_uni1k.Size = new System.Drawing.Size(69, 23);
            this.checkBox_uni1k.TabIndex = 29;
            this.checkBox_uni1k.Text = "Uni1k";
            this.toolTip1.SetToolTip(this.checkBox_uni1k, "Consider the first one thousand character of the unicode charset when doing the t" +
        "raining");
            this.checkBox_uni1k.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(24, 108);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(167, 24);
            this.label13.TabIndex = 28;
            this.label13.Text = "Encoding to train";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(24, 10);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(266, 24);
            this.label16.TabIndex = 10;
            this.label16.Text = "Path to the training data file";
            // 
            // panel_settings
            // 
            this.panel_settings.BackColor = System.Drawing.Color.Transparent;
            this.panel_settings.Controls.Add(this.label15);
            this.panel_settings.Controls.Add(this.button_restore);
            this.panel_settings.Controls.Add(this.button_blacklist_path);
            this.panel_settings.Controls.Add(this.comboBox_encoding);
            this.panel_settings.Controls.Add(this.label8);
            this.panel_settings.Controls.Add(this.blacklist_path);
            this.panel_settings.Controls.Add(this.label11);
            this.panel_settings.Controls.Add(this.label7);
            this.panel_settings.Controls.Add(this.label10);
            this.panel_settings.Controls.Add(this.label6);
            this.panel_settings.Controls.Add(this.label9);
            this.panel_settings.Controls.Add(this.label5);
            this.panel_settings.Controls.Add(this.label12);
            this.panel_settings.Controls.Add(this.label4);
            this.panel_settings.Controls.Add(this.numericUpDown_special);
            this.panel_settings.Controls.Add(this.numericUpDown_allmix);
            this.panel_settings.Controls.Add(this.numericUpDown_3mix);
            this.panel_settings.Controls.Add(this.numericUpDown_number);
            this.panel_settings.Controls.Add(this.numericUpDown_2mix);
            this.panel_settings.Controls.Add(this.numericUpDown_upper);
            this.panel_settings.Controls.Add(this.numericUpDown_lower);
            this.panel_settings.Location = new System.Drawing.Point(827, 24);
            this.panel_settings.Name = "panel_settings";
            this.panel_settings.Size = new System.Drawing.Size(400, 230);
            this.panel_settings.TabIndex = 15;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(24, 158);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(136, 24);
            this.label15.TabIndex = 40;
            this.label15.Text = "Blacklist Path";
            this.toolTip1.SetToolTip(this.label15, "The blacklist should be composed of common password/words that should be banned a" +
        "s password (e.g. 12345, admin...)");
            // 
            // button_restore
            // 
            this.button_restore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(148)))));
            this.button_restore.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_restore.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_restore.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_restore.ForeColor = System.Drawing.Color.White;
            this.button_restore.Location = new System.Drawing.Point(307, 114);
            this.button_restore.Name = "button_restore";
            this.button_restore.Size = new System.Drawing.Size(65, 32);
            this.button_restore.TabIndex = 15;
            this.button_restore.Text = "Reset";
            this.toolTip1.SetToolTip(this.button_restore, "Set all the values back to the default values");
            this.button_restore.UseVisualStyleBackColor = false;
            this.button_restore.Click += new System.EventHandler(this.button_restore_Click);
            // 
            // button_blacklist_path
            // 
            this.button_blacklist_path.BackColor = System.Drawing.Color.Transparent;
            this.button_blacklist_path.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_blacklist_path.BackgroundImage")));
            this.button_blacklist_path.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_blacklist_path.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_blacklist_path.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_blacklist_path.Location = new System.Drawing.Point(28, 185);
            this.button_blacklist_path.Name = "button_blacklist_path";
            this.button_blacklist_path.Size = new System.Drawing.Size(42, 32);
            this.button_blacklist_path.TabIndex = 12;
            this.toolTip1.SetToolTip(this.button_blacklist_path, "The blacklist should be composed of common password/words that should be banned a" +
        "s password (e.g. 12345, admin...)");
            this.button_blacklist_path.UseVisualStyleBackColor = false;
            this.button_blacklist_path.Click += new System.EventHandler(this.button_blacklist_path_Click);
            // 
            // comboBox_encoding
            // 
            this.comboBox_encoding.BackColor = System.Drawing.Color.White;
            this.comboBox_encoding.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_encoding.ForeColor = System.Drawing.Color.Black;
            this.comboBox_encoding.FormattingEnabled = true;
            this.comboBox_encoding.Items.AddRange(new object[] {
            "ASCII",
            "Uni1K"});
            this.comboBox_encoding.Location = new System.Drawing.Point(260, 185);
            this.comboBox_encoding.Name = "comboBox_encoding";
            this.comboBox_encoding.Size = new System.Drawing.Size(112, 32);
            this.comboBox_encoding.TabIndex = 39;
            this.toolTip1.SetToolTip(this.comboBox_encoding, "Use the 95 ASCII printable characters or the first one thousand characters of the" +
        " unicode charset when evaluation the passwords. You need to have the appropriate" +
        " matrix for that.");
            this.comboBox_encoding.SelectedIndexChanged += new System.EventHandler(this.update_settings_encoding);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(309, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 18);
            this.label8.TabIndex = 32;
            this.label8.Text = "Special";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // blacklist_path
            // 
            this.blacklist_path.BackColor = System.Drawing.Color.White;
            this.blacklist_path.Enabled = false;
            this.blacklist_path.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blacklist_path.ForeColor = System.Drawing.Color.Black;
            this.blacklist_path.Location = new System.Drawing.Point(84, 185);
            this.blacklist_path.Name = "blacklist_path";
            this.blacklist_path.ReadOnly = true;
            this.blacklist_path.Size = new System.Drawing.Size(148, 32);
            this.blacklist_path.TabIndex = 13;
            this.blacklist_path.Text = "<Path Not Specified>";
            this.toolTip1.SetToolTip(this.blacklist_path, "The blacklist should be composed of common password/words that should be banned a" +
        "s password (e.g. 12345, admin...)");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(219, 94);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 18);
            this.label11.TabIndex = 32;
            this.label11.Text = "All-Mix";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(215, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 18);
            this.label7.TabIndex = 32;
            this.label7.Text = "Number";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(130, 94);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 18);
            this.label10.TabIndex = 32;
            this.label10.Text = "3-Mix";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(128, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 18);
            this.label6.TabIndex = 32;
            this.label6.Text = "Upper";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(37, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 18);
            this.label9.TabIndex = 32;
            this.label9.Text = "2-Mix";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(35, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 18);
            this.label5.TabIndex = 32;
            this.label5.Text = "Lower";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(256, 158);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(96, 24);
            this.label12.TabIndex = 31;
            this.label12.Text = "Encoding";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(24, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(301, 24);
            this.label4.TabIndex = 31;
            this.label4.Text = "Exhaustive Search Parameters";
            // 
            // numericUpDown_special
            // 
            this.numericUpDown_special.BackColor = System.Drawing.Color.White;
            this.numericUpDown_special.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_special.ForeColor = System.Drawing.Color.Black;
            this.numericUpDown_special.Location = new System.Drawing.Point(307, 57);
            this.numericUpDown_special.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_special.Name = "numericUpDown_special";
            this.numericUpDown_special.Size = new System.Drawing.Size(65, 32);
            this.numericUpDown_special.TabIndex = 30;
            this.numericUpDown_special.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numericUpDown_special, "Set the minimum size of a password composed only of special characters to be cons" +
        "idered as resistant to an exhaustive search ");
            this.numericUpDown_special.ValueChanged += new System.EventHandler(this.update_settings_special);
            // 
            // numericUpDown_allmix
            // 
            this.numericUpDown_allmix.BackColor = System.Drawing.Color.White;
            this.numericUpDown_allmix.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_allmix.ForeColor = System.Drawing.Color.Black;
            this.numericUpDown_allmix.Location = new System.Drawing.Point(214, 114);
            this.numericUpDown_allmix.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_allmix.Name = "numericUpDown_allmix";
            this.numericUpDown_allmix.Size = new System.Drawing.Size(65, 32);
            this.numericUpDown_allmix.TabIndex = 30;
            this.numericUpDown_allmix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numericUpDown_allmix, "Set the minimum size of a password composed of four types of characters (lower, u" +
        "pper, number, special) to be considered as resistant to an exhaustive search ");
            this.numericUpDown_allmix.ValueChanged += new System.EventHandler(this.update_settings_all);
            // 
            // numericUpDown_3mix
            // 
            this.numericUpDown_3mix.BackColor = System.Drawing.Color.White;
            this.numericUpDown_3mix.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_3mix.ForeColor = System.Drawing.Color.Black;
            this.numericUpDown_3mix.Location = new System.Drawing.Point(121, 114);
            this.numericUpDown_3mix.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_3mix.Name = "numericUpDown_3mix";
            this.numericUpDown_3mix.Size = new System.Drawing.Size(65, 32);
            this.numericUpDown_3mix.TabIndex = 30;
            this.numericUpDown_3mix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numericUpDown_3mix, "Set the minimum size of a password composed of three types of characters (lower, " +
        "upper, number, special) to be considered as resistant to an exhaustive search ");
            this.numericUpDown_3mix.ValueChanged += new System.EventHandler(this.update_settings_three);
            // 
            // numericUpDown_number
            // 
            this.numericUpDown_number.BackColor = System.Drawing.Color.White;
            this.numericUpDown_number.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_number.ForeColor = System.Drawing.Color.Black;
            this.numericUpDown_number.Location = new System.Drawing.Point(214, 57);
            this.numericUpDown_number.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_number.Name = "numericUpDown_number";
            this.numericUpDown_number.Size = new System.Drawing.Size(65, 32);
            this.numericUpDown_number.TabIndex = 30;
            this.numericUpDown_number.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numericUpDown_number, "Set the minimum size of a password composed only of numbers to be considered as r" +
        "esistant to an exhaustive search ");
            this.numericUpDown_number.ValueChanged += new System.EventHandler(this.update_settings_num);
            // 
            // numericUpDown_2mix
            // 
            this.numericUpDown_2mix.BackColor = System.Drawing.Color.White;
            this.numericUpDown_2mix.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_2mix.ForeColor = System.Drawing.Color.Black;
            this.numericUpDown_2mix.Location = new System.Drawing.Point(28, 114);
            this.numericUpDown_2mix.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_2mix.Name = "numericUpDown_2mix";
            this.numericUpDown_2mix.Size = new System.Drawing.Size(65, 32);
            this.numericUpDown_2mix.TabIndex = 30;
            this.numericUpDown_2mix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numericUpDown_2mix, "Set the minimum size of a password composed of two types of characters (lower, up" +
        "per, number, special) to be considered as resistant to an exhaustive search ");
            this.numericUpDown_2mix.ValueChanged += new System.EventHandler(this.update_settings_two);
            // 
            // numericUpDown_upper
            // 
            this.numericUpDown_upper.BackColor = System.Drawing.Color.White;
            this.numericUpDown_upper.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_upper.ForeColor = System.Drawing.Color.Black;
            this.numericUpDown_upper.Location = new System.Drawing.Point(121, 57);
            this.numericUpDown_upper.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_upper.Name = "numericUpDown_upper";
            this.numericUpDown_upper.Size = new System.Drawing.Size(65, 32);
            this.numericUpDown_upper.TabIndex = 30;
            this.numericUpDown_upper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numericUpDown_upper, "Set the minimum size of a password composed only of uppercases to be considered a" +
        "s resistant to an exhaustive search ");
            this.numericUpDown_upper.ValueChanged += new System.EventHandler(this.update_settings_upper);
            // 
            // numericUpDown_lower
            // 
            this.numericUpDown_lower.BackColor = System.Drawing.Color.White;
            this.numericUpDown_lower.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_lower.ForeColor = System.Drawing.Color.Black;
            this.numericUpDown_lower.Location = new System.Drawing.Point(28, 57);
            this.numericUpDown_lower.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_lower.Name = "numericUpDown_lower";
            this.numericUpDown_lower.Size = new System.Drawing.Size(65, 32);
            this.numericUpDown_lower.TabIndex = 30;
            this.numericUpDown_lower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numericUpDown_lower, "Set the minimum size of a password composed only of lowercases to be considered a" +
        "s resistant to an exhaustive search ");
            this.numericUpDown_lower.ValueChanged += new System.EventHandler(this.update_settings_lower);
            // 
            // button_multiple
            // 
            this.button_multiple.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(148)))));
            this.button_multiple.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_multiple.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_multiple.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_multiple.ForeColor = System.Drawing.Color.White;
            this.button_multiple.Location = new System.Drawing.Point(212, 128);
            this.button_multiple.Name = "button_multiple";
            this.button_multiple.Size = new System.Drawing.Size(160, 42);
            this.button_multiple.TabIndex = 34;
            this.button_multiple.Text = "Multiple Password";
            this.button_multiple.UseVisualStyleBackColor = false;
            this.button_multiple.Click += new System.EventHandler(this.button_multiple_Click);
            // 
            // button_training
            // 
            this.button_training.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(148)))));
            this.button_training.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_training.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_training.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_training.ForeColor = System.Drawing.Color.White;
            this.button_training.Location = new System.Drawing.Point(212, 194);
            this.button_training.Name = "button_training";
            this.button_training.Size = new System.Drawing.Size(160, 42);
            this.button_training.TabIndex = 34;
            this.button_training.Text = "Training Data";
            this.button_training.UseVisualStyleBackColor = false;
            this.button_training.Click += new System.EventHandler(this.button_training_Click);
            // 
            // button_settings
            // 
            this.button_settings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(148)))));
            this.button_settings.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_settings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_settings.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_settings.ForeColor = System.Drawing.Color.White;
            this.button_settings.Location = new System.Drawing.Point(28, 194);
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(160, 42);
            this.button_settings.TabIndex = 34;
            this.button_settings.Text = "Settings";
            this.button_settings.UseVisualStyleBackColor = false;
            this.button_settings.Click += new System.EventHandler(this.button_settings_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(1221, 511);
            this.shapeContainer1.TabIndex = 38;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 32;
            this.lineShape1.X2 = 364;
            this.lineShape1.Y1 = 253;
            this.lineShape1.Y2 = 253;
            // 
            // about_label
            // 
            this.about_label.AutoSize = true;
            this.about_label.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.about_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(48)))), ((int)(((byte)(168)))));
            this.about_label.Location = new System.Drawing.Point(359, 494);
            this.about_label.Name = "about_label";
            this.about_label.Size = new System.Drawing.Size(40, 14);
            this.about_label.TabIndex = 39;
            this.about_label.Text = "About";
            this.about_label.Click += new System.EventHandler(this.about_Click);
            // 
            // checkBox_hiddenpwd
            // 
            this.checkBox_hiddenpwd.AutoSize = true;
            this.checkBox_hiddenpwd.Font = new System.Drawing.Font("Arial", 12.75F);
            this.checkBox_hiddenpwd.Location = new System.Drawing.Point(28, 88);
            this.checkBox_hiddenpwd.Name = "checkBox_hiddenpwd";
            this.checkBox_hiddenpwd.Size = new System.Drawing.Size(135, 23);
            this.checkBox_hiddenpwd.TabIndex = 31;
            this.checkBox_hiddenpwd.Text = "Hide password";
            this.checkBox_hiddenpwd.UseVisualStyleBackColor = true;
            this.checkBox_hiddenpwd.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1221, 511);
            this.Controls.Add(this.about_label);
            this.Controls.Add(this.checkbox_advanced_view);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.panel_multiple);
            this.Controls.Add(this.panel_settings);
            this.Controls.Add(this.panel_training);
            this.Controls.Add(this.panel_single);
            this.Controls.Add(this.button_training);
            this.Controls.Add(this.button_settings);
            this.Controls.Add(this.button_multiple);
            this.Controls.Add(this.button_single);
            this.Controls.Add(this.console_textbox);
            this.Controls.Add(this.shapeContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JRC - PastMe";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel_single.ResumeLayout(false);
            this.panel_single.PerformLayout();
            this.panel_multiple.ResumeLayout(false);
            this.panel_multiple.PerformLayout();
            this.panel_training.ResumeLayout(false);
            this.panel_training.PerformLayout();
            this.panel_settings.ResumeLayout(false);
            this.panel_settings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_special)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_allmix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_3mix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_number)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_2mix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_upper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_lower)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button button_file_eval;
        private System.Windows.Forms.TextBox pwd_file_textbox;
        private System.Windows.Forms.Button button_import_pwd_file;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox console_textbox;
        private System.Windows.Forms.TextBox output_filename_textbox;
        private System.Windows.Forms.CheckBox checkbox_advanced_view;
        private System.Windows.Forms.ColorDialog colorDialog1;
        internal System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TextBox training_file_textbox;
        internal System.Windows.Forms.Button button_file_train;
        private System.Windows.Forms.Button button_import_training_file;
        private System.Windows.Forms.CheckBox checkBox_ascii;
        private System.Windows.Forms.Button button_single;
        private System.Windows.Forms.Label pwd_score_label;
        internal System.Windows.Forms.TextBox pwd_textbox;
        private System.Windows.Forms.ProgressBar progressBar1;
        internal System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Panel panel_single;
        private System.Windows.Forms.Panel panel_multiple;
        private System.Windows.Forms.Panel panel_training;
        private System.Windows.Forms.Panel panel_settings;
        private System.Windows.Forms.Button button_multiple;
        private System.Windows.Forms.Button button_training;
        private System.Windows.Forms.Button button_settings;
        private System.Windows.Forms.NumericUpDown numericUpDown_lower;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown_special;
        private System.Windows.Forms.NumericUpDown numericUpDown_allmix;
        private System.Windows.Forms.NumericUpDown numericUpDown_3mix;
        private System.Windows.Forms.NumericUpDown numericUpDown_number;
        private System.Windows.Forms.NumericUpDown numericUpDown_2mix;
        private System.Windows.Forms.NumericUpDown numericUpDown_upper;
        private System.Windows.Forms.ComboBox comboBox_encoding;
        internal System.Windows.Forms.Button button_restore;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox checkBox_uni1k;
        internal System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button_blacklist_path;
        private System.Windows.Forms.TextBox blacklist_path;
        private System.Windows.Forms.ToolTip toolTip1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        internal System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label about_label;
        private System.Windows.Forms.CheckBox checkBox_hiddenpwd;
    }
}

