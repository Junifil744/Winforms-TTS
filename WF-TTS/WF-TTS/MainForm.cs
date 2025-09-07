using NAudio.Wave;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace WF_TTS
{
    public partial class MainForm : Form {
        // Windows API functions
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        // Class-Wide variables
        private SpeechSynthesizer synth = new SpeechSynthesizer();
        private IntPtr lastWindow;
        private string sVoice = "Microsoft Zira Desktop";
        private int sCable = 0;
        private int sDevice = 0;
        private readonly string configFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WF-TTS");

        // My own functions
        private void writeConfig(int indexID, string value) {
            if (!Directory.Exists(configFolder)) Directory.CreateDirectory(configFolder);
            if (!File.Exists(configFolder + "\\tts.config")) {
                File.WriteAllText(configFolder + "\\tts.config", $"{sVoice}\n{sCable}\n{sDevice}");
            }
            string[] arrLine = File.ReadAllLines(configFolder + "\\tts.config");
            arrLine[indexID] = value;
            File.WriteAllLines(configFolder + "\\tts.config", arrLine);
        }
        private string readConfig(int indexID) {
            if (!File.Exists(configFolder + "\\tts.config")) return null;
            return File.ReadAllLines(configFolder + "\\tts.config")[indexID];
        }

        private void RefreshDevices() {
            ComboBox cableBox = (ComboBox)Controls.Find("cableBox", true)[0];
            ComboBox deviceBox = (ComboBox)FindForm().Controls.Find("deviceBox", true)[0];
            cableBox.Items.Clear();
            deviceBox.Items.Clear();
            for (int dev = 0; dev < WaveOut.DeviceCount; dev++) {
                var caps = WaveOut.GetCapabilities(dev);
                cableBox.Items.Add(dev + " - " + caps.ProductName);
                deviceBox.Items.Add(dev + " - " + caps.ProductName);
            }
            cableBox.SelectedIndex = 0;
            deviceBox.SelectedIndex = 0;
            return;
        }

        // Winforms Functions
        public MainForm() {
            InitializeComponent();
            RegisterHotKey(Handle, 9000, 0x0002, Keys.T);
            foreach (InstalledVoice voice in synth.GetInstalledVoices()) voiceBox.Items.Add(voice.VoiceInfo.Name);
            for (int dev = 0; dev < WaveOut.DeviceCount; dev++) {
                var caps = WaveOut.GetCapabilities(dev);
                cableBox.Items.Add(dev + " - " + caps.ProductName);
                deviceBox.Items.Add(dev + " - " + caps.ProductName);
            }

            // Setting the Voice, defaulting to the first available voice, if impossible, exit the app.
            try {
                sVoice = synth.GetInstalledVoices()[0].VoiceInfo.Name;
                voiceBox.SelectedIndex = 0;
            } catch { /* pass */ }
            string configVoice = readConfig(0);
            if (configVoice != null) {
                sVoice = configVoice;
                for (int i = 0; i < synth.GetInstalledVoices().Count; i++) {
                    if (synth.GetInstalledVoices()[i].VoiceInfo.Name == configVoice) {
                        voiceBox.SelectedIndex = i;
                        break;
                    }
                }
            }
            if (voiceBox.Text == "null") {
                Application.Exit();
            }

            // Verify the integrity of the config file. Previously, this part of code was written by 5am me. It was nightmare.
            if (File.Exists(configFolder + "\\tts.config")) {
                try {
                    if (Convert.ToInt32(readConfig(1)) > WaveOut.DeviceCount) {
                        sCable = 0;
                        cableBox.SelectedIndex = 0;
                        writeConfig(1, "0");
                    }
                    if (Convert.ToInt32(readConfig(2)) > WaveOut.DeviceCount) {
                        sDevice = 0;
                        deviceBox.SelectedIndex = 0;
                        writeConfig(2, "0");
                    }
                } catch {
                    File.WriteAllText(configFolder + "\\tts.config", $"{sVoice}\n{sCable}\n{sDevice}");
                }
            

            // Setting the CABLE device by pulling from the config or defaulting to ID 1
            string configCable = readConfig(1);
            if (configCable != null) {
                sCable = Convert.ToInt32(configCable);
                cableBox.SelectedIndex = Convert.ToInt32(configCable);
            } else {
                sCable = 1;
                cableBox.SelectedIndex = 1;
            }
            // Setting the output device by pulling from the config or defaulting to ID 0
            string configDevice = readConfig(2);
            if (configDevice != null) {
                sDevice = Convert.ToInt32(configDevice);
                deviceBox.SelectedIndex = Convert.ToInt32(configDevice);
            } else {
                sDevice = 0;
                deviceBox.SelectedIndex = 0;
            }
        }
        private void keyDown(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.Enter) {
                ((TextBox)sender).FindForm().Hide();
                if (lastWindow != IntPtr.Zero) {
                    SetForegroundWindow(lastWindow);
                }
                using (MemoryStream ms = new MemoryStream()) {
                    synth.SelectVoice(sVoice);
                    synth.SetOutputToWaveStream(ms);
                    synth.Speak(((TextBox)sender).Text);
                    synth.SetOutputToDefaultAudioDevice();

                    ms.Position = 0;

                    using (var rdr = new WaveFileReader(ms)) {
                        using (var out1 = new WaveOutEvent { DeviceNumber = sDevice })
                        using (var out2 = new WaveOutEvent { DeviceNumber = sCable }) {
                            var buffer = new byte[rdr.Length];
                            rdr.Read(buffer, 0, buffer.Length);

                            var stream1 = new RawSourceWaveStream(new MemoryStream(buffer), rdr.WaveFormat);
                            var stream2 = new RawSourceWaveStream(new MemoryStream(buffer), rdr.WaveFormat);
                            try {
                                out1.Init(stream1);
                                out2.Init(stream2);
                            } catch {
                                RefreshDevices();
                                e.Handled = true;
                                e.SuppressKeyPress = true;
                                return;
                            }
                            out1.Play();
                            out2.Play();
                            while (out1.PlaybackState == PlaybackState.Playing || out2.PlaybackState == PlaybackState.Playing) { }
                        }
                    }
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            if (e.KeyData == Keys.Escape) {
                ((TextBox)sender).FindForm().Close();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        protected override void WndProc(ref Message m) {
            const int WM_HOTKEY = 0x0312;

            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == 9000) {
                lastWindow = GetForegroundWindow();
                TextBox speechTxtbox = new TextBox();
                Form SpeechForm = new Form();
                #region just fucking generating the speech window
                SpeechForm.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                SpeechForm.AutoScaleMode = AutoScaleMode.Font;
                SpeechForm.ClientSize = new System.Drawing.Size(1000, 60);
                SpeechForm.ControlBox = false;
                SpeechForm.Controls.Add(speechTxtbox);
                SpeechForm.FormBorderStyle = FormBorderStyle.None;
                SpeechForm.MaximumSize = new System.Drawing.Size(1000, 60);
                SpeechForm.MinimumSize = new System.Drawing.Size(1000, 60);
                SpeechForm.ShowIcon = false;
                SpeechForm.ShowInTaskbar = false;
                SpeechForm.TopMost = true;
                SpeechForm.Tag = this;

                speechTxtbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                speechTxtbox.Location = new System.Drawing.Point(12, 12);
                speechTxtbox.Size = new System.Drawing.Size(976, 38);
                speechTxtbox.TabIndex = 0;
                speechTxtbox.KeyDown += new KeyEventHandler(keyDown);

                SpeechForm.ResumeLayout(false);
                SpeechForm.PerformLayout();
                #endregion
                SpeechForm.Show();
                SetForegroundWindow(Handle);
                speechTxtbox.Focus();
                SpeechForm.Location = new System.Drawing.Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - 500, 20);
            }

            base.WndProc(ref m);
        }
        private void OnFormClosing(object sender, FormClosingEventArgs e) {
            UnregisterHotKey(Handle, 9000);
        }

        private void changeVoice(object sender, EventArgs e) {
            sVoice = ((ComboBox)sender).SelectedItem.ToString();
            writeConfig(0, sVoice);
        }
        private void cableChanged(object sender, EventArgs e) {
            sCable = ((ComboBox)sender).SelectedIndex;
            writeConfig(1, sCable.ToString());
        }

        private void deviceChanged(object sender, EventArgs e) {
            sDevice = ((ComboBox)sender).SelectedIndex;
            writeConfig(2, sDevice.ToString());
        }

        private void configNuke_Click(object sender, EventArgs e) {
            if (Directory.Exists(configFolder)) Directory.Delete(configFolder, true);
        }

        private void deviceRefresh_Click(object sender, EventArgs e) {
            RefreshDevices();
        }


    }
}
