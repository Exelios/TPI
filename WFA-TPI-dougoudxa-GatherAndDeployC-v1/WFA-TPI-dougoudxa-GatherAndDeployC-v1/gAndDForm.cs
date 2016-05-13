using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{
    public partial class appForm : Form
    {
        public appForm()
        {
            InitializeComponent();

            //Testing purposes.
            int dummyEntries = 17;
            TargetHost tempHost;

            for (int index = 0; index < dummyEntries; ++index)
            {
                tempHost = new TargetHost("\\\\INF-N511-" + (index + 1).ToString("00"), "dummy test", index);

                hostPanelContainer.Controls.Add(tempHost.getTargetHostPanel());
            }
            //End of test
        }
    }
}
