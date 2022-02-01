﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;

namespace Milestonee1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private string bid = "";
        public Window1(string bid)
        {
            InitializeComponent();
            this.bid = String.Copy(bid);
            loadBusinessDetails();
            loadBusinessNumbers();

        }
        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = milestone1db; password= 0722";
        }
        private void executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        myf(reader);


                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL error - " + ex.Message.ToString());

                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        private void setBusinessDetails(NpgsqlDataReader R)
        {
            bname.Text = R.GetString(0);
            state.Text = R.GetString(1);
            city.Text = R.GetString(2);

        }
        void setNumInState(NpgsqlDataReader R)
        {
            numInState.Content = R.GetInt16(0).ToString();
        }
        void setNumInCity(NpgsqlDataReader R)
        {
            numInCity.Content = R.GetInt16(0).ToString();
        }
        private void loadBusinessNumbers()
        {
            //string sqlStr1 = "SELECT count(*) from business WHERE state = (SELECT state FROM business WHERE business_id = '" + this.bid + "');";
            string sqlStr1 = $"SELECT count(*) from business WHERE state = (SELECT state FROM business WHERE business_id = '{this.bid}');";
            executeQuery(sqlStr1, setNumInState);
            //string sqlStr2 = "SELECT COUNT(*) from business WHERE city = (SELECT city FROM business WHERE business_id = '" + this.bid + "');";
            string sqlStr2 = $"SELECT count(*) from business WHERE city = (SELECT city FROM business WHERE business_id = '{this.bid}');";
            executeQuery(sqlStr2, setNumInCity);

        }

        private void loadBusinessDetails()
        {
            //string sqlStr = "SELECT name,state,city FROM business WHERE business_id = '" + this.bid + "';";
            string sqlStr = $"SELECT name,state,city FROM business WHERE business_id = '{this.bid}';";
            executeQuery(sqlStr, setBusinessDetails);
        }


    }
    
}

    


     