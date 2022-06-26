using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GA_DU_M
{
    public partial class Form1 : Form
    {
        Random r = new Random();
        int cel, number;
        int[] kf_ur;//коэффициенты уравнения
        int[,] population, parents;
        uint[,] population_old;
        double[] fitness;//пригодность особей популяции
        public Form1()
        {
            InitializeComponent();
        }
        void CF()//перенос коэффициентов из уравнения в массив
        {
            List<int> K = new List<int>();
            string number = "";
            string DU = textBox1.Text;
            StringBuilder str = new StringBuilder(DU);
            if (Char.IsLetter(DU[0]) == true)
            {
                str[0] = '1';
                DU = str.ToString();
            }
            for (int i = 0; i < DU.Length; i++)
            {
                if (DU[i] == '=')
                {
                    i += 2;
                    while (i != DU.Length)
                    {
                        number += DU[i];
                        i++;
                    }
                    cel = Convert.ToInt32(number);
                    break;
                }
                else
                {
                    if (Char.IsLetter(DU[i]) == true)
                    {
                        if (DU[i - 1] == ' ')
                        {
                            K.Add(1);
                        }
                        else
                        {
                            int z = i - 1;
                            while (DU[z] != ' ')
                            {
                                number += DU[z];
                                z--;
                            }
                            K.Add(Convert.ToInt32(number.Reverse()));
                            number = "";
                        }
                    }
                    else if (Char.IsDigit(DU[i]) == true)
                    {
                        while (Char.IsDigit(DU[i]) == true)
                        {
                            number += DU[i];
                            i++;
                        }
                        K.Add(Convert.ToInt32(number));
                        number = "";
                    }
                }
            }
            kf_ur = K.ToArray();
        }
        void GetPopulation()//создание популяции
        {
            population = new int[Convert.ToInt32(textBox2.Text), kf_ur.Length];
            for (int i = 0; i < Convert.ToInt32(textBox2.Text); i++)
            {
                for (int z = 0; z < kf_ur.Length; z++)
                {
                    population[i, z] = r.Next(1, cel + 1);
                }
            }
        }
        void GetFitness()//расчёт пригодности
        {
            List<double> number = new List<double>();
            fitness = new double[Convert.ToInt32(textBox2.Text)];
            int sum = 0;
            for (int i = 0; i < Convert.ToInt32(textBox2.Text); i++)
            {
                for (int z = 0; z < kf_ur.Length; z++)
                {
                    sum += kf_ur[z] * population[i, z];
                }
                if (sum - cel != 0)
                {
                    number.Add(((kf_ur.Sum() * cel) - Math.Abs(sum - cel)) / Convert.ToDouble((kf_ur.Sum() * cel)));
                }
                else
                {
                    number.Add(1);
                }
                sum = 0;
            }
            for (int i = 0; i < Convert.ToInt32(textBox2.Text); i++)
            {
                fitness[i] = number[i] * 100;
            }
        }
        void Sort()//сортировка особей
        {
            int[,] pop = new int[Convert.ToInt32(textBox2.Text), kf_ur.Length];
            List<double> fit = new List<double>();
            for (int i = 0; i < fitness.Length; i++)
            {
                fit.Add(fitness.Max());
                for (int z = 0; z < kf_ur.Length; z++)
                {
                    pop[i, z] = population[Array.IndexOf(fitness, fitness.Max()), z];
                }
                fitness[Array.IndexOf(fitness, fitness.Max())] = -1;
            }
            population = pop;
            fitness = fit.ToArray();
        }
        void OutPut()//отображение популяции и пригодностей
        {
            label3.Text = "Особи популяции:";
            label9.Text = "Пригодность каждой особи:";
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            string str = "";
            for (int i = 0; i < Convert.ToInt32(textBox2.Text); i++)
            {
                for (int z = 0; z < kf_ur.Length; z++)
                {
                    str += population[i, z] + " ";
                }
                listBox1.Items.Add(str);
                listBox2.Items.Add(fitness[i] + "%");
                str = "";
            }
            label6.Text = Convert.ToString(number);
        }
        void OutPut_VS()//сравнение новой популяции с предыдущей
        {
            label3.Text = "Особи новой популяции:";
            label9.Text = "Особи предыдущей популяции:";
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            string str = "";
            string str_old = "";
            for (int i = 0; i < Convert.ToInt32(textBox2.Text); i++)
            {
                for (int z = 0; z < kf_ur.Length; z++)
                {
                    str += population[i, z] + " ";
                    str_old += population_old[i, z] + " ";
                }
                listBox1.Items.Add(str);
                listBox2.Items.Add(str_old);
                str = "";
                str_old = "";
            }
            label6.Text = Convert.ToString(number);
        }
        void Save_population()//сохранение хромосом текущей популяции
        {
            population_old = new UInt32[population.GetLength(0), kf_ur.Length];
            for (int i = 0; i < population.GetLength(0); i++)
            {
                for (int z = 0; z < kf_ur.Length; z++)
                {
                    population_old[i, z] = Convert.ToUInt32(population[i, z]);
                }
            }
        }
        void NewPopulation()//Новая популяция
        {
            int[,] pop = new int[Convert.ToInt32(textBox2.Text), kf_ur.Length];
            List<double> fit = new List<double>();
            int i = 0, index;
            double f = fitness.Sum() / fitness.Length;
            while (Convert.ToDouble(fitness[i]) >= f)
            {
                for (int z = 0; z < kf_ur.Length; z++)
                {
                    pop[i, z] = population[i, z];
                }
                fit.Add(fitness[i]);
                i++;
                if (i == fitness.Length)
                {
                    break;
                }
            }
            if (i != 0)
            {
                for (int y = 0; y < fitness.Length; y++)
                {
                    index = r.Next(i);
                    for (int z = 0; z < kf_ur.Length; z++)
                    {
                        population[y, z] = pop[index, z];
                    }
                    fitness[y] = fit[index];
                }
            }
        }
        void Crossingover()//оператор кроссовера
        {
            Save_population();
            int y = 0, j = 0, h = 0;
            int r_m;
            parents = new int[Convert.ToInt32(textBox2.Text) * 2, kf_ur.Length];
            for (int i = 0; i < Convert.ToInt32(textBox2.Text) * 2; i++)
            {
                r_m = r.Next(Convert.ToInt32(textBox2.Text));
                if (i % 2 == 0)
                {
                    for (int z = 0; z < kf_ur.Length; z++)
                    {
                        parents[i, z] = population[h, z];
                    }
                    h++;
                }
                else
                {
                    for (int z = 0; z < kf_ur.Length; z++)
                    {
                        parents[i, z] = population[r_m, z];
                    }
                }
            }
            while (y < Convert.ToInt32(textBox2.Text) * 2)
            {
                for (int z = 0; z < kf_ur.Length; z++)
                {
                    if (y + 1 != Convert.ToInt32(textBox2.Text) * 2)
                    {
                        if (parents[y, z] == parents[y + 1, z])
                        {
                            population[j, z] = parents[y, z];
                        }
                        else
                        {
                            if (r.Next(2) == 0)
                            {
                                population[j, z] = parents[y, z];
                            }
                            else
                            {
                                population[j, z] = parents[y + 1, z];
                            }
                        }
                    }
                    else
                    {
                        population[j, z] = parents[y, z];
                    }
                }
                j++;
                y += 2;
            }
        }
        void Mutation()//точечная мутация
        {
            for (int i = 0; i < population.GetLength(0); i++)
            {
                if (r.Next(101) <= Convert.ToInt32(textBox4.Text))//вероятность мутации
                {
                    population[i, r.Next(kf_ur.Length)] = r.Next(1, cel + 1);
                }
            }
        }
        void Auto()//автоматическое достижение цели
        {
            while (fitness[0] != 100)
            {
                Crossingover();
                Mutation();
                GetFitness();
                Sort();
                NewPopulation();
                GetFitness();
                Sort();
                number++;
            }
            OutPut();
        }
        void Cycle()//циклический запуск алгоритма с выбранным количеством итераций
        {
            for (int i = 0; i < Convert.ToInt32(textBox5.Text); i++)
            {
                Crossingover();
                Mutation();
                GetFitness();
                Sort();
                NewPopulation();
                GetFitness();
                Sort();
                number++;
            }
            OutPut();
        }
        private void button1_Click(object sender, EventArgs e)//генерация начальной популяции с вычислением её пригодности
        {
            label3.Visible = true;
            label9.Visible = true;
            CF();
            GetPopulation();
            GetFitness();
            Sort();
            Save_population();
            number = 1;
            OutPut();
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button10.Visible = true;
            button11.Visible = true;
        }
        private void button2_Click(object sender, EventArgs e)//оператор кроссинговера
        {
            Crossingover();
            GetFitness();
            Sort();
            OutPut();
            label3.Text = "Потомки скрещенных особей:";
            label9.Text = "Пригодность каждой особи:";
            button7.Visible = false;
            button11.Visible = true;
        }
        private void button3_Click(object sender, EventArgs e)//мутация
        {
            Mutation();
            GetFitness();
            Sort();
            OutPut();
            label3.Text = "Мутированные особи с вероятностью = " + textBox4.Text + " :";
            label9.Text = "Пригодность каждой особи:";
            button7.Visible = false;
            button11.Visible = true;
        }
        private void button4_Click(object sender, EventArgs e)//новая популяция 
        {
            NewPopulation();
            GetFitness();
            Sort();
            number++;
            OutPut();
            label3.Text = "Особи новой популяции:";
            label9.Text = "Пригодность каждой особи:";
            button7.Visible = false;
            button11.Visible = true;

        }
        private void button11_Click(object sender, EventArgs e)//отображение новой и предыдущей популяций
        {
            OutPut_VS();
            button7.Visible = true;
            button11.Visible = false;
        }
        private void button5_Click(object sender, EventArgs e)//предоставление пользователю выбора циклической генерации
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button10.Visible = false;
            button11.Visible = false;
            label7.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
        }
        private void button6_Click(object sender, EventArgs e)//автоматическое достижение цели
        {
            Auto();
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button10.Visible = true;
            button11.Visible = true;
            label7.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            label8.Visible = false;
            textBox5.Visible = false;
            button8.Visible = false;
        }
        private void button7_Click(object sender, EventArgs e)//выбор количества итераций
        {
            label7.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            label8.Visible = true;
            textBox5.Visible = true;
            button8.Visible = true;
        }
        private void button8_Click(object sender, EventArgs e)//вернуться в первоначальное окно, кнопка "Назад"
        {
            OutPut();
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button10.Visible = true;
            button11.Visible = true;
            label7.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            label8.Visible = false;
            textBox5.Visible = false;
            button8.Visible = false;
        }
        private void button9_Click(object sender, EventArgs e)//подтвердить количество итераций
        {
            Cycle();
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button10.Visible = true;
            button11.Visible = true;
            label7.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            label8.Visible = false;
            textBox5.Visible = false;
            button8.Visible = false;
        }
        private void button10_Click(object sender, EventArgs e)//выход из программы
        {
            Application.Exit();
        }
    }
}