// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.LocalSaveFile
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace StorecfgGenerator
{
    public class LocalSaveFile
    {
        public static LocalSaveFile Instance { get; set; }

        public LocalSaveFile() => LocalSaveFile.Instance = this;

        private string AppConfigFile { get; set; } = "setup.cfg";

        public SetupConfigJson LoadSetupConfig()
        {
            try
            {
                string str = File.ReadAllText(this.AppConfigFile);
                SetupConfigJson setupConfigJson;
                try
                {
                    setupConfigJson = JsonConvert.DeserializeObject<SetupConfigJson>(str);
                }
                catch (Exception ex)
                {
                    setupConfigJson = JsonConvert.DeserializeObject<SetupConfigJson>(str.Substring(1));
                }
                return setupConfigJson;
            }
            catch (Exception ex)
            {
                return (SetupConfigJson)null;
            }
        }

        public bool EnforeSetupSettings(SetupConfigJson setupConfig)
        {
            if (setupConfig.PasswordForDev != null && !setupConfig.PasswordForDev.Trim().Equals("") && setupConfig.PasswordForDev.Equals("Agile4n@w"))
            {
                MainWindow.Instance.IsDevMode = true;
                return true;
            }
            return setupConfig.CustomerID != null && !setupConfig.CustomerID.Trim().Equals("") && setupConfig.License != null && !setupConfig.License.Trim().Equals("") && setupConfig.ServerSettings != null && setupConfig.ServerSettings.Count != 0;
        }

        public void SaveSettings(StoreCfgJson ss, string path)
        {
            try
            {
                bool encrypted = false;
                if (StoreCfg.Instance.CurrentStoreCfg != null)
                    encrypted = StoreCfg.Instance.CurrentStoreCfg.Profile.EncryptStoreFile;
                this.SaveToFile((object)ss, path, encrypted);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Error when save settings: " + ex.Message);
            }
        }

        public StoreCfgJson LoadSettings(string path)
        {
            try
            {
                string str = this.LoadTextFromFile(path, true);
                StoreCfgJson storeCfgJson;
                try
                {
                    storeCfgJson = JsonConvert.DeserializeObject<StoreCfgJson>(str);
                }
                catch (Exception ex)
                {
                    storeCfgJson = JsonConvert.DeserializeObject<StoreCfgJson>(str.Substring(1));
                }
                return storeCfgJson;
            }
            catch (Exception ex)
            {
                return (StoreCfgJson)null;
            }
        }

        public StoreCfgJson EnforceValidStoredSetting(StoreCfgJson SS_new) => SS_new;

        public string LoadTextFromFile(string filename, bool isAppDataFile = false)
        {
            try
            {
                string data = File.ReadAllText(filename);
                this.DecryptIfNecessary(ref data);
                return data == "" ? "" : data;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public void DecryptIfNecessary(ref string data)
        {
            if (data == "" || data.Substring(0, 20).IndexOf("CurrentHash") != -1)
                return;
            data = Encryptor.DECODE(data);
        }

        private void SaveToFile(object newsettings, string filename, bool encrypted)
        {
            try
            {
                Console.WriteLine("CURRENT SETTINGS SAVED TO FILE");
                string str = JsonConvert.SerializeObject(newsettings);
                if (encrypted)
                {
                    if (GeneralTab.Instance.MacMode.IsChecked.Value)
                        str = "\uFEFF" + str;
                    str = Encryptor.ENCODE(Encoding.Unicode.GetBytes(str));
                }
                File.WriteAllText(filename, str);
            }
            catch (Exception ex)
            {
                Console.Write("Error when save to file");
                Console.Write(ex.Message);
            }
        }
    }
}
