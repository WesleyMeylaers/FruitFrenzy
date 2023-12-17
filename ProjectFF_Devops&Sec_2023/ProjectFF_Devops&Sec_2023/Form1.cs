using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProjectFF_Devops_Sec_2023
{
    public partial class Form1 : Form
    {
        //declaration of the variables
        private Dictionary<string, int> fruitPoints = new Dictionary<string, int>();
        private List<string> availableFruits = new List<string> { "Apple", "Banana", "Orange", "Grape", "Strawberry", "Kiwi", "Watermelon", "Mango", "Pear" };
        private Random randomFruitSelector = new Random();
        private Color currentColor;
        private readonly Random random = new Random();
        private Timer appearanceTimer;
        private int currentScore = 0;
        private string username;
        private int missedFruitsCount = 0;
        private const int maxMissedFruits = 5;
        private bool gameStopped = false;
        public Form1()
        {

            // Initialize the components of the form or control. 
            InitializeComponent();
            // Hide the panel named "panelStats".
            panelStats.Visible = false;
            // Set the variable "currentColor" to the Color.Black.
            currentColor = Color.Black;
            // Start a new Task that runs the method ContinuousColorChange asynchronously.
            Task.Run(() => ContinuousColorChange());
            // Call the method InitializeFruitPoints.
            InitializeFruitPoints();
            // Hide the panel named "GameOverPanel".
            GameOverPanel.Visible = false;
            // Hide the label named "lbllivescore".
            lbllivescore.Visible = false;
            // Retrieve the top 10 high scores from the SQLite database using the method GetTop10HighScores.
            DataTable top10HighScoresTable = sqlite.GetTop10HighScores();
            // Loop through the minimum of 10 or the number of rows in the retrieved high scores table.
            for (int i = 0; i < Math.Min(10, top10HighScoresTable.Rows.Count); i++)
            {
                // Construct the name of the label to find in the controls collection.
                string placeLabelName = $"lblPlace{i + 1}";
                // Find the label control in the form's controls collection with the constructed name.
                Label placeLabel = Controls.Find(placeLabelName, true).FirstOrDefault() as Label;
                // Check if the label was found.
                if (placeLabel != null)
                {
                    // Set the place variable to the current iteration index + 1.
                    int place = i + 1;
                    // Retrieve the username and high score from the current row in the high scores table.
                    string usernameInList = top10HighScoresTable.Rows[i]["username"].ToString();
                    int highscoreInList = Convert.ToInt32(top10HighScoresTable.Rows[i]["highscore"]);
                    // Set the text of the label to display the username and high score.
                    placeLabel.Text = $"{usernameInList} {highscoreInList}";
                }
            }
        }
        private async Task ContinuousColorChange()
        {
            // Infinite loop (while(true)) for continuous color change.
            while (true)
            {
                // Generate a new random color using random RGB values.
                Color newColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                // Define the number of steps for the color transition.
                int steps = 10;
                // Loop through the color transition steps.
                for (int i = 0; i <= steps; i++)
                {
                    // Calculate the intermediate color values between the current color and the new color.
                    int r = (currentColor.R * (steps - i) + newColor.R * i) / steps;
                    int g = (currentColor.G * (steps - i) + newColor.G * i) / steps;
                    int b = (currentColor.B * (steps - i) + newColor.B * i) / steps;
                    // Change the color using the ChangeColor method with the calculated RGB values.
                    ChangeColor(Color.FromArgb(r, g, b));
                    // Pause the execution for 50 milliseconds to create a smooth color transition.
                    await Task.Delay(50);
                }
                // Update the currentColor to the newColor after the transition is complete.
                currentColor = newColor;
            }
        }
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            // Retrieve the username entered in the "UsernameTXTBOX" TextBox.
            username = UsernameTXTBOX.Text;

            // Retrieve the password entered in the "passwordTXTBOX" TextBox.
            string password = passwordTXTBOX.Text;

            // Check if the entered username and password are authenticated using the IsUserAuthenticated method.
            if (IsUserAuthenticated(username, password))
            {
                // Display a MessageBox indicating a successful login if authentication is successful.
                MessageBox.Show("Successfully logged in!");
            }
            else
            {
                // Display a MessageBox indicating an incorrect username or password if authentication fails.
                MessageBox.Show("Incorrect username or password.");
            }

        }
        private void ChangeColor(Color color)
        {
            // Check if invoking is required for certain UI elements.
            if (btn_logout.InvokeRequired || LoginBtn.InvokeRequired || btn_singup.InvokeRequired || lbllivescore.InvokeRequired)
            {
                // If invoking is required, update UI elements using Invoke method.
                btnHighscoreTitle.Invoke((MethodInvoker)(() => btnHighscoreTitle.BackColor = color));
                Start.Invoke((MethodInvoker)(() => Start.BackColor = color));
                btnStats.Invoke((MethodInvoker)(() => btnStats.BackColor = color));
                btn_logout.Invoke((MethodInvoker)(() => btn_logout.BackColor = color));
                LoginBtn.Invoke((MethodInvoker)(() => LoginBtn.BackColor = color));
                btn_singup.Invoke((MethodInvoker)(() => btn_singup.BackColor = color));
                lbllivescore.Invoke((MethodInvoker)(() => lbllivescore.ForeColor = color));
            }
            else
            {
                // If invoking is not required, update UI elements directly.
                btn_logout.BackColor = color;
                LoginBtn.BackColor = color;
            }
        }
        private void InitializeFruitPoints()
        {
            // Giving the fruits a value 
            fruitPoints["Apple"] = 50;
            fruitPoints["Banana"] = 100;
            fruitPoints["Grape"] = 150;
            fruitPoints["Strawberry"] = 200;
            fruitPoints["Kiwi"] = 250;
            fruitPoints["Orange"] = 300;
            fruitPoints["Mango"] = 350;
            fruitPoints["Pear"] = 400;
            fruitPoints["Watermelon"] = 500;
        }        
        private void ClearFruits()
        {
            // Get a list of all PictureBox controls in the form.
            List<PictureBox> fruitPictureBoxes = Controls.OfType<PictureBox>().ToList();
            // Iterate through each PictureBox control representing a fruit.
            foreach (PictureBox fruitPictureBox in fruitPictureBoxes)
            {
                // Get the current position of the mouse cursor relative to the form.
                Point cursorPos = PointToClient(MousePosition);
                // Check if the mouse cursor is outside the bounds of the current fruit PictureBox.
                if (!fruitPictureBox.Bounds.Contains(cursorPos))
                {
                    // If the cursor is outside, increment the count of missed fruits.
                    missedFruitsCount++;
                }
                // Remove the current fruit PictureBox from the form.
                Controls.Remove(fruitPictureBox);
            }
            // Check if the count of missed fruits is equal to or exceeds a specified threshold.
            if (missedFruitsCount >= maxMissedFruits)
            {
                // If the threshold is reached, stop the game.
                StopGame();
            }
        }
        private async void ShowRandomFruits()
        {
            // Clear all existing fruit PictureBox controls from the form.
            ClearFruits();
            // Check if the game is not stopped.
            if (gameStopped == false)
            {
                // Create a list to store PictureBox controls representing fruits.
                List<PictureBox> fruitPictureBoxes = new List<PictureBox>();
                // Generate and add three random fruits to the form.
                for (int i = 0; i < 3; i++)
                {
                    // Get a random fruit name.
                    string randomFruit = GetRandomFruit();
                    // Initialize variables for the position of the fruit PictureBox.
                    int x, y;
                    PictureBox fruitPictureBox;
                    // Check for overlaps with existing fruit PictureBox controls.
                    do
                    {
                        // Generate random coordinates for the fruit PictureBox within specified bounds.
                        x = randomFruitSelector.Next(25, 875);
                        y = randomFruitSelector.Next(96, 646);
                        // Create a new fruit PictureBox at the generated coordinates.
                        fruitPictureBox = CreateFruitPictureBox(randomFruit, x, y);
                    } while (IsOverlap(fruitPictureBoxes, fruitPictureBox)); // Continue looping until no overlap is found.
                    // Add the newly created fruit PictureBox to the form.
                    Controls.Add(fruitPictureBox);
                    // Bring the fruit PictureBox to the front to ensure visibility.
                    fruitPictureBox.BringToFront();
                    // Add the fruit PictureBox to the list of fruit PictureBox controls.
                    fruitPictureBoxes.Add(fruitPictureBox);
                    // Set the background color of the fruit PictureBox.
                    fruitPictureBox.BackColor = Color.FromArgb(126, 175, 52);
                }
            }
        }
        private bool IsOverlap(List<PictureBox> existingPictureBoxes, PictureBox newPictureBox)
        {
            // Define a margin value for the overlap check.
            int margin = 10;
            // Iterate through each existing PictureBox in the list of existingPictureBoxes.
            foreach (var existingPictureBox in existingPictureBoxes)
            {
                // Check if the bounds of the newPictureBox intersect with the expanded bounds of the existingPictureBox.
                if (newPictureBox.Bounds.IntersectsWith(new Rectangle(existingPictureBox.Left - margin, existingPictureBox.Top - margin, existingPictureBox.Width + 2 * margin, existingPictureBox.Height + 2 * margin)))
                {
                    // If there is an intersection, return true (indicating an overlap).
                    return true;
                }
            }
            // If no overlap is found with any existing PictureBox, return false.
            return false;
        }
        private string GetRandomFruit()
        {
            // Generate a random index using randomFruitSelector, within the bounds of availableFruits.Count.
            int index = randomFruitSelector.Next(availableFruits.Count);
            // Return the element at the randomly selected index from the availableFruits list.
            return availableFruits[index];
        }
        private PictureBox CreateFruitPictureBox(string fruitName, int x, int y)
        {
            // Create a new PictureBox instance.
            PictureBox pictureBox = new PictureBox();
            // Construct the full path to the fruit image using the specified directory and fruitName.
            string imagePath = Path.Combine(@"C:\Users\Eigenaar\Desktop\DevOps en Security\FruitFrenzy\FruitFrenzy\Images", fruitName + ".png");
            // Set the Image property of the PictureBox using the image file located at imagePath.
            pictureBox.Image = Image.FromFile(imagePath);
            // Set the PictureBox to stretch the image to fit its size.
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            // Set the size of the PictureBox to be 50x50 pixels.
            pictureBox.Size = new Size(50, 50);
            // Set the location of the PictureBox on the form using the specified x and y coordinates.
            pictureBox.Location = new Point(x, y);
            // Set the Tag property of the PictureBox to store additional information (in this case, the fruitName).
            pictureBox.Tag = fruitName;
            // Set the cursor to a cross cursor when hovering over the PictureBox.
            pictureBox.Cursor = Cursors.Cross;
            // Attach an event handler (PictureBoxFruit_Click) to the Click event of the PictureBox.
            pictureBox.Click += PictureBoxFruit_Click;
            // Return the configured PictureBox.
            return pictureBox;
        }
        private void PictureBoxFruit_Click(object sender, EventArgs e)
        {
            // Retrieve the fruitName stored in the Tag property of the clicked PictureBox.
            string fruitName = (string)((PictureBox)sender).Tag;
            // Retrieve the points associated with the fruitName from the fruitPoints dictionary.
            int points = fruitPoints[fruitName];
            // Retrieve the location of the clicked PictureBox.
            Point pictureBoxLocation = ((PictureBox)sender).Location;
            // Update the score with the points and the location of the clicked PictureBox.
            UpdateScore(points, pictureBoxLocation);
            // Handle the fruit click (perform additional logic related to the clicked fruit).
            HandleFruitClick(fruitName);
            // Remove the clicked PictureBox from the form's Controls collection.
            Controls.Remove((PictureBox)sender);
        }
        private void UpdateScore(int points, Point pictureBoxLocation)
        {
            // Increase the currentScore by the points associated with the clicked fruit.
            currentScore += points;
            // Update the text of the lbllivescore (Label) to display the current score.
            lbllivescore.Text = "Score: " + points.ToString();
            // Set the location of the lbllivescore label above and centered around the clicked PictureBox.
            lbllivescore.Location = new Point(pictureBoxLocation.X - lbllivescore.Width / 2, pictureBoxLocation.Y - lbllivescore.Height - 5);
            // Make the lbllivescore label visible.
            lbllivescore.Visible = true;
            // Create a new Timer named scoreTimer.
            Timer scoreTimer = new Timer();
            // Set the interval of the timer to 1500 milliseconds (1.5 seconds).
            scoreTimer.Interval = 1500;
            // Attach an event handler to the Tick event of the timer.
            scoreTimer.Tick += (timerSender, timerEventArgs) =>
            {
                // Hide the lbllivescore label.
                lbllivescore.Visible = false;
                // Stop and dispose of the timer to release resources.
                scoreTimer.Stop();
                scoreTimer.Dispose();
            };
            // Start the timer.
            scoreTimer.Start();
        }
        private void HandleFruitClick(string fruitName)
        {
            
        }
        private void btnChangeUsername_Click(object sender, EventArgs e)
        {
            // Hide the panelStats.
            panelStats.Visible = false;
            // Reset the currentScore to 0.
            currentScore = 0;
            // Check if appearanceTimer is not null.
            if (appearanceTimer != null)
            {
                // Stop the appearanceTimer.
                appearanceTimer.Stop();
                // Dispose of the appearanceTimer to release resources.
                appearanceTimer.Dispose();
                // Set appearanceTimer to null.
                appearanceTimer = null;
            }
            // Make the LoginPanel visible.
            LoginPanel.Visible = true;
            // Hide the GameOverPanel.
            GameOverPanel.Visible = false;
        }
        private void StopGame()
        {
            // Set the gameStopped flag to true.
            gameStopped = true;
            // Check if appearanceTimer is not null.
            if (appearanceTimer != null)
            {
                // Stop the appearanceTimer.
                appearanceTimer.Stop();
                // Dispose of the appearanceTimer to release resources.
                appearanceTimer.Dispose();
                // Set appearanceTimer to null.
                appearanceTimer = null;
            }

            // Use Invoke to switch to the UI thread and execute the following code.
            Invoke(new Action(() =>
            {
                // Create a temporary form to handle UI updates.
                using (var tempForm = new Form())
                {
                    // Set the parent of GameOverPanel to the temporary form.
                    GameOverPanel.Parent = tempForm;
                    // Make GameOverPanel visible.
                    GameOverPanel.Visible = true;
                    // Set the parent of GameOverPanel back to the original form.
                    GameOverPanel.Parent = this;
                    // Center GameOverPanel within the form.
                    GameOverPanel.Location = new Point(
                        (this.Width - GameOverPanel.Width) / 2,
                        (this.Height - GameOverPanel.Height) / 2
                    );
                    // Bring GameOverPanel to the front.
                    GameOverPanel.BringToFront();
                    // Set focus to GameOverPanel.
                    GameOverPanel.Focus();
                    // Update the score-related labels.
                    lblScore.Text = "Score: " + currentScore.ToString();
                    lblLastgamescore.Text = currentScore.ToString();
                    // Update the score in the database for the current user.
                    UpdateScoreInDatabase(username, currentScore);
                    // Update lblHighscore with the current high score for the user.
                    lblHighscore.Text = sqlite.GetCurrentHighScore(username).ToString();
                    // Retrieve the top 10 high scores from the database and update UI.
                    DataTable top10HighScoresTable = sqlite.GetTop10HighScores();
                    UpdateTop10HighScores(top10HighScoresTable);
                }
            }));

            // Reset the missedFruitsCount to 0.
            missedFruitsCount = 0;

        }

        private void UpdateTop10HighScores(DataTable highScoresTable)
        {
            // Iterate through the top 10 high scores in the DataTable.
            for (int i = 0; i < Math.Min(10, highScoresTable.Rows.Count); i++)
            {
                // Construct the name of the label to find in the Controls collection.
                string placeLabelName = $"lblPlace{i + 1}";
                // Find the label control with the constructed name in the form's Controls collection.
                Label placeLabel = Controls.Find(placeLabelName, true).FirstOrDefault() as Label;
                // Check if the label is found.
                if (placeLabel != null)
                {
                    // Set the place variable to the current iteration index + 1.
                    int place = i + 1;
                    // Retrieve the username and high score from the current row in the high scores table.
                    string usernameInList = highScoresTable.Rows[i]["username"].ToString();
                    int highscoreInList = Convert.ToInt32(highScoresTable.Rows[i]["highscore"]);
                    // Set the text of the label to display the username and high score.
                    placeLabel.Text = $"{usernameInList}: {highscoreInList}";
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Set the gameStopped flag to false, indicating that the game is not stopped.
            gameStopped = false;
            // Reset the missedFruitsCount to 0.
            missedFruitsCount = 0;
            // Hide the GameOverPanel.
            GameOverPanel.Visible = false;
            // Reset the currentScore to 0.
            currentScore = 0;
            // Call the method ShowRandomFruits to initiate the display of random fruits.
            ShowRandomFruits();
            // Create a new Timer named appearanceTimer.
            appearanceTimer = new Timer();
            // Set the interval of the timer to 2000 milliseconds (2 seconds).
            appearanceTimer.Interval = 2000;
            // Attach an event handler to the Tick event of the timer.
            appearanceTimer.Tick += (timerSender, timerEventArgs) => ShowRandomFruits();
            // Start the timer.
            appearanceTimer.Start();

        }
        private void newUser_Click(object sender, EventArgs e)
        {
            
        }
        private void btn_singup_Click(object sender, EventArgs e)
        {
            // Retrieve the username and password from the corresponding TextBox controls.
            string username = UsernameTXTBOX.Text;
            string password = passwordTXTBOX.Text;
            // Check if the entered username is already taken.
            if (IsUsernameTaken(username))
            {
                // Display a message if the username is already taken.
                MessageBox.Show("Username already taken. Please choose a different username.");
            }
            else
            {
                // If the username is not taken, add the user to the database with the provided username and password.
                sqlite.AddUser(username, password);
                // Display a success message for creating the account.
                MessageBox.Show("Account created successfully!");
                // Retrieve the username and password again from the TextBox controls.
                username = UsernameTXTBOX.Text;
                password = passwordTXTBOX.Text;
                // Check if the user can be automatically authenticated (logged in).
                if (IsUserAuthenticated(username, password))
                {
                    // Display a success message if the automatic login is successful.
                    MessageBox.Show("Successfully logged in!");
                    // Set the username property of the current class to the authenticated username.
                    this.username = username;
                }
                else
                {
                    // Display a message if the automatic login fails and prompt the user to sign in manually.
                    MessageBox.Show("Failed to automatically log in. Please sign in manually.");
                }
            }

        }
        private void UsernameTXTBOX_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void Start_Click(object sender, EventArgs e)
        {
            // Hide the panelStats.
            panelStats.Visible = false;
            // Set the gameStopped flag to false, indicating that the game is not stopped.
            gameStopped = false;
            // Retrieve the current high score for the user and display it in the lblHighscore label.
            lblHighscore.Text = sqlite.GetCurrentHighScore(UsernameTXTBOX.Text).ToString();
            // Hide the LoginPanel.
            LoginPanel.Visible = false;
            // Reset the currentScore to 0.
            currentScore = 0;
            // Call the method ShowRandomFruits to initiate the display of random fruits.
            ShowRandomFruits();
            // Create a new Timer named appearanceTimer.
            appearanceTimer = new Timer();
            // Set the interval of the timer to 2000 milliseconds (2 seconds).
            appearanceTimer.Interval = 2000;
            // Attach an event handler to the Tick event of the timer.
            appearanceTimer.Tick += (timerSender, timerEventArgs) => ShowRandomFruits();
            // Start the timer.
            appearanceTimer.Start();
        }
        private bool IsUserAuthenticated(string username, string password)
        {
            // Retrieve user details from the database for the specified username.
            DataTable userDetailsTable = sqlite.GetUserDetails(username);
            // Check if there is at least one row in the user details table and if the entered password matches the stored password.
            return userDetailsTable.Rows.Count > 0 && userDetailsTable.Rows[0]["password"].ToString() == password;
        }
        private bool IsUsernameTaken(string username)
        {
            // Retrieve user details from the database for the specified username.
            DataTable userDetailsTable = sqlite.GetUserDetails(username);
            // Check if there is at least one row in the user details table.
            return userDetailsTable.Rows.Count > 0;
        }
        private void UpdateScore(string username, int points, Point pictureBoxLocation)
        {
            // Increase the currentScore by the points.
            currentScore += points;
            // Update the text of the lbllivescore (Label) to display the current score.
            lbllivescore.Text = "Score: " + currentScore.ToString();
            // Set the location of the lbllivescore label above and centered around the clicked PictureBox.
            lbllivescore.Location = new Point(pictureBoxLocation.X - lbllivescore.Width / 2, pictureBoxLocation.Y - lbllivescore.Height - 5);
            // Make the lbllivescore label visible.
            lbllivescore.Visible = true;
            // Create a new Timer named scoreTimer.
            Timer scoreTimer = new Timer();
            // Set the interval of the timer to 1500 milliseconds (1.5 seconds).
            scoreTimer.Interval = 1500;
            // Attach an event handler to the Tick event of the timer.
            scoreTimer.Tick += (timerSender, timerEventArgs) =>
            {
                // Hide the lbllivescore label.
                lbllivescore.Visible = false;

                // Stop and dispose of the timer to release resources.
                scoreTimer.Stop();
                scoreTimer.Dispose();
            };
            // Start the timer.
            scoreTimer.Start();
        }
        private void UpdateScoreInDatabase(string username, int newScore)
        {
            // Call the UpdateScore method of the sqlite object with the specified username and newScore.
            sqlite.UpdateScore(username, newScore);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void btnStats_Click(object sender, EventArgs e)
        {
            // Hide the GameOverPanel.
            GameOverPanel.Visible = false;

            // Make the panelStats visible.
            panelStats.Visible = true;

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}