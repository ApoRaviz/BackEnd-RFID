using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.Address;
using WIM.Core.Entity.Currency;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class SubCityRepository : Repository<SubCity_MT>, ISubCityRepository
    {
        private CoreDbContext Db;
        public SubCityRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }

        public class ValueWord
        {
            public string word { get; set; }
            public int count { get; set; }
            public int value { get; set; }
        }

        public class locationObj
        {
            public string name { get; set; }
            public int idsys { get; set; }
            public int idsys2 { get; set; }
            public string postal { get; set; }
        }

        public Object GetDto()
        {
            var province = Db.Province_MT.Select(x => x.ProvinceName).ToList();
            var city = Db.City_MT.Select(x => x.CityName).ToList();
            var subcity = Db.SubCity_MT.Select(x => x.SubCityName).ToList();
            var provincetree = (from pv in Db.Province_MT
                                select new locationObj
                                {
                                    name = pv.ProvinceName,
                                    idsys = pv.ProvinceIDSys
                                }
                                ).ToList();
            var citytree = (from pv in Db.City_MT
                                select new locationObj
                                {
                                    name = pv.CityName,
                                    idsys = pv.ProvinceIDSys,
                                    idsys2 = pv.CityIDSys
                                }
                                ).ToList();
            var subcitytree = (from pv in Db.SubCity_MT
                                select new locationObj
                                {
                                    name = pv.SubCityName,
                                    idsys = pv.CityIDSys,
                                    postal = pv.PostalCode
                                }
                                ).ToList();

            List<List<object>> data = new List<List<object>>();
            for (var i = 0; i < provincetree.Count; i++)
            {
                List<List<object>> subdata = new List<List<object>>();
                data.Add(new List<object>());
                data[i].Add(provincetree[i].name);
                var cityid = citytree.Where(a => a.idsys == provincetree[i].idsys).Select(x => x.idsys2).ToList();
                var cityname = citytree.Where(a => a.idsys == provincetree[i].idsys).Select(x => x.name).ToList();
                for (var j = 0; j < cityname.Count; j++)
                {
                    List<List<object>> subdata2 = new List<List<object>>();
                    subdata.Add(new List<object>());
                    subdata[j].Add(cityname[j]);
                    var scityname = subcitytree.Where(a => a.idsys == cityid[j]).Select(x => x.name).ToList();
                    var scitypost = subcitytree.Where(a => a.idsys == cityid[j]).Select(x => x.postal).ToList();
                    for (var k = 0; k < scityname.Count; k++)
                    {
                        subdata2.Add(new List<object>());
                        subdata2[k].Add(scityname[k]);
                        subdata2[k].Add(scitypost[k]);
                    }
                    subdata[j].Add(subdata2);
                }
                data[i].Add(subdata);
            }
            
            //for (var i = 0; i < province.Count; i++)
            //{
            //    int citycount = city.Where(element => element == province[i]).ToList().Count;
            //    int subcitycount = subcity.Where(element => element == province[i]).ToList().Count;
            //    int count = citycount + subcitycount;
            //    if (count > 0)
            //    {
            //        lookup.Add(province[i]);
            //        if (citycount > 0)
            //        {
            //            city = setlookup(city, citycount, lookup.Count - 1, province[i]);
            //        }

            //        if (subcitycount > 0)
            //        {
            //            subcity = setlookup(subcity, subcitycount, lookup.Count - 1, province[i]);
            //        }
            //        province[i] = (lookup.Count - 1).ToString();
            //    }
            //}

            //for (var i = 0; i < city.Count; i++)
            //{
            //    if (!int.TryParse(city[i], out int n))
            //    {
            //        int citycount = city.Where(element => element == city[i]).ToList().Count;
            //        int subcitycount = subcity.Where(element => element == city[i]).ToList().Count;
            //        int count = citycount + subcitycount;

            //        if (count > 1)
            //        {
            //            lookup.Add(city[i]);
            //            if (citycount > 1)
            //            {
            //                city = setlookup(city, citycount, lookup.Count - 1, city[i]);
            //            }

            //            if (subcitycount > 0)
            //            {
            //                subcity = setlookup(subcity, subcitycount, lookup.Count - 1, city[i]);
            //            }
            //        }
            //    }
            //}

            //for (var i = 0; i < subcity.Count; i++)
            //{
            //    if(!int.TryParse(subcity[i], out int n))
            //    {
            //        int count = subcity.Where(element => element == subcity[i]).ToList().Count;

            //        if (count > 1)
            //        {
            //            lookup.Add(subcity[i]);
            //            subcity = setlookup(subcity, count, lookup.Count - 1, subcity[i]);
            //        }
            //    }
            //}

            var lookup = new List<string>();
            var sumdata = new List<string>();
            sumdata.AddRange(province);
            sumdata.AddRange(city);
            sumdata.AddRange(subcity);
            for (var i = 0; i < sumdata.Count; i++)
            {
                if (!int.TryParse(sumdata[i], out int n))
                {
                    int count = sumdata.FindAll(element => element == sumdata[i]).Count;
                    if(count > 1)
                    {
                        lookup.Add(sumdata[i]);
                        sumdata = setlookup(sumdata,count,lookup.Count-1,sumdata[i]);
                    }
                }
            }

            sumdata = sumdata.FindAll(element => !int.TryParse(element, out int n));
            sumdata.AddRange(lookup);
            //var addless = new List<string>();
            //addless.AddRange(numlessLoc(province));
            //addless.AddRange(numlessLoc(city));
            //addless.AddRange(numlessLoc(subcity));
            //addless.AddRange(lookup);

            var word = wordperlist(sumdata).Distinct().ToList();
            List<ValueWord> wordvalue = new List<ValueWord>();

            for (var i = 0; i < word.Count; i++)
            {
                int count = sumdata.FindAll(element => element.Contains(word[i])).Count;
                if (word[i].Length * count > 2 * count + word[i].Length + 15)
                {
                    wordvalue.Add(new ValueWord { word = word[i], count = count, value = word[i].Length * count });
                }
            }

            List<ValueWord> SortedWord = wordvalue.OrderByDescending(o => o.value).ToList();
            List<string> wordtodel = new List<string>();

            for (var i = 0; i < SortedWord.Count; i++)
            {
                var ctword = SortedWord.FindAll(a => a.word.Contains(SortedWord[i].word));
                if (ctword != null)
                {
                    for (var j = 0; j < ctword.Count; j++)
                    {
                        if (ctword[j].word != SortedWord[i].word)
                        {
                            int listvalue = SortedWord[i].value;
                            int ctvalue = ctword[j].value;
                            if (listvalue <= ctvalue)
                            {
                                wordtodel.Add(SortedWord[i].word);
                                break;
                            }
                            else
                            {
                                wordtodel.Add(ctword[j].word);
                            }
                        }
                    }
                }
            }

            var deltemp = wordtodel.Distinct().ToList();
            for (var i = 0; i < deltemp.Count; i++)
            {
                SortedWord.RemoveAll(a => a.word == deltemp[i]);
            }

            string[] headKeys = { "!", "@", "#", "$", "%", "^", "&", "*" };
            List<string> keys = new List<string>();
            foreach (var key in headKeys)
            {
                for (int i = 65; i <= 122; i++)
                {
                    keys.Add(key + Convert.ToChar(i));
                    if (i == 90)
                    {
                        i = 96;
                        continue;
                    }
                }
            }

            string json = new JavaScriptSerializer().Serialize(data);
            string jsonlookup = string.Join("|", lookup.ToArray());
            string jsonword = string.Join("|", SortedWord.Select(x => x.word).ToArray());
            int keyindex = 0;
            for (var i = 0; i < lookup.Count; i++)
            {
                json = json.Replace("\""+lookup[i]+"\"", "\""+i.ToString()+"\"");
            }
            for (var i = 0; i < SortedWord.Count; i++)
            {
                json = json.Replace(SortedWord[i].word.ToString(), keys[keyindex]);
                jsonlookup = jsonlookup.Replace(SortedWord[i].word.ToString(), keys[keyindex]);
                keyindex++;
            }
            data = new JavaScriptSerializer().Deserialize<List<List<object>>>(json);
            object realdata = new { data = data, lookup = jsonlookup, words = jsonword };
            string json1 = new JavaScriptSerializer().Serialize(realdata);
            File.WriteAllText(@"D:\test2.json", json1);

            return lookup;
        }

        public List<string> numlessLoc(List<string> item)
        {
            List<string> numless = new List<string>();
            for (var i = 0; i < item.Count; i++)
            {
                if(!int.TryParse(item[i], out int n))
                {
                    numless.Add(item[i]);
                }
            }
            return numless;
        }

        public List<string> wordperlist(List<string> item)
        {
            List<string> locationWord = new List<string>();
            for (var i = 0; i < item.Count; i++)
            {
                if(item[i].Length > 2) {
                    locationWord.AddRange(wordperString(item[i], item[i].Length));
                }
            }
            return locationWord;
        }
        
        public List<string> wordperString(string word,int len)
        {
            int j = 0;
            List<string> wordcut = new List<string>();
            for (var i = 0; i < len-1; i += 3)
            {
                if (len == 3)
                {
                    wordcut.Add(word);
                }
                else
                {
                    if (len - i > 3)
                    {
                        var ch3 = word.Substring(i, 3);
                        var ch4 = word.Substring(i, 4);
                        wordcut.Add(ch3);
                        wordcut.Add(ch4);
                    }
                    else
                    {
                        var ch3 = word.Substring(i + 1, len - i - 1);
                        wordcut.Add(wordcut[j - 1] + ch3);
                    }
                }
                j += 2;
            }
            return wordcut;
        }

        public List<string> setlookup(List<string> item,int count,int lookindex,string txtsearch) {
            int countcheck = 0;
            for (int f = item.IndexOf(txtsearch, 0); f != -1;)
            {
                int index = f;
                if (f != -1)
                {
                    countcheck++;
                    item[index] = lookindex+"";
                    if (countcheck != count)
                    {
                        f = item.IndexOf(txtsearch, index);
                    }
                    else
                    { 
                        break;
                    }
                }
            }
            return item;
        }

        //var address = (from i in Db.SubCity_MT
        //               join m in Db.City_MT on i.CityIDSys equals m.CityIDSys
        //               join n in Db.Province_MT on m.ProvinceIDSys equals n.ProvinceIDSys
        //               group i by i.CityIDSys into g
        //               select g
        //               ).ToList();
        //return address;

        //string[,] array = new string[4, 2];
        //object[] array8 = new object[7];
        //var groupsubcity = (from i in Db.SubCity_MT
        //               group i.SubCityName by i.CityIDSys into g
        //               select g
        //               ).ToList();
        //return groupsubcity;



        //var mostvalue = counting.OrderByDescending(o => o).ToList();
        //var counttemp = counting;
        //var counttemp = countvalue;
        //List<object> topindex = new List<object>(); //index to find word
        //List<int> topcount = new List<int>(); //most occurrence word count
        //List<string> topword = new List<string>(); //most occurrence word string
        //for (var i = 0; i < 52; i++)
        //{
        //    var k = topword.Where(a => a.Contains(topword[i])).ToList();
        //    if (k != null)
        //    {
        //        for (var j = 0; j < k.Count; j++)
        //        {
        //            if (k[j].ToString() != topword[i])
        //            {
        //                int wordvalue = topword[i].Length * topcount[0];
        //                int kvalue = k[j].ToString().Length * topcount[topword.IndexOf(k[j].ToString())];
        //                if (wordvalue <= kvalue)
        //                {
        //                    topword.Remove(topword[i]);
        //                    topcount.Remove(topcount[i]);
        //                    int maxtmp = counttemp.Max();
        //                    var indextmp = counttemp.IndexOf(maxtmp);
        //                    topcount.Add(maxtmp);
        //                    topword.Add(word[indextmp]);
        //                    counttemp[indextmp] = 0;
        //                    break;
        //                }
        //                else
        //                {
        //                    int maxtmp = counttemp.Max();
        //                    var indextmp = counttemp.IndexOf(maxtmp);
        //                    topcount.Add(maxtmp);
        //                    topword.Add(word[indextmp]);
        //                    counttemp[indextmp] = 0;
        //                    topcount.Remove(topcount[topword.IndexOf(k[j].ToString())]);
        //                    topword.Remove(k[j]);
        //                }
        //            }
        //        }
        //    }
        //}

    }
}