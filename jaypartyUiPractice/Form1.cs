using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace jaypartyUiPractice
{
    public partial class Form1 : Form
    {

        XmlDocument jaydata = new XmlDocument();
        string query = null;
        string userInput = null;
        int value = 0;
        int runningTotal = 0;
        string categoryName;
        Size buttonSize = new Size(75, 23);
        List<question> questions = new List<question>();
        Label instructionsLabel = new Label();
        Label total = new Label();
        TextBox textBox1 = new TextBox();
        Label displayLabel = new Label();
        Label pointsBox = new Label();
        Rectangle screen;
        int categoryNumber = 0;
        int questionPositionOrder = 0;


        public Form1()
        {
            InitializeComponent();
            screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
        }


        List<string> catagories = new List<string>();
        private void Form1_Load(object sender, EventArgs e)
        {
            jaydata.Load("C:\\Users\\Becky\\Desktop\\JayParty data file.xml");

            Point startingPosition = new Point(12, 49);

            Button submit = new Button();

            submit.Size = new Size(75, 23);
            submit.Text = "subMit";
            total.Text = "Total";
            instructionsLabel.Text = "Please select a value.";
            this.AcceptButton = submit;
            int questionNumber = 0;
            string possibleQuestion;
            displayLabel.Location = new Point(1, 1);
            displayLabel.SendToBack();


            foreach (XmlNode node in jaydata.SelectNodes("//Category"))
            {


                Category category = new Category();
                categoryName = node.Attributes["name"].Value;
                Label createLable = new Label();
                createLable.Text = categoryName;
                createLable.Name = categoryName;
                createLable.Size = new Size(45, 13);
                createLable.Location = new Point(18 + categoryNumber * buttonSize.Width, 50);
                questionPositionOrder = 0;
                this.Controls.Add(createLable);

                foreach (XmlNode childNode in node.ChildNodes)
                {
                    int currentPossible = 0;
                    question question = new question();
                    question.rootCategory = node.Attributes["name"].Value;
                    question.questionText = childNode.SelectNodes("//questionText")[questionNumber].InnerText;
                    foreach (XmlNode possiblity in childNode.SelectNodes("//questionText")[questionNumber].ChildNodes)
                    {
                        possibleQuestion = childNode.SelectNodes("//questionText")[questionNumber].ChildNodes[currentPossible].InnerText.ToString();
                        question.possibleQuestions.Add(possibleQuestion);
                        currentPossible++;
                    }
                    question.answerText = childNode.SelectNodes("//answer")[questionNumber].InnerText;
                    question.questionValue = childNode.Attributes.GetNamedItem("value").Value.ToString();
                    Button createButton = new Button();

                    createButton.Size = buttonSize;
                    createButton.Text = question.questionValue;
                    createButton.Location = new Point(categoryNumber * buttonSize.Width + 12, questionPositionOrder * buttonSize.Height + 69);


                    createButton.Click += delegate
                    {
                        if (createButton.BackColor == System.Drawing.Color.Blue)
                        { }
                        else
                        {
                            displayLabel.Text = question.answerText;
                            displayLabel.Font = new Font("arial", 15);
                            displayLabel.BringToFront();
                            displayLabel.BackColor = System.Drawing.Color.Blue;
                            query = question.questionText;
                            createButton.BackColor = System.Drawing.Color.Blue;
                            createButton.Text = null;
                            value = int.Parse(question.questionValue);
                            textBox1.Focus();
                        }
                    };
                    this.Controls.Add(createButton);
                    questionNumber++;
                    questionPositionOrder++;

                }

                this.Controls.Add(instructionsLabel);
                this.Controls.Add(textBox1);
                this.Controls.Add(pointsBox);
                this.Controls.Add(total);
                this.Controls.Add(submit);
                this.Controls.Add(displayLabel);
                categoryNumber++;
                int categoryCount = jaydata.SelectNodes("//Category").Count;
            }
            submit.Click += delegate
            {
                userInput = textBox1.Text;
                if (userInput.ToLower().ToString() == query)
                {
                    instructionsLabel.Text = "Well done! Please select another value.";
                    runningTotal = runningTotal + value;
                    pointsBox.Text = runningTotal.ToString();
                    textBox1.Text = null;
                    query = null;
                    value = 0;

                }
                else
                {
                    instructionsLabel.Text = "YOU SUCK!!! Please select another value.";
                    runningTotal = runningTotal - value;
                    pointsBox.Text = runningTotal.ToString();
                    textBox1.Text = null;
                    query = null;
                    value = 0;
                }
                displayLabel.Text = null;
                displayLabel.BackColor = System.Drawing.Color.White;
                displayLabel.SendToBack();
            };
            this.Size = new Size(buttonSize.Width * categoryNumber + 40, 27 / 2 * buttonSize.Height + buttonSize.Height * questionPositionOrder);
            submit.Location = new Point(this.Size.Width / 2 - submit.Width / 2 - 5, 138 + buttonSize.Height * questionPositionOrder);
            instructionsLabel.Location = new Point(this.Size.Width / 2 - 70, 9);
            instructionsLabel.AutoSize = false;
            instructionsLabel.Size = new Size(this.Size.Width - this.Size.Width/2, 30);
            textBox1.Location = new Point(12, 167 + buttonSize.Height * questionPositionOrder);
            textBox1.Size = new Size(buttonSize.Width * categoryNumber, 20);
            total.Location = new Point(32, 205 + buttonSize.Height * questionPositionOrder);
            pointsBox.Location = new Point(69, 205 + buttonSize.Height * questionPositionOrder);
            displayLabel.Size = new Size(this.Size.Width - 10, this.Size.Height / 2);

        }

    }
}
