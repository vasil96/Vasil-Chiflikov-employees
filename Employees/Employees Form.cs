using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace Employees
{
    public partial class Employees : Form
    {
        public Employees()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Visible = false;
            DialogResult result = openFileDialog1.ShowDialog(); 
            if (result == DialogResult.OK) 
            {

                    textBox1.Text = openFileDialog1.FileName;
      
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = textBox1.Text;
            try
            {
                IList<Info> InfoList = new List<Info>();
                string line;
                int empId;
                int projectId;
                DateTime dateFrom;
                DateTime dateTo;
                int idCounter = 1;
                System.IO.StreamReader file = new System.IO.StreamReader(filePath);
                while ((line = file.ReadLine()) != null)
                {
                    string[] words =line.Split(',');
                    empId = Convert.ToInt32(words[0].Trim());
                    projectId = Convert.ToInt32(words[1].Trim());
                    dateFrom = Convert.ToDateTime(words[2].Trim());
                    if (words[3].Trim() == "NULL")
                        dateTo = DateTime.Today;
                    else
                       dateTo = Convert.ToDateTime(words[3].Trim());

                    Info currentInfo = new Info()
                    {
                        Id = idCounter,
                        EmpID = empId,
                        ProjectID = projectId,
                        DateFrom = dateFrom,
                        DateTo = dateTo
                    };
                    idCounter++;
                    InfoList.Add(currentInfo);
                }

                file.Close();
                var message = String.Empty;
                message = EmployeesMaxTimeInOneProject(InfoList);
                label2.Text = message;
                label2.Visible = true;
                System.Console.WriteLine(message);
                System.Console.ReadLine();
            }
            catch (Exception ex)
            {
                label3.Text = "ERROR: " + ex.Message.ToString();
                label3.Visible = true;
            }
        }


        private string EmployeesMaxTimeInOneProject(IList<Info> infoList)
        {
            var projectList = infoList.Select(emp => emp.ProjectID).Distinct().ToList();
            int firstIdFinal = 0;
            int secondIdFinal = 0;
            int projectIdFinal = 0;
            TimeSpan maxPeriodFinal = TimeSpan.Zero;

            foreach (var p in projectList)
            {
                var elementByProjectList = infoList.Where(emp => emp.ProjectID == p).ToList();
                int firstId = 0;
                int secondId = 0;
                TimeSpan maxPeriod = TimeSpan.Zero;

                foreach (var el1 in elementByProjectList)
                {
                    foreach (var el2 in elementByProjectList)
                    {
                        if (el1.Id == el2.Id || el1.EmpID == el2.EmpID) continue;
                        var period = PeriodCalc(el1.DateFrom, el2.DateFrom, el1.DateTo, el2.DateTo);
                        if (period > maxPeriod)
                        {
                            maxPeriod = period;
                            firstId = el1.EmpID;
                            secondId = el2.EmpID;
                        }
                    }
                }

                if (maxPeriod > maxPeriodFinal)
                {
                    maxPeriodFinal = maxPeriod;
                    firstIdFinal = firstId;
                    secondIdFinal = secondId;
                    projectIdFinal = p;
                }
            }
            string message = "Employee with Id= " + firstIdFinal + " and employee with Id= " + secondIdFinal + " worked most long in a project Id = " + projectIdFinal;
            return message;
        }

        private TimeSpan PeriodCalc (DateTime dateFrom1, DateTime dateFrom2, DateTime dateTo1, DateTime dateTo2)
        {
            TimeSpan period = TimeSpan.Zero;
            DateTime startDate;
            DateTime endDate;

            if (dateFrom1 <= dateFrom2)
                startDate = dateFrom2;
            else startDate = dateFrom1;

            if (dateTo1 <= dateTo2)
                endDate = dateTo1;
            else
                endDate = dateTo2;

            if (endDate >= startDate) 
                period = endDate - startDate;

            return period;
        }
    }
}
