using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace CyberSecurityBotPart2
{

    /// <summary>
    /// Interaction logic for SnakeRollQuizWindow.xaml
    /// </summary>
    public partial class SnakeRollQuizWindow : Window


    {
        private int currentPosition = 0;
        private Dice gameDice = new Dice();
        private List<QuizQuestion> questions = new List<QuizQuestion>();
        private int currentQuestionIndex = 0;
        private int questionsToAsk = 0;
        private int questionsAnswered = 0;
        private int correctThisRound = 0;
        private int totalCorrect = 0;
        private int totalQuestions = 0;
        private Dictionary<int, int> snakes = new Dictionary<int, int>()
{
    {17, 4},
    {26, 12},
    {37, 19},
    {49, 30},
    {58, 39}
};

        private bool immunityQuestion = false;
        private int snakeTail = 0;

        private void LoadQuestions()
        {
            questions.Clear();

            questions.Add(new QuizQuestion("What is phishing?",
                "A) A type of antivirus",
                "B) A scam that tricks users into giving private information",
                "C) A strong password method",
                "D) A firewall setting",
                "B",
                "Phishing tricks users into sharing private information."));

            questions.Add(new QuizQuestion("True or False: It is safe to use the same password for all accounts.",
                "A) True", "B) False", "", "",
                "B",
                "Using one password everywhere is risky. If one account is hacked, others can be hacked too.",
                true));

            questions.Add(new QuizQuestion("Which password is the strongest?",
                "A) password123",
                "B) 12345678",
                "C) Zukhanyise2006",
                "D) T#8mP!92xQ",
                "D",
                "Strong passwords are long and include letters, numbers, and symbols."));

            questions.Add(new QuizQuestion("A message says: 'Your bank account will close today. Click this link now.' What should you do?",
                "A) Click quickly",
                "B) Reply with your password",
                "C) Ignore the warning signs",
                "D) Contact the bank using official details",
                "D",
                "Urgent messages can be phishing. Always verify using official contact details."));

            questions.Add(new QuizQuestion("True or False: Two-factor authentication adds extra protection to an account.",
                "A) True", "B) False", "", "",
                "A",
                "2FA adds another security step, making accounts harder to break into.",
                true));

            questions.Add(new QuizQuestion("Which one is NOT malware?",
                "A) Virus",
                "B) Trojan",
                "C) Firewall",
                "D) Worm",
                "C",
                "A firewall helps protect a system. Viruses, Trojans, and worms are malware."));

            questions.Add(new QuizQuestion("Someone calls pretending to be IT support and asks for your login details. This is an example of:",
                "A) Social engineering",
                "B) Software update",
                "C) Backup",
                "D) Encryption",
                "A",
                "Social engineering tricks people into giving away information."));

            questions.Add(new QuizQuestion("True or False: Public Wi-Fi is always safe for online banking.",
                "A) True", "B) False", "", "",
                "B",
                "Public Wi-Fi can be unsafe. Avoid banking on public networks.",
                true));

            questions.Add(new QuizQuestion("What should you check before clicking a link?",
                "A) If it looks colourful",
                "B) If the sender is famous",
                "C) The actual website address",
                "D) The number of emojis",
                "C",
                "Always check the real website address before clicking."));

            questions.Add(new QuizQuestion("Why are software updates important?",
                "A) They only change colours",
                "B) They fix security weaknesses",
                "C) They delete all files",
                "D) They make passwords public",
                "B",
                "Updates often fix security vulnerabilities."));

            questions.Add(new QuizQuestion("True or False: Antivirus software can help detect harmful programs.",
                "A) True", "B) False", "", "",
                "A",
                "Antivirus software helps detect and block harmful programs.",
                true));

            questions.Add(new QuizQuestion("You receive an email attachment from someone you do not know. What is the safest action?",
                "A) Open it immediately",
                "B) Forward it to friends",
                "C) Download it quickly",
                "D) Do not open it unless verified",
                "D",
                "Unknown attachments can contain malware. Verify before opening."));

            questions.Add(new QuizQuestion("Which action protects your privacy online?",
                "A) Sharing your location publicly",
                "B) Reviewing app permissions",
                "C) Posting your ID number",
                "D) Using weak passwords",
                "B",
                "Reviewing app permissions helps control what personal data apps can access."));

            questions.Add(new QuizQuestion("True or False: A password manager can help you create and store strong passwords.",
                "A) True", "B) False", "", "",
                "A",
                "Password managers help store strong and unique passwords safely.",
                true));

            questions.Add(new QuizQuestion("A website asks for your password but the URL looks slightly misspelled. What is this likely to be?",
                "A) A fake website",
                "B) A safe update",
                "C) A backup page",
                "D) A normal browser feature",
                "A",
                "Fake websites often use misspelled URLs to trick users."));

            questions.Add(new QuizQuestion("What does 2FA stand for?",
                "A) Two-Factor Authentication",
                "B) Two File Access",
                "C) Trusted Firewall Application",
                "D) Total File Agreement",
                "A",
                "2FA means Two-Factor Authentication, which adds another layer of security."));

            questions.Add(new QuizQuestion("True or False: You should install apps only from trusted sources.",
                "A) True", "B) False", "", "",
                "A",
                "Trusted sources reduce the risk of downloading harmful apps.",
                true));

            questions.Add(new QuizQuestion("Which is the safest thing to do before entering login details on a website?",
                "A) Check the URL",
                "B) Close your eyes",
                "C) Share the link",
                "D) Ignore spelling errors",
                "A",
                "Checking the URL helps you avoid fake websites."));

            questions.Add(new QuizQuestion("What is social engineering?",
                "A) Building computer hardware",
                "B) Tricking people into revealing information",
                "C) Updating Windows",
                "D) Creating websites",
                "B",
                "Social engineering uses manipulation to get sensitive information."));

            questions.Add(new QuizQuestion("True or False: A strong password should include your birthday.",
                "A) True", "B) False", "", "",
                "B",
                "Birthdays are easy to guess and should not be used in passwords.",
                true));

            questions.Add(new QuizQuestion("Which one is a safe browsing habit?",
                "A) Clicking every pop-up",
                "B) Downloading unknown files",
                "C) Checking if a site is legitimate",
                "D) Ignoring browser warnings",
                "C",
                "Checking whether a site is legitimate helps prevent scams."));

            questions.Add(new QuizQuestion("What should you do if a browser warns that a website is unsafe?",
                "A) Continue anyway",
                "B) Enter your password quickly",
                "C) Leave the website",
                "D) Disable the warning",
                "C",
                "Browser warnings can protect you from dangerous websites."));

            questions.Add(new QuizQuestion("True or False: Backups can help recover files after ransomware.",
                "A) True", "B) False", "", "",
                "A",
                "Backups can help restore data if files are locked or lost.",
                true));

            questions.Add(new QuizQuestion("Which of these is an example of sensitive information?",
                "A) Favourite colour",
                "B) ID number",
                "C) Shoe size",
                "D) Favourite song",
                "B",
                "An ID number is sensitive and should be protected."));

            questions.Add(new QuizQuestion("What is ransomware?",
                "A) Software that improves speed",
                "B) Malware that locks files and demands payment",
                "C) A password manager",
                "D) A firewall rule",
                "B",
                "Ransomware locks or encrypts files and demands payment."));
            questions.Add(new QuizQuestion("True or False: You should update your password if you suspect an account was hacked.",
    "A) True", "B) False", "", "",
    "A",
    "Changing passwords quickly can help protect compromised accounts.",
    true));

            questions.Add(new QuizQuestion("What is the safest response to a suspicious SMS link?",
                "A) Click it",
                "B) Share it",
                "C) Ignore and verify with the company directly",
                "D) Reply with personal details",
                "C",
                "Suspicious links should be verified using official contact details."));

            questions.Add(new QuizQuestion("Which one is a sign of a phishing email?",
                "A) Urgent threats",
                "B) Correct spelling always",
                "C) No links at all",
                "D) Official invoice only",
                "A",
                "Phishing emails often use urgency to pressure victims."));

            questions.Add(new QuizQuestion("True or False: Logging out of shared computers is important.",
                "A) True", "B) False", "", "",
                "A",
                "Logging out helps prevent others from accessing your account.",
                true));

            questions.Add(new QuizQuestion("What should you do before disposing of an old device?",
                "A) Leave your files on it",
                "B) Remove or wipe personal data",
                "C) Give passwords with it",
                "D) Keep all accounts signed in",
                "B",
                "Old devices should be wiped to protect personal data."));

            questions.Add(new QuizQuestion("Which tool helps block suspicious network traffic?",
                "A) Firewall",
                "B) Calculator",
                "C) Music Player",
                "D) Paint",
                "A",
                "A firewall filters network traffic and blocks suspicious connections."));

            questions.Add(new QuizQuestion("True or False: If an email has your bank's logo it is automatically safe.",
                "A) True", "B) False", "", "",
                "B",
                "Scammers can copy logos. Always verify the sender and links.",
                true));

            questions.Add(new QuizQuestion("What is identity theft?",
                "A) Creating a password",
                "B) Someone using your personal information without permission",
                "C) Installing Windows",
                "D) Buying antivirus",
                "B",
                "Identity theft happens when criminals steal your personal information."));

            questions.Add(new QuizQuestion("Which password is safer?",
                "A) Password123",
                "B) MyBirthday2006",
                "C) T!8n#5Lp$92",
                "D) qwerty",
                "C",
                "Long passwords with symbols, numbers and letters are stronger."));

            questions.Add(new QuizQuestion("True or False: Reviewing your social media privacy settings is good cybersecurity practice.",
                "A) True", "B) False", "", "",
                "A",
                "Privacy settings help protect your personal information.",
                true));

            questions.Add(new QuizQuestion("You receive a login alert you don't recognise. What should you do?",
                "A) Ignore it",
                "B) Change your password immediately",
                "C) Delete the email",
                "D) Reply to the email",
                "B",
                "Unexpected login alerts should be taken seriously."));

            questions.Add(new QuizQuestion("Which file is most risky to open?",
                "A) An attachment from an unknown sender",
                "B) Your own homework",
                "C) A trusted PDF from school",
                "D) A family photo",
                "A",
                "Unknown attachments may contain malware."));

            questions.Add(new QuizQuestion("True or False: Cybersecurity is only important for businesses.",
                "A) True", "B) False", "", "",
                "B",
                "Everyone should protect their devices and personal information.",
                true));

            questions.Add(new QuizQuestion("What is the best action if you suspect phishing?",
                "A) Report it",
                "B) Reply asking if it's real",
                "C) Click the link",
                "D) Forward it to friends",
                "A",
                "Reporting phishing helps protect others."));

            questions.Add(new QuizQuestion("Which action improves your online security?",
                "A) Using Two-Factor Authentication",
                "B) Sharing passwords",
                "C) Clicking every advertisement",
                "D) Ignoring updates",
                "A",
                "2FA greatly improves account security."));

            questions.Add(new QuizQuestion("Which of these is the safest way to protect your online accounts?",
                "A) Use the same password",
                "B) Enable Two-Factor Authentication",
                "C) Share passwords with friends",
                "D) Write passwords on social media",
                "B",
                "Two-Factor Authentication adds an extra layer of protection."));

            questions.Add(new QuizQuestion("True or False: It is safe to save your banking password on a public computer.",
                "A) True", "B) False", "", "",
                "B",
                "Never save banking passwords on shared computers.",
                true));

            questions.Add(new QuizQuestion("What should you do before downloading software?",
                "A) Check it comes from a trusted website",
                "B) Ignore antivirus warnings",
                "C) Download every version",
                "D) Turn off Windows Defender",
                "A",
                "Only download software from trusted websites."));

            questions.Add(new QuizQuestion("Which email address looks most suspicious?",
                "A) support@microsoft.com",
                "B) bank-security@gmail123.com",
                "C) info@unisa.ac.za",
                "D) help@amazon.com",
                "B",
                "Attackers often use strange email addresses."));

            questions.Add(new QuizQuestion("True or False: Cybercriminals only target rich people.",
                "A) True", "B) False", "", "",
                "B",
                "Anyone can become a victim of cybercrime.",
                true));

            questions.Add(new QuizQuestion("What should you do if your antivirus detects malware?",
                "A) Ignore it",
                "B) Follow the antivirus recommendation",
                "C) Disable the antivirus",
                "D) Share the file",
                "B",
                "Your antivirus is designed to quarantine or remove threats."));

            questions.Add(new QuizQuestion("Which PIN is the strongest?",
                "A) 1234",
                "B) 0000",
                "C) 8462",
                "D) 1111",
                "C",
                "Avoid predictable PINs."));

            questions.Add(new QuizQuestion("True or False: HTTPS helps protect your information online.",
                "A) True", "B) False", "", "",
                "A",
                "HTTPS encrypts data between your browser and the website.",
                true));

            questions.Add(new QuizQuestion("Which action helps prevent identity theft?",
                "A) Posting your ID online",
                "B) Using strong passwords",
                "C) Sharing OTPs",
                "D) Clicking unknown links",
                "B",
                "Strong passwords help protect your identity."));

            questions.Add(new QuizQuestion("What should you do if someone asks for your banking OTP?",
                "A) Share it",
                "B) Ignore the request and report it",
                "C) Send only half of it",
                "D) Ask them why first",
                "B",
                "Banks never ask for your OTP. Never share it."));
        }
        private void CreateBoard()
        {
            BoardGrid.Children.Clear();

            int[] cyberFactSquares =
            {
        6,14,22,33,45,54
    };

            int[] snakeSquares =
            {
        17,26,37,49,58
    };

            for (int i = 60; i >= 1; i--)
            {
                Border square = new Border();

                square.BorderBrush = Brushes.DarkOrange;
                square.BorderThickness = new Thickness(1);
                square.Margin = new Thickness(2);
                square.CornerRadius = new CornerRadius(6);

                if (i == 60)
                {
                    square.Background = Brushes.Gold;
                }
                else if (snakeSquares.Contains(i))
                {
                    square.Background = Brushes.IndianRed;
                }
                else if (cyberFactSquares.Contains(i))
                {
                    square.Background = Brushes.Orange;
                }
                else
                {
                    square.Background = Brushes.White;
                }

                TextBlock txt = new TextBlock();
                string label = i.ToString();

                if (i == 60)
                {
                    label = "🏆\nFINISH\n60";
                }
                else if (i == 1)
                {
                    label = "🚀\nSTART\n1";
                }
                else if (snakes.ContainsKey(i))
                {
                    label = "🐍\n" + i;
                }
                else if (cyberFactSquares.Contains(i))
                {
                    label = "💡\n" + i;
                }

                if (i == currentPosition)
                {
                    txt.Text = "🤖\n" + label;
                }
                else
                {
                    txt.Text = label;
                }
                txt.FontWeight = FontWeights.Bold;

                txt.HorizontalAlignment = HorizontalAlignment.Center;

                txt.VerticalAlignment = VerticalAlignment.Center;

                txt.Foreground = Brushes.Black;

                square.Child = txt;

                BoardGrid.Children.Add(square);
            }
        }
        public SnakeRollQuizWindow()
        {
            InitializeComponent();
            CreateBoard();
            LoadQuestions();


        }
        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartGameButton.IsEnabled = false;
            RollDiceButton.IsEnabled = true;

            QuestionText.Text = "Game started! Click ROLL DICE to begin.";
            FeedbackText.Text = "Reach square 60 to win. Good luck!";
        }

        private void RollDiceButton_Click(object sender, RoutedEventArgs e)
        {
            int roll = gameDice.Roll();

            DiceResultText.Text = roll.ToString();

            questionsToAsk = roll;
            questionsAnswered = 0;
            correctThisRound = 0;
           

            FeedbackText.Text = "You rolled a " + roll + ". Answer " + roll + " questions.";

            ShowQuestion();
        }

        private void ShowQuestion()


        {
            if (currentQuestionIndex >= questions.Count)
            {
                currentQuestionIndex = 0;
            }
            QuizQuestion q = questions[currentQuestionIndex];

            QuestionText.Text =
                "Question " + (questionsAnswered + 1) + " of " + questionsToAsk +
                "\n\n" + q.QuestionText;

            OptionAButton.Content = q.OptionA;
            OptionBButton.Content = q.OptionB;
            OptionCButton.Content = q.OptionC;
            OptionDButton.Content = q.OptionD;

            OptionAButton.IsEnabled = true;
            OptionBButton.IsEnabled = true;

            OptionCButton.IsEnabled = !q.IsTrueFalse;
            OptionDButton.IsEnabled = !q.IsTrueFalse;
        }
        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            OptionAButton.IsEnabled = false;
            OptionBButton.IsEnabled = false;
            OptionCButton.IsEnabled = false;
            OptionDButton.IsEnabled = false;

            Button clickedButton = sender as Button;

            string selectedAnswer = clickedButton.Name.Substring(6, 1);

            QuizQuestion currentQuestion = questions[currentQuestionIndex];
            if (immunityQuestion)
            {
                immunityQuestion = false;

                if (selectedAnswer == currentQuestion.CorrectAnswer)
                {
                    MessageBox.Show(
    "SAFE!\nYou escaped the cyber snake.",
    "Immunity Passed",
    MessageBoxButton.OK,
    MessageBoxImage.Information);
                    FeedbackText.Text =
                        "🛡️ Immunity passed! You escaped the cyber snake and stay on square " +
                        currentPosition + ".";
                }
                else
                {
                    currentPosition = snakeTail;

                    PositionText.Text = "Position: " + currentPosition + " / 60";

                    MessageBox.Show(
    "DANGER!\nImmunity failed. You slid down the snake.",
    "Immunity Failed",
    MessageBoxButton.OK,
    MessageBoxImage.Error);
                    FeedbackText.Text =
                        "🐍 Immunity failed! You slid down to square " +
                        currentPosition + ".";
                }

                CreateBoard();

                RollDiceButton.IsEnabled = true;

                return;
            }

            questionsAnswered++;
            totalQuestions++;

            if (selectedAnswer == currentQuestion.CorrectAnswer)


            {
                correctThisRound++;
                totalCorrect++;

                FeedbackText.Text =
                    "Correct! " + currentQuestion.Explanation;
            }
            else
            {
                FeedbackText.Text =
                    "Wrong. The correct answer is " +
                    currentQuestion.CorrectAnswer +
                    ". " + currentQuestion.Explanation;
            }

            currentQuestionIndex++;

            if (questionsAnswered < questionsToAsk)
            {
                NextQuestionButton.Visibility = Visibility.Visible;
            }
            else
            {
                currentPosition += correctThisRound;

                if (currentPosition >= 60)
                {
                    currentPosition = 60;
                    PositionText.Text = "Position: 60 / 60";
                    CreateBoard();
                    EndGame();
                    return;
                }

                PositionText.Text = "Position: " + currentPosition + " / 60";
                ScoreText.Text = "Score: " + totalCorrect + " / " + totalQuestions;

                CreateBoard();

                QuestionText.Text =
                    "Round complete! You got " +
                    correctThisRound + " out of " +
                    questionsToAsk + " correct.";

                FeedbackText.Text +=
                    "\nYou moved " + correctThisRound + " spaces.";

                if (snakes.ContainsKey(currentPosition))
                {
                    snakeTail = snakes[currentPosition];
                    immunityQuestion = true;

                    MessageBox.Show(
    "DANGER!\nYou landed on a cyber snake!\nAnswer the immunity question to escape.",
    "Cyber Snake Warning",
    MessageBoxButton.OK,
    MessageBoxImage.Warning);
                    QuestionText.Text =
                        "🐍 WARNING! You landed on a cyber snake at square " +
                        currentPosition +
                        ".\nAnswer this immunity question to escape!";


                    FeedbackText.Text =
                        "If you answer correctly, you stay safe. If wrong, you slide down to square " +
                        snakeTail + ".";

                    ShowQuestion();
                }
                else
                {
                    RollDiceButton.IsEnabled = true;
                }
            }
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            NextQuestionButton.Visibility = Visibility.Collapsed;

            if (questionsAnswered < questionsToAsk)
            {
                if (currentQuestionIndex >= questions.Count)
                {
                    currentQuestionIndex = 0;
                }

                ShowQuestion();
            }
        }

        private void EndGame()
        {
            double percentage = totalQuestions == 0
                ? 0
                : ((double)totalCorrect / totalQuestions) * 100;

            string rank;

            if (percentage >= 90)
                rank = "🏆 Cybersecurity Master";
            else if (percentage >= 70)
                rank = "🛡️ Cyber Defender";
            else if (percentage >= 50)
                rank = "📚 Cyber Learner";
            else
                rank = "💻 Keep Practicing";

            QuestionText.Text =
                "🏁 GAME COMPLETE!\n\n" +
                "You reached square 60!\n\n" +
                "Final Score: " + totalCorrect + " / " + totalQuestions +
                "\nPercentage: " + percentage.ToString("0") + "%" +
                "\nRank: " + rank;

            FeedbackText.Text =
                "Thank you for playing Snake, Roll, Quiz. Stay cyber smart!";

            RollDiceButton.IsEnabled = false;
            OptionAButton.IsEnabled = false;
            OptionBButton.IsEnabled = false;
            OptionCButton.IsEnabled = false;
            OptionDButton.IsEnabled = false;
            NextQuestionButton.Visibility = Visibility.Collapsed;
        }
    }
}

