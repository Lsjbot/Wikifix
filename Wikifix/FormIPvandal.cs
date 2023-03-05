using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetWikiBot;

namespace Wikifix
{
    public partial class FormIPvandal : Form
    {
        List<string> iplist = new List<string>();
        List<string> neglectlist = new List<string>();
        Site site;
        public FormIPvandal(Site sitepar)
        {
            InitializeComponent();

            site = sitepar;
            site.defaultEditComment = "Undoing mass vandalism";

            //iplist.Add("185.209.196.175");
            //iplist.Add("193.32.249.162");
            //iplist.Add("193.32.127.238");
            //iplist.Add("193.32.127.232");
            //iplist.Add("193.32.127.226");
            //iplist.Add("193.32.127.214");
            //iplist.Add("81.17.24.203");
            iplist.Add("45.162.228.187");

            neglectlist.Add("EmausBot");

            foreach (string ip in iplist)
                LB_IP.Items.Add(ip);
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }



        private void Quitbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            foreach (string ip in iplist)
            {
                PageList pl = new PageList(site);
                pl.FillFromUserContributions(ip, 5000);
                //Skip all namespaces except articles:
                pl.RemoveNamespaces(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 100, 101 });

                memo(pl.Count() + " edits by " + ip);
                int np = 0;

                foreach (Page p in pl)
                {
                    np++;
                    //if (np > 3)
                    //    break;

                    if (!util.tryload(p, 2))
                        continue;
                    if (!p.Exists())
                        continue;
                    memo(p.title + ": " + p.lastUser);
                    if (p.lastUser.Contains("Lsj"))
                        continue;
                    PageList plh = new PageList(site);
                    plh.FillFromPageHistory(p.title, 5);
                    foreach (Page ph in plh)
                    {
                        if (!util.tryload(ph, 2))
                            break;
                        memo("  " + ph.lastUser);
                        if (iplist.Contains(ph.lastUser))
                            continue;
                        if (neglectlist.Contains(ph.lastUser))
                            continue;
                        p.text = ph.text;
                        util.trysave(p, 1,site);
                        break;
                    }
                }
                richTextBox1.Refresh();
            }
        }
    }
}
