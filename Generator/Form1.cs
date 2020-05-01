using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Generator
{
    public partial class Form1 : Form
    {
        OleDbConnection cn = new OleDbConnection();
        MySqlConnection conexion = new MySqlConnection();

        MySqlCommand myCommand = new MySqlCommand();
        MySqlDataReader myReader;

        DataTable schemaTable;
        string tableSelected = "";

        string TableId = "";
        string col_name = "";
        string col_type = "";
        string col_ordinal_value = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Provider=MySql.Data.MySqlClient;Server=localhost;Database=inventory; Uid=root;Pwd=;
            //Provider = SQLOLEDB; Data Source = devlgc.database.windows.net; User ID = adminuser; Password = C3st4r.01; Initial Catalog = LetsGoCanada
            this.txtConeccion.Text = "Server=localhost;Database=inventory; Uid=root;Pwd=";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!txtConeccion.Text.Equals(""))
            {
                this.getTables();
            }
        }

        /**
         * @author Diego.Perez
         * @date 04/25/2020
         **/
        private void getTables()
        {
            try
            {
                conexion.ConnectionString = txtConeccion.Text;
                conexion.Open();

                DataTable dt = conexion.GetSchema("Tables");
                gridTables.DataSource = dt;

                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerator_Click(object sender, EventArgs e)
        {
            conexion.Open();

            myCommand.Connection = conexion;
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT * FROM " + tableSelected + ";";
            myReader = myCommand.ExecuteReader();

            //get the schema from the datatable column
            schemaTable = myReader.GetSchemaTable();
            gridTables.DataSource = schemaTable;

            generateDbModel();
            generateModel();
            generateRouter();
            generateApp();

            conexion.Close();

            MessageBox.Show("Ready completed");
        }

        /**
         * @author Diego.Perez
         * @date 04/28/2020
         **/
        private void generateApp()
        {
            string fileName = "C:\\temp\\ClasesAutogeneradas\\app.ts";

            StreamWriter writer = File.CreateText(fileName);

            writer.WriteLine("const exp = require('express');");
            writer.WriteLine("const bodyParser = require('body-parser');");
            writer.WriteLine("var cors = require('cors');\n");
            writer.WriteLine("const " + tableSelected + "Router = require('./routes/" + tableSelected + "Router');\n");
            writer.WriteLine("var app = exp();");
            writer.WriteLine("app.use(cors());");
            writer.WriteLine("app.use(bodyParser.json());");
            writer.WriteLine("app.use('/" + tableSelected + "', " + tableSelected + "Router);\n");
            writer.WriteLine("let port = 3000;\n");
            writer.WriteLine("app.listen(port, () => console.log(`Listening on http://localhost:${port}/`));");

            writer.Close();

        }

        /**
         * @author Diego.Perez
         * @date 04/28/2020
         **/
        private void generateDbModel()
        {
            string fileName = "C:\\temp\\ClasesAutogeneradas\\db\\db.ts";

            StreamWriter writer = File.CreateText(fileName);

            string conString = this.txtConeccion.Text;
            string temp = "";

            writer.WriteLine("var express = require('express');");
            writer.WriteLine("var mysql = require('mysql');\n");
            writer.WriteLine("var db = mysql.createConnection({");
            if (conString.Contains("Server"))
            {
                temp = conString.Remove(0, conString.IndexOf("Server="));
                temp = temp.Replace("Server=", "");
                temp = temp.Remove(temp.IndexOf(";"), temp.Length - temp.IndexOf(";"));
                writer.WriteLine("\thost: '" + temp + "',");
            }
            else
            {
                writer.WriteLine("\thost: 'localhost',");
            }

            writer.WriteLine("\tuser: 'root',");
            writer.WriteLine("\tpassword: '',");
            if (conString.Contains("Database"))
            {
                //.Remove(conString.IndexOf(';', conString.LastIndexOf('='))
                temp = conString.Remove(0, conString.IndexOf("Database="));
                temp = temp.Replace("Database=", "");
                temp = temp.Remove(temp.IndexOf(";"), temp.Length - temp.IndexOf(";"));
                writer.WriteLine("\tdatabase: '" + temp + "'");
            }
            else
            {
                writer.WriteLine("\tdatabase: 'inventory'");
            }
            writer.WriteLine("});\n");
            writer.WriteLine("db.connect(function (err: any) {");
            writer.WriteLine("\tif (!!err) {");
            writer.WriteLine("\t\tconsole.log('Error');");
            writer.WriteLine("\t} else {");
            writer.WriteLine("\t\tconsole.log('Connected');");
            writer.WriteLine("\t}");
            writer.WriteLine("});\n");

            writer.WriteLine("module.exports = db;");

            writer.Close();
        }

        /**
         * @author Diego.Perez
         * @date 04/28/2020
         **/
        private void generateModel()
        {
            string fileName = "C:\\temp\\ClasesAutogeneradas\\models\\" + tableSelected + ".ts";

            StreamWriter writer = File.CreateText(fileName);

            writer.WriteLine("export class " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tableSelected) + " {\n");

            String dbQuery = "";
            foreach (DataRow myField in schemaTable.Rows)
            {
                foreach (DataColumn myProperty in schemaTable.Columns)
                {
                    if (myProperty.ColumnName == "DataType")
                    {
                        col_type = myField[myProperty].ToString();
                    }

                    if (myProperty.ColumnName == "ColumnName")
                    {
                        col_name = myField[myProperty].ToString();
                    }
                }

                string tempCol = col_type.ToString().Remove(0, col_type.IndexOf('.') + 1);

                if (tempCol.Equals("Int32") || tempCol.Equals("Double"))
                {
                    dbQuery += "\tpublic " + col_name + "?: number;\n";
                }
                else if (tempCol.Equals("String"))
                {
                    dbQuery += "\tpublic " + col_name + "?: string;\n";
                }
                else if (tempCol.Equals("Date"))
                {
                    dbQuery += "\tpublic " + col_name + "?: Date;\n";
                }

            }

            writer.WriteLine(dbQuery);


            writer.WriteLine("\tconstructor() { }");
            writer.WriteLine("}");

            writer.Close();
        }

        /**
         * @author Diego.Perez
         * @date 04/28/2020
         **/
        private void generateRouter()
        {
            //C:\\temp\\ClasesAutogeneradas\\routes\\
            string fileName = "C:\\temp\\ClasesAutogeneradas\\routes\\" + tableSelected + "Router.ts";

            StreamWriter writer = File.CreateText(fileName);

            writer.WriteLine("import { Request, Response } from 'express';");
            writer.WriteLine("import { " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tableSelected) + " } from '../models/" + tableSelected + "';\n");
            writer.WriteLine("const express = require('express');");
            writer.WriteLine("const router = express.Router();");
            writer.WriteLine("const db = require('../db/db');");
            writer.WriteLine("const table: string = '" + tableSelected + "';\n");

            writeComments(writer);
            writeGetMethod(writer);

            writeComments(writer);
            writePostMethod(writer);


            writer.WriteLine("\n");
            writer.WriteLine("module.exports = router;");

            writer.Close();
        }

        /**
         * @author Diego.Perez
         * @date 04/28/2020
         **/
        public void writePostMethod(StreamWriter writer)
        {
            writer.WriteLine("router.post('/', function (req: Request, res: Response) {");
            writer.WriteLine("\tlet " + tableSelected + ": " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tableSelected) + " = new " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tableSelected) + "();");
            writer.WriteLine("\t" + tableSelected + " = req.body;\n");

            String dbQuery = "\tdb.query('INSERT INTO CUSTOMER VALUES (";
            foreach (DataRow myField in schemaTable.Rows)
            {
                foreach (DataColumn myProperty in schemaTable.Columns)
                {
                    if (myProperty.ColumnName == "DataType")
                    {
                        col_type = myField[myProperty].ToString();
                    }

                    if (myProperty.ColumnName == "ColumnName")
                    {
                        col_name = myField[myProperty].ToString();
                    }

                    /*if (myProperty.ColumnName == "ColumnOrdinal")
                    {
                        col_ordinal_value = myField[myProperty].ToString();
                    }*/

                }

                string tempCol = col_type.ToString().Remove(0, col_type.IndexOf('.') + 1);

                if (tempCol.Equals("Int32") || tempCol.Equals("Double"))
                {
                    dbQuery += "' + " + tableSelected + "." + col_name + " + ',";
                }
                else if (tempCol.Equals("String"))
                {
                    dbQuery += " \"' + " + tableSelected + "." + col_name + " + '\",";
                }
                else if (tempCol.Equals("Date"))
                {
                    dbQuery += "Implement the date part that is missing.";
                }
                //writer.WriteLine(col_type.ToString().Remove(0, col_type.IndexOf('.') + 1) + " ->");
                //writer.Write(tableSelected + "." + col_name + " + ', ' + ");

            }

            if (dbQuery.Length == dbQuery.LastIndexOf(',') + 1)
            {
                dbQuery = dbQuery.Remove(dbQuery.LastIndexOf(','));
            }

            dbQuery += ")', (err: any, result: any) => {\n";
            writer.Write(dbQuery);
            writer.WriteLine("\t\tif (err) throw err;");
            writer.WriteLine("\t\telse {");
            writer.WriteLine("\t\t\t" + tableSelected + ".id = result.insertId;");
            writer.WriteLine("\t\t\tlet rslt = {");
            writer.WriteLine("\t\t\t\tdata: " + tableSelected + ",");
            writer.WriteLine("\t\t\t\tmessage: '" + tableSelected + " inteserted correctly.'");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tres.send(rslt);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t});");
            writer.WriteLine("});");

            writer.WriteLine("\n");
        }

        /**
         * @author Diego.Perez
         * @date 04/28/2020
         **/
        public void writeGetMethod(StreamWriter writer)
        {
            writer.WriteLine("router.get('/', function (req: Request, res: Response) {");
            writer.WriteLine("\tdb.query('select * from " + tableSelected + "', (err: any, result: any, fields: any) => {");
            writer.WriteLine("\t\tif (err)");
            writer.WriteLine("\t\t\tthrow err;\n");
            writer.WriteLine("\t\tres.send(result)");
            writer.WriteLine("\t});\n");
            writer.WriteLine("});\n\n");
        }

        /**
         * @author Diego.Perez
         * @date 04/28/2020
         **/
        public void writeComments(StreamWriter writer)
        {
            writer.WriteLine("/**");
            writer.WriteLine(" * @author");
            writer.WriteLine(" * @date");
            writer.WriteLine(" */");
        }

        /**
         * @author Diego.Perez
         * @date 04/28/2020
         **/
        private void gridTables_SelectionChanged(object sender, EventArgs e)
        {
            if (tableSelected.Equals(""))
            {
                string valor1 = (string)gridTables.CurrentRow.Cells["TABLE_NAME"].Value;
                tableSelected = valor1.ToString();
            }

        }

        private void gridTables_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string valor = (string)gridTables.CurrentRow.Cells["TABLE_NAME"].Value;
        }

        private void gridTables_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string valor = (string)gridTables.CurrentRow.Cells["TABLE_NAME"].Value;
            tableSelected = valor.ToString();
        }
    }
}
