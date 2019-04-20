using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace UacControlPanel
{
    public class Engine
    {
        public enum STATO_UAC { DISABILITATO=0, ABILITATO=1 };
        public enum STATO_ELEVAMENTO { DISABILITATO=0, PASSWORD=1, ABILITATO=2 };
        STATO_UAC statoUac;
        STATO_ELEVAMENTO statoElevamento;
        private RegistryKey chiave;
        public Engine() {
            verificaSistema();
            chiave=Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
            int uac=(int) chiave.GetValue("EnableLUA");
            int elevamento=(int) chiave.GetValue("ConsentPromptBehaviorAdmin", 2);
            statoUac=(STATO_UAC) Enum.ToObject(typeof(STATO_UAC), uac);
            statoElevamento=(STATO_ELEVAMENTO) Enum.ToObject(typeof(STATO_ELEVAMENTO), elevamento);
        }

        private void verificaSistema()
        {
            System.OperatingSystem os;
            os = System.Environment.OSVersion;
            if (os.Platform != System.PlatformID.Win32NT || os.Version.Major != 6 || os.Version.Minor != 0)
                throw new Exception("Sistema operativo non supportato.");
        }

        public STATO_UAC getUac() { return statoUac; }
        public STATO_ELEVAMENTO getElevamento() {return statoElevamento;}


        public bool disabilitaUac()
        {
            bool riavvio = false;
            if (statoUac != STATO_UAC.DISABILITATO)
            {
                riavvio = true;
                chiave.SetValue("EnableLUA", 0);
                statoUac = STATO_UAC.DISABILITATO;
            }
            return riavvio;
        }

        public bool abilitaUac(STATO_ELEVAMENTO e)
        {
            bool riavvio = false;
            if (statoUac != STATO_UAC.ABILITATO)
            {
                statoUac = STATO_UAC.ABILITATO;
                chiave.SetValue("EnableLUA", 1);
                riavvio = true;
            }
            statoElevamento = e;
            chiave.SetValue("ConsentPromptBehaviorAdmin", (int)e);
            return riavvio;
        }
    }

}
