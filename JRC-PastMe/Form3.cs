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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRC_PastMe
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //the link has been clicked
            this.linkLabel1.LinkVisited = true;
            //Send the user to the proper URL
            System.Diagnostics.Process.Start("https://joinup.ec.europa.eu/collection/eupl/eupl-text-11-12");
        }
    }
}
