using System.Media;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;

namespace CyberSecurityBotPart2
{
    public partial class MainWindow : Window
    {
        private string userName = "";
        private string conversationStage = "name";

        private MemoryStore memory = new MemoryStore();
        private KeywordResponder keywordResponder = new KeywordResponder();
        private SentimentDetector sentimentDetector = new SentimentDetector();
        private ChatBot chatBot;
        private DatabaseHelper databaseHelper = new DatabaseHelper();
        private bool waitingForTaskDescription = false;
        private bool waitingForTaskReminder = false;
        private string pendingTaskTitle = "";
        private string pendingTaskDescription = "";
        private List<string> recentActions = new List<string>();
        private bool showingMoreActivities = false;


        public MainWindow()
        {
            InitializeComponent();
            ChatArea.Document.Blocks.Clear();

            chatBot = new ChatBot(
                memory,
                keywordResponder,
                sentimentDetector);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartIntroAnimation();
        }

        private void StartIntroAnimation()
        {
            DoubleAnimation slideIn = new DoubleAnimation();
            slideIn.From = -250;
            slideIn.To = 350;
            slideIn.Duration = TimeSpan.FromSeconds(1.5);

            slideIn.Completed += (s, e) =>
            {
                PlayVoiceGreeting();

                DoubleAnimation slideOut = new DoubleAnimation();
                slideOut.From = 350;
                slideOut.To = 900;
                slideOut.Duration = TimeSpan.FromSeconds(1.5);

                slideOut.Completed += (s2, e2) =>
                {
                    MovingBot.Visibility = Visibility.Collapsed;
                    DropTitle();
                    ShowWelcomeText();
                };

                MovingBotTransform.BeginAnimation(
                    TranslateTransform.XProperty,
                    slideOut);
            };

            MovingBotTransform.BeginAnimation(
                TranslateTransform.XProperty,
                slideIn);
        }

        private void DropTitle()
        {
            DoubleAnimation drop = new DoubleAnimation();
            drop.From = -40;
            drop.To = 0;
            drop.Duration = TimeSpan.FromSeconds(1);

            DoubleAnimation fade = new DoubleAnimation();
            fade.From = 0;
            fade.To = 1;
            fade.Duration = TimeSpan.FromSeconds(1);

            TitleDropTransform.BeginAnimation(
                TranslateTransform.YProperty,
                drop);

            TitlePanel.BeginAnimation(
                OpacityProperty,
                fade);
        }

        private void ShowWelcomeText()
        {
            AddBotMessage(
                "Welcome to the Cybersecurity Awareness Assistant!\n\n" +
                "My purpose is to enhance your knowledge of cybersecurity and help you stay safe online. " +
                "I will guide you in identifying threats such as phishing attempts and suspicious links, " +
                "and I will also help you create strong and secure passwords to protect your accounts from hackers. " +
                "With that being said, BUCKLE UP!");

            AddBotMessage(
                "Please enter your name to start.");
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("welcome.wav");
                player.PlaySync();
            }
            catch
            {
                MessageBox.Show("Voice greeting could not be played.");
            }
        }

        private void SendButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            string input = UserInput.Text.Trim();

            if (IsExitCommand(input))
            {
                MessageBox.Show(
                    "Goodbye " + userName +
                    "! Stay safe online.");

                Application.Current.Shutdown();
                return;
            }
            if (string.IsNullOrWhiteSpace(input))
            {
                if (conversationStage == "name")
                {
                    AddBotMessage(
                        "Oops! Please enter your name to start.",
                        true);
                }
                else
                {
                    AddBotMessage(
                        "Oops! Please type a cybersecurity topic.",
                        true);
                    
                

                UserInput.Clear();
            
        }

                
                return;


            }



            AddUserMessage(input);
            if (waitingForTaskDescription)
            {
                pendingTaskDescription = input;
                waitingForTaskDescription = false;
                waitingForTaskReminder = true;

                AddBotMessage(
                    "Description saved.\nWould you like a reminder? Type yes or no.");

                UserInput.Clear();
                return;
            }

            if (waitingForTaskReminder)
            {
                waitingForTaskReminder = false;

                if (input.ToLower() == "yes")
                {
                    databaseHelper.AddTask(
                        pendingTaskTitle,
                        pendingTaskDescription,
                        DateTime.Now.AddDays(3));

                    AddBotMessage(
                        "Great! Your task has been saved.\nA reminder has been set for 3 days from today.");
                    LogActivity("Reminder set for '" + pendingTaskTitle + "' in 3 days.");
                }
                else
                {
                    databaseHelper.AddTask(
                        pendingTaskTitle,
                        pendingTaskDescription,
                        null);

                    AddBotMessage(
                        "Great! Your task has been saved without a reminder.");
                    LogActivity("Task added: '" + pendingTaskTitle + "' with no reminder.");
                }

                pendingTaskTitle = "";
                pendingTaskDescription = "";

                UserInput.Clear();
                return;
            }

            if (conversationStage == "name")
            {
                if (input.ToLower().Contains("cybersecurity") ||
                    input.ToLower().Contains("phishing") ||
                    input.ToLower().Contains("password") ||
                    input.ToLower().Contains("privacy") ||
                    input.ToLower().Contains("scam") ||
                    input.ToLower().Contains("tip"))
                {
                    AddBotMessage(
                        "Oops! Please enter your name first before asking cybersecurity questions.",
                        true);

                    UserInput.Clear();
                    return;
                }

                string cleanedName =
                    CleanName(input);

                if (!IsValidName(cleanedName))
                {
                    AddBotMessage(
                        "Oops! Please enter a valid name between 2 and 60 letters.",
                        true);

                    UserInput.Clear();
                    return;
                }

                userName = cleanedName;
                memory.UserName = userName;

                AddBotMessage(
                    "Nice meeting you " +
                    userName +
                    "! How are you today?");

                conversationStage =
                    "feeling";

                UserInput.Clear();
                return;
            }

            else if (conversationStage == "feeling")
            {
                AddBotMessage(
                    chatBot.HandleFeeling(input));

                conversationStage =
                    "topic";

                UserInput.Clear();
                return;
            }

            else
            {
                if (input.ToLower().StartsWith("add task -") ||
    IsNaturalTaskRequest(input) ||
    IsNaturalReminderRequest(input))
                {
                    pendingTaskTitle = input
    .Replace("add task -", "")
    .Replace("Add task -", "")
    .Replace("add a task to", "")
    .Replace("Add a task to", "")
    .Replace("add task to", "")
    .Replace("remind me to", "")
    .Replace("Remind me to", "")
    .Replace("set a reminder to", "")
    .Replace("remember to", "")
    .Trim();
                    waitingForTaskDescription = true;
                    LogActivity("NLP detected task/reminder request: " + pendingTaskTitle);

                    AddBotMessage(
                        "Task title received: " + pendingTaskTitle +
                        "\nPlease enter the task description.");
                }
                else if (input.ToLower() == "show tasks" ||
         input.ToLower() == "show task" ||
         input.ToLower().Contains("my tasks") ||
         input.ToLower().Contains("view tasks") ||
         input.ToLower().Contains("list tasks"))
                {
                    var tasks = databaseHelper.GetAllTasks();

                    if (tasks.Count == 0)
                    {
                        AddBotMessage("No tasks found.");
                    }
                    else
                    {
                        foreach (var task in tasks)
                        {
                            AddBotMessage(task);
                        }
                    }
                }

                else if (input.ToLower().StartsWith("complete task "))
                {
                    string idText = input.Substring(14).Trim();

                    if (int.TryParse(idText, out int taskId))
                    {
                        databaseHelper.MarkTaskCompleted(taskId);

                        AddBotMessage(
                            "Task " + taskId +
                            " marked as completed.");
                        LogActivity("Task " + taskId + " marked as completed.");
                    }
                    else
                    {
                        AddBotMessage(
                            "Please enter a valid task ID.");
                    }
                }

                else if (input.ToLower().StartsWith("delete task "))
                {
                    string idText = input.Substring(12).Trim();

                    if (int.TryParse(idText, out int taskId))
                    {
                        databaseHelper.DeleteTask(taskId);

                        AddBotMessage(
                            "Task " + taskId +
                            " deleted successfully.");
                        LogActivity("Task " + taskId + " deleted.");
                    }


                    else
                    {
                        AddBotMessage(
                            "Please enter a valid task ID.");
                    }

                }

                else if (IsNaturalQuizRequest(input) ||
         input.ToLower() == "play game" ||
         input.ToLower() == "snake roll quiz" ||
         input.ToLower() == "start quiz")
                {
                    SnakeRollQuizWindow gameWindow = new SnakeRollQuizWindow();
                    gameWindow.Show();
                    LogActivity("NLP detected quiz/game request.");

                    LogActivity("Quiz started: Snake, Roll, Quiz.");
                    AddBotMessage("Opening Snake, Roll, Quiz. Good luck!");
                }

                else if (input.ToLower().Contains("show more"))
                {
                    if (recentActions.Count == 0)
                    {
                        AddBotMessage("There are no activity log entries to show.");
                    }
                    else
                    {
                        string summary = "Full activity log:\n\n";

                        for (int i = 0; i < recentActions.Count; i++)
                        {
                            summary += (i + 1) + ". " + recentActions[i] + "\n";
                        }

                        AddBotMessage(summary);
                    }
                }

                else if (input.ToLower().Contains("activity log") ||
          input.ToLower().Contains("what have you done") ||
          input.ToLower().Contains("recent actions") ||
          input.ToLower().Contains("history"))
                {
                    if (recentActions.Count == 0)
                    {
                        AddBotMessage("I haven't completed any actions for you yet.");
                    }
                    else
                    {
                        string summary = "Here's a summary of recent actions:\n\n";

                        int start = Math.Max(0, recentActions.Count - 10);

                        for (int i = start; i < recentActions.Count; i++)
                        {
                            summary += (i - start + 1) + ". " + recentActions[i] + "\n";
                        }

                        if (recentActions.Count > 10)
                        {
                            summary += "\nType 'show more' to view the full activity log.";
                        }

                        AddBotMessage(summary);
                    }
                }
                else
                {
                    string response = chatBot.GetResponse(input);
                    AddBotMessage(response);
                }

                UserInput.Clear();
            }
        }
        private void AddUserMessage(string message)
        {
            Paragraph p = new Paragraph();

            Run userRun =
                new Run(
                    "You: " +
                    message +
                    "\n");

            userRun.Foreground =
                Brushes.Blue;

            p.Inlines.Add(userRun);

            ChatArea.Document.Blocks.Add(p);
            ChatArea.ScrollToEnd();
        }

        private void AddBotMessage(
            string message,
            bool isError = false)
        {
            Paragraph p = new Paragraph();

            Run r =
                new Run(
                    "ChatBuddy: " +
                    message +
                    "\n");

            r.Foreground =
                isError ?
                Brushes.Red :
                Brushes.Black;

            p.Inlines.Add(r);

            ChatArea.Document.Blocks.Add(p);
            ChatArea.ScrollToEnd();
        }

        private void LogActivity(string action)
        {
            string timeStamp = DateTime.Now.ToString("HH:mm");
            recentActions.Add(timeStamp + " - " + action);
        }

        private bool IsExitCommand(string input)
        {
            string lower = input.ToLower().Trim();

            return lower == "exit" ||
                   lower == "quit" ||
                   lower == "bye" ||
                   lower == "goodbye";
        }

        private bool IsNaturalTaskRequest(string input)
        {
            string lower = input.ToLower();

            return lower.Contains("add task") ||
                   lower.Contains("add a task") ||
                   lower.Contains("create task") ||
                   lower.Contains("create a task") ||
                   lower.Contains("new task") ||
                   lower.Contains("task to");
        }

        private bool IsNaturalReminderRequest(string input)
        {
            string lower = input.ToLower();

            return lower.Contains("remind me") ||
                   lower.Contains("set a reminder") ||
                   lower.Contains("add a reminder") ||
                   lower.Contains("remember to");
        }

        private bool IsNaturalQuizRequest(string input)
        {
            string lower = input.ToLower();

            return lower.Contains("quiz") ||
                   lower.Contains("game") ||
                   lower.Contains("play") ||
                   lower.Contains("snake roll");
        }
        private bool IsValidName(string name)
        {
            if (name.Length < 2 || name.Length > 60)
                return false;

            string lower = name.ToLower();

            if (lower == "hi" ||
                lower == "hello" ||
                lower == "hey")
            {
                return false;
            }

            foreach (char c in name)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                    return false;
            }

            return true;
        }


        private string CleanName(string input)
        {
            string cleaned =
                input.ToLower();

            cleaned = cleaned
                .Replace("my name is", "")
                .Replace("name is", "")
                .Replace("i am", "")
                .Replace("i'm", "")
                .Replace("im", "")
                .Trim();

            return cleaned;
        }
    }
}