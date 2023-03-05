using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Threading;
using System.IO;
using DotNetWikiBot;
using System.Xml;
using System.Text.RegularExpressions;


namespace Wikifix
{
    class util
    {
        public static DateTime oldtime = DateTime.Now;
        public static int pausetime = 6; //time between saves, modified depending on task
        public static bool pauseaftersave = true;
        public static List<string> neglectlist = new List<string>() { "CommonsDelinker" };
        public static string password = null;

        public static Site login(string makelang)
        {
            return login(makelang, "Lsjbot");
        }

        public static Site login(string makelang,string account)
        {
            //string makelang = "en";
            //Console.Write("Password: ");
            if (String.IsNullOrEmpty(password))
                password = util.get_password();
            string botkonto = account;
            try
            {
                Site site = new Site("https://" + makelang + ".wikipedia.org", botkonto, password);
                site.defaultEditComment = "Fixing mistake";
                site.minorEditByDefault = true;
                //MakeSpecies.loggedin = true;
                return site;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static string increment_cat(string cat)
        {
            string rex = @" (\d{4})-(\d{2})";

            foreach (Match m in Regex.Matches(cat,rex))
            {
                int month = tryconvert(m.Groups[2].Value);
                int year = tryconvert(m.Groups[1].Value);
                if (month > 0)
                {
                    month++;
                    if (month > 12)
                    {
                        month = 1;
                        year++;
                    }
                    return cat.Replace(m.Value, " " + year.ToString() + "-" + month.ToString("D2"));
                }
            }
            return cat;
        }

        public static List<string> list_editors(Page p, Site site)
        {
            List<string> ls = new List<string>();
            PageList plh = new PageList(site);
            plh.FillFromPageHistory(p.title, 10);
            foreach (Page ph in plh)
            {
                if (!util.tryload(ph, 2))
                    continue;
                ls.Add(ph.lastUser);
            }
            return ls;
        }

        public static bool human_touched(Page p,Site site)
        {
            PageList plh = new PageList(site);
            plh.FillFromPageHistory(p.title, 10);
            foreach (Page ph in plh)
            {
                if (!util.tryload(ph, 2))
                    return true;
                //memo("  " + ph.lastUser);
                if (ph.lastUser.ToLower().Contains("bot"))
                    continue;
                if (util.get_alphabet(ph.lastUser) == "none")
                    continue;
                if (neglectlist.Contains(ph.lastUser))
                    continue;
                if (ph.comment.Contains("GlobalReplace"))
                    continue;
                return true;
            }
            return false;

        }

        public static Dictionary<string, string> trivdict = new Dictionary<string, string>();
        public static string dyntaxafolder = "I:\\dotnwb3\\";

        public static void FillAllFromCategoryTreeExceptDone(string categoryName, Site site, PageList pl, List<string> doneCats)
        {
            pl.Clear();
            categoryName = site.CorrectNsPrefix(categoryName);
            //List<string> doneCats = new List<string>();
            Console.WriteLine("doneCats " + doneCats.Count.ToString());
            pl.FillAllFromCategory(categoryName);
            doneCats.Add(categoryName);
            for (int i = 0; i < pl.Count(); i++)
                if (pl.pages[i].GetNamespace() == 14 && !doneCats.Contains(pl.pages[i].title))
                {
                    //Console.WriteLine(pl.pages[i].title);
                    pl.FillAllFromCategory(pl.pages[i].title);
                    doneCats.Add(pl.pages[i].title);
                }
            pl.RemoveRecurring();
        }


        public static void read_trivname()
        {

            using (StreamReader sr = new StreamReader(dyntaxafolder + "trivname_ceb.csv"))
            {
                int n = 0;


                String headline = "";
                headline = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    //memo(line);

                    string[] words = line.Split(';');
                    for (int jj = 1; jj < words.Length; jj++)
                    {
                        words[jj] = words[jj].Trim();
                        if (words[jj].Length < 2)
                            words[jj] = "";
                    }

                    if (words.Length >= 2)
                    {
                        string taxon = words[0];
                        if (String.IsNullOrEmpty(taxon))
                            continue;
                        string trivname = words[1];
                        if (String.IsNullOrEmpty(trivname))
                            continue;
                        if (trivdict.ContainsKey(trivname))
                            continue;
                        trivdict.Add(trivname, taxon);
                        n++;
                    }

                }

                //memo("n (trivname) = " + n.ToString());
                //memo("<return>");
                //Console.ReadLine();

            }
        }




        public static string get_webpage(string url)
        {
            WebClient client = new WebClient();

            // Add a user agent header in case the
            // requested URI contains a query.

            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            try
            {
                Stream data = client.OpenRead(url);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                return s;
            }
            catch (WebException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
            }
            return "";
        }

        public static int tryconvert(string word)
        {
            int i = -1;

            if (word.Length == 0)
                return i;

            try
            {
                i = Convert.ToInt32(word);
            }
            catch (OverflowException)
            {
                Console.WriteLine("i Outside the range of the Int32 type: " + word);
            }
            catch (FormatException)
            {
                //if ( !String.IsNullOrEmpty(word))
                //    Console.WriteLine("i Not in a recognizable format: " + word);
            }

            return i;

        }



        public static int get_wdid_by_name(string sites, string titles)
        {
            string url3 = "https://www.wikidata.org/w/api.php?action=wbgetentities&format=xml&sites=" + sites + "&titles=" + titles + "&redirects=yes&props=claims";

            //Console.WriteLine(url3);

            string xmlitem3 = get_webpage(url3);

            if (!String.IsNullOrEmpty(xmlitem3))
            {
                XmlDocument cx = new XmlDocument();
                cx.LoadXml(xmlitem3);

                XmlNodeList elemlist = cx.GetElementsByTagName("entity");
                foreach (XmlNode ee in elemlist)
                {
                    try
                    {
                        int entid = util.tryconvert(ee.Attributes.GetNamedItem("id").Value.Replace("Q", ""));
                        //Console.WriteLine("entid = " + entid.ToString());
                        if (entid > 0)
                        {
                            return entid;
                        }

                    }
                    catch (NullReferenceException e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }

            }
            return -1;
        }


        public static XmlNode get_property_node(int propid, XmlDocument cx)
        {
            return get_node_by_tag(propid, cx, "property");
        }


        public static XmlNode get_node_by_tag(int propid, XmlDocument cx, string tag)
        {
            XmlNodeList elemlist = cx.GetElementsByTagName(tag);
            //Console.WriteLine("get_property_node: elemlist: " + elemlist.Count.ToString());
            foreach (XmlNode ee in elemlist)
            {
                try
                {
                    string id = ee.Attributes.GetNamedItem("property").Value;
                    //Console.WriteLine("property = " + id);
                    if (id == "P" + propid.ToString())
                    {
                        //Console.WriteLine("get_property_node: found!");
                        return ee;
                    }
                }
                catch (NullReferenceException e)
                {
                    //Console.Error.WriteLine(e.Message);
                }
                try
                {
                    string id = ee.Attributes.GetNamedItem("id").Value;
                    //Console.WriteLine("id = " + id);
                    if (id == "P" + propid.ToString())
                    {
                        //Console.WriteLine("get_property_node: found!");
                        return ee;
                    }
                }
                catch (NullReferenceException e)
                {
                    //Console.Error.WriteLine(e.Message);
                }
            }

            //Console.WriteLine("get_property_node: not found");
            return null;

        }



        public static DateTime get_wd_time(int propid, int wdid)
        {
            //https://www.wikidata.org/w/api.php?action=wbgetclaims&entity=Q762&property=P569&format=xml
            string url3 = "https://www.wikidata.org/w/api.php?action=wbgetclaims&format=xml&entity=Q" + wdid.ToString() + "&property=P" + propid.ToString();

            //Console.WriteLine(url3);

            string xmlitem3 = get_webpage(url3);

            DateTime rt = new DateTime(9999, 1, 1);
            //Console.WriteLine("seconds = " + rt.Second);

            //Console.WriteLine(xmlitem3);

            if (!String.IsNullOrEmpty(xmlitem3))
            {
                XmlDocument cx = new XmlDocument();
                cx.LoadXml(xmlitem3);

                XmlNode propnode = get_node_by_tag(propid, cx, "mainsnak");
                if (propnode == null)
                {
                    Console.WriteLine("propnode = null");
                    return rt;
                }

                //Console.WriteLine("propnode = " + propnode.ToString());

                XmlDocument propx = new XmlDocument();
                propx.AppendChild(propx.ImportNode(propnode, true));

                XmlNodeList elemlist = propx.GetElementsByTagName("value");
                string rs = "";
                string precision = "";
                foreach (XmlNode ee in elemlist)
                {
                    try
                    {
                        rs = ee.Attributes.GetNamedItem("time").Value;
                        precision = ee.Attributes.GetNamedItem("precision").Value;
                    }
                    catch (NullReferenceException e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }

                //Console.WriteLine("get_wd_year:rs: " + rs);
                if (String.IsNullOrEmpty(rs))
                    return rt;

                bool bc = (rs[0] == '-');
                //Console.WriteLine("bc = " + bc);

                rs = rs.Remove(0, 1);
                if (rs.Contains("-00-00"))
                    rs = rs.Replace("-00-00", "-01-01");

                try
                {
                    rt = DateTime.Parse(rs);
                    if (bc)
                        rt = rt.AddYears(BCoffset);

                    double iprec = tryconvert(precision);
                    //Console.WriteLine("iprec =" + iprec);
                    if (iprec < 0)
                        iprec = 0;
                    //Console.WriteLine("precision =" + precision);
                    rt = rt.AddSeconds(iprec);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            //Console.WriteLine("seconds = " + rt.Second);
            return rt;
        }

        public static string[] months = new string[13]
        {
            "wala mailhi nga petsa",
            "Enero",
            "Pebrero",
            "Marso",
            "Abril",
            "Mayo",
            "Hunyo",
            "Hulyo",
            "Agosto",
            "Septiyembre",
            "Oktubre",
            "Nobiyembre",
            "Disyembre"
        };


    public static string printdate(DateTime rt)
        {
            string s = "";

            //Console.WriteLine("seconds in printdate= " + rt.Second);

            if (rt.Second < 11) //worse than day precision
                s = months[0];
            else if (rt.Month > 0)
            {
                s = months[rt.Month] + " " + rt.Day.ToString();
            }
            else
                s = months[0];

            return s;
        }

        public static int BCoffset = 3000;
        public static string BClabel = "WK";

        public static string printyear(DateTime rt)
        {
            int i = rt.Year;
            string s = "";

            if (i == 9999)
                return "wala mailhi nga tuig";

            if (i > BCoffset)
            {
                i -= BCoffset;
                s = i.ToString() + " " + BClabel;
            }
            else
                s = i.ToString();

            if (rt.Second < 7)
                s = "wala mailhi nga tuig";
            else if (rt.Second == 7)
                s = "siglo " + s;
            else if (rt.Second == 8)
                s = "dekada " + s;

            return s;
        }

        public static Dictionary<string, string> get_wd_sitelinks(XmlDocument cx)
        {
            Dictionary<string, string> rd = new Dictionary<string, string>();

            XmlNodeList elemlist = cx.GetElementsByTagName("sitelink");
            foreach (XmlNode ee in elemlist)
            {
                try
                {
                    string lang = ee.Attributes.GetNamedItem("site").Value;
                    string value = ee.Attributes.GetNamedItem("title").Value;
                    //Console.WriteLine("get_wd_sitelinks: lang,value : " + lang + " " + value);
                    if (!rd.ContainsKey(lang))
                    {
                        rd.Add(lang, value);
                    }
                }
                catch (NullReferenceException e)
                {
                    //Console.WriteLine(e.Message);
                }
            }

            return rd;
        }



        public static string get_wd_prop(int propid, XmlDocument cx)
        {
            XmlNode propnode = get_property_node(propid, cx);
            if (propnode == null)
                return "";

            //Console.WriteLine("propnode = " + propnode.ToString());

            XmlDocument propx = new XmlDocument();
            propx.AppendChild(propx.ImportNode(propnode, true));

            XmlNodeList elemlist = propx.GetElementsByTagName("datavalue");
            string rs = "";
            foreach (XmlNode ee in elemlist)
            {
                try
                {
                    rs = ee.Attributes.GetNamedItem("value").Value;
                }
                catch (NullReferenceException e)
                {
                    //Console.Error.WriteLine(e.Message);
                }
            }

            //Console.WriteLine("get_wd_prop:rs: " + rs);
            return rs;

        }

        public static XmlDocument get_wd_xml(int wdid)
        {
            string url = "https://www.wikidata.org/w/api.php?action=wbgetentities&format=xml&ids=Q" + wdid.ToString() + "&redirects=yes";
            XmlDocument xd = new XmlDocument();
            string s = get_webpage(url);
            if (String.IsNullOrEmpty(s))
                return null;
            //Console.WriteLine(s);
            try
            {
                xd.LoadXml(s);
            }
            catch (XmlException e)
            {
                string message = e.Message;
                Console.Error.WriteLine("tl we " + message);
                return null;
            }

            return xd;
        }



        public static string fixcase(string ss)
        {
            string s = String.Copy(ss);
            for (int i = 1; i < s.Length; i++)
            {
                if ((s[i - 1] != ' ') && (s[i - 1] != '.'))
                {
                    s = s.Remove(i, 1).Insert(i, Char.ToLower(s[i]).ToString());
                }
            }
            return s;
        }

        public static string get_password()
        {
            InputBox ib = new InputBox("Password:");
            ib.ShowDialog();
            return ib.gettext();
        }


        public static void make_redirect(Site site, string frompage, string topage, string cat)
        {
            make_redirect(site, frompage, topage, cat, true);
        }

        public static void make_redirect(Site site, string frompage, string topage, string cat, bool reallymake)
        {
            Page pred = new Page(site, frompage);
            if (tryload(pred, 1))
            {
                if (!pred.Exists())
                {
                    pred.text = "#REDIRECT " + " [[" + topage + "]]";
                    if (!String.IsNullOrEmpty(cat))
                        pred.AddToCategory(cat);
                    util.trysave(pred, 2, "redirect");
                }

            }

        }

        public static string initialcap(string orig)
        {
            if (String.IsNullOrEmpty(orig))
                return "";

            int initialpos = 0;
            if (orig.IndexOf("[[") == 0)
            {
                if ((orig.IndexOf('|') > 0) && (orig.IndexOf('|') < orig.IndexOf(']')))
                    initialpos = orig.IndexOf('|') + 1;
                else
                    initialpos = 2;
            }
            string s = orig.Substring(initialpos, 1);
            s = s.ToUpper();
            string final = orig;
            final = final.Remove(initialpos, 1).Insert(initialpos, s);
            //s += orig.Remove(0, 1);
            return final;
        }



        public static bool tryload(Page p, int iattempt)
        {
            int itry = 1;


            while (true)
            {

                try
                {
                    p.Load();
                    return true;
                }
                catch (WebException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine(message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                }
            }

        }

        public static bool trysave(Page p, int iattempt, Site site)
        {
            return trysave(p, iattempt, site.defaultEditComment);
        }

        public static bool trysave(Page p, int iattempt, string editcomment)
        {
            return trysave(p, iattempt, editcomment, false, "");
        }

        public static bool trysave(Page p, int iattempt, string editcomment, bool savelocally, string savefolder)
        {
            int itry = 1;


            while (true)
            {

                try
                {
                    //Bot.editComment = mp(60);
                    if (!savelocally)
                    {
                        p.Save(editcomment, false);
                        DateTime newtime = DateTime.Now;
                        while (newtime < oldtime)
                        {
                            newtime = DateTime.Now;
                            Thread.Sleep(1000);
                        }
                        oldtime = newtime.AddSeconds(pausetime);
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(savefolder + p.title + ".txt"))
                        {
                            sw.WriteLine(p.text);
                        }
                    }


                    if (pauseaftersave)
                    {
                        //Console.WriteLine("<ret>");
                        //Console.ReadKey();
                    }
                    return true;
                }
                catch (WebException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts we " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }
                catch (WikiBotException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts wbe " + message);
                    if (message.Contains("Bad title"))
                        return false;
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }
                catch (IOException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts ioe " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }

            }

        }

        public static string unusedfilename(string fn0)
        {
            int n = 1;
            string fn = fn0;
            while (File.Exists(fn))
            {
                fn = fn0.Replace(".", n.ToString() + ".");
                n++;
            }
            return fn;
        }

        public static bool trymove(Page p, string newtitle, int iattempt, string editcomment)
        {
            int itry = 1;


            while (true)
            {

                try
                {
                    //Bot.editComment = mp(60);
                    p.RenameTo(newtitle,editcomment, true,false);
                    DateTime newtime = DateTime.Now;
                    while (newtime < oldtime)
                    {
                        newtime = DateTime.Now;
                        Thread.Sleep(1000);
                    }
                    oldtime = newtime.AddSeconds(pausetime);


                    if (pauseaftersave)
                    {
                        //Console.WriteLine("<ret>");
                        //Console.ReadKey();
                    }
                    return true;
                }
                catch (WebException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts we " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }
                catch (WikiBotException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts wbe " + message);
                    if (message.Contains("Bad title"))
                        return false;
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }
                catch (IOException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts ioe " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }

            }

        }

        public static List<string> findtag(string line)
        {
            string s = line;
            List<string> taglist = new List<string>();
            while (s.IndexOf('<') >= 0)
            {
                if (s.IndexOf('>') < s.IndexOf('<'))
                {
                    Console.WriteLine("Mismatched <> in " + line);
                    s = s.Substring(s.IndexOf('<') + 1);
                }
                else
                {
                    string tag = s.Substring(s.IndexOf('<'), s.IndexOf('>') - s.IndexOf('<') + 1);
                    //Console.WriteLine("Tag = " + tag);
                    taglist.Add(tag);
                    s = s.Substring(s.IndexOf('>') + 1);
                }

            }
            return taglist;
        }

        public static string fixtag(string line)
        {

            string s = line;

            s = s.Replace("<B>", "<b>");
            s = s.Replace("</B>", "</b>");
            s = s.Replace("<I>", "<i>");
            s = s.Replace("</I>", "</i>");

            if (s.Contains("<b>"))
            {
                if (s.Contains("</b>"))
                    s = s.Replace("<b>", "'''").Replace("</b>", "'''");
                else if (s.Contains("</i") && !s.Contains("<i>"))
                    s = s.Replace("<b>", "'''").Replace("</i>", "'''");
            }

            if (s.Contains("<i>"))
            {
                if (s.Contains("</i>"))
                    s = s.Replace("<i>", "''").Replace("</i>", "''");
                else if (s.Contains("</b") && !s.Contains("<b>"))
                    s = s.Replace("<i>", "''").Replace("</b>", "''");
            }

            s = s.Replace("<b>", "");
            s = s.Replace("</b>", "");
            s = s.Replace("<i>", "");
            s = s.Replace("</i>", "");


            return s;
        }

        public static bool is_latin(string name)
        {
            return (get_alphabet(name) == "latin");
        }

        public static string remove_disambig(string s)
        {
            if (s.Contains("("))
                return s.Substring(0, s.IndexOf("(")-1).Trim();
            else
                return s;
        }

        public static string get_alphabet(string name)
        {
            char[] letters = name.ToCharArray();
            //char[] letters = remove_disambig(name).ToCharArray();
            int n = 0;
            int sum = 0;
            //int nlatin = 0;
            Dictionary<string, int> alphdir = new Dictionary<string, int>();
            foreach (char c in letters)
            {
                int uc = Convert.ToInt32(c);
                sum += uc;
                string alphabet = "none";
                if (uc <= 0x0040) alphabet = "none";
                //else if ((uc >= 0x0030) && (uc <= 0x0039)) alphabet = "number";
                //else if ((uc >= 0x0020) && (uc <= 0x0040)) alphabet = "punctuation";
                else if ((uc >= 0x0041) && (uc <= 0x007F)) alphabet = "latin";
                else if ((uc >= 0x00A0) && (uc <= 0x00FF)) alphabet = "latin";
                else if ((uc >= 0x0100) && (uc <= 0x017F)) alphabet = "latin";
                else if ((uc >= 0x0180) && (uc <= 0x024F)) alphabet = "latin";
                else if ((uc >= 0x0250) && (uc <= 0x02AF)) alphabet = "phonetic";
                else if ((uc >= 0x02B0) && (uc <= 0x02FF)) alphabet = "spacing modifier letters";
                else if ((uc >= 0x0300) && (uc <= 0x036F)) alphabet = "combining diacritical marks";
                else if ((uc >= 0x0370) && (uc <= 0x03FF)) alphabet = "greek and coptic";
                else if ((uc >= 0x0400) && (uc <= 0x04FF)) alphabet = "cyrillic";
                else if ((uc >= 0x0500) && (uc <= 0x052F)) alphabet = "cyrillic";
                else if ((uc >= 0x0530) && (uc <= 0x058F)) alphabet = "armenian";
                else if ((uc >= 0x0590) && (uc <= 0x05FF)) alphabet = "hebrew";
                else if ((uc >= 0x0600) && (uc <= 0x06FF)) alphabet = "arabic";
                else if ((uc >= 0x0700) && (uc <= 0x074F)) alphabet = "syriac";
                else if ((uc >= 0x0780) && (uc <= 0x07BF)) alphabet = "thaana";
                else if ((uc >= 0x0900) && (uc <= 0x097F)) alphabet = "devanagari";
                else if ((uc >= 0x0980) && (uc <= 0x09FF)) alphabet = "bengali";
                else if ((uc >= 0x0A00) && (uc <= 0x0A7F)) alphabet = "gurmukhi";
                else if ((uc >= 0x0A80) && (uc <= 0x0AFF)) alphabet = "gujarati";
                else if ((uc >= 0x0B00) && (uc <= 0x0B7F)) alphabet = "oriya";
                else if ((uc >= 0x0B80) && (uc <= 0x0BFF)) alphabet = "tamil";
                else if ((uc >= 0x0C00) && (uc <= 0x0C7F)) alphabet = "telugu";
                else if ((uc >= 0x0C80) && (uc <= 0x0CFF)) alphabet = "kannada";
                else if ((uc >= 0x0D00) && (uc <= 0x0D7F)) alphabet = "malayalam";
                else if ((uc >= 0x0D80) && (uc <= 0x0DFF)) alphabet = "sinhala";
                else if ((uc >= 0x0E00) && (uc <= 0x0E7F)) alphabet = "thai";
                else if ((uc >= 0x0E80) && (uc <= 0x0EFF)) alphabet = "lao";
                else if ((uc >= 0x0F00) && (uc <= 0x0FFF)) alphabet = "tibetan";
                else if ((uc >= 0x1000) && (uc <= 0x109F)) alphabet = "myanmar";
                else if ((uc >= 0x10A0) && (uc <= 0x10FF)) alphabet = "georgian";
                else if ((uc >= 0x1100) && (uc <= 0x11FF)) alphabet = "korean";
                else if ((uc >= 0x1200) && (uc <= 0x137F)) alphabet = "ethiopic";
                else if ((uc >= 0x13A0) && (uc <= 0x13FF)) alphabet = "cherokee";
                else if ((uc >= 0x1400) && (uc <= 0x167F)) alphabet = "unified canadian aboriginal syllabics";
                else if ((uc >= 0x1680) && (uc <= 0x169F)) alphabet = "ogham";
                else if ((uc >= 0x16A0) && (uc <= 0x16FF)) alphabet = "runic";
                else if ((uc >= 0x1700) && (uc <= 0x171F)) alphabet = "tagalog";
                else if ((uc >= 0x1720) && (uc <= 0x173F)) alphabet = "hanunoo";
                else if ((uc >= 0x1740) && (uc <= 0x175F)) alphabet = "buhid";
                else if ((uc >= 0x1760) && (uc <= 0x177F)) alphabet = "tagbanwa";
                else if ((uc >= 0x1780) && (uc <= 0x17FF)) alphabet = "khmer";
                else if ((uc >= 0x1800) && (uc <= 0x18AF)) alphabet = "mongolian";
                else if ((uc >= 0x1900) && (uc <= 0x194F)) alphabet = "limbu";
                else if ((uc >= 0x1950) && (uc <= 0x197F)) alphabet = "tai le";
                else if ((uc >= 0x19E0) && (uc <= 0x19FF)) alphabet = "khmer";
                else if ((uc >= 0x1D00) && (uc <= 0x1D7F)) alphabet = "phonetic";
                else if ((uc >= 0x1E00) && (uc <= 0x1EFF)) alphabet = "latin";
                else if ((uc >= 0x1F00) && (uc <= 0x1FFF)) alphabet = "greek and coptic";
                else if ((uc >= 0x2000) && (uc <= 0x206F)) alphabet = "none";
                else if ((uc >= 0x2070) && (uc <= 0x209F)) alphabet = "none";
                else if ((uc >= 0x20A0) && (uc <= 0x20CF)) alphabet = "none";
                else if ((uc >= 0x20D0) && (uc <= 0x20FF)) alphabet = "combining diacritical marks for symbols";
                else if ((uc >= 0x2100) && (uc <= 0x214F)) alphabet = "letterlike symbols";
                else if ((uc >= 0x2150) && (uc <= 0x218F)) alphabet = "none";
                else if ((uc >= 0x2190) && (uc <= 0x21FF)) alphabet = "none";
                else if ((uc >= 0x2200) && (uc <= 0x22FF)) alphabet = "none";
                else if ((uc >= 0x2300) && (uc <= 0x23FF)) alphabet = "none";
                else if ((uc >= 0x2400) && (uc <= 0x243F)) alphabet = "none";
                else if ((uc >= 0x2440) && (uc <= 0x245F)) alphabet = "optical character recognition";
                else if ((uc >= 0x2460) && (uc <= 0x24FF)) alphabet = "enclosed alphanumerics";
                else if ((uc >= 0x2500) && (uc <= 0x257F)) alphabet = "none";
                else if ((uc >= 0x2580) && (uc <= 0x259F)) alphabet = "none";
                else if ((uc >= 0x25A0) && (uc <= 0x25FF)) alphabet = "none";
                else if ((uc >= 0x2600) && (uc <= 0x26FF)) alphabet = "none";
                else if ((uc >= 0x2700) && (uc <= 0x27BF)) alphabet = "none";
                else if ((uc >= 0x27C0) && (uc <= 0x27EF)) alphabet = "none";
                else if ((uc >= 0x27F0) && (uc <= 0x27FF)) alphabet = "none";
                else if ((uc >= 0x2800) && (uc <= 0x28FF)) alphabet = "braille";
                else if ((uc >= 0x2900) && (uc <= 0x297F)) alphabet = "none";
                else if ((uc >= 0x2980) && (uc <= 0x29FF)) alphabet = "none";
                else if ((uc >= 0x2A00) && (uc <= 0x2AFF)) alphabet = "none";
                else if ((uc >= 0x2B00) && (uc <= 0x2BFF)) alphabet = "none";
                else if ((uc >= 0x2E80) && (uc <= 0x2EFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2F00) && (uc <= 0x2FDF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2FF0) && (uc <= 0x2FFF)) alphabet = "none";
                else if ((uc >= 0x3000) && (uc <= 0x303F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3040) && (uc <= 0x309F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x30A0) && (uc <= 0x30FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3100) && (uc <= 0x312F)) alphabet = "bopomofo";
                else if ((uc >= 0x3130) && (uc <= 0x318F)) alphabet = "korean";
                else if ((uc >= 0x3190) && (uc <= 0x319F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x31A0) && (uc <= 0x31BF)) alphabet = "bopomofo";
                else if ((uc >= 0x31F0) && (uc <= 0x31FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3200) && (uc <= 0x32FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3300) && (uc <= 0x33FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3400) && (uc <= 0x4DBF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x4DC0) && (uc <= 0x4DFF)) alphabet = "none";
                else if ((uc >= 0x4E00) && (uc <= 0x9FFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xA000) && (uc <= 0xA48F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xA490) && (uc <= 0xA4CF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xAC00) && (uc <= 0xD7AF)) alphabet = "korean";
                else if ((uc >= 0xD800) && (uc <= 0xDB7F)) alphabet = "high surrogates";
                else if ((uc >= 0xDB80) && (uc <= 0xDBFF)) alphabet = "high private use surrogates";
                else if ((uc >= 0xDC00) && (uc <= 0xDFFF)) alphabet = "low surrogates";
                else if ((uc >= 0xE000) && (uc <= 0xF8FF)) alphabet = "private use area";
                else if ((uc >= 0xF900) && (uc <= 0xFAFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xFB00) && (uc <= 0xFB4F)) alphabet = "alphabetic presentation forms";
                else if ((uc >= 0xFB50) && (uc <= 0xFDFF)) alphabet = "arabic";
                else if ((uc >= 0xFE00) && (uc <= 0xFE0F)) alphabet = "variation selectors";
                else if ((uc >= 0xFE20) && (uc <= 0xFE2F)) alphabet = "combining half marks";
                else if ((uc >= 0xFE30) && (uc <= 0xFE4F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xFE50) && (uc <= 0xFE6F)) alphabet = "small form variants";
                else if ((uc >= 0xFE70) && (uc <= 0xFEFF)) alphabet = "arabic";
                else if ((uc >= 0xFF00) && (uc <= 0xFFEF)) alphabet = "halfwidth and fullwidth forms";
                else if ((uc >= 0xFFF0) && (uc <= 0xFFFF)) alphabet = "specials";
                else if ((uc >= 0x10000) && (uc <= 0x1007F)) alphabet = "linear b";
                else if ((uc >= 0x10080) && (uc <= 0x100FF)) alphabet = "linear b";
                else if ((uc >= 0x10100) && (uc <= 0x1013F)) alphabet = "aegean numbers";
                else if ((uc >= 0x10300) && (uc <= 0x1032F)) alphabet = "old italic";
                else if ((uc >= 0x10330) && (uc <= 0x1034F)) alphabet = "gothic";
                else if ((uc >= 0x10380) && (uc <= 0x1039F)) alphabet = "ugaritic";
                else if ((uc >= 0x10400) && (uc <= 0x1044F)) alphabet = "deseret";
                else if ((uc >= 0x10450) && (uc <= 0x1047F)) alphabet = "shavian";
                else if ((uc >= 0x10480) && (uc <= 0x104AF)) alphabet = "osmanya";
                else if ((uc >= 0x10800) && (uc <= 0x1083F)) alphabet = "cypriot syllabary";
                else if ((uc >= 0x1D000) && (uc <= 0x1D0FF)) alphabet = "byzantine musical symbols";
                else if ((uc >= 0x1D100) && (uc <= 0x1D1FF)) alphabet = "musical symbols";
                else if ((uc >= 0x1D300) && (uc <= 0x1D35F)) alphabet = "tai xuan jing symbols";
                else if ((uc >= 0x1D400) && (uc <= 0x1D7FF)) alphabet = "none";
                else if ((uc >= 0x20000) && (uc <= 0x2A6DF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2F800) && (uc <= 0x2FA1F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xE0000) && (uc <= 0xE007F)) alphabet = "none";

                bool ucprint = false;
                if (alphabet != "none")
                {
                    n++;
                    if (!alphdir.ContainsKey(alphabet))
                        alphdir.Add(alphabet, 0);
                    alphdir[alphabet]++;
                }
                else if (uc != 0x0020)
                {
                    //Console.Write("c=" + c.ToString() + ", uc=0x" + uc.ToString("x5") + "|");
                    //ucprint = true;
                }
                if (ucprint)
                    Console.WriteLine();
            }

            int nmax = 0;
            string alphmax = "none";
            foreach (string alph in alphdir.Keys)
            {
                //Console.WriteLine("ga:" + alph + " " + alphdir[alph].ToString());
                if (alphdir[alph] > nmax)
                {
                    nmax = alphdir[alph];
                    alphmax = alph;
                }
            }

            if (letters.Length > 2 * n) //mostly non-alphabetic
                return "none";
            else if (nmax > n / 2) //mostly same alphabet
                return alphmax;
            else
                return "mixed"; //mixed alphabets
        }

        public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.IndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }

        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.LastIndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }
    }


}
