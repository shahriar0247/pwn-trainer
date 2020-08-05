using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.sun.org.apache.xml.@internal.security;
using java.lang;
using java.util.logging;
using Memory;

namespace Pwn_trainer_1
{
    public partial class Form1 : Form
    {

        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        public Mem Memory = new Mem();

        public Form1()
        {
            InitializeComponent();



        }

        void change_mana(string value)
        {
            Memory.WriteMemory("pwn3.exe+0x01900600,AC,3C,20C,E4,18,3E0,BC", "int", value);

        }

        void change_height(string value)
        {
            Memory.WriteMemory("pwn3.exe+0x01900600,20,64,720,318,238,114,98", "float", value);
        }
        float get_height()
        {
            return Memory.ReadFloat("pwn3.exe+0x01900600,20,64,720,318,238,114,98");
        }

        void change_x(string value)
        {
            Memory.WriteMemory("pwn3.exe+0x01900600,20,64,720,318,238,114,94", "float", value);
        }
        void change_y(string value)
        {
            Memory.WriteMemory("pwn3.exe+0x01900600,20,64,720,318,238,114,90", "float", value);
        }

        float get_rotation()
        {
            return Memory.ReadFloat("pwn3.exe+0x019019A0,44C,C0,3C8,4,108,2FC,254");
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            change_mana(textBox1.Text);
        }

        int getting_x_from_rotation(double x)
        {
            float temp_y = (float)(0.006173 * x * x - 2.222 * x + 100.0);
            int y = (int)MathF.Round(temp_y, 0);
            return y;
          
        }
        int getting_y_from_rotation(float x)
        {
            float temp_y = MathF.Sin(x * (MathF.PI) / 180) * 100;
            int y = (int)MathF.Round(temp_y, 0);
            return y;
        }

        public static Runnable getkey()
        {
            Mem Memory = new Mem();
            int ProcessID = Memory.GetProcIdFromName("pwn3.exe");

            Memory.OpenProcess(ProcessID);

            float height = Memory.ReadFloat("pwn3.exe+0x01900600,20,64,720,318,238,114,98");
            
            
            float new_y = 0;
            float new_x = 0;
            float rotation = Memory.ReadFloat("pwn3.exe+0x019019A0,44C,C0,3C8,4,108,2FC,254");
            float temp_x = 0;
            float temp_y = 0;
            float current_y =0;
            float current_x =0;
            bool lock_height = false;
            while (true)
            {
                current_x = Memory.ReadFloat("pwn3.exe+0x01900600,20,64,720,318,238,114,94");
                current_y = Memory.ReadFloat("pwn3.exe+0x01900600,20,64,720,318,238,114,90");
                rotation = Memory.ReadFloat("pwn3.exe+0x019019A0,44C,C0,3C8,4,108,2FC,254");
                height = Memory.ReadFloat("pwn3.exe+0x01900600,20,64,720,318,238,114,98");


                Thread.sleep(5);
                for (int i = 32; i < 127; i++)
                {
                   
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 32768)
                    {
                        if (i == 32 && lock_height == true)
                        {
                            height = Memory.ReadFloat("pwn3.exe+0x01900600,20,64,720,318,238,114,98");
                            height = height + 100;
                         
                        }

                        
                        if (i == 48)
                        {
                            lock_height = false;
                        }
                        if (i == 57)
                        {
                            lock_height = true;
                        }

                        if ((Char)i == 'E')
                        {
                            current_x = Memory.ReadFloat("pwn3.exe+0x01900600,20,64,720,318,238,114,94");
                           
                           
                            temp_x = MathF.Sin(rotation * (MathF.PI) / 180) * 100;
                            new_x = current_x + temp_x;

                         
                        }

                        if ((Char)i == 'Q')
                        {

                            current_y = Memory.ReadFloat("pwn3.exe+0x01900600,20,64,720,318,238,114,90");

                            temp_y = MathF.Sin((rotation - 90) * (MathF.PI) / 180) * 100;
                            new_y = current_y + -temp_y;
                            Debug.WriteLine("rotation: " + rotation + "temp_y: " + temp_y);

                        }


                        if ((Char)i == 'W')
                        {
                            current_y = Memory.ReadFloat("pwn3.exe+0x01900600,20,64,720,318,238,114,90");

                            temp_y = MathF.Sin((rotation - 90) * (MathF.PI) / 180) * 100;
                            new_y = current_y + -temp_y;


                            current_x = Memory.ReadFloat("pwn3.exe+0x01900600,20,64,720,318,238,114,94");


                            temp_x = MathF.Sin(rotation * (MathF.PI) / 180) * 100;
                            new_x = current_x + temp_x;

                        }
                            //if ((Char)i == 'E')
                            //{

                            //    new_x = current_x + 100;


                            //}

                            //if ((Char)i == 'Q')
                            //{

                            //    new_y = current_y + 100;

                            //}
                            if ((Char)i == 'R')
                        {
                            Memory.WriteMemory("pwn3.exe+0x01900600,20,64,720,318,238,114,94", "float", "0");
                            Memory.WriteMemory("pwn3.exe+0x01900600,20,64,720,318,238,114,90", "float", "0");
                            Memory.WriteMemory("pwn3.exe+0x01900600,20,64,720,318,238,114,98", "float", "5005");
                        }

                    }
                    //Memory.WriteMemory("pwn3.exe+0x01900600,20,64,720,318,238,114,98", "float", height.ToString());
                  

                    if (lock_height == true)
                    {
                        Memory.WriteMemory("pwn3.exe+0x01900600,20,64,720,318,238,114,98", "float", height.ToString());
                        Memory.WriteMemory("pwn3.exe+0x01900600,20,64,720,318,238,114,94", "float", new_x.ToString());
                        Memory.WriteMemory("pwn3.exe+0x01900600,20,64,720,318,238,114,90", "float", new_y.ToString());
                    }
                }
            }
        }
        public static Runnable constheight()
        {
            Mem Memory = new Mem();
            int ProcessID = Memory.GetProcIdFromName("pwn3.exe");

            Memory.OpenProcess(ProcessID);
            while (true)
            {
                float height = Memory.ReadFloat("pwn3.exe+0x01900600,20,64,720,318,238,114,98");
                Memory.WriteMemory("pwn3.exe+0x01900600,20,64,720,318,238,114,98", "float", height.ToString());
                Thread.sleep(10);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
       {
            int ProcessID = Memory.GetProcIdFromName("pwn3.exe");

            if (ProcessID != null)
                Memory.OpenProcess(ProcessID);


            Thread thread = new Thread(getkey());
            thread.start();


        }
    }
}
