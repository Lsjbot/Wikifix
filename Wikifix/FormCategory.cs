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
    public partial class FormCategory : Form
    {
        Site site = null;
        string wiki = "ceb";
        public FormCategory(string wikipar)
        {
            InitializeComponent();
            wiki = wikipar;
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

        private void button1_Click(object sender, EventArgs e) //List articles in FROM category
        {
            if (site == null)
                site = util.login(wiki);
            if (site == null)
            {
                memo("Login failed");
                return;
            }


            string cat = TBfrom.Text;

            PageList pl = new PageList(site);
            pl.FillAllFromCategory(cat);
            memo(pl.Count() + " pages in " + cat);
            foreach (Page p in pl)
                memo(p.title);
            memo("===DONE===");

        }

        private void movebutton_Click(object sender, EventArgs e)
        {
            if (site == null)
                site = util.login(wiki);
            if (site == null)
            {
                memo("Login failed");
                return;
            }


            string fromcat = TBfrom.Text;
            string tocat =   TBto.Text;
            site.defaultEditComment = "Moving category " + fromcat + " to " + tocat;

            PageList pl = new PageList(site);
            pl.FillAllFromCategory(fromcat);
            memo(pl.Count() + " pages in " + fromcat);
            foreach (Page p in pl)
            {
                memo(p.title);
                if (util.tryload(p,2))
                {
                    if (p.Exists())
                    {
                        p.RemoveFromCategory(fromcat);
                        p.AddToCategory(tocat);
                        util.trysave(p, 3,site);
                    }
                }
            }
            memo(DateTime.Now.ToString());
            memo("===DONE===");

        }

        private void movetreebutton_Click(object sender, EventArgs e)
        {
            if (site == null)
                site = util.login(wiki);

            if (site==null)
            {
                memo("Login failed");
                return;
            }

            string fromcat = TBfrom.Text;
            string oldending = TBoldending.Text;
            string newending = TBnewending.Text;
            site.defaultEditComment = "Moving category " + fromcat + " to ..." + newending;
            memo(site.defaultEditComment);

            if (String.IsNullOrEmpty(oldending))
            {
                memo("Empty oldending");
                return;
            }
            if (String.IsNullOrEmpty(newending))
            {
                memo("Empty newending");
                return;
            }
            if (!fromcat.EndsWith(oldending))
            {
                memo(fromcat + " does not end with " + oldending);
                return;
            }

            movetree(fromcat, oldending, newending);
            memo(DateTime.Now.ToString());
            memo("==== DONE ======");
        }

        private void movetree(string root,string oldending, string newending)
        {
            if (!root.EndsWith(oldending))
                return;

            memo("Moving " + root);

            string newcat = util.ReplaceLastOccurrence(root, oldending, newending);
            Page proot = new Page(site, root);
            util.tryload(proot, 2);
            if (!proot.Exists())
            {
                return;
            }



            PageList pl = new PageList(site);
            pl.FillAllFromCategory(root);
            int iremain = pl.Count();
            memo(iremain + " pages to do");
            foreach (Page p in pl)
            {
                if (iremain % 100 == 0)
                    memo("-- " + iremain + " remaining --");
                iremain--;
                util.tryload(p, 2);

                if (p.Exists())
                {
                    p.RemoveFromCategory(root);
                    p.AddToCategory(newcat);
                    util.trysave(p, 2, site);
                }

                if (p.title.Contains(":"))
                {
                    movetree(p.title, oldending, newending);
                }

            }

            Page pnew = new Page(site, newcat);
            util.tryload(pnew, 2);
            if (pnew.Exists())
            {
                proot.text = "#REDIRECT[[" + newcat + "]]";
                util.trysave(proot, 2,site);
                memo("Redirecting " + root);
            }
            else
            {
                util.trymove(proot, newcat, 3, "Moving category page");
                memo("Done moving " + root);
            }

        }

        private void subdivisionButton_Click(object sender, EventArgs e)
        {
            string globalcat = "Kategoriya:Mga subdibisyon sa nasod";
            if (site == null)
                site = util.login(wiki);

            if (site == null)
            {
                memo("Login failed");
                return;
            }
            site.defaultEditComment = "Making province categories";


            string startcountry = "Argentina";
            string endcountry = "XXX";

            bool startfound = false;

            PageList plglobal = new PageList(site);
            plglobal.FillAllFromCategory(globalcat);

            foreach (Page pglobal in plglobal)
            {
                memo(pglobal.title);
                if (pglobal.title.EndsWith(startcountry))
                    startfound = true;
                if (!startfound)
                    continue;
                if (!pglobal.title.Contains("Mga subdi"))
                    continue;

                PageList plcountry = new PageList(site);
                plcountry.FillAllFromCategory(pglobal.title);

                foreach (Page p in plcountry)
                {
                    if (p.title.Contains(":"))
                    {
                        if (p.title.Contains(":Mga subdibisyon sa"))
                        {
                            if (util.tryload(p, 2))
                            {
                                p.RemoveFromCategory(pglobal.title);
                                p.AddToCategory(p.title.Replace("Mga subdibisyon sa ", ""));
                                util.trysave(p, 3, site);
                            }
                        }
                    }
                    else
                    {
                        Page pcat = new Page(site, "Kategoriya:" + p.title);
                        pcat.text = "";
                        pcat.AddToCategory(pglobal.title);
                        util.trysave(pcat, 3, site);
                        if (util.tryload(p, 2))
                        {
                            p.AddToCategory(pcat.title + "| ");
                            util.trysave(p, 3, site);
                        }
                    }
                }

                if (pglobal.title.EndsWith(endcountry))
                    break;
            }

            memo("===DONE===");
        }

        private void Heyobutton_Click(object sender, EventArgs e)
        {
            string globalcat = "Kategoriya:Mga subdibisyon sa nasud";
            if (site == null)
                site = util.login(wiki);

            if (site == null)
            {
                memo("Login failed");
                return;
            }
            site.defaultEditComment = "Making province categories";


            string startcountry = "Alemanya";
            string endcountry = "XXX";

            bool startfound = false;

            PageList plglobal = new PageList(site);
            plglobal.FillAllFromCategory(globalcat);

            foreach (Page pglobal in plglobal)
            {
                memo(pglobal.title);
                if (pglobal.title.EndsWith(startcountry))
                    startfound = true;
                if (!startfound)
                    continue;
                if (!pglobal.title.Contains("Mga subdi"))
                    continue;

                PageList plcountry = new PageList(site);
                plcountry.FillAllFromCategory(pglobal.title);

                foreach (Page p in plcountry)
                {
                    if (p.title.Contains(":"))
                    {
                        Page pheyo = new Page(site, p.title.Replace(":", ":Heyograpiya sa "));
                        if (util.tryload(pheyo,2))
                        {
                            if (pheyo.Exists())
                            {
                                pheyo.AddToCategory(p.title);
                                util.trysave(pheyo, 3,"Adding Heyograpiya to province categories");
                            }
                        }
                    }
                }

                if (pglobal.title.EndsWith(endcountry))
                    break;
            }

            memo("===DONE===");

        }
    }
}
