using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApplication
{
    public class ADHelper
    {
        /// <summary>
        /// 获取客户端IP。
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string GetIP4Address(string address)
        {
            string IP4Address = String.Empty;

            if (address == "::1")
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(address))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }

                if (IP4Address != String.Empty)
                {
                    return IP4Address;
                }

                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }
            }
            else
            {
                IP4Address = address;
            }

            return IP4Address;
        }

        public static IDictionary<string, string> GetUser(string domainName, string filterName, string filterValue)
        {
            var allProperties = "name,displayname,givenName,samaccountname,mail,title";
            var properties = allProperties.Split(new char[] { '\r', '\n', ',' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + domainName);

                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = $"({filterName}={filterValue})";
                //search.Filter = "(samaccountname=" + loginName + ")";
                //search.Filter = "(mail=" + userEmail + ")";

                //foreach (string p in properties)
                //{
                //    search.PropertiesToLoad.Add(p);
                //}
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    Dictionary<string, string> res = new Dictionary<string, string>();

                    foreach (DictionaryEntry p in result.Properties)
                    {
                        ResultPropertyValueCollection value = p.Value as ResultPropertyValueCollection;
                        res.Add(p.Key.ToString(),value[0].ToString());
                    }

                    return res;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// 根据email获取用户相关数据 LL 20200117
        /// </summary>
        /// <param name="email">samaccountname</param>
        /// <param name="domainName">pacrim</param>
        /// <returns></returns>
        public static string GetWindowsUserInfo(string email, string domainName)
        {
            string samaccountname = string.Empty;
            string givenName = string.Empty;
            string name = string.Empty;
            string displayname = string.Empty;
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(domainName))
            {
                var allProperties = "name,displayname,givenName,samaccountname,mail";
                var properties = allProperties.Split(new char[] { '\r', '\n', ',' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    DirectoryEntry entry = new DirectoryEntry("LDAP://" + domainName);

                    DirectorySearcher search = new DirectorySearcher(entry);
                    search.Filter = "(mail=" + email + ")";
                    foreach (string p in properties)
                    {
                        search.PropertiesToLoad.Add(p);
                    }
                    SearchResult result = search.FindOne();
                    if (result != null)
                    {
                        foreach (string p in properties)
                        {
                            ResultPropertyValueCollection collection = result.Properties[p];
                            for (int i = 0; i < collection.Count; i++)
                            {
                                if (p == "samaccountname")
                                {
                                    samaccountname = collection[i].ToString();
                                }
                                if (p == "mail")
                                {
                                    email = collection[i].ToString();
                                }
                                if (p == "givenName")
                                {
                                    givenName = collection[i].ToString();
                                }
                                if (p == "displayname")
                                {
                                    displayname = collection[i].ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return "";
                }
            }
            return email + ";" + givenName + ";" + displayname + ";" + samaccountname;
        }

    }
}
