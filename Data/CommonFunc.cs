using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Tekno.DashboardAgentService.Common
{
    public static class CommonFunc
    {
        public static List<String> SplitTextByLength(String text, int length)
        {
            List<String> returnVal = new List<string>();
            try
            {
                int tLength = text.Length;
                int loopCnt = (int)Math.Floor((decimal)tLength / length);
                for (int i = 0; i < loopCnt; i++)
                {
                    returnVal.Add(text.Substring(i * length, length));
                    tLength -= length;
                }
                if (tLength > 0)
                {
                    returnVal.Add(text.Substring(returnVal.Count * 50, tLength));
                }

                for (int i = returnVal.Count; i < 2; i++)
                {
                    returnVal.Add("");
                }
            }
            catch (Exception ex)
            {
            }

            return returnVal;
        }

        public static DateTime getDateTimeFromUnixTimeStamp(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string ConvertDataTableToHTML(DataTable dt, bool IsGmail)
        {
            string html = "";
            if (IsGmail)
            {
                html = "<table style=\"border: 1px solid black; border-style: collapse;\"> \r\n";

                //add header row
                html += "<tr style=\"border: 1px solid black; padding: 10px;\">";
                for (int i = 0; i < dt.Columns.Count; i++)
                    html += "<th style=\"border: 1px solid black; padding: 10px;\">" + dt.Columns[i].ColumnName + "</th>";
                html += "</tr>";
                //add rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html += "<tr style=\"border: 1px solid black; padding: 10px;\">";
                    for (int j = 0; j < dt.Columns.Count; j++)
                        html += "<td style=\"border: 1px solid black; padding: 10px;\">" + dt.Rows[i][j].ToString() + "</td>";
                    html += "</tr>";
                }
                html += "</table>";
            }
            else
            {
                html = "<style type=\"text / css\"> \r\n" +
                       ".tg  {border-collapse:collapse;border-spacing:0;border-color:#93a1a1;} \r\n" +
                       ".tg td{font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:#93a1a1;color:#002b36;background-color:#fdf6e3;} \r\n" +
                       ".tg th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:#93a1a1;color:#fdf6e3;background-color:#657b83;} \r\n" +
                       ".tg .tg-hmp3{background-color:#eee8d5;text-align:left;vertical-align:top} \r\n" +
                       ".tg .tg-mb3i{background-color:#eee8d5;text-align:right;vertical-align:top} \r\n" +
                       ".tg .tg-lqy6{text-align:right;vertical-align:top} \r\n" +
                       ".tg .tg-0lax{text-align:left;vertical-align:top} \r\n" +
                       "</style> \r\n" +
                       "<table class=\"tg\"> \r\n";

                //add header row
                html += "<tr>";
                for (int i = 0; i < dt.Columns.Count; i++)
                    html += "<th class=\"tg-0lax\">" + dt.Columns[i].ColumnName + "</th>";
                html += "</tr>";
                //add rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string cssClass = "tg-0lax";
                    if (i % 2 == 1) cssClass = "tg-0lax";

                    html += "<tr>";
                    for (int j = 0; j < dt.Columns.Count; j++)
                        html += "<td class=\"" + cssClass + "\">" + dt.Rows[i][j].ToString() + "</td>";
                    html += "</tr>";
                }
                html += "</table>";
            }
            return html;
        }

        public static DateTime GetLinkerTimestampUtc(Assembly assembly)
        {
            var location = assembly.Location;
            return GetLinkerTimestampUtc(location);
        }

        public static DateTime GetLinkerTimestampUtc(string filePath)
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var bytes = new byte[2048];

            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                file.Read(bytes, 0, bytes.Length);
            }

            var headerPos = BitConverter.ToInt32(bytes, peHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(bytes, headerPos + linkerTimestampOffset);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            return dt.AddSeconds(secondsSince1970) + TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
        }

        public static DateTime EpochTimetoDateTime(long epochtime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //CommonFunc.ConsoleLog(LogType.Info, "Log", epoch.AddMilliseconds(epochtime).AddHours(3).ToLongDateString());
            return epoch.AddMilliseconds(epochtime).AddHours(3);
            //return epoch.AddSeconds(Math.Floor(epochtime / 1000.0)).AddHours(3);
        }

        public static bool SendExceptionMail(String To, String Title, String CompanyName, Exception exp)
        {
            bool returnVal = false;
            try
            {
                //MailSender mailSender = new MailSender();
                //String body = "Sayın Yetkili,<br>Tekno.Integration servisinde hata alındı.<br>" +
                //"Ayrıntılar için aşağıdaki hata bilgilerine bakabilirsiniz.<br><br>" +
                //"<b>Message:</b> " + exp.Message + "<br>" +
                //"<b>Source:</b> " + exp.Source + "<br>" +
                //"<b>StackTrace:</b> " + exp.StackTrace + "<br><br>";

                //if (exp.InnerException != null)
                //    body += "<b>Inner Exception Message:</b> " + exp.InnerException.Message + "<br>";

                //returnVal = mailSender.SendMail(To, null, null, Title + " - " + CompanyName + " Error", body, true, null, null);
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }

        /*
        public static string SendSatinalmaMail(Config configXml, String OrderNo, String EvrakSeri)
        {
            string returnVal = null;
            int MailTemplateNo = 1;
            string MailTemplate = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MailTemplates\\" + configXml.CompanyCode.ToUpper() + "_MailTemplate_" + MailTemplateNo.ToString("00") + ".html";
            try
            {
                if (File.Exists(MailTemplate))
                {
                    string body = File.ReadAllText(MailTemplate);
                    MailSender mailSender = new MailSender();
                    //SQL
                    string query = "SELECT SIP.sip_belgeno AS SIPARIS_NO,STO.sto_kod AS STO_KOD,STO.sto_isim AS STOK_ADI,SIP.sip_miktar AS ADET,BT.bar_kodu AS BARKOD,((SIP.sip_tutar+SIP.sip_vergi)/SIP.sip_miktar) AS URUN_FIYAT, \r\n" +
                                   "UPPER(CH.cari_unvan1) AS MUSTERI_ADI, CHA.adr_cadde+' '+CHA.adr_ilce+'/'+CHA.adr_il+'/'+CHA.adr_ulke AS MUSTERI_ADRESI,CHT.cari_EMail AS FIRMA_MAIL,CHT.cari_unvan1 AS FIRMA_ADI,SIP.sip_stok_kod \r\n" +
                                   "FROM SIPARISLER SIP \r\n"+
                                   "LEFT JOIN STOKLAR STO ON STO.sto_kod = SIP.sip_stok_kod \r\n" +
                                   "LEFT JOIN BARKOD_TANIMLARI BT ON BT.bar_stokkodu = SIP.sip_stok_kod \r\n" +
                                   "LEFT JOIN CARI_HESAPLAR CH ON CH.cari_kod = SIP.sip_musteri_kod \r\n" +
                                   "LEFT JOIN CARI_HESAP_ADRESLERI CHA ON CHA.adr_cari_kod = CH.cari_kod  \r\n" +
                                   "LEFT JOIN CARI_HESAPLAR CHT ON CHT.cari_kod = STO.sto_sat_cari_kod AND STO.sto_hammadde_kodu='T' \r\n"+
                                   "WHERE SIP.sip_belgeno = '" + OrderNo + "' AND SIP.sip_evrakno_seri = '"+ EvrakSeri + "' AND BT.bar_master = 1";
                    DataTable dt = MSSQLDataConnection.SelectDataFromDBDT(query, ConfigurationManager.ConnectionStrings["MSSQL"].ConnectionString);

                    if (dt != null && dt.Rows.Count > 0 && !String.IsNullOrEmpty(dt.Rows[0]["FIRMA_ADI"].ToString()))
                    {
                        string To = dt.Rows[0]["FIRMA_MAIL"].ToString();
                        int CountProduct = MSSQLDataConnection.SelectIntFromDB("SELECT dbo.fn_DepodakiMiktar('" + dt.Rows[0]["sip_stok_kod"].ToString() + "', 1, CONVERT(NVARCHAR(50), GETDATE(), 112))", ConfigurationManager.ConnectionStrings["MSSQL"].ConnectionString);

                        body = body.Replace("[FIRMA_ADI]", dt.Rows[0]["FIRMA_ADI"].ToString());
                        body = body.Replace("[STOK_KODU]", dt.Rows[0]["STO_KOD"].ToString());
                        body = body.Replace("[STOK_ADI]", dt.Rows[0]["STOK_ADI"].ToString());
                        body = body.Replace("[STOK_ADETI]", dt.Rows[0]["ADET"].ToString());
                        body = body.Replace("[URUN_FIYAT]", dt.Rows[0]["URUN_FIYAT"].ToString());
                        body = body.Replace("[MUSTERI_ADI]", dt.Rows[0]["MUSTERI_ADI"].ToString());
                        body = body.Replace("[MUSTERI_ADRESI]", dt.Rows[0]["MUSTERI_ADRESI"].ToString());
                        body = body.Replace("[BARKOD]", dt.Rows[0]["BARKOD"].ToString());

                        if (String.IsNullOrEmpty(To))
                            To = configXml.SendBCCMailAddress;

                        if (CountProduct > 0)
                        {
                            //To = configXml.SendBCCMailAddress;
                            returnVal = "Firmada stok olduğu için tedarikçiye mail gönderilmemiştir.Stok Sayısı:" + CountProduct;
                        }
                        else
                        {
                            if (mailSender.SendMail(To, null, configXml.SendBCCMailAddress, configXml.CompanyName + " Firması Satınalma Maili", body, true, null, null))
                                returnVal = "Mail gönderilmiştir.";
                            else
                                returnVal = "Mail gönderilmemiştir. Hata:" + mailSender.Response;
                        }
                    }
                    else
                        returnVal = "Uygun kayıt bulunamadığı için mail gönderilmemiştir.";
                }
                else
                    returnVal = "Mail şablonu olmadığı için mail gönderilememiştir.";
            }
            catch (Exception ex)
            {
                returnVal = "Exception: " + ex.Message + " >> " + ex.StackTrace;
            }
            return returnVal;
        }
        public static Config LoadConfigXml()
        {
            Config configXml = new Config();
            if (File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\config.xml"))
            {
                String Xml = File.ReadAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\config.xml");
                if (!String.IsNullOrEmpty(Xml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Config));
                    using (TextReader reader = new StringReader(Xml))
                    {
                        configXml = (Config)serializer.Deserialize(reader);
                    }
                }
            }
            return configXml;
        }
        public static void SaveConfigXml<T>(T file)
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\config.xml";
            if (File.Exists(path))
            {
                File.Copy(path, path.Replace(".xml", "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml"));
                File.Delete(path);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, file);
            }
        }*/

        public static string DataTableToJSONWithNewtonJson(DataTable table)
        {
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (System.Data.DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return JsonConvert.SerializeObject(parentRow);
        }

        public static List<Dictionary<string, object>> GetDictionary(DataTable dt)
        {
            List<Dictionary<string, object>> returnValList = null;
            Dictionary<string, object> returnVal = null;
            try
            {
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        returnVal = new Dictionary<string, object>();
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            returnVal.Add(dt.Columns[k].ColumnName, dt.Rows[i][k]);
                        }
                        if (returnValList == null) returnValList = new List<Dictionary<string, object>>();
                        returnValList.Add(returnVal);
                    }
                }
            }
            catch (Exception ex)
            {
                returnValList = null;
            }

            return returnValList;
        }

        public static string GetErrorCode(int result)
        {
            if (result == 0)
                return "İşlem başarılı";
            else
            {
                string message = "";
                switch (result)
                {
                    case -1: message = "SONUC -1 : İşlem Başarısız. Tanımlanmamış Hata!"; break;
                    case 1: message = "SONUC 1 : Hatali Table No"; break;
                    case 2: message = "SONUC 2 : Hatali Parametre No"; break;
                    case 3: message = "SONUC 3 : Header`da Tanim (APN) dosyasi belirtilmemis"; break;
                    case 4: message = "SONUC 4 : Tanim dosyasi yok"; break;
                    case 5: message = "SONUC 5 : Import dosyasi bulunamadi yada XML Hatali"; break;
                    case 6: message = "SONUC 6 : Tekrarlayan Index"; break;
                    case 7: message = "SONUC 7 : Sirket Bulunamadi"; break;
                    case 8: message = "SONUC 8 : Kullanici / Sifre Hatali"; break;
                    case 9: message = "SONUC 9 : Firma Calisma dizini Hatali"; break;
                    case 10: message = "SONUC 10 : Parametre Hatasi"; break;
                    case 11: message = "SONUC 11 : Firma Yolu Bulunamadi"; break;
                    case 12: message = "SONUC 12 : Firma Bilgileri Bulunamadi"; break;
                    case 13: message = "SONUC 13 : Export Dosyasi Yaratilamiyor"; break;
                    case 14: message = "SONUC 14 : Alan ismi Bulunamadi"; break;
                    case 15: message = "SONUC 15 : Sisteme Girilmemis"; break;
                    case 16: message = "SONUC 16 : Kayit yapilamadi"; break;
                    default: message = String.Format("SONUC {0} : Hatanın kapsamı bilinmiyor", result); break;
                }
                return message;
            }
        }

        public static string getExternalIP()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    return client.DownloadString("http://canihazip.com/s");
                }
                catch (WebException e)
                {
                    // this one is offline
                }

                try
                {
                    return client.DownloadString("http://wtfismyip.com/text");
                }
                catch (WebException e)
                {
                    // offline...
                }

                try
                {
                    return client.DownloadString("http://ip.telize.com/");
                }
                catch (WebException e)
                {
                    // offline too...
                }

                // if we got here, all the websites are down, which is unlikely
                return null;
            }
        }

        public static void CreateXSDFile(String configXml)
        {
            XmlReader reader = XmlReader.Create(configXml);
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            XmlSchemaInference schema = new XmlSchemaInference();

            schemaSet = schema.InferSchema(reader);

            XmlWriter writer;
            int count = 0;
            foreach (XmlSchema s in schemaSet.Schemas())
            {
                writer = XmlWriter.Create("config.xsd");
                s.Write(writer);
                writer.Close();
            }
            reader.Close();
        }

        public static double ConvertString2Money(string money)
        {
            double retMoney = 0.0;
            char sep = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (money.IndexOf(sep) > -1) retMoney = double.Parse(money);
            else
            {
                if (sep == '.')
                    retMoney = double.Parse(money.Replace(",", "."));
                else
                    retMoney = double.Parse(money.Replace(".", ","));
            }

            return retMoney;
        }
    }
}