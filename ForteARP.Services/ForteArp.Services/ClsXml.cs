using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ForteArg.Services
{

    public class ClsXml : IDisposable
    {
        public string GetXMLPath()
        {
            //return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ForteData");
            //return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ForteData");
            //MessageBox.Show(System.AppDomain.CurrentDomain.BaseDirectory);
            // (MsgTypes.INFO, MsgSources.APPSTART, System.AppDomain.CurrentDomain.BaseDirectory);
            // (MsgTypes.INFO, MsgSources.APPSTART, "......................");
            // (MsgTypes.INFO, MsgSources.APPSTART, "Stertup Read XML files");
            return System.AppDomain.CurrentDomain.BaseDirectory; //Directory.GetCurrentDirectory();
        }

        public string XMLGdvFilePath
        {
            get { return Path.Combine(GetXMLPath(), "GridviewItems.xml"); }
        }

        public string XMLHdrFilePath
        {
            get { return Path.Combine(GetXMLPath(), "HdrItems.xml"); }
        }

        public ClsXml()
        {
            ClsSerilog.LogMessage(ClsSerilog.Info, $"Check and create xml files -> {DateTime.Now}");
            CheckandCreateXMLFiles("GridviewItems.xml", "CustomGridView");
            CheckandCreateXMLFiles("GdvRealtimeList.xml", "CustomGridView");
            CheckandCreateXMLFiles("HdrItems.xml", "CustomHdr");
        }

        public void CheckandCreateXMLFiles(string xmlfile, string StartElement)
        {
            String FileLocation = Path.Combine(GetXMLPath(), xmlfile);

            if (!System.IO.File.Exists(FileLocation))
            {
                ClsSerilog.LogMessage(ClsSerilog.Info, $"Create xml file " + FileLocation);

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };

                using (XmlWriter writer = XmlWriter.Create(FileLocation, settings))
                {
                    //Begin write
                    writer.WriteStartDocument();
                    //Node
                    writer.WriteStartElement(StartElement);

                    writer.WriteEndDocument();
                    writer.Close();
                }
            }
            ClsSerilog.LogMessage(ClsSerilog.Info, $"Checked all XML files -> " + xmlfile);
        }

        public List<string> ReadXmlGridView(string FileLocation)
        {
            List<string> XmlGridView = new List<string>();
            XmlGridView.Clear();
            XmlDocument doc = new XmlDocument();

            try
            {
                if (File.Exists(FileLocation))
                {
                    doc.Load(FileLocation);
                    XmlNodeList xnl = doc.SelectNodes("CustomGridView/Field/Name");

                    if ((xnl != null) && (xnl.Count > 0))
                    {
                        foreach (XmlNode xn in xnl)
                        {
                            if (File.Exists(FileLocation))
                                XmlGridView.Add(xn.InnerText);
                        }
                    }
                }
                if (XmlGridView.Count == 0) XmlGridView.Add("Forte");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in ReadXmlGridView - " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"Error in  ReadXmlGridView -> {ex.Message}");
            }
            return XmlGridView;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedHdrList"></param>
        /// <param name="settingsGdvFile"></param>
        public void UpdateXMlcolumnList(ObservableCollection<string> selectedHdrList, string settingsGdvFile)
        {

            try
            {
                if (File.Exists(settingsGdvFile))
                {
                    File.SetAttributes(settingsGdvFile, FileAttributes.Normal);

                    File.Delete(settingsGdvFile);

                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Indent = true
                    };

                    using (XmlWriter writer = XmlWriter.Create(settingsGdvFile, settings))
                    {
                        //Begin write
                        writer.WriteStartDocument();
                        //Node
                        writer.WriteStartElement("CustomGridView");

                        foreach (var item in selectedHdrList)
                        {
                            writer.WriteStartElement("Field");
                            writer.WriteElementString("Name", item);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in UpdateXMlcolumnList " + ex);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"ERROR in  UpdateXMlcolumnList -> {ex.Message}");
            }
        }

        public List<int> ReadXmlHdrList(string FileLocation)
        {
            List<int> ihdrlist = new List<int>();
            ihdrlist.Clear();
            XmlDocument doc = new XmlDocument();

            try
            {
                if (File.Exists(FileLocation))
                {
                    doc.Load(FileLocation);
                    XmlNodeList xnl = doc.SelectNodes("CustomHdr/Field/Value");
                    if ((xnl != null) && (xnl.Count > 0))
                    {
                        foreach (XmlNode xn in xnl)
                        {
                            ihdrlist.Add(Int32.Parse(xn.InnerText));
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error in ReadXmlHdrList - " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"ERROR in  ReadXmlHdrList -> {ex.Message}");
            }
            return ihdrlist;
        }


        public void WriteXmlGridView(List<CheckedListItem> StringsListBox, string FileLocation)
        {

            try
            {
                if (System.IO.File.Exists(FileLocation))
                    System.IO.File.Delete(FileLocation);

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };

                using (XmlWriter writer = XmlWriter.Create(FileLocation, settings))
                {
                    //Begin write
                    writer.WriteStartDocument();
                    //Node
                    writer.WriteStartElement("CustomGridView");

                    foreach (var item in StringsListBox)
                    {
                        writer.WriteStartElement("Field");
                        writer.WriteElementString("Id", item.Id.ToString());
                        writer.WriteElementString("Name", item.Name);
                        writer.WriteElementString("FieldType", item.FieldType);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndDocument();
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in WriteXmlGridView - " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"ERROR in  WriteXmlGridView -> {ex.Message}");
            }
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

    }
}
