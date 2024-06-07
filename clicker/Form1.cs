using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clicker
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        // Определяем структуру для хранения информации о вводе
        struct INPUT
        {
            public uint Type;
            public MOUSEINPUT MouseInput;
        }

        // Определяем структуру для хранения информации о клике мыши
        struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        // Константы для имитации клика мыши
        const uint INPUT_MOUSE = 0x0000;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;

        private bool isStopped = false;

        public Form1()
        {
            InitializeComponent();

            // Добавляем обработчик события нажатия клавиш
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем, была ли нажата клавиша Q для аварийной остановки
            if (e.KeyCode == Keys.Q)
            {
                isStopped = true;
            }

            // Проверяем, были ли нажаты клавиши Ctrl и Shift, и клавиша D
            if (e.Control && e.Shift && e.KeyCode == Keys.D)
            {
                // Вызываем метод PerformClick для кнопки button1
                button1.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем значения из текстовых полей
            int numClicks = int.Parse(textBoxClicks.Text);
            int delay = int.Parse(textBoxDelay.Text);

            // Задержка в 3 секунды перед началом кликов
            Thread.Sleep(3000);

            // Выполняем клики
            for (int i = 0; i < numClicks; i++)
            {
                if (isStopped)
                {
                    MessageBox.Show("AutoClicker stopped");
                    return;
                }

                // Получаем текущую позицию курсора мыши
                Point cursorPosition = Cursor.Position;

                // Имитируем клик левой кнопкой мыши
                INPUT input = new INPUT();
                input.Type = INPUT_MOUSE;
                input.MouseInput.X = cursorPosition.X;
                input.MouseInput.Y = cursorPosition.Y;
                input.MouseInput.Flags = MOUSEEVENTF_LEFTDOWN;
                SendInput(1, new INPUT[] { input }, Marshal.SizeOf(input));
                input.MouseInput.Flags = MOUSEEVENTF_LEFTUP;
                SendInput(1, new INPUT[] { input }, Marshal.SizeOf(input));

                // Задержка перед следующим кликом
                Thread.Sleep(delay);
            }

            // Выводим сообщение о завершении работы автокликера
            MessageBox.Show("AutoClicker finished");
        }
    }
}
