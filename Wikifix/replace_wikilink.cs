using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetWikiBot;
using System.Text.RegularExpressions;

namespace Wikifix
{
    class replace_wikilink
    {
        static Site site = null;
        public static void replace(Site sitepar, string makelang, Form1 parent)
        {
            site = sitepar;
            if (site == null)
                site = util.login(makelang);

            site.defaultEditComment = "Fixing mistake";
            site.minorEditByDefault = true;

            int nedit = 0;
            int nround = 0;
            bool pauseaftersave = true;
            //string cat = "Paghimo ni bot 2016-09";
            string cat = "Heyograpiya sa Province of KwaZulu-Natal";
            bool resume = String.IsNullOrEmpty(parent.TB_resumeat.Text);
            string resume_at = parent.TB_resumeat.Text;

            do
            {
                nedit = 0;
                nround++;
                PageList pl = new PageList(site);
                PageList pl1 = new PageList(site);

                //Select how to get pages. Uncomment as needed.

                //Add pages "by hand":
                //addpages(site,pl);
                //Find articles from a category
                //pl.FillAllFromCategoryTree("Geografi i Goiás");
                //pl1.FillAllFromCategoryTree("Eufriesea");
                //foreach (Page p in pl1)
                //    pl.Add(p);
                //pl1.FillAllFromCategoryTree("Euglossa");
                //foreach (Page p in pl1)
                //    pl.Add(p);
                //pl1.FillAllFromCategoryTree("Eulaema");
                //foreach (Page p in pl1)
                //    pl.Add(p);
                //pl1.FillAllFromCategoryTree("Exaerete");
                //foreach (Page p in pl1)
                //    pl.Add(p);
                parent.memo("Loading category "+cat);
                pl.FillAllFromCategoryTree(cat);
                //cat = util.increment_cat(cat);



                //Find subcategories of a category
                //pl.FillSubsFromCategory("Svampars vetenskapliga namn");

                //Find articles from all the links to an article, mostly useful on very small wikis
                //pl.FillFromLinksToPage("Sisjön (lanaw sa Sweden, Västra Götalands Län)");

                //Find articles containing a specific string
                //pl.FillFromSearchResults("insource:\" kay \"", 4999);

                //Set specific article:
                //Page pp = new Page(site, "Citrontrogon");pl.Add(pp);

                //Skip all namespaces except articles:
                //pl.RemoveNamespaces(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 100, 101 });
                //Skip all namespaces except categories:
                //pl.RemoveNamespaces(new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 100, 101 });

                Dictionary<string, string> replacedict = new Dictionary<string, string>();

                //replacedict.Add("== Saysay ==","");
                //replacedict.Add("<references group=\"saysay\"/>","");
                //replacedict.Add("administratibo nga mga dibisyon sa Bangladesh", "administratibo nga mga dibisyon sa Burkina Faso");
                //replacedict.Add("image = Бесцветный богомол.jpg", "image =");
                //replacedict.Add("bild = Бесцветный богомол.jpg", "bild =");
                //replacedict.Add(" = [[Brčko]]", " = [[Brčko (distrikt)|Brčko]]");
                //replacedict.Add(" = Entitet", " = Distrikt");
                //replacedict.Add("entiteten <!--ADM1-->[[Brčko]]", "distriktet <!--ADM1-->[[Brčko (distrikt)|Brčko]]");
                //replacedict.Add(" = [[Brčko]]", "| state                       = [[Brčko (distrikt)|Brčko]]");
                //replacedict.Add("En underart finns: ''", "Utöver nominatformen finns också underarten ''");
                //replacedict.Add("| timezone               = [[Fernando de Noronha Time|FNT]]", "| timezone             = [[Brasilia Time|BRT]]");
                //replacedict.Add("| timezone_DST           = [[Amazon Summer Time|AMST]]", "| timezone_DST         = [[Brasilia Summer Time|BRST]]");
                //replacedict.Add("| utc_offset             = -2", "| utc_offset           = -3");
                //replacedict.Add("| utc_offset_DST         = -3", "| utc_offset_DST       = -2");
                //replacedict.Add("[[Kungariket Olanda]]", "[[Olanda]]");
                //replacedict.Add("Ð", "Đ"); //från isländskt Đ till bosniskt Đ
                //replacedict.Add("Schweizs administrativa indelning", "Schweiz administrativa indelning");
                //replacedict.Add("Mer om algoritmen finns här: [[Användare:Lsjbot/Algoritmer]].", "{{Lsjbot-algoritmnot}}");
                //replacedict.Add(" Bethune-baker", " [[George Thomas Bethune-Baker]]"); 
                //replacedict.Add("[[parish]]", "[[Civil parish|parish]]");
                //replacedict.Add("==See also==", "== Tan-awa usab ==");
                //replacedict.Add("== See also ==", "== Tan-awa usab ==");
                //replacedict.Add("[[Time in ", "[[Panahon sa ");
                //replacedict.Add(" maoy pangalan sa mga sumusunod:", " ngalan niining mga mosunod:");
                //replacedict.Add("[[Kategoriya:Shinano]] ug [[Chikuma tubig-saluran]]", "[[Kategoriya:Shinano ug Chikuma tubig-saluran]]");
                //replacedict.Add("[[Kategoriya:Tigris River]] ug [[Euphrates River tubig-saluran]]", "[[Kategoriya:Tigris ug Euphrates tubig-saluran]]");
                //replacedict.Add("Burma|", "Birmanya|");
                //replacedict.Add("Burma]]", "Birmanya]]");
                //replacedict.Add("Burma}}", "Birmanya}}");
                //replacedict.Add("sa Burma", "sa Birmanya");
                //replacedict.Add("= Burma", "= Birmanya");
                //replacedict.Add("[[Washington, D.C. (ulohang dakbayan sa Estados Unidos)|Washington, D.C.]].", "[[Washington, D.C.]]");
                //replacedict.Add("[[administratibo nga mga dibisyon sa Litwanya|kapital sa munisipyo]] sa [[Latvia]]", "[[administratibo nga mga dibisyon sa Latvia|kapital sa munisipyo]] sa [[Latvia]]");
                //replacedict.Add("<i>European register of marine species: a check-list of the marine species in Europe and a bibliography of guides to their identification. Collection Patrimoines Naturels,<,", "<i>European register of marine species: a check-list of the marine species in Europe and a bibliography of guides to their identification.</i> Collection Patrimoines Naturels,");
                //replacedict.Add(" sa sa ", " sa ");
                //replacedict.Add("{{Location map|United States |", "{{Location map|USA Hawaii |");
                //replacedict.Add("{{Location map|United States |", "{{Location map|USA Hawaiian Islands|");
                //replacedict.Add("{{Location map|USA Hawaii |", "{{Location map|USA Hawaiian Islands|");
                //replacedict.Add(" sa Tinipong Bansa. | label = ", " sa Hawaii. | label = ");
                //replacedict.Add(" sa Estados Unidos. | label = ", " sa Hawaii. | label = ");

                //replacedict.Add("bakteriya", "insekto");
                //replacedict.Add("[[Kategoriya:Bacteria]]", "[[Kategoriya:Bacteria (insekto)]]");

                //replacedict.Add("[[Makedoniya]]", "[[Republika sa Amihanan Makedoniya|Makedoniya]]");
                //replacedict.Add("[[labaw pamilya (biyolohiya)|Labaw pamilya] nga naapil", "[[labaw pamilya (biyolohiya)|Kapunoang-banay]] nga naapil");
                //string oldname = "[[China Time|CT]]";
                //string newname = "[[Malaysia Standard Time|MST]]";
                string oldname = "|position=right|";
                string newname = "|position=left|";

                replacedict.Add(oldname, newname);

                Dictionary<string, string> regexdict = new Dictionary<string, string>();
                //regexdict.Add(@"\| timezone *= \[\[Fernando de Noronha Time\|FNT\]\]", "| timezone             = [[Brasilia Time|BRT]]");
                //regexdict.Add(@"\| timezone_DST *= \[\[Amazon Summer Time\|AMST\]\]", "| timezone_DST         = [[Brasilia Summer Time|BRST]]");
                //regexdict.Add(@"\| utc_offset *= -2", "| utc_offset           = -3");
                //regexdict.Add(@"\| utc_offset_DST *= -3", "| utc_offset_DST       = -2");
                //regexdict.Add(@"\| category *= Parish", "| category             = Civil parish");
                //Kinahabogang dapit sa palibot ang [[Sisjön (lanaw sa Sweden, Västra Götalands Län)|Sisjön]], {{formatnum:700}} ka metros ni kahaboga ibabaw sa dagat, {{formatnum:9.2}} km sa habagatan sa Göteborg.
                //regexdict.Add(@"Kinahabogang dapit sa palibot ang \[\[Sisjön \(lanaw sa Sweden\, Västra Götalands Län\)\|Sisjön\]\]\, \{\{formatnum:\d+\}\} ka metros ni kahaboga ibabaw sa dagat\, \{\{formatnum:[\d\.]+\}\} km sa [\w\-]+ sa \w+\.\{\{efn\|group\=saysay\|Ang punto nga labing taas sa ibabaw sa mga lokal nga kapunawpunawan\, sumala sa gihabogon data sa GeoNames\.\<ref name \= \042gn\d+\042/>}}", "");



                List<string> linkword = new List<string>();
                //linkword.Add("Catalogue of Life");

                //Require title to contain one in requiretitle list:
                List<string> requiretitle = new List<string>();
                //requiretitle.Add("Radioprogram nerlagda");

                //Require ALL in requireword list:
                List<string> requireword = new List<string>();
                //requireword.Add("obotskapad");
                //requireword.Add("= -3\n");
                //requireword.Add("Azerbajdzjan");
                //requireword.Add("Latvia");


                //Require AT LEAST ONE in requireone list:
                List<string> requireone = new List<string>();



                List<string> vetoword = new List<string>();
                //vetoword.Add("Argentina");
                //vetoword.Add("Island");
                //vetoword.Add("isländska");


                DateTime oldtime = DateTime.Now;
                oldtime = oldtime.AddSeconds(5);

                parent.memo("Pages to change : " + pl.Count().ToString());

                int iremain = pl.Count();

                foreach (Page p in pl)
                {
                    //////SPECIAL FOR KAY REPLACEMENT!
                    //string s1 = "Ang yuta palibot sa " + util.remove_disambig(p.title);
                    //string s2 = "Ang yuta sa " + util.remove_disambig(p.title);
                    //replacedict.Clear();
                    //replacedict.Add(s1 + " kay ", s1 + " ");
                    //replacedict.Add(s2 + " kay ", s2 + " ");
                    //replacedict.Add(util.remove_disambig(p.title) + "  ", util.remove_disambig(p.title) + " ");
                    //////SPECIAL FOR KAY REPLACEMENT!

                    iremain--;
                    if (iremain % 100 == 0)
                        parent.memo(iremain.ToString()+" remaining. "+p.title);

                    //Skip start of alphabet:
                    //if (String.Compare(p.title,"Tir Koh") < 0 )
                    //    continue;
                    if (!resume)
                    {
                        if (p.title == resume_at) //also set resume=true/false before loop
                            resume = true;
                        else
                            continue;
                    }


                    if (!util.tryload(p, 2))
                    {
                        parent.memo("Load failed " + p.title);
                        continue;
                    }
                    if (!p.Exists())
                        continue;

                    string origtitle = p.title;

                    //Follow redirect:

                    if (p.IsRedirect())
                    {
                        p.title = p.RedirectsTo();
                        if (!util.tryload(p, 2))
                            continue;
                        if (!p.Exists())
                            continue;
                    }

                    //Check so required title actually present:

                    if (requiretitle.Count > 0)
                    {
                        bool onefound = false;
                        foreach (string s in requiretitle)
                            if (p.title.Contains(s))
                                onefound = true;

                        if (!onefound)
                        {
                            Console.WriteLine("requiretitle not found");
                            continue;
                        }
                    }


                    //Check so all required strings actually present:

                    bool allfound = true;
                    foreach (string s in requireword)
                        if (!p.text.Contains(s))
                            allfound = false;

                    if (!allfound)
                    {
                        Console.WriteLine("requireword not found");
                        continue;
                    }

                    if (requireone.Count > 0)
                    {
                        bool onefound = false;
                        foreach (string s in requireone)
                            if (p.text.Contains(s))
                                onefound = true;

                        if (!onefound)
                        {
                            Console.WriteLine("requireone not found");
                            continue;
                        }
                    }

                    //Check so no vetoword are present:

                    bool vetofound = false;
                    foreach (string s in vetoword)
                        if (p.text.Contains(s))
                            vetofound = true;

                    if (vetofound)
                    {
                        Console.WriteLine("vetoword found");
                        continue;
                    }

                    //If redirect, go back to redirect page:

                    //if (origtitle != p.title)
                    //{
                    //    p.title = origtitle;
                    //    p.Load();
                    //}

                    string origtext = p.text;

                    //Do the actual replacement:

                    foreach (KeyValuePair<string, string> replacepair in replacedict)
                    {
                        p.text = p.text.Replace(replacepair.Key, replacepair.Value);

                    }

                    foreach (KeyValuePair<string, string> replacepair in regexdict)
                    {
                        p.text = Regex.Replace(p.text, replacepair.Key, replacepair.Value);

                    }


                    //special for mismatching tags:
                    //int itag = p.text.ToLower().IndexOf("<i>");
                    //int refend = p.text.IndexOf("</ref>", itag);
                    //int bend = p.text.ToLower().IndexOf("</b>", itag);

                    //if (refend < 0)
                    //    refend = 999999;
                    //if (bend < 0)
                    //    bend = 999999;
                    //if (refend < bend)
                    //{
                    //    p.text = ReplaceOne(p.text, "</ref>", "''</ref>", itag);
                    //    p.text = p.text.Replace("<i>", "''").Replace("<I>", "''");
                    //}
                    //else if (bend < refend)
                    //{
                    //    p.text = ReplaceOne(p.text, "</b>", "''</b>", itag);
                    //    p.text = ReplaceOne(p.text, "</B>", "''</B>", itag);
                    //    p.text = p.text.Replace("<i>", "''").Replace("<I>", "''");
                    //}
                    //else
                    //    p.text = p.text.Replace("<i>", "").Replace("<I>", "");

                    //Wikilink first occurrence of each word, if not linked already:

                    foreach (string s in linkword)
                    {
                        if (p.text.IndexOf(s) < 0)
                            continue;
                        string slinked = "[[" + s + "]]";
                        if (p.text.IndexOf(slinked) < 0)
                        {
                            p.text = p.text.Insert(p.text.IndexOf(s), "[[");
                            p.text = p.text.Replace("[[" + s, slinked);
                        }
                    }

                    //Save the result:

                    if (p.text != origtext)
                    {
                        //Bot.editComment = "Ersätter och wikilänkar";
                        //isMinorEdit = true;

                        if (util.trysave(p, 4, site))
                        {
                            if ((nedit < 10) || (nedit % 100 == 0))
                                parent.memo("nedit = " + nedit);
                            nedit++;
                            if (nedit >= 4)
                            {
                                pauseaftersave = false;
                            }
                        }
                        else
                            parent.memo("Save failed "+p.title);
                    }
                    Console.WriteLine(iremain.ToString() + " remaining.");

                    //if ( p.title.Contains("sa Burma"))
                    //{
                    //    movepage(site, p.title, p.title.Replace("sa Burma", "sa Birmanya"));
                    //}
                }

                Console.WriteLine("Round "+nround+" Total # edits = " + nedit.ToString());
                parent.memo("Round " + nround + " Total # edits = " + nedit.ToString());

            }
            while (nedit > 0);

        }
    }
}
