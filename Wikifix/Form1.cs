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
using System.Text.RegularExpressions;
using System.Collections;
using System.Xml;
using System.Threading;
using System.Net;
using System.IO;


namespace Wikifix
{
    public partial class Form1 : Form
    {
        public Site site = null;
        public Form1()
        {
            InitializeComponent();
            LB_wiki.Items.Add("ceb");
            LB_wiki.Items.Add("sv");
            LB_wiki.Items.Add("war");
            LB_wiki.Items.Add("en");
            LB_wiki.SelectedItem = "ceb";

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

        private void Testbutton_Click(object sender, EventArgs e)
        {
            if (site == null)
                site = util.login(LB_wiki.SelectedItem.ToString());
            if (site != null)
                memo("Logged in to " + site.address);
            else
                memo("LOGIN FAILED!");
        }

        private void InfoboxWDbutton_Click(object sender, EventArgs e)
        {
            List<string> donecats = new List<string>();

            if (site == null)
                site = util.login(LB_wiki.SelectedItem.ToString());
            if (site != null)
                memo("Logged in to " + site.address);
            else
                memo("LOGIN FAILED!");
            site.defaultEditComment = "pagdugang Infobox ug pagkatawo/kamatayon kategoriya";
            site.minorEditByDefault = true;

            int birthprop = 569;
            int deathprop = 570;

            string[] maincats = new string[4] {"Kategoriya:Mga natawo sa tuig", "Kategoriya:Mga namatay sa tuig", "Kategoriya:Mga natawo sa petsa", "Kategoriya:Mga namatay sa petsa" };

            int nedit = 0;

            do
            {
                nedit = 0;
                PageList pl = new PageList(site);
                PageList pl1 = new PageList(site);

                //Select how to get pages. Uncomment as needed.
                pl.FillFromCategoryTree("Mga tawo");
                pl1.FillFromCategory("Mga awtor");

                pl.RemoveNamespaces(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 100, 101 });



                List<string> linkword = new List<string>();
                //linkword.Add("Catalogue of Life");

                //Require title to contain one in requiretitle list:
                List<string> requiretitle = new List<string>();
                //requiretitle.Add("Radioprogram nerlagda");

                //Require title to NOT contain any in vetotitle list:
                List<string> vetotitle = new List<string>();
                vetotitle.Add("Talaan sa");

                //Require ALL in requireword list:
                List<string> requireword = new List<string>();
                //requireword.Add("obotskapad");
                //requireword.Add("= -3\n");
                //requireword.Add("Azerbajdzjan");


                //Require AT LEAST ONE in requireone list:
                List<string> requireone = new List<string>();



                List<string> vetoword = new List<string>();
                //vetoword.Add("{{infobox");
                //vetoword.Add("{{Infobox");
                //vetoword.Add("Island");
                //vetoword.Add("isländska");


                //oldtime = oldtime.AddSeconds(5);

                Console.WriteLine("Pages to change : " + pl.Count().ToString());

                int iremain = pl.Count();

                foreach (Page p in pl)
                {
                    //Skip start of alphabet:
                    //if (String.Compare(p.title,"Sicydium") < 0 )
                    //    continue;
                    memo(p.title);

                    iremain--;

                    //Check so no vetotitle words are present:

                    bool vetotitlefound = false;
                    foreach (string s in vetotitle)
                        if (p.title.Contains(s))
                            vetotitlefound = true;
                    if (vetotitlefound)
                    {
                        memo("vetotitle " + p.title);
                        continue;
                    }


                    if (pl1.Contains(p.title)) //skip pages in pl1
                        continue;

                    if (!util.tryload(p, 2))
                        continue;
                    if (!p.Exists())
                        continue;

                    //Check so no vetoword are present:

                    bool vetofound = false;
                    foreach (string s in vetoword)
                        if (p.text.Contains(s))
                            vetofound = true;

                    if (vetofound)
                        continue;

                    memo("Getting wdid");
                    int wdid = util.get_wdid_by_name("cebwiki", p.title);
                    memo(p.title + ": wdid = " + wdid);
                    if (wdid < 0)
                        continue;

                    XmlDocument cx = util.get_wd_xml(wdid);

                    Dictionary<string, string> iwdict = util.get_wd_sitelinks(cx);

                    memo("iw-count = " + iwdict.Count);

                    if (iwdict.Count < 3)
                    {
                        if (!iwdict.ContainsKey("enwiki"))
                        {
                            memo("english lacking");
                            continue;
                        }
                    }

                    DateTime birthdate = util.get_wd_time(birthprop, wdid);
                    DateTime deathdate = util.get_wd_time(deathprop, wdid);
                    memo("Born " + util.printdate(birthdate) + " " + util.printyear(birthdate));
                    memo("Died " + util.printdate(deathdate) + " " + util.printyear(deathdate));

                    bool allinvalid = true;
                    if (deathdate.Year < 9999)
                        allinvalid = false;
                    if (birthdate.Year < 9999)
                        allinvalid = false;

                    if (allinvalid)
                        continue;

                    bool alive = false;
                    if ((deathdate.Year == 9999) && (birthdate.Year > 1900) && (birthdate.Year < util.BCoffset))
                        alive = true;

                    string[] cats = new string[4] {
                        "Kategoriya:Mga natawo sa " + util.printyear(birthdate),
                        "Kategoriya:Mga namatay sa " + util.printyear(deathdate),
                        "Kategoriya:Mga natawo sa " + util.printdate(birthdate),
                        "Kategoriya:Mga namatay sa " + util.printdate(deathdate) };
                    string[] catsort = new string[4] { util.printyear(birthdate), util.printyear(deathdate), util.printdate(birthdate), util.printdate(deathdate) };

                    string origtext = p.text;

                    for (int i = 0; i < 4; i++)
                    {
                        if (alive && ((i == 1) || (i == 3)))
                            continue;

                        p.AddToCategory(cats[i]);

                        if (!donecats.Contains(cats[i]))
                        {
                            Page pc = new Page(site, cats[i]);
                            if (util.tryload(pc, 1))
                            {
                                donecats.Add(cats[i]);
                                if (!pc.Exists())
                                {
                                    pc.AddToCategory(maincats[i] + "|" + catsort[i]);
                                    if (i == 0)
                                        pc.AddToCategory(util.printyear(birthdate));
                                    else if (i == 1)
                                        pc.AddToCategory(util.printyear(deathdate));
                                    util.trysave(pc, 1,site);
                                }
                            }

                        }
                    }



                    if (!p.text.Contains("nfobox"))
                        p.text = "{{infobox tawo WD}}\n\n" + p.text;




                    if (p.text != origtext)
                    //if (false)
                    {
                        //Bot.editComment = "Ersätter och wikilänkar";
                        //isMinorEdit = true;

                        if (util.trysave(p, 4,site))
                        {
                            nedit++;
                            if (nedit < 4)
                            {
                                Console.Write("<ret>");
                                Console.ReadLine();
                            }
                            //DateTime newtime = DateTime.Now;
                            //while (newtime < oldtime)
                            //    newtime = DateTime.Now;
                            //oldtime = newtime.AddSeconds(5);
                        }
                    }
                    //iremain--;
                    memo(iremain.ToString() + " remaining.");
                }

                memo("Total # edits = " + nedit.ToString());

            }
            while (false);

        }

        private void Replacebutton_Click(object sender, EventArgs e)
        {
            replace_wikilink.replace(site, LB_wiki.SelectedItem.ToString(),this);
        }

        private void Contribtestbutton_Click(object sender, EventArgs e)
        {
            if (site == null)
                site = util.login(LB_wiki.SelectedItem.ToString());
            PageList pl = new PageList(site);

            pl.FillFromUserContributions("Lsj", 5000);
            memo(pl.Count().ToString());
            int n = 0;
            foreach (Page p in pl)
            {
                if (n % 100 == 0)
                    memo(p.title);
                n++;

            }
        }


        private void Vandalbutton_Click(object sender, EventArgs e)
        {
            if (site == null)
                site = util.login(LB_wiki.SelectedItem.ToString());
            FormIPvandal fv = new FormIPvandal(site);
            fv.Show();
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (site == null)
                    site = util.login(LB_wiki.SelectedItem.ToString(),"Lsj");
                if (util.trivdict.Count == 0)
                    util.read_trivname();

                PageList pl = new PageList(site);
                string fn = openFileDialog1.FileName;
                using (StreamReader sr = new StreamReader(fn))
                {
                    while (!sr.EndOfStream)
                        pl.Add(new Page(sr.ReadLine().Split('\t')[0]));
                }

                DialogResult result = MessageBox.Show("Really delete " + pl.Count() + " articles?", "Delete?", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    int n = 0;
                    string fnlog = util.unusedfilename(@"I:\dotnwb3\badarticle\deletelog_.txt");

                    using (StreamWriter sw = new StreamWriter(fnlog))
                    {

                        foreach (Page p in pl)
                        {
                            if (util.trivdict.ContainsKey(p.title.ToLower()))
                            {
                                memo("Skipping trivname " + p.title);
                                continue;
                            }
                            if (util.tryload(p, 2))
                            {
                                if (p.Exists())
                                {
                                    if (!util.human_touched(p, site))
                                    {
                                        n++;
                                        if (n % 100 == 0)
                                            memo(n.ToString());
                                        memo("Deleting " + p.title);
                                        try
                                        {
                                            p.Delete("Not found in source");
                                            Thread.Sleep(10000);
                                        }
                                        catch (Exception ee)
                                        {
                                            sw.WriteLine(p.title + "\tfailed delete\t"+ee.Message);
                                            sw.Flush();
                                            Thread.Sleep(10000);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        memo(p.title);
                                        foreach (string s in util.list_editors(p, site))
                                            memo("   " + s);
                                    }
                                }
                            }
                            else
                            {
                                sw.WriteLine(p.title + "\tfailed load");
                                sw.Flush();
                                continue;
                            }
                        }
                    }
                }

            }
        }

        public void newtaxontext(Page pold,Page pnew,string oldname,string newname)
        {
            pnew.text = pold.text.Replace("''" + oldname, "''" + newname).Replace("commonscat|"+oldname,"commonscat|"+newname);
            pnew.SetTemplateParameter("Taxobox", "binomial", newname,true);
        }

        private void Redirectbutton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (site == null)
                    site = util.login(LB_wiki.SelectedItem.ToString());
                if (util.trivdict.Count == 0)
                    util.read_trivname();

                //Page p = new Page("Simalia clastolepis");
                //util.tryload(p, 2);
                //memo("Images:");
                //foreach (string s in p.GetImages())
                //    memo(s);
                //memo("image param = " + p.GetFirstTemplateParameter("Taxobox", "image"));
                //return;
                List<string> onlylist = new List<string>();

                string dir = @"I:\dotnwb3\badarticle\";
                if (CB_onlylogged.Checked)
                {
                    foreach (string fnn in Directory.GetFiles(dir))
                    {
                        if (!fnn.Contains("redirectlog"))
                            continue;
                        memo(fnn);
                        using (StreamReader sr = new StreamReader(fnn))
                        {
                            while (!sr.EndOfStream)
                            {
                                string line = sr.ReadLine();
                                onlylist.Add(line.Split('\t')[0]);
                            }
                        }
                    }
                }
                memo("Onlylist " + onlylist.Count);


                    //PageList pl = new PageList(site);
                    string fn = openFileDialog1.FileName;
                string fnlog = util.unusedfilename(@"I:\dotnwb3\badarticle\redirectlog_.txt");

                using (StreamWriter sw = new StreamWriter(fnlog))
                using (StreamReader sr = new StreamReader(fn))
                {
                    int n = 0;
                    while (!sr.EndOfStream)
                    {
                        n++;
                        if (n % 100 == 0)
                            memo("..n = " + n);
                        //if (n > 100)
                        //    break;
                        string line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        if (words.Length < 3)
                            continue;
                        string old = words[0];
                        //if (util.trivdict.ContainsKey(old))
                        //    continue;
                        if (CB_onlylogged.Checked)
                        {
                            if (!onlylist.Contains(old))
                                continue;
                        }
                        string target = words[2];
                        //pl.Add(new Page(sr.ReadLine().Split('\t')[0]));
                        Page pold = new Page(old);
                        Page ptarget = new Page(target);

                        if (!util.tryload(pold, 2))
                        {
                            sw.WriteLine(old + "\tfailed load");
                            sw.Flush();
                            continue;
                        }
                        if (!pold.Exists())
                            continue;
                        if (pold.IsRedirect())
                        {
                            if (pold.RedirectsTo() == target)
                                continue;
                            else
                            {
                                memo(old + "\tredirects to\t" + pold.RedirectsTo() + "\tinstead of\t" + target);
                                sw.WriteLine(old + "\tredirects to\t" + pold.RedirectsTo() + "\tinstead of\t" + target);
                                continue;
                            }
                        }

                        util.tryload(ptarget, 2);
                        if (ptarget.Exists())
                        {
                            if (ptarget.IsRedirect())
                            {
                                if (ptarget.RedirectsTo() == old)
                                {
                                    newtaxontext(pold, ptarget, old, target);
                                    if (util.trysave(ptarget, 3, "CoL update"))
                                    {
                                        pold.text = "#REDIRECT[[" + target + "]]";
                                        util.trysave(pold, 3, "CoL redirect to new name");
                                    }
                                }
                                else
                                {
                                    newtaxontext(pold, ptarget, old, target);
                                    if (util.trysave(ptarget, 3, "CoL update"))
                                    {
                                        pold.text = "#REDIRECT[[" + target + "]]";
                                        util.trysave(pold, 3, "CoL redirect to new name");
                                    }
                                }
                            }
                            else if (ptarget.text.Contains("axobox"))
                            {
                                pold.text = "#REDIRECT[[" + target + "]]";
                                util.trysave(pold, 3, "CoL redirect to new name");
                            }
                            else
                            {
                                memo(old + "\ttarget has no taxobox\t" + target);
                                sw.WriteLine(old + "\ttarget has no taxobox\t" + target);
                                sw.Flush();
                                continue;
                            }
                        }
                        else
                        {
                            newtaxontext(pold, pold, old, target);
                            util.trysave(pold, 3, "CoL update");
                            util.trymove(pold, target, 3, "CoL update");
                        }

                    }
                }

            }

        }

        private void Revertbutton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (site == null)
                    site = util.login(LB_wiki.SelectedItem.ToString());
                if (util.trivdict.Count == 0)
                    util.read_trivname();

                string fn = openFileDialog1.FileName;
                string fnlog = util.unusedfilename(@"I:\dotnwb3\badarticle\redirectlog_.txt");

                using (StreamWriter sw = new StreamWriter(fnlog))
                using (StreamReader sr = new StreamReader(fn))
                {
                    int n = 0;
                    int ncircle = 0;
                    while (!sr.EndOfStream)
                    {
                        n++;
                        if (n % 100 == 0)
                            memo("..n = " + n);
                        //if (n > 100)
                        //    break;
                        string line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        if (words.Length < 3)
                            continue;
                        string old = words[0];
                        string target = words[2];

                        if (old != target)
                            continue;

                        ncircle++;

                        Page pold = new Page(old);

                        util.tryload(pold, 2);

                        if (!pold.Exists())
                            continue;

                        if (!pold.IsRedirect())
                            continue;

                        if (pold.RedirectsTo() != old)
                            continue;

                        pold.Revert("Reverting mistake",false);


                        //if (!util.trivdict.ContainsKey(old.ToLower()))
                        //    continue;
                        //string target = words[2];
                        ////pl.Add(new Page(sr.ReadLine().Split('\t')[0]));
                        //Page pold = new Page(old);
                        //Page ptarget = new Page(target);

                        //if (!util.tryload(pold, 2))
                        //{
                        //    sw.WriteLine(old + "\tfailed load");
                        //    sw.Flush();
                        //    continue;
                        //}
                        //if (!pold.Exists())
                        //    continue;
                        //if (pold.IsRedirect())
                        //{
                        //    if (pold.RedirectsTo() != target)
                        //        continue;
                        //    else
                        //    {
                        //        util.tryload(ptarget, 2);
                        //        if (ptarget.Exists())
                        //        {
                        //            pold.text = ptarget.text;
                        //            util.trysave(pold, 2, "Reverting mistaken move");
                        //            ptarget.text = "#REDIRECT[[" + old + "]]";
                        //            util.trysave(ptarget, 2, "Reverting mistaken move");
                        //        }
                        //        else
                        //        {
                        //            memo(target + " missing");
                        //        }

                        //    }
                        //}


                    }
                    memo("ncircle = " + ncircle);
                }

            }

        }

        private void synonymbutton_Click(object sender, EventArgs e)
        {
            if (site == null)
                site = util.login(LB_wiki.SelectedItem.ToString());

            string connectionstring = "Data Source=DESKTOP-JOB29A9;Initial Catalog=\"COL2019\";Integrated Security=True";

            COL2019 db = new COL2019(connectionstring);

            Dictionary<string, string> groupnamedict = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(@"I:\dotnwb3\groupname_ceb.csv"))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split(';');
                    groupnamedict.Add(words[0], words[3]);
                }
            }


            foreach (string tt in groupnamedict.Keys)
            {
                Taxon tax = (from c in db.Taxon where c.ScientificName == tt select c).FirstOrDefault();
                if ( tax == null)
                {
                    memo(tt + " not found");
                    continue;
                }

                string top = groupnamedict[tax.Kingdom];

                string catname = "Kategoriya:Sinonimo nga ";

                Page pcat = new Page(site, catname+groupnamedict[tt]);
                util.tryload(pcat, 2);
                if (pcat.Exists())
                    continue;
                pcat.text = "[[" + catname + top + "]]\n[[Kategoriya:" + util.initialcap(groupnamedict[tt]) + "]]";
                util.trysave(pcat, 2,site);
                //break;

            }
        }

        private void Imagebutton_Click(object sender, EventArgs e)
        {
            FormImageSuggestions fi = new FormImageSuggestions();
            fi.Show();
        }

        private void Categorybutton_Click(object sender, EventArgs e)
        {
            FormCategory fc = new FormCategory(LB_wiki.SelectedItem.ToString());
            fc.Show();
        }
    }
}
