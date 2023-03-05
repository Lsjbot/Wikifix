using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Net;
using DotNetWikiBot;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Threading;

namespace Wikifix
{
    public partial class FormImageSuggestions : Form
    {
        Dictionary<string, Site> sitedict = new Dictionary<string, Site>();
        int minsize = 50;
        string botaccount = "Lsjbot";
        Site cmsite;
        Site svsite;

        public FormImageSuggestions()
        {
            InitializeComponent();
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }

        public static List<string> GetImageParams(Page p, string image)
        {
            List<string> paramlist = new List<string>();

            int start = p.text.IndexOf(image);
            if (start <= 0)
                return paramlist;

            start += image.Length;

            int end = p.text.IndexOf("]]", start);
            if (end < start)
                return paramlist;

            string paramstring = p.text.Substring(start, end - start);

            foreach (string pp in paramstring.Split('|'))
                if (!String.IsNullOrEmpty(pp))
                    paramlist.Add(pp);

            return paramlist;
        }

        public static List<string> GetImagesInTemplates(Site site, Page p)
        {
            List<string> imagelist = new List<string>();
            List<string> paramlist = new List<string>();
            paramlist.Add("image");
            paramlist.Add("imagen");
            paramlist.Add("immagine");
            paramlist.Add("afbeelding");
            paramlist.Add("bild");
            paramlist.Add("bildname");

            foreach (string template in p.GetTemplates(true, false))
            {
                Dictionary<string, string> parameters = Page.ParseTemplate(template);
                foreach (string param in parameters.Keys)
                    if (paramlist.Contains(param.ToLower()))
                        if (!String.IsNullOrEmpty(parameters[param].Trim()))
                            imagelist.Add(parameters[param].Trim());
            }
            return imagelist;
        }

        private Dictionary<string,string> get_newpix_IW(Page p)
        {
            //find iw:

            Dictionary<string, string> newpix = new Dictionary<string, string>();

            //string[] iw = p.GetInterWikiLinks();
            //Check if pix from iw is already used in target article:
            //

            List<string> iwlist = new List<string>();
            try
            {
                iwlist = p.GetInterLanguageLinks();
            }
            catch (WebException e4)
            {
                string message = e4.Message;
                Console.Error.WriteLine(message);
                Thread.Sleep(10000);//milliseconds
            }

            Console.WriteLine("iwlist.Count " + iwlist.Count);

            //if (iw.Length == 0)
            //    iwlist = Interwiki(wdsite, p.title);
            //else
            //{
            //    foreach (string iws in iw)
            //        iwlist.Add(iws);
            //}


            foreach (string iws in iwlist)
            {
                string[] ss = iws.Split(':');
                string iwcode = ss[0];
                string iwtitle = ss[1];
                Console.WriteLine("iw - " + iwcode + ":" + iwtitle);

                if (iwcode == "nah")
                    continue;

                if (!sitedict.ContainsKey(iwcode))
                {
                    string iwurl = "https://" + iwcode + ".wikipedia.org";
                    try
                    {
                        try
                        {
                            sitedict.Add(iwcode, util.login(iwcode,botaccount));
                        }
                        catch (WebException e5)
                        {
                            Console.WriteLine(e5.Message);
                            continue;
                        }
                    }
                    catch (WikiBotException e6)
                    {
                        Console.WriteLine(e6.Message);
                        continue;
                    }
                }

                Page piw = new Page(sitedict[iwcode], iwtitle);
                try
                {
                    piw.Load();
                }
                catch (WebException e7)
                {
                    Console.WriteLine(e7.Message);
                    continue;
                }

                if (!piw.Exists())
                {
                    Console.WriteLine("Not found despite iw");
                    continue;
                }


                List<string> iwpix = piw.GetImages();
                List<string> iwpix2 = GetImagesInTemplates(sitedict[iwcode], piw);
                foreach (string pic in iwpix2)
                    iwpix.Add(pic);

                foreach (string iwpicture in iwpix)
                {
                    string iwpic = iwpicture;
                    //Remove file prefix:
                    if (iwpic.Contains(":"))
                        iwpic = iwpic.Split(':')[1];

                    //Skip if smaller than minsize:
                    int size = 999;
                    foreach (string pp in GetImageParams(piw, iwpic))
                    {
                        if (pp.Contains("px"))
                        {
                            size = util.tryconvert(pp.Replace("px", ""));
                            break;
                        }
                    }
                    if ((size > 0) && (size < minsize))
                        continue;

                    //Replace space with underscore:
                    iwpic = iwpic.Replace(" ", "_");

                    //Add to list:
                    if (newpix.ContainsKey(iwpic))
                        newpix[iwpic] = newpix[iwpic] + ":" + iwcode;
                    else
                        newpix.Add(iwpic, iwcode);
                }

            }

            bool fromcommons = false;

            if (newpix.Count == 0)
            {
                if (p.text.Contains("ommonscat|"))
                {
                    fromcommons = true;
                    string s = "";
                    if (p.text.IndexOf("{{commonscat|") > 0)
                    {
                        s = p.text.Remove(0, p.text.IndexOf("{{commonscat|"));
                        s = s.Remove(s.IndexOf("}}"));
                        s = s.Remove(0, "{{commonscat|".Length);
                    }
                    else if (p.text.IndexOf("{{Commonscat|") > 0)
                    {
                        s = p.text.Remove(0, p.text.IndexOf("{{Commonscat|"));
                        s = s.Remove(s.IndexOf("}}"));
                        s = s.Remove(0, "{{Commonscat|".Length);
                    }

                    if (String.IsNullOrEmpty(s))
                        return newpix;

                    if (s.Contains("|"))
                        s = s.Remove(s.IndexOf("|"));


                    s = "Category:" + s;
                    //Console.WriteLine(s);
                    //Console.ReadLine();

                    PageList plc = new PageList(cmsite);
                    try
                    {
                        plc.FillFromCategory(s);
                    }
                    catch (WebException e8)
                    {
                        Console.WriteLine(e8.Message);
                        Thread.Sleep(10000);//milliseconds

                        //continue;
                    }


                    foreach (Page pc in plc)
                    {
                        Console.WriteLine("pc = " + pc.title);
                        newpix.Add(pc.title.Replace(" ", "_"), "cm");
                    }
                }
            }

            return newpix;
        }

        private List<string> get_newpix_API(Page p)
        {
            //https://image-suggestion-api.toolforge.org/image-suggestions/v0/wikipedia/ceb/pages?offset=10


            return new List<string>();
        }

        //private List<string> get_newpix(Page p, Dictionary<string,JObject> jdict)
        //{
        //    List<string> newpix = new List<string>();
        //    if (CB_suggestions.Checked)
        //    {
        //        newpix.AddRange(get_newpix_API(p));
        //    }
        //    if (CB_IW.Checked)
        //    {
        //        Dictionary<string, string> iwpix = get_newpix_IW(p);
        //        foreach (string s in iwpix.Keys)
        //            newpix.Add(s);
        //    }
        //    if (CB_pages.Checked)
        //    {
        //        foreach (JToken jt in jdict[p.title]["suggestions"])
        //        {
        //            string image = jt["filename"].ToString();
        //            if (image.StartsWith("File:"))
        //                image = image.Replace("File:", "");
        //            if (jt["confidence_rating"].ToString() != "low")
        //                newpix.Add(image);
        //        }
        //    }
        //    return newpix;
        //}

        private List<string> get_newpix(Page p, Dictionary<string, List<string>> picdict)
        {
            List<string> newpix = new List<string>();
            if (CB_suggestions.Checked)
            {
                newpix.AddRange(get_newpix_API(p));
            }
            if (CB_IW.Checked)
            {
                Dictionary<string, string> iwpix = get_newpix_IW(p);
                foreach (string s in iwpix.Keys)
                    newpix.Add(s);
            }
            if (CB_pages.Checked || CB_fromfile.Checked)
            {
                foreach (string jt in picdict[p.title])
                {
                    string image = jt.Split('\t')[0].Replace(" ", "_");
                    if (image.StartsWith("File:"))
                        image = image.Replace("File:", "");
                    if (newpix.Contains(image))
                        continue;
                    if (jt.Split('\t')[1] != "low")
                        newpix.Add(image);
                    else if (p.title.Contains(" ") && image.Contains(p.title.Replace(" ", "_")))
                        newpix.Add(image);
                }
            }
            return newpix;
        }

        private int get_nchoice()
        {
            ///////////////////////////////////////
            //Choose what to do with the pix that are found:
            // nchoice = 0: do nothing, except list on standard output
            // nchoice = 1: add as gallery in target article 
            // nchoice = 2: add as separate pix in target article
            // nchoice = 3: add in article discussion
            // nchoice = 4: list in separate workpage "Användare:Botaccount/Gallery"
            ////////////////////////////////////////
            if (RB_nothing.Checked)
                return 0;
            if (RB_gallery.Checked)
                return 1;
            if (RB_separate.Checked)
                return 2;
            if (RB_talk.Checked)
                return 3;
            if (RB_gallerylog.Checked)
                return 4;
            return 0;
        }

        private int add_pix_to_page(Page p, List<string> newpix, string makelang, List<string> blacklist, List<string> blacktype) //returns true if pix added
        {
            if (newpix.Count == 0)
                return 0;
            if (!util.tryload(p, 1))
                return 0;
            if (!p.Exists())
                return 0;

            string origtext = p.text;

            //find images already in page:

            List<string> oldpix = p.GetImages();
            List<string> oldpix2 = GetImagesInTemplates(svsite, p);
            //Console.WriteLine("Oldpix:");
            int npix = 0;
            foreach (string oldpic in oldpix)
            {
                //Console.WriteLine(oldpic);
                npix++;
            }
            foreach (string pic in oldpix2)
                npix++;
            Console.WriteLine("npix = " + npix.ToString());
            if (CB_skipillustrated.Checked)
            {
                if (npix > 0)
                    return 0;
                if (p.text.Contains(".jp"))
                    return 0;
                if (p.text.Contains(".gif"))
                    return 0;
                if (p.text.Contains(".JP"))
                    return 0;
                if (p.text.Contains(".GIF"))
                    return 0;
                if (p.text.Contains(".png"))
                    return 0;
                if (p.text.Contains(".PNG"))
                    return 0;
            }

            //if it already has a gallery, skip it:
            if (p.text.Contains("<gallery>"))
                return 0;

            //if it doesn't contain "Lsjbot", skip it:
            //if (!p.text.Contains("Lsjbot"))
            //    continue;


            List<string> usepix = new List<string>();

            foreach (string newpic in newpix)
            {

                //Check if pix from iw is already used in target article:
                if (p.text.Contains(newpic))
                    continue;//newpix[newpic] = "/// ALREADY USED";
                else if (newpic.Contains(":"))
                {
                    if (p.text.Contains(newpic.Remove(0, newpic.IndexOf(':') + 1)))
                        continue;// newpix[newpic] = "/// ALREADY USED";

                }

                if ((!newpic.Contains(".")) || (newpic.LastIndexOf('.') < newpic.Length - 5))
                {
                    continue;// newpix[newpic] = "/// NOT A FILE";

                }

                //Check if pic in blacklist:
                if (blacklist.Contains(newpic))
                    continue;// newpix[newpic] = "/// BLACKLISTED IMAGE";

                foreach (string filetype in blacktype)
                {
                    if (newpic.Contains(filetype))
                        continue;// newpix[newpic] = "/// BLACKLISTED FILETYPE";
                }

                usepix.Add(newpic);


                //Check if pic really exists on Commons:

                //if (!fromcommons)
                //{

                //    string res = cmsite.indexPath + "?title=" +
                //                        WebUtility.UrlEncode("File:" + newpic);
                //    //Console.WriteLine("commonsres = " + res);
                //    string src = "";
                //    try
                //    {
                //        src = cmsite.GetWebPage(res); // cmsite.GetPageHTM(res);
                //    }
                //    catch (WebException e10)
                //    {
                //        newpix[newpic] = "/// NOT FOUND ON COMMONS";
                //        string message = e10.Message;
                //        if (message.Contains(": (404) "))
                //        {       // Not Found
                //            Console.Error.WriteLine(Bot.Msg("Page \"{0}\" doesn't exist."), newpic);
                //            Console.WriteLine("Image not found " + newpic);
                //            continue;
                //        }
                //        else
                //        {
                //            Console.Error.WriteLine(message);
                //            continue;
                //        }
                //    }
                //}
            }

            int nnew = usepix.Count;

            Console.WriteLine("# new pix = " + nnew.ToString());



            if (nnew == 0)
                return 0;

            //OK, so we found some pix. Now what do we do with them?


            ////Then figure out which new pix have the most interwiki use:
            //List<string> pixtouse = new List<string>();
            //if ((ntop > 0) && (ntop < nnew))
            //{


            //    int nused = 0;
            //    while (nused < ntop)
            //    {
            //        string longest = "";
            //        int maxlength = -1;
            //        foreach (string newpic in dummykeys)
            //        {
            //            if (newpix[newpic].Length > maxlength)
            //            {
            //                longest = newpic;
            //                maxlength = newpix[newpic].Length;
            //            }
            //        }
            //        pixtouse.Add(longest);
            //        newpix[longest] = "";
            //        nused++;
            //    }
            //}
            //else
            //    foreach (string newpic in newpix.Keys)
            //        if (newpix[newpic] != "")
            //            pixtouse.Add(newpic);

            //Then actually use them, according to nchoice value:

            string gallerylabel = "Bildgalleri";
            string talkpage = "Diskussion";
            string disktext = "\n\n==Bilder från interwiki==\nBoten " + botaccount + " har identifierat följande bilder som används på andra språkversioner av den här artikeln:\n\n";
            string disksig = "~~~~";

            switch (makelang)
            {
                case "sv":
                    gallerylabel = "Bildgalleri";
                    talkpage = "Diskussion";
                    disktext = "\n\n==Bilder från interwiki==\nBoten " + botaccount + " har identifierat följande bilder som används på andra språkversioner av den här artikeln:\n\n";
                    break;
                case "ceb":
                    gallerylabel = "Galeriya sa hulagway";
                    talkpage = "Hisgot";
                    break;
                case "war":
                    gallerylabel = "Image gallery";
                    talkpage = "Hiruhimangraw";
                    break;
                case "it":
                    gallerylabel = "Galleria di immagini";
                    talkpage = "Discussione";
                    disktext = "== Suggerimento di immagini ==\n{{Suggerimento immagini}}";
                    disksig = "Cordiali saluti, ~~~~";
                    //logpage = "Utente:Lsjbot/imagelog";
                    break;
                case "nl":
                    gallerylabel = "Galleria di immagini";
                    talkpage = "Discussione";
                    disktext = "== Immagine suggerimento ==\n{{Immaginesuggerimento2015}}";
                    disksig = " -- ~~~~";
                    //logpage = "Utente:Lsjbot/imagelog";
                    break;
                default:
                    gallerylabel = "Image gallery";
                    break;
            }

            string gallery = "\n\n== " + gallerylabel + " ==\n\n<gallery>\n";

            int nchoice = get_nchoice();
            switch (nchoice)
            {
                case 1:
                    foreach (string newpic in usepix)
                        gallery = gallery + newpic + "\n";
                    gallery = gallery + "</gallery>\n\n";

                    int ipos = p.text.IndexOf("[[Kategori");
                    if ((ipos < 0) && (makelang == "war"))
                        ipos = p.text.IndexOf("[[Kaarangay");

                    string botendtext = "== Källor ==";
                    if (p.text.Contains(botendtext))
                    {
                        ipos = p.text.IndexOf(botendtext);
                    }
                    if (ipos > 0)
                        p.text = p.text.Insert(ipos, gallery);
                    else
                        p.text += gallery;
                    break;
                case 2:
                    foreach (string newpic in usepix)
                        p.text = p.text.Replace("[[Kategori", "[[Fil:" + newpic + "|thumb|right|]]\n\n" + "[[Kategori");
                    break;
                case 3:

                    Page pdisk = new Page(svsite, talkpage + ":" + p.title);
                    if (!util.tryload(pdisk, 2))
                        return 0;
                    //Skip if already processed by the bot:
                    if (pdisk.text.Contains(disktext) || pdisk.text.Contains(botaccount))
                        return 0;

                    if (!String.IsNullOrEmpty(pdisk.text))
                        pdisk.text += "\n\n";
                    pdisk.text = pdisk.text + disktext;
                    gallery = gallery.Replace("\n== " + gallerylabel + " ==\n\n", "");//"=== " + gallerylabel + " ===");
                    foreach (string newpic in usepix)
                        gallery = gallery + newpic + "\n";
                    gallery = gallery + "</gallery>\n" + disksig + "\n";
                    pdisk.text = pdisk.text + gallery;
                    //Bot.editComment = "Fixar bildförslag från iw";
                    //isMinorEdit = false;
                    util.trysave(pdisk, 2, svsite);
                    p.text = "";
                    try
                    {
                        p.text = "";
                        p.Watch();
                    }
                    catch (WebException exx)
                    {
                        string message = exx.Message;
                        Console.Error.WriteLine(message);
                        Thread.Sleep(10000);//milliseconds
                    }

                    //Thread.Sleep(55000);//milliseconds
                    //Console.WriteLine("<ret>");
                    //Console.ReadLine();
                    break;
                //case 4:
                //    pwork.text = pwork.text + "===" + p.title + "===\n";
                //    foreach (string newpic in pixtouse)
                //        gallery = gallery + newpic + "\n";
                //    gallery = gallery + "</gallery>\n\n";
                //    pwork.text = pwork.text + gallery;
                //    break;
            }
            //DONE!  Now save if needed.


            //Bot.editComment = editcomment;
            //isMinorEdit = false;
            if ((nchoice == 1) || (nchoice == 2))
            {
                int ntry = 0;
                if (p.text != origtext)
                    util.trysave(p, 3, svsite);
            }
            //if (nchoice == 4)
            //    util.trysave(pwork, 3, svsite);
            //Thread.Sleep(4000);//milliseconds
            //Console.WriteLine("nexttime = "+nexttime.ToLongTimeString());
            //Console.WriteLine("Now = " + DateTime.Now.ToLongTimeString());
            //while (DateTime.Now.CompareTo(nexttime) < 0)
            //    continue;
            //oldtime = DateTime.Now;

            //nedit++;

            return 1;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string makelang = "ceb";
            //string cattodo = "Persoon naar beroep";
            string cattodo = "Persone per nazionalità";
            string editcomment = "Images from API";
            string logpage = "Gumagamit:Lsjbot/imagelog";
            string resume_at = "";
            //string resume_at = "";
            //string botaccount = "Lsjbot";
            if (!RB_file.Checked && !RB_nothing.Checked)
            {
                svsite = util.login(makelang, botaccount);
                string password = "dummy";
                cmsite = new Site("https://commons.wikimedia.org", botaccount, util.password);
                //Site wdsite = new Site("http://wikidata.org", botaccount, password);
                svsite.defaultEditComment = editcomment;
                svsite.minorEditByDefault = false;
                Console.WriteLine("apipath = " + svsite.apiPath);
            }



            List<string> doneCats = new List<string>();
            doneCats.Add("Categoria:Persone del X secolo");
            doneCats.Add("Categoria:Persone dell'XI secolo");
            doneCats.Add("Categoria:Persone del XII secolo");
            doneCats.Add("Categoria:Persone del XIII secolo");
            doneCats.Add("Categoria:Persone del XIV secolo");
            doneCats.Add("Categoria:Persone del XV secolo");
            doneCats.Add("Categoria:Persone del XVI secolo");
            doneCats.Add("Categoria:Persone del XVII secolo");
            doneCats.Add("Categoria:Persone del XVIII secolo");
            doneCats.Add("Categoria:Persone del XIX secolo");
            doneCats.Add("Categoria:Persone del XX secolo");
            doneCats.Add("Persone per secolo");
            doneCats.Add("Italiani");
            doneCats.Add("Statunitensi");
            doneCats.Add("Svedesi");
            doneCats.Add("Tedeschi");
            doneCats.Add("Spagnoli");

            switch (makelang)
            {
                case "sv":
                    editcomment = "Fixar bilder från iw, Kategori:" + cattodo;
                    break;
                case "ceb":
                    editcomment = "Galeriya sa hulagway sa API";
                    break;
                case "nl":
                    editcomment = "Fotogalerij van interwiki, Categorie:" + cattodo;
                    break;
                case "it":
                    editcomment = "Galleria di immagini da interwiki, Categoria:" + cattodo;
                    break;
                default:
                    editcomment = "Image gallery from interwiki";
                    break;
            }





            //Skip images in blacklist:
            List<string> blacklist = new List<string>();
            List<string> vetocatlist = new List<string>();

            bool blackwrite = false;
            bool blackread = true;
            string blackfile = @"I:\dotnwb3\blacklist.txt";

            if (blackread)
            {
                int nblack = 0;
                using (StreamReader sr = new StreamReader(blackfile))
                {
                    while (!sr.EndOfStream)
                    {
                        string s = sr.ReadLine();
                        blacklist.Add(s);
                        nblack++;
                    }
                }
                memo("nblack=" + nblack.ToString());

            }
            else if (!RB_file.Checked && !RB_nothing.Checked)
            {
                vetocatlist.Add("Image placeholders");
                vetocatlist.Add("Icons by subject");
                vetocatlist.Add("Logos of Eurovision");
                vetocatlist.Add("Flags by country");
                vetocatlist.Add("Audio files");
                //vetocatlist.Add("");

                foreach (string vc in vetocatlist)
                {
                    PageList pldummy = new PageList(cmsite);
                    bool loaded = false;
                    do
                    {
                        try
                        {
                            pldummy.FillFromCategoryTree(vc);
                            loaded = true;
                        }
                        catch (WebException ee)
                        {
                            string message = ee.Message;
                            Console.Error.WriteLine(message);
                        }
                    }
                    while (!loaded);
                    foreach (Page pd in pldummy)
                    {
                        //Console.WriteLine(pd.title);
                        blacklist.Add(pd.title.Replace("File:", "").Replace(" ", "_"));
                    }
                    pldummy.Clear();
                }
                //Console.ReadLine();

                if (blackwrite)
                {
                    using (StreamWriter sw = new StreamWriter("blacklist.txt"))
                    {

                        foreach (string s in blacklist)
                        {
                            sw.WriteLine(s);
                        }
                    }
                }
            }


            //Skip pages in watchlist:
            //svsite.watchList = new PageList(svsite);
            //svsite.watchList.FillFromWatchList();
            //Console.WriteLine("Watchlist pages: " + svsite.watchList.Count());

            List<string> blacktype = new List<string>();
            //blacktype.Add(".svg");
            //blacktype.Add(".png");

            PageList pl = null;
            if (!RB_file.Checked && !RB_nothing.Checked)
                pl = new PageList(svsite);

            ////////////////////////////////////
            //Select how to get pages. Uncomment as needed.
            ////////////////////////////////////

            //Find articles from a category
            //bool loaded2 = false;
            //do
            //{
            //    try
            //    {
            //        util.FillAllFromCategoryTreeExceptDone(cattodo, svsite, pl, doneCats);
            //        loaded2 = true;
            //    }
            //    catch (WebException eee)
            //    {
            //        string message = eee.Message;
            //        Console.Error.WriteLine(message);
            //    }
            //}
            //while (!loaded2);

            //Find articles from all the links to a template, mostly useful on very small wikis
            //        pl.FillFromLinksToPage("Mall:Taxobox");

            //Set specific article:
            //Page ppp = new Page(svsite, "Dina Tersago");pl.Add(ppp);

            Dictionary<string, JObject> jsondict = new Dictionary<string, JObject>();
            Dictionary<string, int> picstats = new Dictionary<string, int>();
            Dictionary<string, List<string>> picdict = new Dictionary<string, List<string>>();
            hbookclass confhist = new hbookclass("Image confidence");
            hbookclass maxconfhist = new hbookclass("Image confidence");

            string folder = @"I:\dotnwb3\";

            if (CB_fromfile.Checked)
            {
                openFileDialog1.InitialDirectory = folder;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fn = openFileDialog1.FileName;
                    using (StreamReader sr = new StreamReader(fn))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (String.IsNullOrEmpty(line))
                                continue;
                            if (line.StartsWith("##"))
                                continue;
                            string[] words = line.Split('\t');
                            string tit = words[0];
                            if (!picdict.ContainsKey(tit))
                                picdict.Add(tit, new List<string>());
                            for (int i=2;i<words.Length;i+=2)
                            {
                                string pic = words[i - 1] + "\t" + words[i];
                                picdict[tit].Add(pic);
                            }
                            if (picdict[tit].Count > 0)
                                pl.Add(new Page(svsite, tit));
                        }
                    }
                }
                memo("From file: " + pl.Count());
            }
            else if (CB_pages.Checked)
            {
                //Get from image suggestions API
                int offset = util.tryconvert(TB_offset.Text);
                if (offset < 0)
                    offset = 0;
                int limit = 500;
                int loops = util.tryconvert(TB_loops.Text); ;
                if (offset <= 0)
                    offset = 1;
                int badjson = 0;
                string fn = util.unusedfilename(folder + "imagesuggestions.txt");
                string urlbase = @"https://image-suggestion-api.wmcloud.org/image-suggestions/v0/wikipedia/ceb/pages?seed=5678&limit="+limit+"&offset=";
                //string urlbase = @"https://image-suggestion-api.toolforge.org/image-suggestions/v0/wikipedia/ceb/pages?offset=";

                using (StreamWriter sw = new StreamWriter(fn))
                {
                    DateTime starttime = DateTime.Now;
                    for (int i = 0; i < loops; i++)
                    {
                        memo("loop " + i);
                        string url = urlbase + offset.ToString();
                        string jsonresult = util.get_webpage(url);
                        //memo(jsonresult);

                        //memo(jsonresult);
                        if (String.IsNullOrEmpty(jsonresult))
                        {
                            memo("empty json!");
                            offset++;
                            badjson++;
                            if (badjson > 10)
                                break;
                            continue;
                        }
                        JObject json = JObject.Parse(jsonresult);

                        //List<JObject> jsonlist = JsonConvert.DeserializeObject<List<JObject>>(jsonresult);
                        List<JObject> jsonlist = new List<JObject>();


                        foreach (JObject jj in json["pages"])
                        {
                            string tit = jj["page"].ToString().Replace("_", " ");
                            //memo(tit);
                            if (picdict.ContainsKey(tit))
                                continue;
                            StringBuilder sb = new StringBuilder(tit);

                            if (!RB_file.Checked)
                            {
                                Page pp = new Page(svsite, tit);
                                pl.Add(pp);
                                jsondict.Add(tit, jj);
                            }
                            picdict.Add(tit, new List<string>());

                            string maxconf = "(none)";
                            foreach (JToken jt in jj["suggestions"])
                            {
                                string image = jt["filename"].ToString();
                                string conf = jt["confidence_rating"].ToString();
                                confhist.Add(conf);
                                if (conf == "high")
                                    maxconf = conf;
                                else if (conf == "medium")
                                {
                                    if (maxconf != "high")
                                        maxconf = conf;
                                }
                                else if (conf == "low")
                                {
                                    if (maxconf == "(none)")
                                        maxconf = conf;
                                }
                                //memo("\t" + image + "\t" + conf);
                                picdict[tit].Add(image + "\t" + conf);
                                if (RB_file.Checked)
                                    sb.Append("\t" + image + "\t" + conf);
                                if (!picstats.ContainsKey(image))
                                    picstats.Add(image, 1);
                                else
                                    picstats[image]++;
                            }
                            maxconfhist.Add(maxconf);
                            if (RB_file.Checked)
                                sw.WriteLine(sb.ToString());
                        }
                        offset += limit;
                    }
                    memo(confhist.GetSHist());
                    memo(maxconfhist.GetSHist());
                    memo("Running time per loop = " + (DateTime.Now - starttime).TotalSeconds / loops);
                    sw.WriteLine("##OFFSET##\t" + offset);
                }
                if (RB_file.Checked)
                {
                    memo("Picstats==========================");
                    foreach (string ps in picstats.Keys)
                        if (picstats[ps] > 10)
                            memo(ps + "\t" + picstats[ps]);
                    return;
                }
            }
            //Skip all namespaces except regular articles:
            //pl.RemoveNamespaces(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 100, 101 });

            ///////////////////////////////////////
            //Choose what to do with the pix that are found:
            // nchoice = 0: do nothing, except list on standard output
            // nchoice = 1: add as gallery in target article 
            // nchoice = 2: add as separate pix in target article
            // nchoice = 3: add in article discussion
            // nchoice = 4: list in separate workpage "Användare:Botaccount/Gallery"
            ////////////////////////////////////////
            int nchoice = get_nchoice();

            // If ntop is non-zero, don't use all pix but only the ntop most used ones.
            //int ntop = 20;

            // Skip pictures with size smaller than minsize.
            //int minsize = 50;

            //Skip articles that already have at least one pic:
            //bool skipillustrated = true;

            Page pwork = new Page(svsite, "Användare:" + botaccount + "/Gallery");
            if (nchoice == 4)
            {
                pwork.Load();
            }

            string sbrack = "[]'† ?";
            char[] brackets = sbrack.ToCharArray();

            //int nfound = 0;

            DateTime oldtime = DateTime.Now;

            int nedit = 0;

            int iremain = pl.Count();

            //Console.ReadLine();

            foreach (Page p in pl)
            {

                iremain--;
                memo(iremain.ToString() + " remaining.");

                //DateTime nexttime = oldtime.AddSeconds(7);
                //Skip start of alphabet:
                //if (String.Compare(p.title,"Acacia tortilis") < 0 )
                //    continue;

                //skip until specific article
                if (resume_at != "")
                {
                    if (resume_at == p.title)
                        resume_at = "";
                    else
                        continue;
                }

                //Skip pages in watchlist
                if (p.watched)
                {
                    Console.WriteLine("Skip watched");
                    continue;
                }

                //if (svsite.watchList.Contains(p))
                //{
                //    Console.WriteLine("Skip page in watchlist");
                //    continue;
                //}

                List<string> newpix = get_newpix(p, picdict);// jsondict);

                nedit += add_pix_to_page(p, newpix, makelang,blacklist,blacktype);


            }

            memo("Total #edits = " + nedit.ToString());
            Page plog = new Page(svsite, logpage);
            util.tryload(plog, 2);
            plog.text += "\n# Category:" + cattodo + "; Total # pages = " + pl.Count().ToString() + "; Total #edits = " + nedit.ToString() + "\n";
            util.trysave(plog, 2,svsite);
        }
    }
}
