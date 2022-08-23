using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Policy;
using System.Diagnostics;

namespace MovieRecommendationEngine
{
    public partial class Form1 : Form
    {
        public string text = "Insert your text";
        public bool edittext;
        public Form1()
        {
            InitializeComponent();
            textBox2.Text = text;
            textBox2.ForeColor = Color.Black;
        }
        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

		}
        private void TextBox3_TextChanged(object sender, EventArgs e)
        {

		}
        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
			textBox4.ReadOnly = true;
		}
		private void TextBox5_TextChanged(object sender, EventArgs e)
        {
			textBox5.ReadOnly = true;
		}
        private void TextBox6_TextChanged(object sender, EventArgs e)
        {

		}
		private void TextBox1_TextChanged_1(object sender, EventArgs e)
		{
			textBox1.ReadOnly = true;
		}
		private void Txt_Enter(object sender, EventArgs e)
        {
            if (!edittext)
            {
                textBox2.Clear();
                textBox2.ForeColor = Color.Black;
            }
        }
		private void textBox7_TextChanged_1(object sender, EventArgs e)
		{

		}

		private void Button_click(object sender, EventArgs e)
        {
            MakeFile();
            
            string[] lines = File.ReadLines("C:\\Users\\Konstantin\\source\\repos\\AI_Project\\AI_Project\\Movies.txt").ToArray();
            string checkGender = CheckMaleOrFemale(lines[0]);
            textBox1.Text = checkGender;

            string posOrNeg = IstreamerordNegativeOrPositive(lines[1]);
            textBox4.Text = posOrNeg;

            List<string> movie = new(lines);
            movie.RemoveAt(0);
            movie.RemoveAt(0);

            string result = String.Join(" ", movie.ToArray());
            textBox5.Text = result;

            string finalmovie;
			
            
            if (textBox4.Text == "Negative")
            {
                string neg = textBox1.Text;
                finalmovie = ScrapeMovie(neg);
                textBox6.Text = finalmovie;

			}
            else
            {
                finalmovie = ScrapeMovie(result);
                textBox6.Text = finalmovie;
				textBox6.Text = finalmovie;
			}

			// Tried to scrape the places where we could watch a movie. Sometimes it works and sometimes it doesn't - don't know why.

			string movieoffer;

			if (textBox7.Text == "Negative")
			{
				string neg = textBox7.Text;
				movieoffer = ScrapeMovieOffer(neg);
				textBox6.Text = movieoffer;

			}
			else
			{
				movieoffer = ScrapeMovieOffer(result);
				textBox7.Text = finalmovie;
				textBox7.Text = movieoffer;
			}
			

		}

		//basically writes the text file with the information that was given by the user in the text box
        public void MakeFile()
        {
            StreamWriter streamer = new("C:\\Users\\Konstantin\\source\\repos\\AI_Project\\AI_Project\\Movies.txt");

            string words = textBox2.Text;

            string[] space = words.Split(' ');

            foreach (var item in space)
            {
                streamer.WriteLine(item);
            }

            streamer.Close();
        }

		//Gender API that checks if the person is male/female - Ethical issue is that some names are detected as male and female
        public static string CheckMaleOrFemale(string gender)
        {
            string data = GetAPIDataForGender(gender);
            List<string> info = ParseDataForGender(data);
            return info[0];
        }
        public static string IstreamerordNegativeOrPositive(string word)
        {
            string[] postreamerords = {"like","likes", "love", "loves", "adore", "adores", 
                 "want", "wants", "need", "needs", "enjoy", "enjoys", "prefer", "prefers" };

            if (postreamerords.Contains(word))
            {
                return "Positive";
            }
            else
            {
                return "Negative";
            }
        }
		private static List<String> ParseDataForGender(string data)
		{
			List<String> information = new();
			dynamic jsonData = JObject.Parse(data);
			string probableGender = jsonData.gender;
			information.Add(probableGender);
			return information;
		}
		private static string GetAPIDataForGender(string name)
        {
			WebRequest wr = WebRequest.Create("https://api.genderize.io?name=" + name);
            WebResponse response = wr.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            System.IO.StreamReader dataReader = new(stream);
            string allData = dataReader.ReadToEnd();
            return allData;
        }
		public static string ScrapeMovie(string movie)
		{
			HtmlAgilityPack.HtmlWeb website = new();
			HtmlAgilityPack.HtmlDocument document = website.Load("https://www.whatismymovie.com/results?text=" + movie);
			var data = document.DocumentNode.SelectNodes("//div[@class='col-sm-7']").ToList();

			foreach (var item in data)
			{
				if (true)
				{
					string moviefinal = data[0].InnerText;
					return "Movie: " + moviefinal;
				}
			}
			return "No movie was found";
		}

		//I tried to scrape the where the movie is watchable but it seems to be a bug that I couldn't resolve yet

		public static string ScrapeMovieOffer(string provider)
		{
			HtmlAgilityPack.HtmlWeb website = new();
			HtmlAgilityPack.HtmlDocument document = website.Load("https://www.metacritic.com/movie/" + provider);
			var data = document.DocumentNode.SelectNodes("//div[@class='esite_items clearfix list_expand_wrapper']").ToList();

			foreach (var item in data)
			{
				if (true)
				{
					string movieProvider = data[0].InnerText;
					return "Movie Provider: " + movieProvider;
				}
			}
			return "No provider was found";
		}
	}
}
