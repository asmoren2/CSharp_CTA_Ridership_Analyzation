//
// Authors: Chris Hong, Adolfo Moreno
// Assignment: Final Project CTA using Linq
// Date: 11/30/15
// University of IL. Chicago
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final341
{
  public partial class Form1 : Form
  {
    private CtaDataContext c_cta;
  

    public Form1()
    {
      InitializeComponent();
      c_cta = new CtaDataContext();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      var query = from c in c_cta.Stations
                  orderby c.Name
                  select c;                             // Selects all station objects from Stations

      foreach (var c in query)                          // Iterates through each one and dispays names
      {
        this.listBox1.Items.Add(c.Name);
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      var query = (from c in c_cta.Riderships select c.DailyTotal).Count();
      string message = String.Format("{0}", query);
      this.listBox1.Items.Add(message);
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.listBox2.Items.Clear();                      // Clears Stops listBox
      this.textBox1.Text = "";                          // Clears Handicap textBox
      this.textBox2.Text = "";                          // Clears Direction textBox
      this.textBox3.Text = "";                          // Clears Location textBox
      this.listBox3.Items.Clear();                      // Clears Color listBox
      this.textBox4.Text = "";                          // Clears Total Ridership textBox
      this.textBox5.Text = "";                          // Clears Average Ridership textBox

      var query = from c in c_cta.Stations
                  where c.Name == this.listBox1.Text
                  join x in c_cta.Stops on c.StationID equals x.StationID
                  select new
                  {
                    StationID = x.StationID,
                    StopName = x.Name
                  };                                    // Selects the StopName to the 
                                                        // corresponding StationName

      foreach(var x in query)                           // Fills listBox2 with data
      {
        this.listBox2.Items.Add(x.StopName);
      }

      var stationID = query.First();                    // First thing selected from query

      var query1 = from c in c_cta.Riderships
                   where c.StationID == stationID.StationID
                   group c by c.StationID into x
                   select new
                   {
                     totalRidership = x.Sum(w => w.DailyTotal),
                     averageRidership = x.Average(y => y.DailyTotal)
                   };                                             // Sums up total ridership and
                                                                  // calculate average ridership

      var ridership = query1.First();                             // First thing selected from query1

      this.textBox4.Text = ridership.totalRidership.ToString();   // Total ridership for current station
      
      this.textBox5.Text = ridership.averageRidership.ToString(); // Average ridership for current station

    }

    private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.textBox1.Text = "";                          // Clears Handicap textBox
      this.textBox2.Text = "";                          // Clears Direction textBox
      this.textBox3.Text = "";                          // Clears Location textBox
      this.listBox3.Items.Clear();                      // Clears Color listBox

      var query = from c in c_cta.Stops
                  where c.Name == this.listBox2.Text
                  select new
                  {
                    StopID = c.StopID,
                    ADA = c.ADA,
                    direction = c.Direction,
                    latitude = c.Latitude,
                    longitude = c.Longitude
                  };                                    // Selects information for textBox fields
                
      var currStop = query.First();                     // First thing selected from query

      this.textBox1.Text = currStop.ADA.ToString();     // Adds true or false to handicap textBox
      
      this.textBox2.Text = currStop.direction.ToString(); // Direction for current stop in direction textBox

      this.textBox3.Text = "(" + currStop.latitude.ToString() + ", " + currStop.longitude.ToString() +")";
                                                          // Location for current stop in location textBox

      var query1 = from c in c_cta.StopDetails
                where c.StopID == currStop.StopID
                join x in c_cta.Lines on c.LineID equals x.LineID
                select new
                {
                  color = x.Color
                };                                      // Selects color for current stopID

      foreach(var line in query1)                       // Adds each color for this certain stop
      {
        this.listBox3.Items.Add(line.color);
      }

    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {

    }

    private void button2_Click_1(object sender, EventArgs e)
    {
      var query = from c in c_cta.Stations
                  where c.Name == this.textBox6.Text
                  select c;                             // Looks for station name in Stations
                                                        // that matches listBox6

      if(query.Count() <= 0)                            // Checks if query is empty
      {
        MessageBox.Show(this.textBox6.Text + " was not found");
      }

      else
      {
        MessageBox.Show(query.First().Name + " was found");
      }
    }
  }
}
